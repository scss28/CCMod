using System;
using Terraria;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System.Linq;
using Terraria.ID;

namespace CCMod.Utils
{
	static partial class CCModTool
	{
		/// <summary>Returns if there's any nearby npcs, friendly or non-friendly</summary>
		public static bool AnyNPCWithinRange(this Vector2 position, float distance)
		{
			return position.NPCsWithinRange(distance).Any();
		}

		// This function is kinda useless
		/// <summary>Returns the position of the closest npc to <paramref name="position"/></summary>
		/// <param name="position"></param>
		/// <param name="distance"></param>
		/// <returns>The closest npc position within the given <paramref name="distance"/> or <see cref="Vector2.Zero"/> if no npc is found</returns>
		public static Vector2 LookForClosestNPCPositionWithinRange(this Vector2 position, float distance)
		{
			if (!position.ClosestNPCWithinRange(out NPC npc, distance))
				return npc.position;
			return Vector2.Zero;
		}

		public static bool ClosestNPCWithinRange(this Vector2 position, out NPC closestNPC, float distance)
		{
			List<NPC> npcsWithinRange = position.NPCsWithinRange(distance);
			npcsWithinRange.Sort(
				(npc1, npc2) => npc1.Center.DistanceSQ(position).CompareTo(npc2.Center.DistanceSQ(position))
			);
			return (closestNPC = npcsWithinRange.FirstOrDefault()) is not null && closestNPC.type != NPCID.TargetDummy && !closestNPC.friendly;
		}

		/// <summary>Collects nearby npcs into a list.</summary>
		/// <param name="position"></param>
		/// <param name="npc">The list containing the npcs, the list is not ordered from closest to furthest.</param>
		/// <param name="distance">The max distance</param>
		public static List<NPC> NPCsWithinRange(this Vector2 position, float distance)
		{
			return Main.npc.Where(npc => npc.active && npc.Center.WithinRange(position, distance) && npc.type != NPCID.TargetDummy && !npc.friendly).ToList();
		}
	}
}
