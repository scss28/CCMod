using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using CCMod.Utils;
using Terraria.DataStructures;
using Terraria.GameContent.Creative;
using CCMod.Common.Attributes;

namespace CCMod.Content.Items.Weapons.Magic

{
	[CodedBy("Pexiltd")]
	[SpritedBy("Pexiltd")]
	[ConceptBy("Pexiltd")]
	internal class ShroomishBranch : ModItem
	{
		public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
		{
			Projectile.NewProjectile(source, position, velocity, ProjectileID.Mushroom, 3, 11, player.whoAmI);
			for (int i = 0; i < 0; i++)
			{
				Vector2 vec = velocity.EvenArchSpread(1f, 2, i);
				Projectile.NewProjectile(source, position, vec, type, damage, knockback, player.whoAmI);
			}
			return base.Shoot(player, source, position, velocity, type, damage, knockback);
		}

		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Shroomish Branch");
			// Tooltip.SetDefault("[A simple yet effective tool made from magic shrooms and willpower]);
			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
			Item.staff[Item.type] = true;
		}
		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddRecipeGroup(RecipeGroupID.Wood, 10);
			recipe.AddIngredient(ItemID.GlowingMushroom, 5);
			recipe.AddIngredient(ItemID.FallenStar, 3);
			recipe.AddTile(TileID.WorkBenches);
			recipe.Register();
		}
		public override void SetDefaults()
		{
			Terraria.Audio.SoundStyle item1 = SoundID.Item1;
			Item.UseSound = item1;
			Item.SetDefaultMagic(16, 16, 7, 5, 14, 14, ItemUseStyleID.Shoot, ProjectileID.Mushroom, 60, 4, true);
		}
	}
}
