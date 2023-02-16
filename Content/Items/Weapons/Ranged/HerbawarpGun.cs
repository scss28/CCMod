using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace CCMod.Content.Items.Weapons.Ranged
{
    public class HerbawarpGun : ModItem
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("Shoots bullets from the mouse towards the player");
        }

        public override void SetDefaults()
        {
            Item.damage = 70;
            Item.crit = (int)0f;
            Item.DamageType = DamageClass.Ranged;
            Item.width = 42;
            Item.height = 26;
            Item.useTime = 7;
            Item.useAnimation = 7;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.noMelee = false;
            Item.knockBack = 2;
            Item.value = 10000;
            Item.rare = ItemRarityID.Green;
            Item.UseSound = SoundID.Item114;
            Item.autoReuse = true;
            Item.shootSpeed = 10f;
            Item.shoot = ProjectileID.PurificationPowder;
            Item.useAmmo = AmmoID.Bullet;
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            int numberProjectiles = 4 + Main.rand.Next(2); // 4 or 5 shots
            for (int i = 0; i < numberProjectiles; i++)
            {
                Projectile projectile = Main.projectile[Projectile.NewProjectile(source, Main.MouseWorld, Vector2.Zero, type, damage, knockback, player.whoAmI)];
                Vector2 perturbedSpeed = (projectile.DirectionTo(player.Center) * Item.shootSpeed).RotatedByRandom(MathHelper.ToRadians(5));
                projectile.velocity = perturbedSpeed;
                projectile.timeLeft = 300;
            }
            Dust dust;
            // You need to set position depending on what you are doing. You may need to subtract width/2 and height/2 as well to center the spawn rectangle.
            Vector2 pos = Main.MouseWorld;
            for (int u = 0; u < 40; ++u)
            {
                dust = Main.dust[Terraria.Dust.NewDust(pos, 10, 10, 254, 0f, 0f, 0, new Color(255, 255, 255), 2.5f)];
                dust.noGravity = true;
                dust.fadeIn = 3f;
            }
            return false;
        }
        public override void AddRecipes()
        {

        }
    }
}

