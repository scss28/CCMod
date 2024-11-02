using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using CCMod.Utils;
using Terraria.DataStructures;
using CCMod.Common;
using Terraria.GameContent.Creative;
using CCMod.Common.Attributes;

namespace CCMod.Content.Items.Weapons.Ranged
{
	[CodedBy("Pexiltd")]
	[SpritedBy("Pexiltd")]
	[ConceptBy("Pexiltd")]
	public class Gunnade : ModItem
	{

		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Gunnade");
			// Tooltip.SetDefault("[A piece from the mind of a demolitionist.]);
			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
		}
		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(ItemID.Boomstick, 1);
			recipe.AddIngredient(ItemID.StickyGrenade, 150);
			recipe.AddIngredient(ItemID.DemoniteBar, 15);
			recipe.AddTile(TileID.Anvils);
			recipe.Register();
		}
		public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
		{
			for (int i = 0; i < 3; i++)
			{
				Vector2 vec = velocity.EvenArchSpread(20f, 30, i);
				Projectile.NewProjectile(source, position, vec, type, damage, knockback, player.whoAmI);
			}
			return base.Shoot(player, source, position, velocity, type, damage, knockback);
		}
		public override void SetDefaults() 
		{
			Item.SetDefaultRanged(12, 18, 55, 5, 75, 75, ItemUseStyleID.Shoot, ProjectileID.StickyGrenade, 13, true);

			Terraria.Audio.SoundStyle item36 = SoundID.Item36;
			Item.UseSound = item36;
		}
	}
}
