using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

#nullable enable
namespace CCMod.Utils
{
	partial class CCModUtils
	{
		/// <summary>
		/// Spawns dust in a circle with with <paramref name="radius"/> away from <paramref name="center"/>, with a random speed range of <paramref name="minMaxSpeedFromCenter"/>.
		/// </summary>
		/// <param name="center">The center of the dust circle.</param>
		/// <param name="radius">The distance of the dusts from the center.</param>
		/// <param name="dustTypeFunc">A function that takes the current loop index and returns the dust type to spawn.</param>
		/// <param name="amount">The dust ammount</param>
		/// <param name="rotation">Rotation offset of the circle.</param>
		/// <param name="minMaxSpeedFromCenter">Random speed range for the spawned dusts or <see langword="null"/> for no velocity.</param>
		/// <param name="dustAction">Action to be invoked per dust.</param>
		public static void NewDustCircular(Vector2 center, float radius, Func<int, int> dustTypeFunc, int amount = 8, float rotation = 0, (float minSpeed, float maxSpeed)? minMaxSpeedFromCenter = null, Action<Dust>? dustAction = null)
		{
			Vector2[] positions = center.GenerateCircularPositions(radius, amount, rotation);
			for (int i = 0; i < positions.Length; i++)
			{
				Vector2 pos = positions[i];
				Vector2 velocity = minMaxSpeedFromCenter is not null ? (center.DirectionTo(pos) * Main.rand.NextFloat(minMaxSpeedFromCenter.Value.minSpeed, minMaxSpeedFromCenter.Value.maxSpeed)) : Vector2.Zero;

				Dust dust = Dust.NewDustDirect(pos, 0, 0, dustTypeFunc.Invoke(i), velocity.X, velocity.Y);

				dustAction?.Invoke(dust);
			}
		}

		/// <summary>
		/// Spawns dust in a circle with with <paramref name="radius"/> away from <paramref name="center"/>, with random speed range <paramref name="minMaxSpeedFromCenter"/>.
		/// </summary>
		/// <param name="center">The center of the dust circle.</param>
		/// <param name="radius">The distance of the dusts from the center.</param>
		/// <param name="dustType">The type of dust.</param>
		/// <param name="amount">The dust ammount</param>
		/// <param name="rotation">Rotation offset of the circle.</param>
		/// <param name="minMaxSpeedFromCenter">Random speed range for the spawned dusts or <see langword="null"/> for no velocity.</param>
		/// <param name="dustAction">Action to be invoked per dust.</param>
		public static void NewDustCircular(Vector2 center, float radius, int dustType, int amount = 8, float rotation = 0, (float, float)? minMaxSpeedFromCenter = null, Action<Dust>? dustAction = null)
		{
			NewDustCircular(center, radius, i => dustType, amount, rotation, minMaxSpeedFromCenter, dustAction);
		}
	}
}