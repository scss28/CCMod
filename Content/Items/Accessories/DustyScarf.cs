using CCMod.Common.Attributes;
using Terraria;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.ModLoader;

namespace CCMod.Content.Items.Accessories
{
	[CodedBy("Pexiltd")]
	[SpritedBy("Pexiltd")]
	[ConceptBy("Pexiltd")]
	internal class DustyScarf : ModItem
	{
		public override void UpdateEquip(Player player)
		{
			player.GetModPlayer<DustyScarfPlayer>().DustyScarf = true;
		}
		public class MyGlobalNPC : GlobalNPC
		{
			public override void ModifyNPCLoot(NPC npc, NPCLoot npcLoot)
			{
				// First, we need to check the npc.type to see if the code is running for the vanilla NPCwe want to change
				if (npc.type == NPCID.RustyArmoredBonesSwordNoArmor)
				{
					// This is where we add item drop rules for VampireBat, here is a simple example:
					npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<DustyScarf>(), 5, 1, 1));
				}
				// We can use other if statements here to adjust the drop rules of other vanilla NPC
			}
		}
		public override void SetDefaults()
		{
			Item.width = 20;
			Item.height = 20;
			Item.rare = ItemRarityID.Red;
			Item.accessory = true;
			Item.value = 1000;
		}
		public class DustyScarfPlayer : ModPlayer
		{
			public bool DustyScarf = false;
			public override void ResetEffects()
			{
				DustyScarf = false;
			}

		}
	
	}
}
