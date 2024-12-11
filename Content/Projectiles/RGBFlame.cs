using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.Audio;
using Terraria.ID;
using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Microsoft.CodeAnalysis;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Microsoft.Build.Evaluation;

namespace CCMod.Content.Projectiles
{
	internal class RGBFlame : ModProjectile
	{
		public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
		{
			target.AddBuff(BuffID.CursedInferno, 100);
			target.AddBuff(BuffID.Frostburn, 150);
			target.AddBuff(BuffID.OnFire, 200);
		}

		public override void OnKill(int timeLeft)
		{
			// This code and the similar code above in OnTileCollide spawn dust from the tiles collided with. SoundID.Item10 is the bounce sound you hear.
			Collision.HitTiles(Projectile.position + Projectile.velocity, Projectile.velocity, Projectile.width, Projectile.height);
			SoundEngine.PlaySound(SoundID.Item10, Projectile.position);
		}

		public override void SetDefaults()
		{
			Projectile.width = 40;
			Projectile.height = 64;
			Projectile.friendly = true;
			Projectile.penetrate = 3;
			Projectile.tileCollide = true;
			Projectile.ignoreWater = true;
			Projectile.timeLeft = 300;
			Projectile.alpha = 0;
			Projectile.light = 2.0f;
			Projectile.damage = 25;
			Projectile.CritChance = 15;
			Projectile.spriteDirection = Math.Sign(Projectile.velocity.X);
			Projectile.usesLocalNPCImmunity = true;
			Projectile.localNPCHitCooldown = 1;
			Projectile.DamageType = DamageClass.Melee;
		}

		public override void AI()
		{
			if (++Projectile.frameCounter >= 5)
			{
				Projectile.frameCounter = 0;
				if (++Projectile.frame >= Main.projFrames[Projectile.type])
				{
					Projectile.frame = 0;
				}
			}

			if (Projectile.ai[0] >= 400f)
			{
				Projectile.Kill();
			}

			Projectile.direction = Projectile.spriteDirection = (Projectile.velocity.X > 1f) ? 1 : -1;
			Projectile.rotation = Projectile.velocity.ToRotation();
			if (Projectile.spriteDirection == -1)
			{
				Projectile.rotation += MathHelper.Pi;
			}

			Projectile.velocity.Y += Projectile.ai[0];
			if (Main.rand.NextBool(3))
			{
				Dust.NewDust(Projectile.position + Projectile.velocity, Projectile.width, Projectile.height, DustID.LifeDrain, Projectile.velocity.X * 0.5f, Projectile.velocity.Y * 0.5f);
				Dust.NewDust(Projectile.position + Projectile.velocity, Projectile.width, Projectile.height, DustID.GlowingMushroom, Projectile.velocity.X * 0.5f, Projectile.velocity.Y * 0.5f);
				Dust.NewDust(Projectile.position + Projectile.velocity, Projectile.width, Projectile.height, DustID.GreenFairy, Projectile.velocity.X * 0.5f, Projectile.velocity.Y * 0.5f);
			}
		}
	}
}
