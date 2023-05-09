using System;
using Terraria;
using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace CCMod.Tool
{
	static partial class CCModTool
	{
		public static bool IsAnyNPCIsNearWithinRange(this Vector2 position, float distance)
		{
			for (int i = 0; i < Main.maxNPCs; i++)
			{
				if (Main.npc[i].active)
				{
					if (CompareSquareFloatValue(position, Main.npc[i].Center, distance))
					{
						return true;
					}
				}
			}
			return false;
		}
		public static Vector2 LookForClosestNPCPositionWithinRange(this Vector2 position, float distance)
		{
			var vector2List = new List<Vector2>();
			var ListOfDistance = new List<float>();
			for (int i = 0; i < Main.maxNPCs; i++)
			{
				if (Main.npc[i].active && CompareSquareFloatValue(position, Main.npc[i].Center, distance))
				{
					vector2List.Add(Main.npc[i].Center);
					ListOfDistance.Add(Vector2.DistanceSquared(position, Main.npc[i].Center));
				}
			}
			if (vector2List.Count > 0)
			{
				float smallNum = FloatSmallestInList(ListOfDistance);
				//idk why but IndexOf always return 0 so we are searching manually
				for (int i = 0; i < ListOfDistance.Count; i++)
				{
					if (ListOfDistance[i] == smallNum)
						return vector2List[i];
				}
			}
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
			var npcList = new List<NPC>();
			var vector2List = new List<Vector2>();
			for (int i = 0; i < Main.maxNPCs; i++)
			{
				if (Main.npc[i].active && CompareSquareFloatValue(Main.npc[i].Center, position, distance))
				{
					npcList.Add(Main.npc[i]);
					vector2List.Add(position - Main.npc[i].position);
				}
			}
			if (npcList.Count > 0 || vector2List.Count > 0)
			{
				Vector2 closestPos = Vector2SmallestInList(vector2List);
				//idk why but IndexOf always return 0 so we are searching manually
				for (int i = 0; i < vector2List.Count; i++)
				{
					if (vector2List[i] == closestPos)
					{
						npc = npcList[i];
						return true;
					}
				}
			}
			npc = null;
			return false;
		}
		public static void LookForAllNPCWithinRange(this Vector2 position, out List<NPC> npc, float distance)
		{
			var localNPC = new List<NPC>();
			for (int i = 0; i < Main.maxNPCs; i++)
			{
				NPC npcLocal = Main.npc[i];
				if (npcLocal.active && CompareSquareFloatValue(npcLocal.Center, position, distance))
					localNPC.Add(npcLocal);
			}
			npc = localNPC;
		}
	}
}
