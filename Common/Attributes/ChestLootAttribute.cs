using System;
using System.Collections.Generic;
using System.Reflection;
using Terraria;
using Terraria.ModLoader;
using Terraria.ObjectData;

namespace CCMod.Common.Attributes
{
	[AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
	internal class ChestLootAttribute : Attribute
	{
		public (ChestTypeDescriptor chestType, int chanceDenominator, int minimumItemStack, int maximumItemStack) ChestLoot { get; }

		public ChestLootAttribute(ChestType chestType, int chanceDenominator, int minimumItemStack, int maximumItemStack)
		{
			ChestLoot = (ChestTypeDescriptor.FromChestType(chestType), chanceDenominator, minimumItemStack, maximumItemStack);
		}

		public ChestLootAttribute(ChestType chestType, int chanceDenominator = 1, int itemStack = 1)
			: this(chestType, chanceDenominator, itemStack, itemStack) { }
	}

	internal enum ChestType
	{
		WoodenChest,
		GoldChest,
		FrozenChest,
		LockedGoldChest,
		LockedShadowChest,
		IvyChest,
		LizhardChest,
		LivingWoodChest,
		MushroomChest,
		RichMahoganyChest,
		SandstoneChest,
		SkywareChest,
		WaterChest,
		WebCoveredChest,
		GraniteChest,
		MarbleChest,
		GoldenChest,
		LockedJungleChest,
		LockedCorruptionChest,
		LockedCrimsonChest,
		LockedHallowedChest,
		LockedIceChest,
		LockedDesertChest

	}

	internal record ChestTypeDescriptor(int TileType, int TileStyle)
	{
		// I know this is preety awfull but there is no other way for it to work with attributes.
		public static ChestTypeDescriptor FromChestType(ChestType chestType)
		{
			return chestType switch
			{
				ChestType.GoldChest => new(21, 1),
				ChestType.FrozenChest => new(21, 11),
				ChestType.LockedGoldChest => new(21, 2),
				ChestType.LockedShadowChest => new(21, 4),
				ChestType.IvyChest => new(21, 10),
				ChestType.LizhardChest => new(21, 16),
				ChestType.LivingWoodChest => new(21, 12),
				ChestType.MushroomChest => new(21, 32),
				ChestType.RichMahoganyChest => new(21, 8),
				ChestType.SandstoneChest => new(467, 11),
				ChestType.SkywareChest => new(21, 13),
				ChestType.WaterChest => new(21, 17),
				ChestType.WebCoveredChest => new(21, 15),
				ChestType.GraniteChest => new(21, 51),
				ChestType.MarbleChest => new(21, 52),
				ChestType.GoldenChest => new(21, 53),
				ChestType.LockedJungleChest => new(21, 23),
				ChestType.LockedCorruptionChest => new(21, 24),
				ChestType.LockedCrimsonChest => new(21, 25),
				ChestType.LockedHallowedChest => new(21, 26),
				ChestType.LockedIceChest => new(21, 27),
				ChestType.LockedDesertChest => new(467, 14),
				_ => new(21, 0),
			};
		}
	}

	internal class ChestLootSystem : ModSystem
	{
		public override void PostWorldGen()
		{
			foreach (ModItem modItem in Mod.GetContent<ModItem>())
			{
				IEnumerable<ChestLootAttribute> chestLootAttributes;
				if ((chestLootAttributes = modItem.GetType().GetCustomAttributes<ChestLootAttribute>()) is null)
				{
					continue;
				}

				foreach (ChestLootAttribute chestLootAttribute in chestLootAttributes)
				{
					TrySpawnItemInChest(
						modItem.Type,
						chestLootAttribute.ChestLoot.chestType,
						chestLootAttribute.ChestLoot.chanceDenominator,
						chestLootAttribute.ChestLoot.minimumItemStack,
						chestLootAttribute.ChestLoot.maximumItemStack
					);
				}
			}
		}

		public static void TrySpawnItemInChest(int itemType, ChestTypeDescriptor chestType, int chanceDenominator, int minimumItemStack, int maximumItemStack)
		{
			foreach (Chest chest in Main.chest)
			{
				if (chest is null)
				{
					continue;
				}

				Tile chestTile = Main.tile[chest.x, chest.y];
				if (chestTile.TileType != chestType.TileType || TileObjectData.GetTileStyle(chestTile) != chestType.TileStyle)
				{
					continue;
				}
					

				chanceDenominator = Math.Max(chanceDenominator, 1);
				if (!Main.rand.NextBool(chanceDenominator))
				{
                    continue;
                }

				int itemSpawnIndex = Array.FindIndex(chest.item, item => item.IsAir);
				if (itemSpawnIndex == -1)
				{
					continue;
				}

				Item item = chest.item[itemSpawnIndex];

				item.SetDefaults(itemType);
				item.stack = Math.Clamp(
					Main.rand.Next(minimumItemStack, maximumItemStack + 1),
					1,
					item.maxStack
				);
			}
		}
	}
}
