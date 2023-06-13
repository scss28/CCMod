using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using CCMod.Common;
using Terraria.GameContent.Creative;
using CCMod.Common.Attributes;

namespace CCMod.Content.Items.Weapons.Melee
{
	[CodedBy("???")]
	[SpritedBy("???")]
	public class KnockbackStick : ModItem
	{
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("knockback Stick");
			// Tooltip.SetDefault("[For some reason you cant say, This seems out of place]);
			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
		}
		public override void SetDefaults()
		{
			Item.damage = 7;
			Item.DamageType = DamageClass.Melee;
			Item.width = 5;
			Item.height = 9;
			Item.useTime = 20;
			Item.CanRollPrefix(PrefixID.Legendary);
			Item.CanRollPrefix(PrefixID.Massive);
			Item.CanRollPrefix(PrefixID.Tiny);
			Item.CanRollPrefix(PrefixID.Awful);
			Item.useAnimation = 20;
			Item.useTurn = true;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.knockBack = 23;
			Item.value = 10000;
			Item.rare = ItemRarityID.Green;
			Terraria.Audio.SoundStyle item1 = SoundID.Item1;
			Item.UseSound = item1;
			Item.autoReuse = true;
			Item.crit = 20;

		}
		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddRecipeGroup(RecipeGroupID.Wood, 10);
			recipe.AddIngredient(ItemID.GlowingMushroom, 1);
			recipe.AddTile(TileID.WorkBenches);
			recipe.Register();
		}
	}
}
