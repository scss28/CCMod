using System;
using Terraria;
using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace CCMod.Utils
{
	static partial class CCModTool
	{
		/// <summary>Returns if there's any nearby npcs, friendly or non-friendly</summary>
		public static bool IsAnyNPCIsNearWithinRange(this Vector2 position, float distance)
		{
			for (int i = 0; i < Main.maxNPCs; i++)
			{
				if (Main.npc[i] is { active: true } npc && npc.WithinRange(position, distance))
				{
					return true;
				}
			}
			return false;
		}
		/// <summary>Returns the position of the closest npc to <paramref name="position"/></summary>
		/// <param name="position"></param>
		/// <param name="distance"></param>
		/// <returns>The closest npc position within the given <paramref name="distance"/> or <see cref="Vector2.Zero"/> if no npc is found</returns>
		public static Vector2 LookForClosestNPCPositionWithinRange(this Vector2 position, float distance)
		{
			if (!LookForClosestNPCWithinRange(position, out NPC npc, distance))
				return npc.position;
			return Vector2.Zero;
		}
		public static float FloatSmallestInList(List<float> flag)
		{
			List<float> finalflag = flag;
			for (int i = 0; i < flag.Count;)
			{
				float index = finalflag[i];
				for (int l = i + 1; l < flag.Count; ++l)
				{
					if (index > flag[l])
						index = flag[l];
				}
				return index;
			}
			return 0;
		}
		public static bool LookForClosestNPCWithinRange(this Vector2 position, out NPC npc, float distance)
		{
			npc = null;
			float currentFoundMinDistSQ = -1;
			for (int i = 0; i < Main.maxNPCs; i++)
			{
				if (Main.npc[i] is { active: true } a)
				{
					float distanceSQ = a.DistanceSQ(position);
					if((distanceSQ < distance && distanceSQ < currentFoundMinDistSQ) || currentFoundMinDistSQ == -1) // -1 means its the first npc found
					{
						currentFoundMinDistSQ = distanceSQ;
						npc = a;
					}
				}
			}
			return npc != null;
		}
		/// <summary>Collects nearby npcs into a list.</summary>
		/// <param name="position"></param>
		/// <param name="npc">The list containing the npcs, the list is not ordered from closest to furthest.</param>
		/// <param name="distance">The max distance</param>
		public static void LookForAllNPCWithinRange(this Vector2 position, out List<NPC> npc, float distance)
		{
			var localNPC = new List<NPC>();
			for (int i = 0; i < Main.maxNPCs; i++)
			{
				NPC npcLocal = Main.npc[i];
				if (npcLocal.active && npcLocal.Center.WithinRange(position, distance))
					localNPC.Add(npcLocal);
			}
			npc = localNPC;
		}
	}
}
