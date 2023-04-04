using System;
using System.Collections.Generic;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CCMod.Common.ModPlayers
{
	public class ScreenShakePlayer : ModPlayer
	{
		float screenShakeStrenght;
		float screenShakeDesolve;
		public void ShakeScreen(float strenght, float desolve = 0.95f)
		{
			screenShakeStrenght = strenght;
			screenShakeDesolve = Math.Clamp(desolve, 0, 0.9999f);
		}

		public override void ModifyScreenPosition()
		{
			if (screenShakeStrenght > 0.001f)
			{
				Main.screenPosition += screenShakeStrenght * Main.rand.NextVector2Unit();
				screenShakeStrenght *= screenShakeDesolve;
			}
		}
	}
}