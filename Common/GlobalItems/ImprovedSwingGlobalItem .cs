using Terraria;
using CCMod.Utils;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;

namespace CCMod.Common.GlobalItems
{
	public class ImprovedSwingGlobalItem : GlobalItem
	{
		public const float PLAYERARMLENGTH = 12f;
		public override void UseStyle(Item item, Player player, Rectangle heldItemFrame)
		{
			if (item.ModItem is not IMeleeWeaponWithImproveSwing || item.noMelee)
			{
				return;
			}
			SwipeAttack(player, player.GetModPlayer<ImprovedSwingGlobalItemPlayer>(), 1);
		}

		private static void SwipeAttack(Player player, ImprovedSwingGlobalItemPlayer modplayer, int direct)
		{
			float percentDone = player.itemAnimation / (float)player.itemAnimationMax;
			percentDone = CCModUtils.InExpo(percentDone);
			float baseAngle = modplayer.data.ToRotation();
			float angle = MathHelper.ToRadians(baseAngle + 120) * player.direction;
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
			if (item.ModItem is IMeleeWeaponWithImproveSwing)
			{
				Vector2 handPos = Vector2.UnitY.RotatedBy(player.compositeFrontArm.rotation);
				float length = item.Size.Length() * player.GetAdjustedItemScale(player.HeldItem);
				Vector2 endPos = handPos * length;

				handPos += player.MountedCenter;
				endPos += player.MountedCenter;

				(int X1, int X2) = CCModUtils.Order(handPos.X, endPos.X);
				(int Y1, int Y2) = CCModUtils.Order(handPos.Y, endPos.Y);
				hitbox = new Rectangle(X1 - 2, Y1 - 2, X2 - X1 + 2, Y2 - Y1 + 2);
			}
		}
	}
	interface IMeleeWeaponWithImproveSwing { }
	public class ImprovedSwingGlobalItemPlayer : ModPlayer
	{
		public Vector2 data = Vector2.Zero;
		public Vector2 mouseLastPosition = Vector2.Zero;
		public override void PreUpdate()
		{
			Player.attackCD = 0;
			if (Player.HeldItem.ModItem is not IMeleeWeaponWithImproveSwing || Player.HeldItem.noMelee)
			{
				return;
			}
		}
		public override void PostUpdate()
		{
			if (Player.HeldItem.ModItem is not IMeleeWeaponWithImproveSwing || Player.HeldItem.noMelee)
			{
				return;
			}
			if (Player.ItemAnimationJustStarted)
			{
				data = (Main.MouseWorld - Player.MountedCenter).SafeNormalize(Vector2.Zero);
			}
			if (Player.ItemAnimationActive)
			{
				Player.direction = data.X > 0 ? 1 : -1;
			}
			Player.attackCD = 0;
		}
	}
}
