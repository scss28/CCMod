using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.GameContent.ItemDropRules;
using Terraria.ModLoader;

namespace CCMod.Common.Attributes
{

	[AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
	internal class CommonNPCDropAttribute : Attribute
	{
		public CommonNPCDropInfo NPCDropInfo { get; }

		public CommonNPCDropAttribute(int npcType, int chanceDenominator = 1, int minimumDropped = 1, int maximumDropped = 1)
		{
			NPCDropInfo = new CommonNPCDropInfo(npcType, chanceDenominator, minimumDropped, maximumDropped);
		}
	}

	internal record CommonNPCDropInfo(int NPCType, int ChanceDenominator, int MinimumDropped, int MaximumDropped) { }

	internal record CommonItemDropInfo(int ItemType, int ChanceDenominator, int MinimumDropped, int MaximumDropped) { }

	internal class CommonNPCDropGlobalNPC : GlobalNPC
	{
		public override void Unload()
		{
			CommonItemDropInfos.Clear();
			CommonItemDropInfos = null;
		}

		private static Dictionary<int, List<CommonItemDropInfo>> CommonItemDropInfos { get; set; }

		public override void ModifyNPCLoot(NPC npc, NPCLoot npcLoot)
		{
			if (CommonItemDropInfos is null)
			{
				CommonItemDropInfos = new Dictionary<int, List<CommonItemDropInfo>>();
				foreach (ModItem modItem in Mod.GetContent<ModItem>())
				{
					foreach (CommonNPCDropInfo commonNPCDropInfo in modItem.GetType().GetCustomAttributes<CommonNPCDropAttribute>().Select(dropAttribute => dropAttribute.NPCDropInfo))
					{
						if (!CommonItemDropInfos.ContainsKey(commonNPCDropInfo.NPCType))
						{
							CommonItemDropInfos[commonNPCDropInfo.NPCType] = new List<CommonItemDropInfo>();
						}

						CommonItemDropInfos[commonNPCDropInfo.NPCType].Add(
							new CommonItemDropInfo(
								modItem.Type,
								commonNPCDropInfo.ChanceDenominator,
								commonNPCDropInfo.MinimumDropped,
								commonNPCDropInfo.MaximumDropped
							)
						);
					}
				}
			}

			if (CommonItemDropInfos.TryGetValue(npc.type, out List<CommonItemDropInfo> value))
			{
				foreach (CommonItemDropInfo commonItemDropInfo in value)
				{
					npcLoot.Add(
						ItemDropRule.Common(
							commonItemDropInfo.ItemType,
							commonItemDropInfo.ChanceDenominator,
							commonItemDropInfo.MinimumDropped,
							commonItemDropInfo.MaximumDropped
						)
					);
				}
			}
		}
	}
}
