using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace CCMod.Content.Projectiles
{
	public class VoidBubble2 : ModProjectile
	{
		public float RotationRate = 0.5f;
		//Deeznuts
		public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
		{
			target.AddBuff(BuffID.CursedInferno, 220);
		}
		public override void AI()
		{

			Projectile.ai[0]++;
			Projectile.ai[1] = Main.rand.Next(120, 440);
			float speed = 12f;
			float distance = Vector2.Distance(Main.MouseWorld, Projectile.Center);
			if (distance > 140)
			{

				Projectile.velocity = (Main.MouseWorld - Projectile.Center).SafeNormalize(Vector2.Zero) * speed;
			}
			else if (Projectile.ai[0] > Projectile.ai[1])
			{
				Projectile.ai[0] = 0;
				Projectile.ai[1] = Main.rand.Next(100, 450);
				Projectile.velocity = Main.rand.NextFloatDirection().ToRotationVector2() * Main.rand.NextFloat(7, 11);
			}
		}
		public override void SetDefaults()
		{
			Projectile.width = 5;
			Projectile.height = 5;
			Projectile.friendly = true;
			Projectile.penetrate = 9999999;
			Projectile.tileCollide = true;
			Projectile.ignoreWater = true;
			Projectile.timeLeft = 600;
			Projectile.alpha = 100;
			Projectile.light = 0.5f;
			Projectile.damage = 40;
		}
		public override bool OnTileCollide(Vector2 oldVelocity)
		{
			// If collide with tile, reduce the penetrate.
			// So the projectile can reflect at most 5 times
			Projectile.penetrate--;
			if (Projectile.penetrate <= 1)
			{
				Projectile.Kill();
			}
			else
			{
				Collision.HitTiles(Projectile.position, Projectile.velocity, Projectile.width, Projectile.height);
				SoundEngine.PlaySound(SoundID.Item10, Projectile.position);

				// If the projectile hits the left or right side of the tile, reverse the X velocity
				if (Math.Abs(Projectile.velocity.X - oldVelocity.X) > float.Epsilon)
				{
					Projectile.velocity.X = -oldVelocity.X;
				}

				// If the projectile hits the top or bottom side of the tile, reverse the Y velocity
				if (Math.Abs(Projectile.velocity.Y - oldVelocity.Y) > float.Epsilon)
				{
					Projectile.velocity.Y = -oldVelocity.Y;
				}
			}

			return false;
		}
		public override void Kill(int timeLeft)
		{
			// This code and the similar code above in OnTileCollide spawn dust from the tiles collided with. SoundID.Item10 is the bounce sound you hear.
			Collision.HitTiles(Projectile.position + Projectile.velocity, Projectile.velocity, Projectile.width, Projectile.height);
			SoundEngine.PlaySound(SoundID.Item10, Projectile.position);
		}
	}
}
