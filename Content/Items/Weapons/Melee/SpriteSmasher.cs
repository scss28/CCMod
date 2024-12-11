using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using CCMod.Utils;
using CCMod.Common.GlobalItems;
using CCmod.Content.Effects.Debuffs;
using CCMod.Common.Attributes;
using Terraria.GameContent.ItemDropRules;

namespace CCMod.Content.Items.Weapons.Melee
{
	[CodedBy("Pexiltd")]
	[SpritedBy("Pexiltd")]
	[ConceptBy("Pexiltd")]
	public class SpriteSmasher : ModItem, IMeleeWeaponWithImprovedSwing
	{
		public override bool CanRightClick()
		{
			return true;
		}
		public override void ModifyItemLoot(ItemLoot itemLoot)
		{
			itemLoot.Add(ItemDropRule.OneFromOptions(1, ModContent.ItemType<SpriteSlicer>()));
		}
		public float SwingDegree => 100;
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
				scale = 4.5f;
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
				Item.useTime = 45;
				Item.useAnimation = 45;
				Item.damage = 225;
				Item.crit = 100;
				Item.width = 84;
				Item.height = 52;
				Item.knockBack = 15;
			}
			else
			{
				Item.damage = 70;
				Item.DamageType = DamageClass.Melee;
				Item.width = 65;
				Item.height = 68;
				Item.useTime = 75;
				Item.useAnimation = 75;
				Item.useStyle = ItemUseStyleID.Swing;
				Item.knockBack = 6;
				Item.value = 50000;
				Terraria.Audio.SoundStyle item1 = SoundID.Item1;
				Item.UseSound = item1;
				Item.autoReuse = true;
				Item.scale = 3f;
				Item.crit = 10;
			}
			return base.CanUseItem(player);
		}
		public override void OnHitNPC(Player player, NPC target, NPC.HitInfo hit, int damageDone)
		{
			if (player.altFunctionUse == 2)
			{
				target.AddBuff(BuffID.CursedInferno, 1240);
				target.AddBuff(BuffID.Dazed, 100);
				player.AddBuff(ModContent.BuffType<Exhausted>(), 100);
			}
			else
			{
				target.AddBuff(BuffID.CursedInferno, 333);
				target.AddBuff(BuffID.Weak, 333);
				target.AddBuff(BuffID.Frostburn, 333);
			}
			CCModTool.LifeStealOnHit(player.whoAmI, target.whoAmI, 3, 1, 1, 3);
		}
		public override void SetDefaults()
		{
			Item.damage = 135;
			Item.DamageType = DamageClass.Melee;
			Item.width = 55;
			Item.height = 58;
			Item.useTime = 60;
			Item.useAnimation = 60;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.knockBack = 6;
			Item.CanRollPrefix(PrefixID.Legendary);
			Item.CanRollPrefix(PrefixID.Awful);
			Item.value = 50000;
			Item.rare = ItemRarityID.Orange;
			Terraria.Audio.SoundStyle item1 = SoundID.Item1;
			Item.UseSound = item1;
			Item.autoReuse = true;
			Item.scale = 3.1f;
			Item.crit = 10;
			Item.useLimitPerAnimation = 5;
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
