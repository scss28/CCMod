using CCMod.Common;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace CCMod.Content.Items.Weapons.Ranged
{
    public class SakuraSlashProjectile : ModProjectile
    {
        public override string Texture => "CCMod/Assets/FX/Glow";

        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[Type] = 80;
            ProjectileID.Sets.TrailingMode[Type] = 2;
        }

        public override void SetDefaults()
        {
            Projectile.width = 5;
            Projectile.height = 5;

            Projectile.aiStyle = -1;

            Projectile.DamageType = DamageClass.Ranged;

            Projectile.penetrate = -1;

            Projectile.friendly = true;
            Projectile.hostile = false;

            Projectile.ignoreWater = true;
            Projectile.tileCollide = false;

            Projectile.timeLeft = 120;

            Projectile.extraUpdates = 4;

            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 999;

            Projectile.scale = 0.2f;
            Projectile.netImportant = true;
        }

        Vector2 initialCenter;
        public override void OnSpawn(IEntitySource source)
        {
            initialCenter = Projectile.Center;
            SoundEngine.PlaySound(SoundID.Item115, Projectile.Center);
        }

        public override void AI()
        {
            Vector2 endPos = initialCenter + Projectile.velocity * 2;
            Projectile.Center = Vector2.Lerp(Projectile.Center, endPos, 0.07f);
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            Projectile.velocity += Projectile.velocity.Normalized() * (target.width + target.height) * 0.5f;
        }

        public override void Kill(int timeLeft)
        {
            CCModUtils.NewDustCircular(Projectile.Center, 10, DustID.WhiteTorch, 24, minMaxSpeedFromCenter: (5, 12), dustAction: d => d.noGravity = true);
        }

        public override bool ShouldUpdatePosition() => false;

        public override bool PreDraw(ref Color lightColor)
        {
            Color allColor = Color.FloralWhite;

            for (int i = 0; i < Projectile.oldPos.Length; i++)
            {
                float prog = ((float)Projectile.oldPos.Length - i) / Projectile.oldPos.Length;

                Vector2 pos = Projectile.oldPos[i] + Projectile.Size * 0.5f;
                Color color = Color.Lerp(Color.DeepPink, allColor, prog);

                float scale = Projectile.scale * prog * Main.rand.NextFloat(0.5f, 1.5f);

                Texture2D tex = TextureAssets.Projectile[Type].Value;
                Main.spriteBatch.Draw(
                    tex,
                    pos - Main.screenPosition,
                    null,
                    color * 0.6f,
                    0f,
                    tex.Size() * 0.5f,
                    scale,
                    SpriteEffects.None,
                    0
                    );

                Color lightingColor = color * 0.006f;
                Lighting.AddLight(pos, lightingColor.R, lightingColor.G, lightingColor.B);
            }

            Projectile.EasyDraw(allColor * 0.7f, position: Projectile.Center + Main.rand.NextVector2Unit() * 3);
            return false;
        }
    }
}
