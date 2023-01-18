using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CCMod.Content.Items.Weapons.Magic
{
    public class BloodBolt : ModProjectile
    {
        int snowflakeDelay = 0;
        public override void SetDefaults()
        {
            Projectile.width = 8;               //The width of projectile hitbox
            Projectile.height = 8;              //The height of projectile hitbox
            Projectile.friendly = true;         //Can the projectile deal damage to enemies?
            Projectile.penetrate = 1;           //How many monsters the projectile can penetrate. (OnTileCollide below also decrements penetrate for bounces as well)
            Projectile.timeLeft = 600;
            Projectile.ignoreWater = true;          //Does the projectile's speed be influenced by water?
            Projectile.tileCollide = false;          //Can the projectile collide with tiles?
            Projectile.alpha = 255;
            Projectile.extraUpdates = 2;
        }
        public override void AI()
        {
            for (int i = 0; i < 4; i++)
            {
                Dust dust;
                dust = Main.dust[Dust.NewDust(Projectile.position, 1, 1, DustID.Blood, 0, 0, 0, default, 0.6f)];
                dust.noGravity = true;
                dust.fadeIn = 0.7f;
            }
            Projectile.ai[0]++;
            if (Projectile.ai[0] >= 30)
            {
                if (Projectile.Distance(Main.MouseWorld) >= 40)
                {
                    Projectile.velocity += Vector2.Lerp(Projectile.velocity, Projectile.DirectionTo(Main.MouseWorld) * 8f, .4f);
                    if (Projectile.velocity.Length() < 35f)
                        Projectile.velocity *= .5f;
                }
            }

        }
        public override bool PreDraw(ref Color lightColor)
        {
            return true;
        }
    }
}