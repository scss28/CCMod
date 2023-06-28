using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CCMod.Content.Effects.Debuffs
{
	public class Exhausted2 : ModBuff
	{
		public override void SetStaticDefaults()
		{
			Main.debuff[Type] = true;  // Is it a debuff?
			Main.pvpBuff[Type] = true; // Players can give other players buffs, which are listed as pvpBuff
			Main.buffNoSave[Type] = true; // Causes this buff not to persist when exiting and rejoining the world
			BuffID.Sets.LongerExpertDebuff[Type] = true; // If this buff is a debuff, setting this to true will make this buff last twice as long on players in expert mode
		}

		// Allows you to make this buff give certain effects to the given player
		public override void Update(Player player, ref int buffIndex)
		{
			player.GetModPlayer<ExaustedPlayer2>().ExhaustedDebuff = true;
			player.slow = true;
		}
	}

	public class ExaustedPlayer2 : ModPlayer
	{

		public bool ExhaustedDebuff;

		public override void ResetEffects()
		{
			ExhaustedDebuff = false;
		}

		// Allows you to give the player a negative life regeneration based on its state (for example, the "On Fire!" debuff makes the player take damage-over-time)
		// This is typically done by setting player.lifeRegen to 0 if it is positive, setting player.lifeRegenTime to 0, and subtracting a number from player.lifeRegen
		// The player will take damage at a rate of half the number you subtract per second
		public override void UpdateBadLifeRegen()
		{
			if (ExhaustedDebuff)
				Player.lifeRegen -= 32;
		}
	}
}