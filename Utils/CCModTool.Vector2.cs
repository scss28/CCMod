using Terraria;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System.Linq;

namespace CCMod.Utils
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
		/// <param name="vector">The velocity, this should be use in shoot hook</param>
		/// <param name="projectileCount">The number of projectile</param>
		/// <param name="degree">The degree of the spread</param>
		/// <param name="i"></param>
		/// <returns>Return velocity</returns>
		public static Vector2 EvenArchSpread(this Vector2 vector, float projectileCount, float degree, int i)
		{
			if (projectileCount > 1)
			{
				degree = MathHelper.ToRadians(degree);
				return vector.RotatedBy(MathHelper.Lerp(degree * .5f, degree * -.5f, i / (projectileCount - 1f)));
			}
			return vector;
		}

		public static Vector2 NextVector2RotatedByRandom(this Vector2 vector, float degrees, int v, int i)
		{
			float rotation = MathHelper.ToRadians(degrees);
			return vector.RotatedByRandom(rotation);
		}

		public static Vector2 NextSpread(this Vector2 vector, float spread, float additionalMultiplier = 1)
		{
			vector.X += Main.rand.NextFloat(-spread, spread) * additionalMultiplier;
			vector.Y += Main.rand.NextFloat(-spread, spread) * additionalMultiplier;
			return vector;
		}

		public static Vector2 SmallestInList(List<Vector2> vectors)
		{
			var sortedVectors = new List<Vector2>(vectors);
			sortedVectors.Sort((v1, v2) => v1.LengthSquared().CompareTo(v2.LengthSquared()));
			return sortedVectors.FirstOrDefault();
		}
	}
}
