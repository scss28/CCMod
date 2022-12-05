﻿using System.Reflection;
using Terraria.ModLoader;
using Microsoft.Xna.Framework.Graphics;

namespace CCMod.Common
{
    public static class SpriteBatchSnapshotCache 
    {
        private const BindingFlags SBBindingFlags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance;
        internal static FieldInfo _sortModeField, _blendStateField, _samplerStateField, _depthStencilStateField, _rasterizerStateField, _effectField, _transformMatrixField;
        internal static FieldInfo SortModeField => _sortModeField ??= typeof(SpriteBatch).GetField("sortMode", SBBindingFlags);
        internal static FieldInfo BlendStateField => _blendStateField ??= typeof(SpriteBatch).GetField("blendState", SBBindingFlags);
        internal static FieldInfo SamplerStateField => _samplerStateField ??= typeof(SpriteBatch).GetField("samplerState", SBBindingFlags);
        internal static FieldInfo DepthStencilStateField => _depthStencilStateField ??= typeof(SpriteBatch).GetField("depthStencilState", SBBindingFlags);
        internal static FieldInfo RasterizerStateField => _rasterizerStateField ??= typeof(SpriteBatch).GetField("rasterizerState", SBBindingFlags);
        internal static FieldInfo EffectField => _effectField ??= typeof(SpriteBatch).GetField("customEffect", SBBindingFlags);
        internal static FieldInfo TransformMatrixField => _transformMatrixField ??= typeof(SpriteBatch).GetField("transformMatrix", SBBindingFlags);

        public static void Begin(this SpriteBatch spriteBatch, in SpriteBatchSnapshot snapshot)
        {
            spriteBatch.Begin(snapshot.SortMode, snapshot.BlendState, snapshot.SamplerState, snapshot.DepthStencilState, snapshot.RasterizerState, snapshot.Effect, snapshot.TransformMatrix);
        }

        /// <inheritdoc cref="SpriteBatchSnapshot.Capture(SpriteBatch)"/>
        public static SpriteBatchSnapshot CaptureSnapshot(this SpriteBatch spriteBatch)
        {
            return SpriteBatchSnapshot.Capture(spriteBatch);
        }

        class Loader : ILoadable
        {
            void ILoadable.Load(Mod mod)
            {
            }

            void ILoadable.Unload()
            {
                _sortModeField = null;
                _blendStateField = null;
                _samplerStateField = null;
                _depthStencilStateField = null;
                _rasterizerStateField = null;
                _effectField = null;
                _transformMatrixField = null;
            }
        }
    }
}