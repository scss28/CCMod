using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using CCMod.Utils;
using CCMod.Common;
using Terraria.DataStructures;
using Terraria.Audio;
using Terraria.GameContent.Creative;

namespace CCMod.Content.Items.Weapons.Magic

{
	public class MushroomJavilen : ModItem, IMadeBy
	{
		public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
		{
			Projectile.NewProjectile(source, position, velocity, ProjectileID.FlaironBubble, 40, 3, player.whoAmI);
			for (int i = 0; i < 4; i++)
			{
				Vector2 vec = velocity.NextVector2RotatedByRandom(20f, 30, i);
				Projectile.NewProjectile(source, position, vec, type, damage, knockback, player.whoAmI);
			}
			return base.Shoot(player, source, position, velocity, type, damage, knockback);
		}
		public string CodedBy => "Pexiltd";

		public string SpritedBy => "Pexiltd";

		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Shroomish Staff");
			// Tooltip.SetDefault("[Turns out the spear isnt so differnt from a staff.]);
			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
			Item.staff[Item.type] = true;
		}
		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(ItemID.MushroomSpear, 1);
			recipe.AddIngredient(ItemID.ManaCrystal, 1);
			recipe.AddTile(TileID.WorkBenches);
			recipe.Register();
		}
		public override void SetDefaults()
		{

			Terraria.Audio.SoundStyle item1 = SoundID.Item1;
			Item.UseSound = item1;
			Item.CCModItemSetDefaultMagic(9, 9, 20, 6, 14, 14, ItemUseStyleID.Shoot, ProjectileID.Mushroom, 60, 4, true);
			Item.CanRollPrefix(PrefixID.Mythical);
			Item.CanRollPrefix(PrefixID.Taboo);
			Item.CanRollPrefix(PrefixID.Celestial);
			Item.CanRollPrefix(PrefixID.Unhappy);
		}
	}
}

