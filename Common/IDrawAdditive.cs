using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CCMod.Common
{
	/// <summary>Allows a <see cref="ModProjectile"/> to draw stuff with <see cref="BlendState.Additive"/>.</summary>
	public interface IDrawAdditive
	{
		/// <summary>
		/// Allows you to return a custom snapshot to overwrite the spritebatch state.
		/// </summary>
		/// <param name="snapshot">A snapshot containing the current <see cref="SpriteBatch"/> state.</param>
		/// <returns>A snapshot to set the spritebatch state before calling <see cref="DrawAdditive"/>.</returns>
		/// <remarks>The default interface implementation of this method returns the same snapshot with <see cref="BlendState.Additive"/>.</remarks>
		SpriteBatchSnapshot ModifySpritebatchState(SpriteBatchSnapshot snapshot)
		{
			return snapshot with { BlendState = BlendState.Additive };
		}

		// /// The <paramref name="snapshot"/> provided is the spritebatch's current state without changes.<br />

		/// <summary>
		/// Called in <see cref="DrawAdditiveGlobalProjectile.PostDraw(Projectile, Color)"/>.<br />
		/// Before calling this, the spritebatch is ended and begun with <see cref="BlendState.Additive"/> (unless <br/> 
		/// a custom <see cref="SpriteBatchSnapshot"/> is provided). <br />
		/// After the call to this method the spritebatch ends and begins again.
		/// </summary>
		void DrawAdditive(Color lightColor);
	}

	public class DrawAdditiveGlobalProjectile : GlobalProjectile
	{
		public override void PostDraw(Projectile projectile, Color lightColor)
		{
			if (projectile.ModProjectile is IDrawAdditive drawAdditive)
			{
				SpriteBatchSnapshot snapshit = Main.spriteBatch.CaptureSnapshot();
				Main.spriteBatch.End();
				SpriteBatchSnapshot newSnapshit = drawAdditive.ModifySpritebatchState(snapshit);
				Main.spriteBatch.Begin(in newSnapshit);
				//Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.Additive, Main.DefaultSamplerState, DepthStencilState.None, Main.Rasterizer, null, Main.GameViewMatrix.TransformationMatrix);

				drawAdditive.DrawAdditive(lightColor);

				Main.spriteBatch.End();
				Main.spriteBatch.Begin(snapshit); // restore the spritebatch to how it was before
												  //Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, Main.Rasterizer, null, Main.GameViewMatrix.TransformationMatrix);
			}
		}
	}
}