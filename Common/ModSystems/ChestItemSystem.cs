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
    /// <summary>After worldgen, it fills chests with items that implement <see cref="IChestItem"/></summary>
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
                    if (chestItem.SpawnChance && chestTile.TileType == chestItem.ChestType && TileObjectData.GetTileStyle(chestTile) == chestItem.ChestStyle)
                        SpawnItemInChest(chest, modItem.Type, chestItem.Stack);
                }
            }
        }

        static bool SpawnItemInChest(Chest chest, int itemType, int stack)
        {
            int itemStack = stack;
            int chestIndex = Array.FindIndex(chest.item, item => item.IsAir);

            if (chestIndex >= 0)
            {
                Item item = chest.item[chestIndex];

                item.SetDefaults(itemType);
                item.stack = Math.Clamp(itemStack, 1, item.maxStack);
                return true;
            }
            return false;
        }
    }

    /// <summary>Represents an item that can spawn in chests after worldgen.<br />
    /// <see cref="ModItem"/>s can implement this interface.
    /// </summary>
    public interface IChestItem
    {
        /// <summary>The item's type</summary>
        public int ChestType { get; }
        public int ChestStyle { get; }
        public int Stack { get; }
        public bool SpawnChance { get; }
    }
}
