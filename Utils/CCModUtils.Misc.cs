using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;

namespace CCMod.Utils
{
	public static partial class CCModTool
	{
		public class CColor
		{
			public Color Value = new Color();
			public CColor(Color color)
			{
				Value = color;
			}
			public CColor(int r, int g, int b)
			{
				Value = new Color(r, g, b);
			}
		}
		public class NPCHitModifiers
		{
			public NPC.HitModifiers Value = new NPC.HitModifiers();
			public NPCHitModifiers(NPC.HitModifiers modifiers)
			{
				Value = modifiers;
			}
		}
		public class PlayerHurtModifiers
		{
			public Player.HurtModifiers Value = new Player.HurtModifiers();
			public PlayerHurtModifiers(Player.HurtModifiers modifiers)
			{
				Value = modifiers;
			}
		}
	}
}
