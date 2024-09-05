using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.GameContent;
using Microsoft.Xna.Framework;

namespace CCMod.Utils
{
	static partial class CCModTool
	{
		/// <summary>
		/// This ensure that you can never heal over the limit, use it in hook that is only active when on hit a NPC<br/>
		/// The code make sure that you can't life steal using Dummy and if the NPC in question have less than 5 max life
		/// </summary>
		/// <param name="PlayerWhoAmI">insert player.WhoAmI or owner.WhoAmI depend on the name of that Player class</param>
		/// <param name="TargetWhoAmI">insert target.WhoAmI or npc.WhoAmI depend on the name of that NPC class</param>
		/// <param name="HealStartAtAmount">The fixed amount to be heal, insert 10 and you will heal by 10</param>
		/// <param name="HealRNGrange">The is vary amount that can be range depend on the value you input, for example if you input 10 then it will vary from 0 to 10</param>
		/// <param name="HealMultiplier">This is a fixed multiplier, this multiplier occur after all the calculation with <paramref name="HealStartAtAmount"/> and <paramref name="HealRNGrange"/> completed</param>
		/// <param name="MultiplierRangeMin">This is a vary multiplier that change how much <paramref name="HealMultiplier"/> will multiply, use with <paramref name="MultiplierRangeMax"/>,<br/>if <paramref name="MultiplierRangeMax"/> is 0 then it will use the value you assign to it</param>
		/// <param name="MultiplierRangeMax">This is a vary multiplier that change how much <paramref name="HealMultiplier"/> will multiply, use with <paramref name="MultiplierRangeMin"/>,<br/>it doesn't matter if you don't assign value to it either, use this just to make the code clear and easy to read</param>
		public static void LifeStealOnHit(int PlayerWhoAmI, int TargetWhoAmI, int HealStartAtAmount, int HealRNGrange = 0, float HealMultiplier = 1, float MultiplierRangeMin = 0, int MultiplierRangeMax = 0)
		{
			if (Main.npc[TargetWhoAmI].lifeMax < 5 || Main.npc[TargetWhoAmI].type == NPCID.TargetDummy)
			{
				return;
			}
			if (HealRNGrange > 0)
			{
				HealRNGrange = Main.rand.Next(HealRNGrange + 1);
			}
			if (HealMultiplier > 1)
			{
				(float minMulti, float maxMulti) order = CCModUtils.OrderFloat(MultiplierRangeMin, MultiplierRangeMax);
				HealMultiplier += Main.rand.NextFloat(order.minMulti, order.maxMulti);
			}
			int RealHealAmount = (int)((HealStartAtAmount + HealRNGrange) * HealMultiplier);
			if (RealHealAmount < 1)
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
		/// <summary>
		/// Spawn combat text above player without the random Y position
		/// </summary>
		/// <param name="location">use <see cref="Main.LocalPlayer.Hitbox"/> if you want this to be client side which you should</param>
		/// <param name="color"></param>
		/// <param name="combatMessage"></param>
		/// <param name="offsetposY"></param>
		/// <param name="dramatic"></param>
		/// <param name="dot"></param>
		public static void CombatTextRevamp(Rectangle location, Color color, string combatMessage, int offsetposY = 0, bool dramatic = false, bool dot = false)
		{
			int drama = 0;
			if (dramatic)
			{
				drama = 1;
			}
			int text = CombatText.NewText(new Rectangle(), color, combatMessage, dramatic, dot);
			if(text < 0 || text >= Main.maxCombatText)
			{
				return;
			}
			CombatText cbtext = Main.combatText[text];
			Vector2 vector = FontAssets.CombatText[drama].Value.MeasureString(cbtext.text);
			cbtext.position.X = location.X + location.Width * 0.5f - vector.X * 0.5f;
			cbtext.position.Y = location.Y + offsetposY + location.Height * 0.25f - vector.Y * 0.5f;
			cbtext.lifeTime += 30;
		}
	}
}