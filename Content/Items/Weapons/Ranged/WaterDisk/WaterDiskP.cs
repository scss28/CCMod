using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using CCMod.Utils;
using Terraria.GameContent;

namespace CCMod.Content.Items.Weapons.Ranged.WaterDisk
{
	public class WaterDiskP : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			ProjectileID.Sets.TrailCacheLength[Type] = 30;
			ProjectileID.Sets.TrailingMode[Type] = 2;
		}
		public override string Texture => CCModTool.GetSameTextureAs<WaterDisk>();
		public override void SetDefaults()
		{
			Projectile.width = Projectile.height = 46;
			Projectile.friendly = true;
			Projectile.penetrate = 5;
			Projectile.DamageType = DamageClass.Ranged;
			Projectile.tileCollide = true;
			Projectile.timeLeft = 360;
			Projectile.extraUpdates = 4;
		}
		public override bool OnTileCollide(Vector2 oldVelocity)
		{
			Projectile.ai[1]++;
			if (Projectile.ai[1] >= 30)
			{
				Projectile.Kill();
			}
			return false;
		}
		public override bool? CanDamage()
		{
			return Projectile.penetrate != 1;
		}
		public override void AI()
		{
			if (Projectile.penetrate == 1)
			{
				if (Projectile.timeLeft > 30)
				{
					Projectile.timeLeft = 30;
					Projectile.penetrate = -1;
					Projectile.friendly = false;
					Projectile.hostile = false;
					Projectile.velocity = Vector2.Zero;
				}
				return;
			}
			Projectile.rotation += 0.4f * Projectile.direction;
			Projectile.ai[2]--;
			if (++Projectile.ai[0] >= 40f)
			{
				Projectile.ai[0] = 0f;
				Projectile.netUpdate = true;
				Vector2 vec = new(Main.rand.NextFloat(-2, 2), Main.rand.NextFloat(-2, 2));
				Vector2 pos = Projectile.Center;
				Projectile.NewProjectile(Projectile.GetSource_FromAI(), Projectile.Center, vec, ModContent.ProjectileType<BubbleP>(), Projectile.damage, 0f, Projectile.owner);
			}
			int dust = Dust.NewDust(Projectile.Center + Main.rand.NextVector2Circular(23, 23), 0, 0, DustID.BubbleBlock, Scale: Main.rand.NextFloat(.5f, .8f));
			Main.dust[dust].velocity = Vector2.Zero;
			Main.dust[dust].noGravity = true;
		}
		public override void OnKill(int timeLeft)
		{
			SpawmBubble();
		}
		public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
		{
			if (Projectile.ai[2] <= 0)
			{
				SpawmBubble();
				Projectile.ai[2] = 12 * 4;
			}
		}
		private void SpawmBubble()
		{
			float rotateRandom = Main.rand.Next(90);
			for (int i = 0; i < 8; i++)
			{
				Vector2 rotate = Vector2.One.EvenArchSpread(8, 360, i) * 3;
				Projectile.NewProjectile(Projectile.GetSource_FromAI(), Projectile.Center, rotate.RotatedBy(MathHelper.ToRadians(rotateRandom)), ModContent.ProjectileType<BubbleP>(), Projectile.damage, 0f, Projectile.owner);
			}
		}
		public override bool PreDraw(ref Color lightColor)
		{
			Projectile.DrawTrailWithoutColorAdjustment(new Color(255, 255, 255, 40) * .1f);
			return base.PreDraw(ref lightColor);
		}
		public override void PostDraw(Color lightColor)
		{
			Projectile.ProjectileDefaultDrawInfo(out Texture2D texture, out Vector2 origin);
			Color color = new Color(255, 255, 255, 50) * .3f;
			Vector2 drawpos = Projectile.position - Main.screenPosition + origin + new Vector2(0f, Projectile.gfxOffY);
			Main.EntitySpriteDraw(texture, drawpos, null, color, Projectile.rotation, origin, Projectile.scale, SpriteEffects.None, 0);
		}
	}
}