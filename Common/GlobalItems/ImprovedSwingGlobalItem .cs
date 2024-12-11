using Terraria;
using CCMod.Utils;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using CCMod.Common.GlobalItems;

namespace CCMod.Common.GlobalItems
{
	public class ImprovedSwingGlobalItem : GlobalItem
	{
		public const float PLAYERARMLENGTH = 12f;
		/// <summary>
		/// Do not mistake this with giving i frame to melee weapon<br/>
		/// this instead will make it so that the NPC i frame reach faster by setting the end point equal to this<br/>
		/// W.I.P
		/// </summary>
		public int? CustomIFrame = null;
		public override bool InstancePerEntity => true;
		public override void UseStyle(Item item, Player player, Rectangle heldItemFrame)
		{
			if (item.ModItem is IMeleeWeaponWithImprovedSwing itemswing && !item.noMelee)
			{
				SwipeAttack(player, player.GetModPlayer<ImprovedSwingGlobalItemPlayer>(), itemswing.SwingDegree, 1);
			}
		}

		private void SwipeAttack(Player player, ImprovedSwingGlobalItemPlayer modplayer, float swingdegree, int direct)
		{
			float percentDone = player.itemAnimation / (float)player.itemAnimationMax;
			percentDone = CCModUtils.InExpo(percentDone);
			float baseAngle = modplayer.data.ToRotation();
			float angle = MathHelper.ToRadians(baseAngle + swingdegree) * player.direction;
			float start = baseAngle + angle * direct;
			float end = baseAngle - angle * direct;
			float currentAngle = MathHelper.SmoothStep(start, end, percentDone);
			player.itemRotation = currentAngle;
			player.itemRotation += player.direction > 0 ? MathHelper.PiOver4 : MathHelper.PiOver4 * 3f;
			player.compositeFrontArm = new Player.CompositeArmData(true, Player.CompositeArmStretchAmount.Full, currentAngle - MathHelper.PiOver2);
			player.itemLocation = player.MountedCenter + Vector2.UnitX.RotatedBy(currentAngle) * PLAYERARMLENGTH;
		}

		//Credit hitbox code to Stardust
		public override void UseItemHitbox(Item item, Player player, ref Rectangle hitbox, ref bool noHitbox)
		{
			if (item.ModItem is IMeleeWeaponWithImprovedSwing)
			{
				Vector2 handPos = Vector2.UnitY.RotatedBy(player.compositeFrontArm.rotation);
				float length = new Vector2(item.width, item.height).Length() * player.GetAdjustedItemScale(player.HeldItem);
				Vector2 endPos = handPos;
				endPos *= length;
				handPos += player.MountedCenter;
				endPos += player.MountedCenter;
				(int X1, int X2) XVals = CCModUtils.Order(handPos.X, endPos.X);
				(int Y1, int Y2) YVals = CCModUtils.Order(handPos.Y, endPos.Y);
				hitbox = new Rectangle(XVals.X1 - 2, YVals.Y1 - 2, XVals.X2 - XVals.X1 + 2, YVals.Y2 - YVals.Y1 + 2);
			}
		}
		public override bool? CanMeleeAttackCollideWithNPC(Item item, Rectangle meleeAttackHitbox, Player player, NPC target)
		{
			if (item.ModItem is IMeleeWeaponWithImprovedSwing extradata)
			{
				float itemsize = item.Size.Length() * player.GetAdjustedItemScale(player.HeldItem);
				if (CustomIFrame == null)
				{
					return false;
				}
				if (target.immune[player.whoAmI] > 0)
				{
					return false;
				}
				int Amount = 36;
				for (int i = 0; i < Amount; i++)
				{
					Vector2 point = player.Center + Vector2.UnitX.EvenArchSpread(Amount, extradata.SwingDegree, i)
						.RotatedBy((player.GetModPlayer<ImprovedSwingGlobalItemPlayer>().MouseLastPositionBeforeAnimation - player.Center).ToRotation()) * itemsize;
					if (Collision.CheckAABBvLineCollision(target.Hitbox.TopLeft(), target.Size * target.scale, player.Center, point))
					{
						return true;
					}
				}
			}
			return base.CanMeleeAttackCollideWithNPC(item, meleeAttackHitbox, player, target);
		}
	}
}
/// <summary>
/// TODO : Remove this interface cause outdated design, not scalable
/// </summary>
interface IMeleeWeaponWithImprovedSwing
{
	float SwingDegree { get; }
}

public class ImprovedSwingGlobalItemPlayer : ModPlayer
{
	public Vector2 data = Vector2.Zero;
	public Vector2 mouseLastPosition = Vector2.Zero;
	public override void PreUpdate()
	{
		Player.attackCD = 0;
		if (Player.HeldItem.ModItem is not IMeleeWeaponWithImprovedSwing || Player.HeldItem.noMelee)
		{
			return;
		}
	}
	public Vector2 MouseLastPositionBeforeAnimation = Vector2.Zero;
	public override void PostUpdate()
	{
		if (Player.HeldItem.ModItem is not IMeleeWeaponWithImprovedSwing || Player.HeldItem.noMelee)
		{
			return;
		}
		if (Player.ItemAnimationJustStarted)
		{
			data = (Main.MouseWorld - Player.MountedCenter).SafeNormalize(Vector2.Zero);
			Item item = Player.HeldItem;
			ImprovedSwingGlobalItem globalitem = item.GetGlobalItem<ImprovedSwingGlobalItem>();
			if (globalitem.CustomIFrame != null && globalitem.CustomIFrame > -1)
			{
				for (int i = 0; i < Player.meleeNPCHitCooldown.Length; i++)
				{
					if (Player.meleeNPCHitCooldown[i] > 0)
					{
						Player.meleeNPCHitCooldown[i] = (int)globalitem.CustomIFrame;
					}

				}
			}
		}
		if (Player.ItemAnimationActive)
		{
			Player.direction = data.X > 0 ? 1 : -1;
		}
		else
		{
			MouseLastPositionBeforeAnimation = Main.MouseWorld;
		}
	}
	public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
	{
		Item item = Player.HeldItem;
		ImprovedSwingGlobalItem globalitem = item.GetGlobalItem<ImprovedSwingGlobalItem>();
		if (globalitem.CustomIFrame != null && globalitem.CustomIFrame > -1)
		{
			if (Player.meleeNPCHitCooldown[target.whoAmI] > 0)
			{
				Player.meleeNPCHitCooldown[target.whoAmI] = (int)globalitem.CustomIFrame;
			}
		}
	}
}
