using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using CCMod.Utils;
using CCMod.Common.GlobalItems;
using CCmod.Content.Effects.Debuffs;
using CCMod.Common.Attributes;

namespace CCMod.Content.Items.Weapons.Melee
{
	[CodedBy("Pexiltd")]
	[SpritedBy("Pexiltd")]
	[ConceptBy("Pexiltd")]
	public class SpriteSmasher : ModItem, IMeleeWeaponWithImprovedSwing
	{
		public float SwingDegree => 120;
		public override void MeleeEffects(Player player, Rectangle hitbox)
		{
			if (Main.rand.NextFloat(1) < 0.6511628f)
			{
				if (player.altFunctionUse == 2)
				{
					int dust = Dust.NewDust(new Vector2(hitbox.X, hitbox.Y), hitbox.Width, hitbox.Height, DustID.Snow, 3f, 3f, 100, new(0, 0, 0), 2f);
					Main.dust[dust].noGravity = true;
					Main.dust[dust].velocity.X += player.direction * 2f;
					Main.dust[dust].velocity.Y += 2f;
				}
				else
				{
					int dust = Dust.NewDust(new Vector2(hitbox.X, hitbox.Y), hitbox.Width, hitbox.Height, DustID.CursedTorch, 0f, 0f, 100, new(0, 0, 0), 2f);
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
				scale = 4f;
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
				Item.useTime = 30;
				Item.useAnimation = 30;
				Item.damage = 140;
				Item.crit = 50;
				Item.width = 54;
				Item.height = 52;
				Item.knockBack = 13;

			}
			else
			{
				Item.damage = 90;
				Item.DamageType = DamageClass.Melee;
				Item.width = 55;
				Item.height = 58;
				Item.useTime = 90;
				Item.useAnimation = 90;
				Item.useStyle = ItemUseStyleID.Swing;
				Item.knockBack = 8;
				Item.CanRollPrefix(PrefixID.Legendary);
				Item.CanRollPrefix(PrefixID.Awful);
				Item.value = 50000;
				Item.rare = ItemRarityID.Orange;
				Terraria.Audio.SoundStyle item1 = SoundID.Item1;
				Item.UseSound = item1;
				Item.autoReuse = true;
				Item.scale = 3f;
				Item.crit = 15;
			}
			return base.CanUseItem(player);
		}
		public override void OnHitNPC(Player player, NPC target, NPC.HitInfo hit, int damageDone)
		{
			if (player.altFunctionUse == 2)
			{
				target.AddBuff(BuffID.CursedInferno, 3000);
				player.AddBuff(ModContent.BuffType<Exhausted>(), 180);
			}
			else
			{
				target.AddBuff(BuffID.OnFire, 480);
				target.AddBuff(BuffID.Bleeding, 540);
				target.AddBuff(BuffID.Frostburn, 220);
			}
			CCModTool.LifeStealOnHit(player.whoAmI, target.whoAmI, 3, 3, 1, 3);
		}
		public override void SetStaticDefaults()
		{
			ItemID.Sets.ShimmerTransformToItem[Type] = ModContent.ItemType<SpriteSlicer>();

		}
		public override void SetDefaults()
		{
			Item.damage = 90;
			Item.DamageType = DamageClass.Melee;
			Item.width = 55;
			Item.height = 58;
			Item.useTime = 80;
			Item.useAnimation = 80;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.knockBack = 8;
			Item.CanRollPrefix(PrefixID.Legendary);
			Item.CanRollPrefix(PrefixID.Awful);
			Item.value = 50000;
			Item.rare = ItemRarityID.Orange;
			Terraria.Audio.SoundStyle item1 = SoundID.Item1;
			Item.UseSound = item1;
			Item.autoReuse = true;
			Item.scale = 2.75f;
			Item.crit = 15;

			}
		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(ItemID.HallowedBar, 13);
			recipe.AddIngredient(ItemID.CursedFlame, 10);
			recipe.AddIngredient(ItemID.BreakerBlade, 1);
			recipe.AddIngredient(ItemID.SoulofFright, 10);
			recipe.AddIngredient(ItemID.SoulofSight, 10);
			recipe.AddIngredient(ItemID.SoulofMight, 10);
			recipe.AddTile(TileID.Anvils);
			recipe.Register();
		}
	}
}
