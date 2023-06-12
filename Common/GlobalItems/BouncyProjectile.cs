using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace CCMod.Common.GlobalItems
{
	internal class BouncyProjectile : GlobalProjectile
	{
		public override void SetDefaults(Projectile entity)
		{
			base.SetDefaults(entity);
			if(entity.ModProjectile is IBouncyProjectile)
			{
				entity.tileCollide = true;
			}
		}
		public override bool OnTileCollide(Projectile projectile, Vector2 oldVelocity)
		{
			if (projectile.ModProjectile is IBouncyProjectile Iproj)
			{
				if (projectile.velocity.X != oldVelocity.X)
				{
					projectile.velocity.X = -oldVelocity.X * Iproj.ChangeVelocityPerBounce;
				}
				if (projectile.velocity.Y != oldVelocity.Y)
				{
					projectile.velocity.Y = -oldVelocity.Y * Iproj.ChangeVelocityPerBounce;
				}
				if (Iproj.BounceTime > 0)
				{
					Iproj.BounceTime--;
				}
				return false;
			}
			return base.OnTileCollide(projectile, oldVelocity);
		}
		public override void PostAI(Projectile projectile)
		{
			base.PostAI(projectile);
			if (projectile.ModProjectile is IBouncyProjectile Iproj)
			{
				if(Iproj.BounceTime < 1)
				{
					projectile.Kill();
				}
			}
		}
	}
	interface IBouncyProjectile
	{
		/// <summary>
		/// Set amount of time can a projectile bounce
		/// set -1 if you want it to bounce infinitely
		/// </summary>
		public int BounceTime { get; set; }
		/// <summary>
		/// This is to decrease the velocity of the projectile
		/// Set this to 1 if you don't want to decrease the velocity at all
		/// This use multiplication, no point to make this any lesser than 0
		/// </summary>
		public int ChangeVelocityPerBounce => 1;
	}
}