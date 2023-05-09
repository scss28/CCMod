using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.GameContent.Creative;


namespace CCMod.Content.Items.Weapons.Magic
{
	public class ElderBranch : ModItem
	{

        public string CodedBy => "Kerm";
        public string SpritedBy => "Person_";
        public string ConceptBy => "Kerm";
        public override void SetStaticDefaults()
		{
			//DisplayName.SetDefault("Elder Branch");
			//Tooltip.SetDefault("A tree which appears to be elder");
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
			Item.useStyle = 5;
			Item.knockBack = 2;
            Item.mana = 1;
            Item.value = 5;
			Item.rare = 0;
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

	