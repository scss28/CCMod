using Terraria.ID;
using Terraria.ModLoader;
using Terraria;
using Microsoft.Xna.Framework;
using Terraria.DataStructures;
using CCMod.Common.GlobalItems;

namespace CCMod.Utils
{
	static partial class CCModTool
	{
		public static void Set_MeleeIFrame(this Item item, int iframe)
		{
			if (item.TryGetGlobalItem(out ImprovedSwingGlobalItem globalitem))
			{
				globalitem.CustomIFrame = iframe;
			}
		}
		public static void SetDefault(this Item item, int width, int height, int damage, float knockback, int useTime, int useAnimation, int useStyle, bool autoReuse)
		{
			item.width = width;
			item.height = height;
			item.damage = damage;
			item.knockBack = knockback;
			item.useTime = useTime;
			item.useAnimation = useAnimation;
			item.useStyle = useStyle;
			item.autoReuse = autoReuse;
		}
		/// <summary>
		/// Use this to set required value for a consumable item
		/// </summary>
		/// <param name="item"></param>
		/// <param name="width"></param>
		/// <param name="height"></param>
		public static void SetDefaultConsumable(this Item item, int width, int height)
		{
			item.width = width;
			item.height = height;
			item.useTime = 15;
			item.useAnimation = 15;
			item.useStyle = ItemUseStyleID.HoldUp;
			item.autoReuse = false;
			item.consumable = true;
		}
		/// <summary>
		/// 
		/// </summary>
		/// <param name="item">The item to sets the stats for</param>
		/// <param name="spearType">The type of projectile the spear uses.</param>
		/// <param name="shootSpeed"></param>
		public static void SetDefaultSpear(Item item, int spearType, float shootSpeed)
		{
			item.noUseGraphic = true;
			item.noMelee = true;
			item.shootSpeed = shootSpeed;
			item.shoot = spearType;
		}
		/// <summary>
		/// Use this to set required value for a melee item
		/// </summary>
		/// <param name="item"></param>
		/// <param name="width"></param>
		/// <param name="height"></param>
		/// <param name="damage"></param>
		/// <param name="knockback"></param>
		/// <param name="useTime"></param>
		/// <param name="useAnimation"></param>
		/// <param name="useStyle"></param>
		/// <param name="autoReuse"></param>
		public static void SetDefaultMelee(this Item item, int width, int height, int damage, float knockback, int useTime, int useAnimation, int useStyle, bool autoReuse)
		{
			SetDefault(item, width, height, damage, knockback, useTime, useAnimation, useStyle, autoReuse);
			item.DamageType = DamageClass.Melee;
		}
		/// <summary>
		/// Use this to set required value for a range item
		/// </summary>
		public static void SetDefaultRanged(this Item item, int width, int height, int damage, float knockback, int useTime, int useAnimation, int useStyle, int shoot, float shootSpeed, bool autoReuse, int useAmmo = 0)
		{
			SetDefault(item, width, height, damage, knockback, useTime, useAnimation, useStyle, autoReuse);
			item.shoot = shoot;
			item.shootSpeed = shootSpeed;
			item.useAmmo = useAmmo;
			item.noMelee = true;
			item.DamageType = DamageClass.Ranged;
		}
		/// <summary>
		/// Use this to set required value for a magic item
		/// </summary>
		public static void SetDefaultMagic(this Item item, int width, int height, int damage, float knockback, int useTime, int useAnimation, int useStyle, int shoot, float shootSpeed, int manaCost, bool autoReuse)
		{
			SetDefault(item, width, height, damage, knockback, useTime, useAnimation, useStyle, autoReuse);
			item.shoot = shoot;
			item.shootSpeed = shootSpeed;
			item.mana = manaCost;
			item.noMelee = true;
			item.DamageType = DamageClass.Magic;
		}
	}
}