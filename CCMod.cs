using CCMod.Common;
using CCMod.Content.Items.Weapons.Melee;
using System;
using System.IO;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CCMod
{
	public class CCMod : Mod
	{
		public enum MessageType
		{
			GenericBlackSwordPlayer
		}

		public static Mod Instance { get; private set; }
		public CCMod()
		{
			Instance = this;
		}

		public override void Load()
		{
			SoundManager.Load(Instance);
		}

		public override void Unload()
		{
			Instance = null;
		}

		public override void HandlePacket(BinaryReader reader, int whoAmI)
		{
			MessageType messageType = (MessageType)reader.ReadByte();

			switch (messageType)
			{
				case MessageType.GenericBlackSwordPlayer:
					byte playernumber = reader.ReadByte();
					GenericBlackSwordPlayer blackSwordPlayer = Main.player[playernumber].GetModPlayer<GenericBlackSwordPlayer>();
					blackSwordPlayer.HowDIDyouFigureThatOut = reader.ReadInt32();
					if (Main.netMode == NetmodeID.Server)
					{
						blackSwordPlayer.SyncPlayer(-1, whoAmI, false);
					}

					break;

			}
		}
	}
}