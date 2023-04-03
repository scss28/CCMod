using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;

namespace CCMod.Common
{
	/// <summary>Contains the data for a <see cref="SpriteBatch.Begin(SpriteSortMode, BlendState, SamplerState, DepthStencilState, RasterizerState, Effect, Matrix)"/> call.</summary>
	public struct SpriteBatchSnapshot
	{
		public SpriteSortMode SortMode;
		public BlendState BlendState;
		public SamplerState SamplerState;
		public DepthStencilState DepthStencilState;
		public RasterizerState RasterizerState;
		public Effect Effect;
		public Matrix TransformMatrix;

		public SpriteBatchSnapshot(SpriteSortMode sortMode, BlendState blendState, SamplerState samplerState, DepthStencilState depthStencilState, RasterizerState rasterizerState, Effect effect, Matrix transformMatrix)
		{
			this.SortMode = sortMode;
			this.BlendState = blendState;
			this.SamplerState = samplerState;
			this.DepthStencilState = depthStencilState;
			this.RasterizerState = rasterizerState;
			this.Effect = effect;
			this.TransformMatrix = transformMatrix;
		}

		/// <summary>Call <see cref="SpriteBatch.Begin(SpriteSortMode, BlendState, SamplerState, DepthStencilState, RasterizerState, Effect, Matrix)"/> with the fields in this <see cref="SpriteBatchSnapshot"/> instance</summary>.
		/// <param name="spriteBatch">The spritebatch to begin.</param>
		public void Begin(SpriteBatch spriteBatch)
		{
			SpriteBatchSnapshotCache.Begin(spriteBatch, in this);
		}

		/// <summary>
		/// Obtains the data passed to <see cref="SpriteBatch.Begin"/> and saves it into a <see cref="SpriteBatchSnapshot"/>.
		/// </summary>
		/// <param name="spriteBatch">The spritebatch to capture it's data.</param>
		/// <returns></returns>
		/// <remarks>
		/// If <see cref="SpriteBatch.Begin"/> has not been called, the contents of the <see cref="SpriteBatchSnapshot"/> <br />
		/// are whatever is in the <paramref name="spriteBatch"/> instance.
		/// </remarks>
		public static SpriteBatchSnapshot Capture(SpriteBatch spriteBatch)
		{
			SpriteSortMode sortMode = (SpriteSortMode)SpriteBatchSnapshotCache.SortModeField.GetValue(spriteBatch);
			BlendState blendState = (BlendState)SpriteBatchSnapshotCache.BlendStateField.GetValue(spriteBatch);
			SamplerState samplerState = (SamplerState)SpriteBatchSnapshotCache.SamplerStateField.GetValue(spriteBatch);
			DepthStencilState depthStencilState = (DepthStencilState)SpriteBatchSnapshotCache.DepthStencilStateField.GetValue(spriteBatch);
			RasterizerState rasterizerState = (RasterizerState)SpriteBatchSnapshotCache.RasterizerStateField.GetValue(spriteBatch);
			Effect effect = (Effect)SpriteBatchSnapshotCache.EffectField.GetValue(spriteBatch);
			Matrix transformMatrix = (Matrix)SpriteBatchSnapshotCache.TransformMatrixField.GetValue(spriteBatch);

			return new SpriteBatchSnapshot(sortMode, blendState, samplerState, depthStencilState, rasterizerState, effect, transformMatrix);
		}
	}
}