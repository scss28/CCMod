using System;
using System.Buffers;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

using Microsoft.Xna.Framework;

namespace CCMod.Common
{
    public static partial class CCModUtils
    {
        public static Vector2 Bezier(float ammount, Vector2 a, Vector2 b, Vector2 c)
        {
            Vector2 q1 = Vector2.Lerp(a, b, ammount);
            Vector2 q2 = Vector2.Lerp(b, c, ammount);
            return Vector2.Lerp(q1, q2, ammount);
        }

        public static Vector2 Bezier(float ammount, Vector2 a, Vector2 b, Vector2 c, Vector2 d)
        {
            Vector2 p1 = Vector2.Lerp(a, b, ammount);
            Vector2 p2 = Vector2.Lerp(b, c, ammount);
            Vector2 p3 = Vector2.Lerp(c, d, ammount);

            Vector2 q1 = Vector2.Lerp(p1, p2, ammount);
            Vector2 q2 = Vector2.Lerp(p2, p3, ammount);
            return Vector2.Lerp(q1, q2, ammount);
        }

    }
}
