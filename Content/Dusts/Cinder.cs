using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Graphics.Effects;
using Terraria.Graphics.Shaders;
using Terraria.ModLoader;

namespace CCMod.Content.Dusts
{
	public class Cinder : ModDust
	{
		public override void Load()
		{
			var screenRef = new Ref<Effect>(Mod.Assets.Request<Effect>("Assets/Effects/GlowingDust", ReLogic.Content.AssetRequestMode.ImmediateLoad).Value);
			Filters.Scene["GlowingDust"] = new Filter(new ScreenShaderData(screenRef, "GlowingDustPass"), EffectPriority.High);
			Filters.Scene["GlowingDust"].Load();
		}

		public override Color? GetAlpha(Dust dust, Color lightColor)
		{
			Color newColor = dust.color;
			newColor.A = 128;
			dust.color = newColor;
			return dust.color;
		}

		public override void OnSpawn(Dust dust)
		{
			dust.noGravity = true;
			dust.scale *= 0.38f;
			dust.frame = new Rectangle(0, 0, 160, 160);
			dust.fadeIn = 0;
			dust.shader = new Terraria.Graphics.Shaders.ArmorShaderData(new Ref<Effect>(Mod.Assets.Request<Effect>("Assets/Effects/GlowingDust", ReLogic.Content.AssetRequestMode.ImmediateLoad).Value), "GlowingDustPass");
			dust.noLightEmittence = false;
		}

		public override bool Update(Dust dust)
		{
			if (!dust.noLightEmittence)
			{
				dust.position -= Vector2.One * 80 * dust.scale;
				dust.noLightEmittence = true;
				dust.velocity.Y *= 1.1f;
			}

			if (dust.customData is Vector2 target)
			{
				Vector2 pos = dust.position + Vector2.One * 80 * dust.scale;
				dust.velocity = Vector2.Lerp(dust.velocity, pos.DirectionTo(target) * pos.Distance(target) * 0.1f, 0.03f);
				if (pos.Distance(target) < 3f)
				{
					dust.active = false;
				}
			}

			if (dust.customData is Player player && dust.fadeIn > 0)
			{
				dust.position += player.velocity;
			}

			dust.shader.UseColor(dust.color * (1 - dust.fadeIn / 100f));

			dust.fadeIn += 3;
			if (dust.fadeIn > 100)
			{
				dust.active = false;
			}

			if (dust.noGravity)
			{
				if (Main.rand.NextBool())
				{
					dust.velocity += Main.rand.NextVector2Circular(0.2f, 0.05f);
				}

				dust.velocity.Y -= 0.01f;

				if (dust.fadeIn > 20)
				{
					dust.velocity *= 0.98f;
				}
			}
			else if (dust.fadeIn > 10)
			{
				dust.velocity.Y += 0.25f;
				if (Collision.SolidTiles(dust.position + new Vector2(0, 22), 8, 8))
				{
					dust.velocity = Vector2.Zero;
				}
			}

			dust.velocity = dust.velocity.RotatedBy(0.002f * dust.alpha * dust.velocity.Length());

			dust.position += dust.velocity;

			return false;
		}
	}
}