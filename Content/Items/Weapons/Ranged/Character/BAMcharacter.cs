using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace CCMod.Content.Items.Weapons.Ranged.Character
{
	public class BAMcharacter : ModProjectile
	{
		public override void SetDefaults()
		{
			Projectile.width = 46;
			Projectile.height = 22;
			Projectile.timeLeft = 10;
			Projectile.penetrate = -1;
			Projectile.friendly = true;
			Projectile.tileCollide = false;
			Projectile.DamageType = DamageClass.Ranged;
			Projectile.rotation = MathHelper.ToRadians(Main.rand.Next(-50, 50));
		}
		public override void AI()
		{
			Projectile.scale -= 0.1f;
			Projectile.alpha -= 1;
		}
	}
	public abstract class CharacterProjectile : ModProjectile
	{
		public override void SetDefaults()
		{
			Projectile.width = 12;
			Projectile.height = 18;
			Projectile.friendly = true;
			Projectile.DamageType = DamageClass.Ranged;
			Projectile.penetrate = 1;
			Projectile.timeLeft = 150;
			Projectile.tileCollide = true;
		}
		int onfirstframe = 0;
		public override void AI()
		{
			if (onfirstframe == 0)
			{
				Projectile.scale = 0.1f;
				onfirstframe = 1;
				Projectile.Resize((int)(12 * Projectile.scale), (int)(18 * Projectile.scale));
			}
			if (Projectile.scale < 1f)
			{
				Projectile.scale += 0.1f;
				Projectile.Resize((int)(12 * Projectile.scale), (int)(18 * Projectile.scale));
			}
			Projectile.rotation = Projectile.velocity.ToRotation() + (Projectile.direction != 1 ? MathHelper.Pi : 0);
		}
		public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
		{
			Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.position.X + 6, Projectile.position.Y + 9, 0f, 0f, ModContent.ProjectileType<BAMcharacter>(), 0, 0, Projectile.owner);
		}
	}
	public class Bcharacter : CharacterProjectile
	{
	}
	public class Echaracter : CharacterProjectile
	{
	}
	public class Lcharacter : CharacterProjectile
	{
	}
	public class Tcharacter : CharacterProjectile
	{
	}
	public class Ucharacter : CharacterProjectile
	{
	}
}