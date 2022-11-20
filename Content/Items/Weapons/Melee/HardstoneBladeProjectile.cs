using CCMod.Common;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace CCMod.Content.Items.Weapons.Melee
{
    public class HardstoneBladeProjectile : ModProjectile
    {
        public override string Texture => "Terraria/Images/Item_" + ItemID.Emerald;

        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[Type] = 5;
            ProjectileID.Sets.TrailingMode[Type] = 2;
        }

        public override void SetDefaults()
        {
            Projectile.width = 12;
            Projectile.height = 12;

            Projectile.aiStyle = -1;

            Projectile.DamageType = DamageClass.Melee;

            Projectile.penetrate = 1;

            Projectile.friendly = true;
            Projectile.hostile = false;

            Projectile.ignoreWater = false;
            Projectile.tileCollide = true;

            Projectile.timeLeft = 500;
        }

        float rotationSpeed;
        public override void OnSpawn(IEntitySource source)
        {
            rotationSpeed = Main.rand.NextFloat(0.3f);
        }

        public override void AI()
        {
            Projectile.rotation += rotationSpeed;
            Projectile.velocity.Y += 0.12f;

            if (Main.rand.NextBool(3))
                Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, DustID.GemEmerald, Scale: 0.6f).noGravity = true;
        }

        public override void Kill(int timeLeft)
        {
            for (int i = 0; i < 4; i++)
                Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.GemEmerald, Projectile.velocity.X, Projectile.velocity.Y);
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Projectile.EasyDrawAfterImage();
            Projectile.EasyDraw(lightColor);
            return false;
        }
    }
}
