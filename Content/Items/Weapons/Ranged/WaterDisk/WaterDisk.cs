using Terraria.ID;
using Terraria;
using Terraria.ModLoader;
using CCMod.Common.Attributes;

namespace CCMod.Content.Items.Weapons.Ranged.WaterDisk
{
	[CodedBy("LowQualityTrash-Xinim")]
	[SpritedBy("LowQualityTrash-Xinim")]
	[ConceptBy("LowQualityTrash-Xinim")]
	public class WaterDisk : ModItem
	{
		public override void SetDefaults()
		{
			Item.DamageType = DamageClass.Ranged;
			Item.autoReuse = true;
			Item.noMelee = true;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.shootSpeed = 4f;
			Item.shoot = ModContent.ProjectileType<WaterDiskP>();
			Item.damage = 14;
			Item.knockBack = 4.5f;
			Item.width = Item.height = 46;
			Item.UseSound = SoundID.Item1;
			Item.useAnimation = Item.useTime = 40;
			Item.noUseGraphic = true;
			Item.rare = ItemRarityID.Blue;
			Item.value = 500;
		}

		public override void AddRecipes()
		{
			CreateRecipe()
				.AddIngredient(ItemID.WaterBucket, 4)
				.AddIngredient(ItemID.Sapphire, 5)
				.AddTile(TileID.DemonAltar)
				.Register();
		}
	}
}
