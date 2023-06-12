using CCMod.Common.Attributes;
using Terraria;
using Terraria.GameContent.Creative;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.ModLoader;

namespace CCMod.Content.Items.Consumable
{
	[CodedBy("sucss")]
	[SpritedBy("RockyStan")]
	[ConceptBy("RockyStan")]
	[ChestLoot(ChestType.GoldChest, 4)]
	public class GemPouch : ModItem
	{
		public override void SetStaticDefaults()
		{
			// Tooltip.SetDefault("Right click to open");
			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 4;
		}

		public override void SetDefaults()
		{
			Item.width = 25;
			Item.height = 25;
			Item.rare = ItemRarityID.Green;
			Item.maxStack = 99;
			Item.value = 3000;
		}

		public override bool CanRightClick()
		{
			return true;
		}
		public override void ModifyItemLoot(ItemLoot itemLoot)
		{
			/* dont ask .______.
            itemLoot.Add(
                ItemDropRule.AlwaysAtleastOneSuccess(
                    ItemDropRule.Common(ItemID.Emerald, 5, 1, 20),
                    ItemDropRule.Common(ItemID.Amber, 5, 1, 20),
                    ItemDropRule.Common(ItemID.Ruby, 5, 1, 20),
                    ItemDropRule.Common(ItemID.Sapphire, 5, 1, 20),
                    ItemDropRule.Common(ItemID.Diamond, 5, 1, 20),
                    ItemDropRule.Common(ItemID.Topaz, 5, 1, 20),
                    ItemDropRule.Common(ItemID.Amethyst, 5, 1, 20)
                    )
                );

            itemLoot.Add(
                ItemDropRule.AlwaysAtleastOneSuccess(
                    ItemDropRule.Common(ItemID.Emerald, 5, 1, 20),
                    ItemDropRule.Common(ItemID.Amber, 5, 1, 20),
                    ItemDropRule.Common(ItemID.Ruby, 5, 1, 20),
                    ItemDropRule.Common(ItemID.Sapphire, 5, 1, 20),
                    ItemDropRule.Common(ItemID.Diamond, 5, 1, 20),
                    ItemDropRule.Common(ItemID.Topaz, 5, 1, 20),
                    ItemDropRule.Common(ItemID.Amethyst, 5, 1, 20)
                    )
                );*/

			for (int i = 0; i < Main.rand.Next(5, 14); i++)
				itemLoot.Add(ItemDropRule.OneFromOptions(1, ItemID.Amethyst, ItemID.Topaz, ItemID.Diamond, ItemID.Ruby, ItemID.Amber, ItemID.Emerald));
			for (int i = 0; i < Main.rand.Next(5, 14); i++)
				itemLoot.Add(ItemDropRule.OneFromOptions(1, ItemID.Amethyst, ItemID.Topaz, ItemID.Diamond, ItemID.Ruby, ItemID.Amber, ItemID.Emerald));
		}
	}
}