using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CCMod.Common
{
    public partial class CCModUtils
    {
        public static void NewDustCircular(Vector2 center, float radius, Func<int, int> dustTypeFunc, int amount = 8, float rotation = 0, (float, float)? minMaxSpeedFromCenter = null, Action<Dust> dustAction = null)
        {
            Vector2[] positions = center.GenerateCircularPositions(radius, amount, rotation);
            for (int i = 0; i < positions.Length; i++)
            {
                Vector2 pos = positions[i];
                Vector2 velocity = minMaxSpeedFromCenter is not null ? (center.DirectionTo(pos) * Main.rand.NextFloat(minMaxSpeedFromCenter.Value.Item1, minMaxSpeedFromCenter.Value.Item2)) : Vector2.Zero;

                Dust dust = Dust.NewDustDirect(pos, 0, 0, dustTypeFunc.Invoke(i), velocity.X, velocity.Y);

                dustAction?.Invoke(dust);
            }
        }

        public static void NewDustCircular(Vector2 center, float radius, int dustType, int amount = 8, float rotation = 0, (float, float)? minMaxSpeedFromCenter = null, Action<Dust> dustAction = null)
        {
            NewDustCircular(center, radius, i => dustType, amount, rotation, minMaxSpeedFromCenter, dustAction);
        }
    }
}
