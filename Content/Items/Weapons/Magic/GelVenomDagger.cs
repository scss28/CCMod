using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.DataStructures;
using Microsoft.Xna.Framework;
using CCMod.Common;

namespace CCMod.Content.Items.Weapons.Magic
{
    internal class GelVenomDagger : ModItem, IMadeBy
    {
        public string CodedBy => "LowQualityTrash-Xinim";

        public string SpritedBy => "PixelGaming";

        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("W.I.P");
        }
        public override void SetDefaults()
        {
            Item.width = 12;
            Item.height = 32;

            Item.useTime = 15;
            Item.useAnimation = 15;

            Item.damage = 50;
            Item.knockBack = 1;

            Item.rare = ItemRarityID.Pink;

            Item.shoot = ModContent.ProjectileType<GelVenomDaggerP>();
            Item.shootSpeed = 15;

            Item.mana = 7;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.DamageType = DamageClass.Magic;

            Item.noUseGraphic = true;
            Item.noMelee = true;
            Item.autoReuse = true;
        }
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            return base.Shoot(player, source, position, velocity, type, damage, knockback);
        }
    }

    class GelVenomDaggerP : ModProjectile
    {
        public override string Texture => "CCMod/Content/Items/Weapons/Magic/GelVenomDagger";

        public override void SetDefaults()
        {
            Projectile.width = 20;
            Projectile.height = 20;
            Projectile.penetrate = 1;
            Projectile.timeLeft = 100;
            Projectile.friendly = true;
            Projectile.tileCollide = true;
            Projectile.DamageType = DamageClass.Magic;
            Projectile.scale = .75f;
            DrawOriginOffsetY -= 16;
        }

        int timerBeforeRotate = 0;
        public override void AI()
        {
            Projectile.alpha += 3;
            Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver2 + 
                (timerBeforeRotate >= 10 ? MathHelper.ToRadians((timerBeforeRotate-20) * (timerBeforeRotate-20) * .5f) : 0);
            Projectile.velocity.Y += timerBeforeRotate >= 10 && Projectile.velocity.Y <= 18 ? .75f : 0;
            timerBeforeRotate++;
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            Collision.TileCollision(Projectile.position, Projectile.velocity, Projectile.width, Projectile.height);
            if(Projectile.velocity.X != oldVelocity.X)
            {
                Projectile.velocity.X = -oldVelocity.X;
            }
            if (Projectile.velocity.Y != oldVelocity.Y)
            {
                Projectile.velocity.Y = -oldVelocity.Y;
            }
            return false;
        }
    }
}
