using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CCMod.Utils
{
	static partial class CCModTool
	{
		/// <summary>
		/// This ensure that you can never heal over the limit, use it in hook that is only active when on hit a NPC
		/// </summary>
		/// <param name="PlayerWhoAmI">insert player.WhoAmI or owner.WhoAmI depend on the name of that Player class</param>
		/// <param name="TargetWhoAmI">insert target.WhoAmI or npc.WhoAmI depend on the name of that NPC class</param>
		/// <param name="HealStartAtAmount">The fixed amount to be heal, insert 10 and you will heal by 10</param>
		/// <param name="HealRNGrange">The is vary amount that can be range depend on the value you input, for example if you input 10 then it will vary from 0 to 10</param>
		/// <param name="HealMultiplier">This is a fixed multiplier, this multiplier occur after all the calculation with <paramref name="HealStartAtAmount"/> and <paramref name="HealRNGrange"/> completed</param>
		/// <param name="MultiplierRange">This is a vary multiplier that change how much a <paramref name="HealMultiplier"/> will multiply</param>
		public static void LifeStealOnHit(int PlayerWhoAmI, int TargetWhoAmI, int HealStartAtAmount, int HealRNGrange = 0, float HealMultiplier = 1, float MultiplierRange = 0)
		{
			if(Main.npc[TargetWhoAmI].lifeMax < 5 || Main.npc[TargetWhoAmI].type == NPCID.TargetDummy)
			{
				return;
			}
			if(HealRNGrange > 0)
			{
				HealRNGrange = Main.rand.Next(HealRNGrange + 1);
			}
			if(HealMultiplier > 1)
			{
				HealMultiplier += Main.rand.NextFloat(MultiplierRange);
			}
			int RealHealAmount = (int)((HealStartAtAmount + HealRNGrange) * HealMultiplier);
			if(RealHealAmount < 1)
			{
				return;
			}
			int CalculationPreHeal = Main.player[PlayerWhoAmI].statLife + RealHealAmount;
			if (CalculationPreHeal > Main.player[PlayerWhoAmI].statLifeMax2)
			{
				int leftOver = Main.player[PlayerWhoAmI].statLifeMax2 - Main.player[PlayerWhoAmI].statLife;
				Main.player[PlayerWhoAmI].Heal(leftOver);
			}
			else
			{
				Main.player[PlayerWhoAmI].Heal(RealHealAmount);
			}
		}
	}
}