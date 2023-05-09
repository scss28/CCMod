using Terraria.ID;
using Terraria.ModLoader;
using Terraria;

namespace CCMod.Tool
{
	static partial class CCModTool
	{
		public static void CCModItemSetDefault(this Item item, int width, int height, int damage, float knockback, int useTime, int useAnimation, int useStyle, bool autoReuse)
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
		public static void CCModItemSetDefaultToConsume(this Item item, int width, int height)
		{
			item.width = width;
			item.height = height;
			item.useTime = 15;
			item.useAnimation = 15;
			item.useStyle = ItemUseStyleID.HoldUp;
			item.autoReuse = false;
			item.consumable = true;
		}
		public static void CCModItemSetSetDefaultSpear(Item item, int spearType, float shootSpeed)
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
		public static void CCModItemSetDefaultMelee(this Item item, int width, int height, int damage, float knockback, int useTime, int useAnimation, int useStyle, bool autoReuse)
		{
			CCModItemSetDefault(item, width, height, damage, knockback, useTime, useAnimation, useStyle, autoReuse);
			item.DamageType = DamageClass.Melee;
		}
		/// <summary>
		/// Use this to set required value for a range item
		/// </summary>
		public static void CCModItemSetDefaultRange(this Item item, int width, int height, int damage, float knockback, int useTime, int useAnimation, int useStyle, int shoot, float shootSpeed, bool autoReuse, int useAmmo = 0)
		{
			CCModItemSetDefault(item, width, height, damage, knockback, useTime, useAnimation, useStyle, autoReuse);
			item.shoot = shoot;
			item.shootSpeed = shootSpeed;
			item.useAmmo = useAmmo;
			item.noMelee = true;
			item.DamageType = DamageClass.Ranged;
		}
		/// <summary>
		/// Use this to set required value for a magic item
		/// </summary>
		public static void CCModItemSetDefaultMagic(this Item item, int width, int height, int damage, float knockback, int useTime, int useAnimation, int useStyle, int shoot, float shootSpeed, int manaCost, bool autoReuse)
		{
			CCModItemSetDefault(item, width, height, damage, knockback, useTime, useAnimation, useStyle, autoReuse);
			item.shoot = shoot;
			item.shootSpeed = shootSpeed;
			item.mana = manaCost;
			item.noMelee = true;
			item.DamageType = DamageClass.Magic;
		}
	}
}
