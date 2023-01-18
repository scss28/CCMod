using Terraria;
using Terraria.ModLoader;

namespace CCMod.Content.Items.Weapons.Ranged.ShadowShuriken
{
    public class ShadowExplosion : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.width = 28;
            Projectile.height = 28;
            Projectile.friendly = true;
            Projectile.DamageType = DamageClass.Ranged;
            Projectile.penetrate = -1;
            Projectile.timeLeft = 10;
            Projectile.aiStyle = 1;
            Projectile.tileCollide = false;
        }
        public override void AI()
        {
            if (Projectile.timeLeft > 0)
            {
                Projectile.scale -= 0.25f;
            }
        }
        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            target.immune[Projectile.owner] = 1;
        }
    }
}