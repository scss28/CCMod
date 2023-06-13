using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using CCMod.Utils;
using CCMod.Common;
using Terraria.GameContent.Creative;
using CCMod.Common.GlobalItems;

namespace CCMod.Content.Items.Weapons.Melee
{
	internal class GenericBlackStick : ModItem, MeleeWeaponWithImproveSwing, IMadeBy
	{
		public override void OnHitNPC(Player player, NPC target, NPC.HitInfo hit, int damageDone)
		{
			base.OnHitNPC(player, target, hit, damageDone);
			player.AddBuff(BuffID.ParryDamageBuff, 70);
			player.AddBuff(BuffID.ShadowDodge, 70);
			player.AddBuff(BuffID.BrokenArmor, 70);
			player.AddBuff(BuffID.Ichor, 70);
			player.AddBuff(BuffID.WitheredArmor, 70);
			player.AddBuff(BuffID.Bleeding, 120);
		}
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("knockback Stick");
			// Tooltip.SetDefault("[For some reason you cant say, This seems out of place]);
			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
		}
		public string CodedBy => "Pexiltd";

		public string SpritedBy => "Pexiltd";

		public override void SetDefaults()
		{
			Item.damage = 3;
			Item.DamageType = DamageClass.Melee;
			Item.width = 10;
			Item.height = 10;
			Item.useTime = 10;
			Item.CanRollPrefix(PrefixID.Legendary);
			Item.CanRollPrefix(PrefixID.Massive);
			Item.CanRollPrefix(PrefixID.Tiny);
			Item.CanRollPrefix(PrefixID.Awful);
			Item.useAnimation = 10;
			Item.useTurn = true;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.knockBack = 3;
			Item.value = 10000;
			Item.rare = ItemRarityID.Green;
			Terraria.Audio.SoundStyle item1 = SoundID.Item1;
			Item.UseSound = item1;
			Item.autoReuse = true;
			Item.crit = 30;
			Item.scale = 10.0f;

		}
		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddRecipeGroup(RecipeGroupID.Wood, 5);
			recipe.AddIngredient(ItemID.PlatinumBar, 10);
			recipe.AddIngredient(ItemID.Star, 5);
			recipe.AddIngredient(ItemID.GoldCrown, 1);
			recipe.AddTile(TileID.Anvils);
			recipe.AddTile(TileID.WorkBenches);
			recipe.Register();
		}
	}
}
