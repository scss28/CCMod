using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace CCMod.Common.GlobalProjectiles
{
	internal class BouncyGlobalProjectile : GlobalProjectile
	{
		public override bool InstancePerEntity => true;
		public override void SetDefaults(Projectile entity)
		{
			base.SetDefaults(entity);
			if (entity.ModProjectile is IBouncyProjectile Iproj)
			{
				entity.tileCollide = true;
				BounceAmount = Iproj.BounceTime;
			}
		}
		int BounceAmount = 1;
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
				if (BounceAmount > 0)
				{
					BounceAmount--;
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
				if (BounceAmount == 0)
				{
					projectile.Kill();
				}
			}
		}
	}
	/// <summary>
	/// Add this to make your projectile can bounce
	/// </summary>
	interface IBouncyProjectile
	{
		/// <summary>
		/// Set amount of time can a projectile bounce
		/// set -1 if you want it to bounce infinitely
		/// </summary>
		public int BounceTime { get; }
		/// <summary>
		/// This is to decrease the velocity of the projectile
		/// Set this to 1 if you don't want to decrease the velocity at all
		/// This use multiplication, no point to make this any lesser than 0
		/// </summary>
		public float ChangeVelocityPerBounce => 1;
	}
}