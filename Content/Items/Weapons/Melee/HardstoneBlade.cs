using CCMod.Common;
using System;
using System.Collections.Generic;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CCMod.Content.Items.Weapons.Melee
{
    public class HardstoneBlade : ModItem, IMadeBy
    {
        public string CodedBy => "sucss";
        public string SpritedBy => "_person, AnUncreativeName";

        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("[c/4bad5e:Shoots stone projectiles that have a tiny chance to drop emeralds on hit.]");
        }

        public override void SetDefaults()
        {
            Item.width = 54;
            Item.height = 54;

            Item.crit = 7;
            Item.damage = 12;
            Item.knockBack = 4f;
            Item.DamageType = DamageClass.Melee;

            Item.UseSound = SoundID.Item1;
            Item.useTime = 25;
            Item.useAnimation = Item.useTime;
            Item.autoReuse = true;

            Item.useStyle = ItemUseStyleID.Swing;

            Item.noMelee = true;

            Item.value = Item.sellPrice(0, 0, 20, 0);
            Item.rare = ItemRarityID.Green;

            Item.shoot = ModContent.ProjectileType<HardstoneBladeHeldProj>();
            Item.shootSpeed = 0f;

            Item.noUseGraphic = true;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.StoneBlock, 25)
                .AddIngredient(ItemID.Emerald, 4)
                .AddTile(TileID.Anvils)
                .Register();
        }
    }
}
