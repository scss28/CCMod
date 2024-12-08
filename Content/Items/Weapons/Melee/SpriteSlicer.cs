using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using CCMod.Common.Attributes;
using Microsoft.Xna.Framework;
using CCMod.Common.GlobalItems;
using Terraria.DataStructures;
using CCmod.Content.Effects.Debuffs;
using CCMod.Content.Projectiles;
using CCMod.Common.ECS.Projectiles;
using CCMod.Common.ECS;
using CCMod.Content.ProjectileModifiers;
using Terraria.GameContent.ItemDropRules;

namespace CCMod.Content.Items.Weapons.Melee
{
	[CodedBy("Pexiltd")]
	[SpritedBy("Pexiltd")]
	[ConceptBy("Pexiltd")]
	public class SpriteSlicer : ModItem, IMeleeWeaponWithImprovedSwing
	{
		public override bool CanRightClick()
		{
			return true;
		}
		public override void ModifyItemLoot(ItemLoot itemLoot)
		{
			itemLoot.Add(ItemDropRule.OneFromOptions(1, ModContent.ItemType<SpriteSmasher>()));
		}
		public float SwingDegree => 120;
		public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
		{		    
			var manager = (IEntity)Projectile.NewProjectileDirect(source, position, velocity * 2, type, damage, knockback).GetGlobalProjectile<ProjectileModifierManager>();
			manager.InstallComponent(new Homing(manager, 216, 0.45f));
			return false;
		}
		public override void MeleeEffects(Player player, Rectangle hitbox)
		{
			if (Main.rand.NextFloat(2) < 0.6511628f)
			{
				if (player.altFunctionUse == 2)
				{
					int dust = Dust.NewDust(new Vector2(hitbox.X, hitbox.Y), hitbox.Width, hitbox.Height, DustID.RainbowTorch, 5f, 5f, 100, new(0, 0, 0), 5f);
					Main.dust[dust].noGravity = true;
					Main.dust[dust].velocity.X += player.direction * 2f;
					Main.dust[dust].velocity.Y += 2f;
				}
				else
				{
					int dust = Dust.NewDust(new Vector2(hitbox.X, hitbox.Y), hitbox.Width, hitbox.Height, DustID.Frost, 3f, 3f, 100, new(0, 0, 0), 3f);
					Main.dust[dust].noGravity = true;
					Main.dust[dust].velocity.X += player.direction * 2f;
					Main.dust[dust].velocity.Y += 2f;
				}
			}

		}
		public override bool AltFunctionUse(Player player)
		{
			return true;
		}
		public override void ModifyItemScale(Player player, ref float scale)
		{
			if (player.altFunctionUse == 2)
			{
				scale = 3.75f;
			}
		}
		public override bool CanUseItem(Player player)
		{
			if (player.HasBuff(ModContent.BuffType<Exhausted>()))
			{
				return false;
			}
			if (player.altFunctionUse == 2)
			{
				Item.useStyle = ItemUseStyleID.Swing;
				Item.useTime = 70;
				Item.useAnimation = 70;
				Item.damage = 200;
				Item.crit = 100;
				Item.width = 54;
				Item.height = 52;
				Item.shoot = ProjectileID.None;
				Item.knockBack = 13;
			}
			else
			{
				Item.useStyle = ItemUseStyleID.Swing;
				Item.knockBack = 3;
				Item.useTime = 20;
				Item.useAnimation = 20;
				Item.damage = 30;
				Item.autoReuse = true;
				Item.scale = 2.15f;
				Item.crit = 10;
				Item.width = 54;
				Item.height = 52;
				Item.useTime = 25;
				Item.useAnimation = 25;
				Item.shootSpeed = 10;
				Item.shoot = ModContent.ProjectileType<RGBFlame>();
			}
			return base.CanUseItem(player);
		}

		public override void OnHitNPC(Player player, NPC target, NPC.HitInfo hit, int damageDone)
		{
			base.OnHitNPC(player, target, hit, damageDone);
			target.AddBuff(BuffID.CursedInferno, 280);
			target.AddBuff(BuffID.Bleeding, 340);
			target.AddBuff(BuffID.Frostburn, 120);
			if (player.altFunctionUse == 2)
			{
				target.AddBuff(BuffID.Frostburn2, 2000);
				player.AddBuff(ModContent.BuffType<Exhausted>(), 100);
			}
			else
			{
				target.AddBuff(BuffID.CursedInferno, 120);
				target.AddBuff(BuffID.Bleeding, 540);
				target.AddBuff(BuffID.Frostburn, 580);
			}
		}
		public override void SetDefaults()
		{
			Item.damage = 70;
			Item.DamageType = DamageClass.Melee;
			Item.width = 54;
			Item.height = 50;
			Item.useTime = 30;
			Item.useAnimation = 30;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.knockBack = 3;
			Item.CanRollPrefix(PrefixID.Legendary);
			Item.CanRollPrefix(PrefixID.Awful);
			Item.value = 50000;
			Item.rare = ItemRarityID.Orange;
			Terraria.Audio.SoundStyle item1 = SoundID.Item1;
			Item.UseSound = item1;
			Item.autoReuse = true;
			Item.scale = 1.0f;
			Item.crit = 20;
		}
	}
}
