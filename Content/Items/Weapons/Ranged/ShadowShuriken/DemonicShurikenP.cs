using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using System;

namespace CCMod.Content.Items.Weapons.Ranged.ShadowShuriken
{
    public class DemonicShurikenP : ModProjectile
    {
        public override string Texture => "CCMod/Content/Items/Weapons/Ranged/ShadowShuriken/DemonicShuriken";
        public override void SetDefaults()
        {
            Projectile.width = 42;
            Projectile.height = 42;
            Projectile.aiStyle = 2;
            Projectile.friendly = true;
            Projectile.tileCollide = true;
            Projectile.penetrate = -1;
            Projectile.timeLeft = 35;
            Projectile.DamageType = DamageClass.Ranged;
        }
        public override void AI()
        {
            Projectile.spriteDirection = Projectile.direction;
            Projectile.ai[0] += 1f;
            if (Projectile.ai[0] == 10f)
            {
                Projectile.ai[0] = 0f;
                Projectile.netUpdate = true;
                Projectile.velocity.Y += 0.5f;
                if (Projectile.velocity.Y > 16f)
                {
                    Projectile.velocity.Y = 16f;
                }
            }
        }
        public override void Kill(int timeLeft)
        {
            int randomProjectileNum = Main.rand.Next(3, 7);
            for (int i = 0; i < randomProjectileNum; i++)
            {
                Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, Main.rand.NextVector2Circular(5, 5), ModContent.ProjectileType<ShadowSpirit>(), 5, 0.5f, Projectile.owner);
            }

        }
    }
    public class DemonicShurikenP2 : ModProjectile
    {
        public override string Texture => "CCMod/Content/Items/Weapons/Ranged/ShadowShuriken/DemonicShuriken";
        public override void SetDefaults()
        {
            Projectile.width = 42;
            Projectile.height = 42;
            Projectile.aiStyle = 2;
            Projectile.friendly = true;
            Projectile.tileCollide = true;
            Projectile.penetrate = -1;
            Projectile.timeLeft = 35;
            Projectile.DamageType = DamageClass.Ranged;
        }
        public override void AI()
        {
            Projectile.spriteDirection = Projectile.direction;
            Projectile.ai[0] += 1f;
            if (Projectile.ai[0] == 10f)
            {
                Projectile.ai[0] = 0f;
                Projectile.netUpdate = true;
                Projectile.velocity.Y += 1f;
                Projectile.velocity.X -= Projectile.velocity.X * .08f;
                if (Projectile.velocity.Y > 16f)
                {
                    Projectile.velocity.Y = 16f;
                }
            }
        }
        public override void Kill(int timeLeft)
        {
            int numProj = 4;
            for (int i = 0; i < numProj; i++)
            {
                float speedX = Main.rand.Next(-4, 4) + Projectile.velocity.X;
                float speedY = Main.rand.Next(-1, 3) + Projectile.velocity.Y;
                Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.position.X + 21, Projectile.position.Y + 21, speedX, speedY, ModContent.ProjectileType<DemonicShurikenLeftOverP>(), 12, 2f, Projectile.owner);
            }
        }
    }
    public class DemonicShurikenLeftOverP : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.width = 20;
            Projectile.height = 14;
            Projectile.aiStyle = 1;
            Projectile.friendly = true;
            Projectile.tileCollide = true;
            Projectile.penetrate = 1;
            Projectile.timeLeft = 70;
            Projectile.DamageType = DamageClass.Ranged;
        }
        public override void AI()
        {
            Projectile.rotation += 1f;
            if (Projectile.timeLeft > 25)
            {
                if (Projectile.localAI[0] == 0f)
                {
                    AdjustMagnitude(ref Projectile.velocity);
                    Projectile.localAI[0] = 1f;
                }
                Vector2 move = Vector2.Zero;
                float distance = 400f;
                bool target = false;
                for (int k = 0; k < 200; k++)
                {
                    if (Main.npc[k].active && !Main.npc[k].dontTakeDamage && !Main.npc[k].friendly && Main.npc[k].lifeMax > 5)
                    {
                        Vector2 newMove = Main.npc[k].Center - Projectile.Center;
                        float distanceTo = (float)Math.Sqrt(newMove.X * newMove.X + newMove.Y * newMove.Y);
                        if (distanceTo < distance)
                        {
                            move = newMove;
                            distance = distanceTo;
                            target = true;
                        }
                    }
                }
                if (target)
                {
                    AdjustMagnitude(ref move);
                    Projectile.velocity = (4 * Projectile.velocity + move) / 3f;
                    AdjustMagnitude(ref Projectile.velocity);
                }
                void AdjustMagnitude(ref Vector2 vector)
                {
                    float magnitude = (float)Math.Sqrt(vector.X * vector.X + vector.Y * vector.Y);
                    if (magnitude > 30f)
                    {
                        vector *= 20f / magnitude;
                    }
                }
            }
            else
            {
                Projectile.velocity.X -= Projectile.velocity.X * 0.01f;
                Projectile.velocity.Y += 0.2f;
                if (Projectile.velocity.Y == 10f)
                {
                    Projectile.velocity.Y = 10f;
                }
            }
        }
        public override void Kill(int timeLeft)
        {
            Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.position.X + 10, Projectile.position.Y + 7, 0, 0, ModContent.ProjectileType<ShadowExplosion>(), 10, 2f, Projectile.owner);

            if (timeLeft < 25)
            {
                int numProj = 4;
                for (int i = 0; i < numProj; i++)
                {
                    Vector2 speed = new Vector2(Main.rand.Next(-3, 3) + Projectile.velocity.X, Main.rand.Next(-1, 5) + Projectile.velocity.Y);
                    Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, speed, ModContent.ProjectileType<DemonicShardP>(), 5, 2f, Projectile.owner);
                }
            }
        }
    }
    public class DemonicShardP : ModProjectile
    {
        public override string Texture => "CCMod/Content/Items/Weapons/Ranged/ShadowShuriken/DemonicShurikenLeftOverP";
        public override void SetDefaults()
        {
            Projectile.width = 10;
            Projectile.height = 7;
            Projectile.aiStyle = 1;
            Projectile.friendly = true;
            Projectile.tileCollide = true;
            Projectile.penetrate = 1;
            Projectile.timeLeft = 30;
            Projectile.DamageType = DamageClass.Ranged;
            Projectile.scale = .5f;
        }
        public override void AI()
        {
            Projectile.rotation += 1f;

            Projectile.velocity.Y += 0.5f;
            if (Projectile.velocity.Y == 16f)
            {
                Projectile.velocity.Y = 16f;
            }
        }
        public override void Kill(int timeLeft)
        {
            Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, Vector2.Zero, ModContent.ProjectileType<ShadowExplosion>(), 10, 2f, Projectile.owner);
        }
    }
}