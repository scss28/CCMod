using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ObjectData;

namespace CCMod.Common.ModSystems
{
    public class ChestItemSystem : ModSystem
    {
        public override void PostWorldGen()
        {
            List<ModItem> modChestItems = Mod.GetContent<ModItem>().Where(modItem => modItem is IChestItem).ToList();
            SpawnItemsInChests(modChestItems);
        }

        static void SpawnItemsInChests(List<ModItem> modChestItems)
        {
            foreach (Chest chest in Main.chest)
            {
                if (chest == null) continue;

                foreach (ModItem modItem in modChestItems)
                {
                    IChestItem chestItem = modItem as IChestItem;
                    Tile chestTile = Main.tile[chest.x, chest.y];
                    if (chestItem.ShouldSpawnChestItem && chestTile.TileType == chestItem.ChestTypeChestItem && TileObjectData.GetTileStyle(chestTile) == chestItem.ChestStyleChestItem)
                        SpawnItemInChest(chest, modItem.Type, chestItem.StackChestItem);
                }
            }
        }

        static void SpawnItemInChest(Chest chest, int itemType, int stack)
        {
            int itemStack = stack;
            int chestIndex = Array.FindIndex(chest.item, item => item.type == ItemID.None);

            if (chestIndex >= 0)
            {
                Item item = chest.item[chestIndex];

                item.SetDefaults(itemType);
                item.stack = Math.Clamp(itemStack, 1, item.maxStack);
            }
        }
    }

    public interface IChestItem
    {
        public int ChestTypeChestItem { get; }
        public int ChestStyleChestItem { get; }
        public int StackChestItem { get; }
        public bool ShouldSpawnChestItem { get; }
    }
}
