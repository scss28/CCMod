using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Terraria.DataStructures;

namespace CCMod.Content.Items.Weapons.Ranged.ShadowShuriken
{
    public class DemonicShuriken : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Demonic Shuriken");
            Tooltip.SetDefault("Ashes Away" +
                "\nShoot out a shuriken that spawn out 3 to 7 Shadow Spirit to attack many enemies" +
                "\nAlt click to throw out a faster Shuriken that spawn out 4 scythes that home in to enemy for a moment" +
                "\nafter that will be affect by gravity and will spawn out 4 shards");
        }

        public override void SetDefaults()
        {
            Item.damage = 15;
            Item.DamageType = DamageClass.Ranged;
            Item.useStyle = 1;
            Item.noMelee = true;
            Item.noUseGraphic = true;
            Item.autoReuse = true;
            Item.width = 42;
            Item.height = 42;
            Item.useTime = 40;
            Item.useAnimation = 40;
            Item.knockBack = 2.5f;
            Item.value = 10;
            Item.rare = ItemRarityID.Blue;
            Item.shoot = ModContent.ProjectileType<DemonicShurikenP>();
            Item.shootSpeed = 10f;
        }

        public override bool AltFunctionUse(Player player)
        {
            return true;
        }

        public override bool CanUseItem(Player player)
        {
            if (player.altFunctionUse == 2)
            {
                Item.useTime = 35;
                Item.useAnimation = 35;
                Item.shootSpeed = 15f;
            }
            else
            {
                Item.useAnimation = 20;
                Item.useTime = 20;
                Item.shootSpeed = 10f;
            }
            return base.CanUseItem(player);
        }
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            if (player.altFunctionUse == 2)
            {
                Projectile.NewProjectile(source, position, velocity, ModContent.ProjectileType<DemonicShurikenP2>(), damage, knockback, player.whoAmI);
            }
            else
            {
                Projectile.NewProjectile(source, position, velocity, ModContent.ProjectileType<DemonicShurikenP>(), damage, knockback, player.whoAmI);
            }
            return false;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
            .AddIngredient(ItemID.DemoniteBar, 18)
            .AddTile(TileID.Anvils)
            .Register();
        }
    }
}
