﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.Utilities;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;

namespace CCMod.Common
{
    partial class CCModUtils
    {
        /// <summary>
        /// Returns a random item from the given <paramref name="span"/>.
        /// </summary>
        /// <typeparam name="T">The items type.</typeparam>
        /// <param name="random">The random instance.</param>
        /// <param name="span">The span.</param>
        /// <returns>A random item from the given <paramref name="span"/>.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T Next<T>(this UnifiedRandom random, Span<T> span) => span[random.Next(span.Length)];

        /// <summary>
        /// Returns a random item from the given <paramref name="span"/>.
        /// </summary>
        /// <typeparam name="T">The items type.</typeparam>
        /// <param name="random">The random instance.</param>
        /// <param name="span">The span.</param>
        /// <returns>A random item from the given <paramref name="span"/>.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T Next<T>(this UnifiedRandom random, ReadOnlySpan<T> span) => span[random.Next(span.Length)];
    }
}
