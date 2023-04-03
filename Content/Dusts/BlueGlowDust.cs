using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace CCMod.Content.Dusts
{
	public class BlueGlowDust : ModDust
	{
		public override Color? GetAlpha(Dust dust, Color lightColor)
		{
			return Color.White with { A = 0 };
		}

		public override void OnSpawn(Dust dust)
		{
			dust.noGravity = true;
			dust.noLight = false;
		}

		public override bool Update(Dust dust)
		{
			dust.velocity *= 0.95f;
			dust.position += dust.velocity;
			dust.rotation += dust.velocity.X * 0.15f;
			dust.scale *= 0.98f;
			if (dust.scale <= 0.2f)
				dust.active = false;
			return false;
		}
	}
}