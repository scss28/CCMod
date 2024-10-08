using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CCMod.Content.Items.Weapons.Ranged.WaterDisk
{
	public class BubbleP : ModProjectile
	{
		public override void SetDefaults()
		{
			Projectile.width = Projectile.height = 20;
			Projectile.friendly = true;
			Projectile.penetrate = 2;
			Projectile.tileCollide = true;
			Projectile.timeLeft = 25;
		}

		public override void AI()
		{
			if (Projectile.timeLeft == 10)
			{
				Projectile.velocity.X -= Projectile.velocity.X;
				Projectile.velocity.Y -= Projectile.velocity.Y;
			}
			Projectile.scale = Projectile.timeLeft / 25f;
			int dust = Dust.NewDust(Projectile.Center + Main.rand.NextVector2Circular(10, 10), 0, 0, DustID.BubbleBlock, Scale: Main.rand.NextFloat(.5f, .8f) * Projectile.scale);
			Main.dust[dust].velocity = Microsoft.Xna.Framework.Vector2.Zero;
			Main.dust[dust].noGravity = true;
		}

	}
}