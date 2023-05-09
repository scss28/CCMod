using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

using Terraria;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.PlayerDrawLayer;

namespace CCMod.Utils
{
	partial class CCModUtils
	{
		/// <summary>
		/// Array segment skipping the last projectile in <see cref="Main.projectile"/>.<br/>
		/// Can be used in <see langword="foreach"/> statements.
		/// </summary>
		public static ArraySegment<Projectile> ProjectileForeach => new(Main.projectile, 0, Main.projectile.Length - 1);
		public static void EasyDraw(this Projectile projectile, Color color, Vector2? position = null, float? rotation = null, Vector2? origin = null, float? scale = null, SpriteEffects? spriteEffects = null, Texture2D altTex = null)
		{
			Texture2D tex = altTex ?? TextureAssets.Projectile[projectile.type].Value;

			int frameHeight = tex.Height / Main.projFrames[projectile.type];
			Rectangle rect = new Rectangle(0, frameHeight * projectile.frame, tex.Width, frameHeight);

			Main.spriteBatch.Draw(
				tex,
				(position ?? projectile.Center) - Main.screenPosition,
				rect,
				color * ((255f - Math.Clamp(projectile.alpha, 0, 255)) / 255f),
				rotation ?? projectile.rotation,
				origin ?? (rect.Size() * 0.5f),
				scale ?? projectile.scale,
				spriteEffects ?? (projectile.spriteDirection == -1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None),
				0
				);
		}

		public static void EasyDrawAfterImage(this Projectile projectile, Color? color = null, Vector2[] oldPos = null, Vector2? origin = null, SpriteEffects? spriteEffects = null, Texture2D altTex = null)
		{
			Texture2D tex = altTex ?? TextureAssets.Projectile[projectile.type].Value;

			int frameHeight = tex.Height / Main.projFrames[projectile.type];
			Rectangle rect = new Rectangle(0, frameHeight * projectile.frame, tex.Width, frameHeight);

			Vector2[] positions = oldPos ?? projectile.oldPos;
			for (int i = 0; i < positions.Length; i++)
			{
				Vector2 position = positions[i];

				Main.spriteBatch.Draw(
					tex,
					position + (oldPos is null ? projectile.Size * 0.5f : Vector2.Zero) - Main.screenPosition,
					rect,
					(color ?? Color.White) * ((float)(positions.Length - (i + 1)) / positions.Length),
					projectile.rotation,
					origin ?? rect.Size() * 0.5f,
					projectile.scale,
					spriteEffects ?? (projectile.spriteDirection == -1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None),
					0
				);
			}
		}
	}
}