using Terraria;
using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace CCMod.Tool
{
	static partial class CCModTool
	{
		/// <summary>
		/// Use this method if you going to make a weapon that fire out set of projectile with even spread <br/>
		/// Example :
		///<code>
		/// public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
		/// {
		///		for(int i = 0; i &lt; 10; i++)
		///		{
		///			Vector2 vec = velocity.Vector2EvenArchSpread(10f, 30, i);
		///			Projectile.NewProjectile(source, position, vec, type, damage, knockback, player.whoAmI);
		///		}
		///		return false;
		/// }
		/// </code>
		/// </summary>
		/// <param name="velocity">The velocity, this should be use in shoot hook</param>
		/// <param name="AmountofProjectile">The number of projectile</param>
		/// <param name="degree">The degree of the spread</param>
		/// <param name="i"></param>
		/// <returns>Return velocity</returns>
		public static Vector2 Vector2EvenArchSpread(this Vector2 velocity, float AmountofProjectile, float degree, int i)
		{
			if (AmountofProjectile > 1)
			{
				degree = MathHelper.ToRadians(degree);
				return velocity.RotatedBy(MathHelper.Lerp(degree * .5f, degree * -.5f, i / (AmountofProjectile - 1f)));
			}
			return velocity;
		}
		public static Vector2 NextVector2RotatedByRandom(this Vector2 Vec2ToRotate, float ToRadians)
		{
			float rotation = MathHelper.ToRadians(ToRadians);
			return Vec2ToRotate.RotatedByRandom(rotation);
		}
		public static Vector2 NextVector2Spread(this Vector2 ToRotateAgain, float Spread, float additionalMultiplier = 1)
		{
			ToRotateAgain.X += Main.rand.NextFloat(-Spread, Spread) * additionalMultiplier;
			ToRotateAgain.Y += Main.rand.NextFloat(-Spread, Spread) * additionalMultiplier;
			return ToRotateAgain;
		}
		public static Vector2 Vector2SmallestInList(List<Vector2> flag)
		{
			for (int i = 0; i < flag.Count;)
			{
				Vector2 vector2 = flag[i];
				for (int l = i + 1; l < flag.Count; ++l)
				{
					if (vector2.LengthSquared() > flag[l].LengthSquared())
						vector2 = flag[l];
				}
				return vector2;
			}
			return Vector2.Zero;
		}
	}
}
