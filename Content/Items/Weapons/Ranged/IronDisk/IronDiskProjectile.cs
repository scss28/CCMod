using CCMod.Utils;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace CCMod.Content.Items.Weapons.Ranged.IronDisk
{
	public class IronDiskProjectile : ModProjectile
	{
		public override string Texture => CCModTool.GetSameTextureAs<IronDisk>();
		public override void SetDefaults()
		{
			Projectile.width = Projectile.height = 50;
			Projectile.friendly = true;
			Projectile.penetrate = 6;
			Projectile.DamageType = DamageClass.Ranged;
			Projectile.extraUpdates = 1;
		}
		public override void AI()
		{
			Projectile.rotation += MathHelper.ToRadians(30) * (Projectile.velocity.X > 0).ToDirectionInt();
			if (++Projectile.ai[0] < 90)
				return;
			if (Projectile.velocity.Y < 16)
				Projectile.velocity.Y += .1f;
		}
	}
}