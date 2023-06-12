using CCMod.Common.Attributes;
using CCMod.Content.Projectiles;
using CCMod.Utils;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent.Creative;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.ModLoader;

namespace CCMod.Content.Items.Weapons.Ranged.StormGun
{
	[ConceptBy("LowQualityTrashXinim")]
	[SpritedBy("mayhemmm")]
	internal class StormGun : ModItem
	{
		public override void SetStaticDefaults()
		{
			/* Tooltip.SetDefault("Doesn't require mana to operate, but would be a lot more powerful" +
				"\nDamage only scale up whenever you have more than 150 max mana"); */
			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
		}

		public override void SetDefaults()
		{
			Item.width = 68;
			Item.height = 30;

			Item.damage = 75;
			Item.knockBack = 10;
			Item.crit = 30;
			Item.useTime = 40;
			Item.useAnimation = 40;
			Item.value = 1000;

			Item.shoot = ModContent.ProjectileType<StormGunProjectile>();
			Item.shootSpeed = 20;
			Item.mana = 100;

			Item.UseSound = SoundID.Item91;
			Item.DamageType = DamageClass.Ranged;
			Item.rare = ItemRarityID.LightPurple;
			Item.useStyle = ItemUseStyleID.Shoot;

			Item.autoReuse = true;
			Item.noMelee = true;
		}
		public override Vector2? HoldoutOffset()
		{
			return new Vector2(-8, 2);
		}

		public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
		{
			position = position.OffsetPosition(velocity, 75);
		}
	}

	public class StormGunProjectile : ModProjectile
	{
		public override void SetDefaults()
		{
			Projectile.width = 30;
			Projectile.height = 30;
			Projectile.light = 3f;
			Projectile.timeLeft = 100;
			Projectile.extraUpdates = 6;
			Projectile.tileCollide = true;
			Projectile.friendly = true;
			Projectile.hide = true;
		}
		public override void AI()
		{
			for (int i = 0; i < 10; i++)
			{
				Vector2 randPos = Main.rand.NextVector2Circular(4, 4);
				int dust = Dust.NewDust(Projectile.Center, 0, 0, Main.rand.Next(new int[] { DustID.GemRuby, DustID.GemDiamond }), 0, 0, 0, default, 2f);
				Main.dust[dust].noGravity = true;
				Main.dust[dust].velocity = randPos;
			}
		}

		public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
		{
			Player player = Main.player[Projectile.owner];
			int count = 0;
			SoundEngine.PlaySound(SoundID.Item88);
			Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, Vector2.Zero, ModContent.ProjectileType<GhostHitBox>(), Projectile.damage, 0, Projectile.owner);
			float rotation = MathHelper.ToRadians(180);
			float multiplier = 1f + player.statMana <= 150 ? 0 : player.statMana >= 400 ? 250 * .05f : (player.statMana - 150) * .05f;
			float dustNum = 200f;
			for (int i = 0; i < dustNum; i++)
			{
				multiplier += i % 50 == 0 ? 1.5f : 0;
				count += i % 50 == 0 ? 1 : 0;
				Vector2 rotate = Vector2.One.RotatedBy(MathHelper.Lerp(rotation, -rotation, i % 50 / (50f - 1))) * multiplier;
				int dust = Dust.NewDust(Projectile.Center, 0, 0, count != 4 ? DustID.GemRuby : DustID.GemDiamond, rotate.X, rotate.Y, 0, default, 3f);
				Main.dust[dust].noGravity = true;
				Main.dust[dust].velocity = rotate;
				Main.dust[dust].fadeIn = 2;
			}
		}
	}

	public class StormGunPlayer : ModPlayer
	{
		public override void OnMissingMana(Item item, int neededMana)
		{
			if (item.type == ModContent.ItemType<StormGun>())
			{
				Player.statMana += neededMana;
			}
		}
		public override void ModifyWeaponDamage(Item item, ref StatModifier damage)
		{
			if (item.type == ModContent.ItemType<StormGun>() && Player.statMana >= 150)
			{
				damage += (Player.statMana - 150) * .0175f;
			}
		}
	}

	public class StormGunDrop : GlobalNPC
	{
		public override void ModifyNPCLoot(NPC npc, NPCLoot npcLoot)
		{
			LeadingConditionRule leadingConditionRule = new LeadingConditionRule(new StormGunCondition());
			if (npc.type == NPCID.Retinazer)
			{
				leadingConditionRule.OnSuccess(ItemDropRule.ByCondition(new Conditions.MissingTwin(), ModContent.ItemType<StormGun>()));
				npcLoot.Add(leadingConditionRule);
			}
		}
	}
	public class StormGunCondition : IItemDropRuleCondition
	{
		public bool CanDrop(DropAttemptInfo info)
		{
			if (!info.IsInSimulation)
			{
				return
					Main.bloodMoon
					&& info.IsMasterMode;
			}

			return false;
		}
		public bool CanShowItemDropInUI()
		{
			return true;
		}

		public string GetConditionDescription()
		{
			return "This is Master mode drop and only drop if blood moon active";
		}
	}
}