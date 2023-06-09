using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.GameContent.Creative;
using CCMod.Common.Attributes;

namespace CCMod.Content.Items.Weapons.Magic
{
	[CodedBy("Kerm")]
	[SpritedBy("person_")]
	[ConceptBy("Kerm")]
	public class ElderBranch : ModItem
	{
        public override void SetStaticDefaults()
		{
			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
			Item.staff[Item.type] = true;
		}

		public override void SetDefaults()
		{
			Item.damage = 4;
			Item.DamageType = DamageClass.Magic;
			Item.width = 46;
			Item.height = 50;
			Item.useTime = 9;
			Item.useAnimation = 9;
			Item.useStyle = ItemUseStyleID.Shoot;
			Item.knockBack = 2;
            Item.mana = 1;
            Item.value = 5;
			Item.rare = ItemRarityID.White;
			Item.UseSound = SoundID.Grass;
			Item.autoReuse = true;
			Item.useTurn = false;
			Item.shoot = ProjectileID.Leaf;
			Item.shootSpeed = 9f;
		}
        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.Wood, 25)
                .AddIngredient(ItemID.GrassSeeds, 10)
                .AddIngredient(ItemID.Acorn, 2)
                .AddTile(TileID.WorkBenches)
                .Register();
        }
    }
}

	