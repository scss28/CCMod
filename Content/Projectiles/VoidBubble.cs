using CCMod.Utils;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Security.Cryptography.X509Certificates;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace CCMod.Content.Projectiles
{
	public class VoidBubble : ModProjectile
	{

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
			Projectile.damage = 75;
		}

		//Deeznuts
		public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
		{
			target.AddBuff(
				Version == BubbleVersion.V1 ? BuffID.Frostburn2 : Version == BubbleVersion.V2 ? BuffID.CursedInferno : BuffID.OnFire3, 
				220
			);

			if (Version == BubbleVersion.V3)
			{
				CCModTool.LifeStealOnHit(Projectile.owner, target.whoAmI, 3, 3, 1, 3);
			}
		}

		public override void OnSpawn(IEntitySource source)
		{
			if (source.Context is null)
			{
				Projectile.active = false;
				return;
			}

			if (!byte.TryParse(source.Context, out byte version))
			{
				return;
			}

			Version = (BubbleVersion) version;
		}

		private enum BubbleVersion : byte
		{
			V1,
			V2,
			V3
		}

		BubbleVersion Version { get; set; }

		public override void AI()
		{
			Projectile.ai[0]++;
			Projectile.ai[1] = Main.rand.Next(
				Version == BubbleVersion.V1 ? 250 : Version == BubbleVersion.V2 ? 120 : 30,
				Version == BubbleVersion.V1 ? 540 : Version == BubbleVersion.V2 ? 440 : 60
			);

			float speed = Version == BubbleVersion.V1 ? 8f : Version == BubbleVersion.V2 ? 12f : 24f;
			float distance = Vector2.Distance(Main.MouseWorld, Projectile.Center);
			if (distance > (Version == BubbleVersion.V1 ? 150 : Version == BubbleVersion.V2 ? 140 : 80))
			{

				Projectile.velocity = (Main.MouseWorld - Projectile.Center).SafeNormalize(Vector2.Zero) * speed;
			}
			else if (Projectile.ai[0] > Projectile.ai[1] )
			{
				Projectile.ai[0] = 0;
				Projectile.ai[1] = Main.rand.Next(
					Version == BubbleVersion.V1 ? 260 : Version == BubbleVersion.V2 ? 100 : 40,
					Version == BubbleVersion.V1 ? 580 : Version == BubbleVersion.V2 ? 450 : 100
				);
				Projectile.velocity = Main.rand.NextFloatDirection().ToRotationVector2() * Main.rand.NextFloat(
					Version == BubbleVersion.V1 ? 4 : Version == BubbleVersion.V2 ? 7 : 13,
					Version == BubbleVersion.V1 ? 7 : Version == BubbleVersion.V2 ? 11 : 20
				);
			}

			Projectile.direction = Projectile.spriteDirection = (Projectile.velocity.X > 0f) ? 1 : -1;
			Projectile.rotation = Projectile.velocity.ToRotation() + (Projectile.spriteDirection == -1 ? MathHelper.Pi : 0f);
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

		public override void OnKill(int timeLeft)
		{
			// This code and the similar code above in OnTileCollide spawn dust from the tiles collided with. SoundID.Item10 is the bounce sound you hear.
			Collision.HitTiles(Projectile.position + Projectile.velocity, Projectile.velocity, Projectile.width, Projectile.height);
			SoundEngine.PlaySound(SoundID.Item10, Projectile.position);
		}

		public override bool PreDraw(ref Color lightColor)
		{
			Main.spriteBatch.Draw(
				TextureAssets.Projectile[Type].Value,
				Projectile.Center - Main.screenPosition,
				new Rectangle(0, Version == BubbleVersion.V1 ? 0 : Version == BubbleVersion.V2 ? 32 : 64, 32, 32),
				lightColor,
				Projectile.rotation,
				Vector2.One * 16,
				Projectile.scale,
				SpriteEffects.None,
				0f
			);

			return false;
		}
	}
}
	