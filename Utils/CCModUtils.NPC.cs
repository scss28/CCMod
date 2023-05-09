using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CCMod.Utils
{
	partial class CCModUtils
	{
		/// <summary>
		/// Array segment skipping the last npc in <see cref="Main.npc"/>.<br/>
		/// Can be used in <see langword="foreach"/> statements.
		/// </summary>
		public static ArraySegment<NPC> NPCForeach => new(Main.npc, 0, Main.npc.Length - 1);

		public static void ForeachNPCInRange(Vector2 center, float range, Action<NPC> npcAction)
		{
			foreach (NPC npc in NPCForeach)
			{
				Vector2 dir = center.DirectionTo(npc.Center);
				if (Collision.CheckAABBvLineCollision(npc.TopLeft, npc.Size, center, center + dir * range))
				{
					npcAction.Invoke(npc);
				}
			}
		}
	}
}