using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using CCMod.Common.GlobalItems;
using Terraria.DataStructures;
using CCMod.Content.Projectiles;
using CCMod.Content.Effects.Debuffs;
using CCMod.Utils;
using CCMod.Common.Attributes;

namespace CCMod.Content.Items.Weapons.Melee
{
	[CodedBy("Pexiltd")]
	[SpritedBy("Pexiltd")]
	[ConceptBy("Pexiltd")]
	public class CodeBreaker : ModItem, IMeleeWeaponWithImprovedSwing
	{
		public float SwingDegree => 120;
		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(ItemID.BlackAndWhiteDye, 1);
			recipe.AddIngredient(ItemID.CursedFlame, 10);
			recipe.AddIngredient(ModContent.ItemType<SpriteSlicer>(), 1);
			recipe.AddIngredient(ModContent.ItemType<SpriteSmasher>(), 1);
			recipe.AddIngredient(ItemID.FragmentSolar, 10);
			recipe.AddIngredient(ItemID.FragmentStardust, 10);
			recipe.AddIngredient(ItemID.FragmentNebula, 10);
			recipe.AddTile(TileID.LunarCraftingStation);
			recipe.Register();
		}
		public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
		{
			for (byte i = 0; i < 3; i++)
			{
				Projectile.NewProjectile(
					player.GetSource_ItemUse_WithPotentialAmmo(Item, source.AmmoItemIdUsed, $"{i}"),
					position,
					velocity,
					ModContent.ProjectileType<VoidBubble>(),
					i == 0 ? 150 : i == 1 ? 100 : 40,
					i == 0 ? 5 : i == 1 ? 3 : 15, 
					player.whoAmI
				);
			}

			return false;
		}
		public override bool AltFunctionUse(Player player)
		{
			return true;
		}
		public override void ModifyItemScale(Player player, ref float scale)
		{
			if (player.altFunctionUse == 2)
			{
				scale = 2.50f;
			}
		}

		public override bool CanUseItem(Player player)
		{
			if (player.HasBuff(ModContent.BuffType<Exhausted2>()))
			{
				return false;
			}
			if (player.altFunctionUse == 2)
			{
				Item.shoot = ModContent.ProjectileType<VoidBubble>();
				Item.useStyle = ItemUseStyleID.Swing;
				Item.useTime = 20;
				Item.useAnimation = 20;
				player.AddBuff(BuffID.Thorns, 5000);
				player.AddBuff(BuffID.Shine, 5000);
				Item.damage = 300;
				Item.crit = 100;
				Item.shootSpeed = 10;
				player.AddBuff(ModContent.BuffType<Exhausted2>(), 600);
			}
			else
			{
				Item.damage = 130;
				Item.DamageType = DamageClass.Melee;
				Item.width = 54;
				Item.height = 50;
				Item.useTime = 30;
				Item.useAnimation = 30;
				Item.useStyle = ItemUseStyleID.Swing;
				Item.knockBack = 6;
				Item.CanRollPrefix(PrefixID.Legendary);
				Item.CanRollPrefix(PrefixID.Awful);
				Item.value = 100000;
				Item.rare = ItemRarityID.Orange;
				Terraria.Audio.SoundStyle item1 = SoundID.Item1;
				Item.UseSound = item1;
				Item.autoReuse = true;
				Item.scale = 2.5f;
				Item.crit = 40;
				Item.shoot = ProjectileID.None;
				Item.shootSpeed = 10;
			}
			return base.CanUseItem(player);
		}

		public override void OnHitNPC(Player player, NPC target, NPC.HitInfo hit, int damageDone)
		{
			base.OnHitNPC(player, target, hit, damageDone);
			target.AddBuff(BuffID.CursedInferno, 480);
			target.AddBuff(BuffID.Bleeding, 540);
			target.AddBuff(BuffID.Frostburn2, 220);
			target.AddBuff(BuffID.OnFire3, 220);
			if (player.altFunctionUse == 2)
			{
				player.AddBuff(BuffID.Shine, 300);
			}
			else
			{
				target.AddBuff(BuffID.CursedInferno, 480);
				target.AddBuff(BuffID.Bleeding, 540);
				target.AddBuff(BuffID.Frostburn2, 220);
				target.AddBuff(BuffID.OnFire3, 220);
			}
			CCModTool.LifeStealOnHit(player.whoAmI, target.whoAmI, 3, 3, 1, 3);
		}
		public override void SetDefaults()
		{
			Item.damage = 130;
			Item.DamageType = DamageClass.Melee;
			Item.width = 54;
			Item.height = 50;
			Item.useTime = 40;
			Item.useAnimation = 40;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.knockBack = 6;
			Item.CanRollPrefix(PrefixID.Legendary);
			Item.CanRollPrefix(PrefixID.Awful);
			Item.value = 100000;
			Item.rare = ItemRarityID.Orange;
			Terraria.Audio.SoundStyle item1 = SoundID.Item1;
			Item.UseSound = item1;
			Item.autoReuse = true;
			Item.scale = 2.5f;
			Item.crit = 40;
			Item.CanRollPrefix(PrefixID.Legendary);
			Item.CanRollPrefix(PrefixID.Legendary2); //2 huh?
		}
		public override void MeleeEffects(Player player, Rectangle hitbox)
		{
			if (Main.rand.NextBool(2))
			{
				if (player.altFunctionUse == 2)
				{
					int dust = Dust.NewDust(new Vector2(hitbox.X, hitbox.Y), hitbox.Width, hitbox.Height, DustID.t_Meteor, 2f, 2f, 100, default, 3f);
					Main.dust[dust].noGravity = true;
					Main.dust[dust].velocity.X += player.direction * 2f;
					Main.dust[dust].velocity.Y += 0.2f;
				}
				else
				{
					int dust = Dust.NewDust(new Vector2(hitbox.X, hitbox.Y), hitbox.Width, hitbox.Height, DustID.FireworksRGB, player.velocity.X * 0.2f + (float)(player.direction * 3), player.velocity.Y * 0.2f, 100, default, 2.5f);
					Main.dust[dust].noGravity = true;
				}
			}
		}
	}
}
