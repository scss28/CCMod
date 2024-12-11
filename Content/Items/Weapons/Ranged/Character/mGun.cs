using Terraria;
using CCMod.Utils;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.DataStructures;
using Microsoft.Xna.Framework;

namespace CCMod.Content.Items.Weapons.Ranged.Character
{
	public class mGun : ModItem
	{
		int counter = 0;

		public override void SetDefaults()
		{
			Item.SetDefaultRanged(40, 22, 13, 3f, 5, 30, ItemUseStyleID.Shoot, ProjectileID.Bullet, 5f, true, AmmoID.Bullet);

			Item.reuseDelay = 10;
			Item.value = 10000;
			Item.rare = ItemRarityID.Blue;
			Item.UseSound = SoundID.Item11;
		}
		public override bool AllowPrefix(int pre)
		{
			return false;
		}

		public override Vector2? HoldoutOffset()
		{
			return new Vector2(6, 2);
		}
		public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
		{
			int[] CharacterType = [
			ModContent.ProjectileType<Bcharacter>(),
			ModContent.ProjectileType<Ucharacter>(),
			ModContent.ProjectileType<Lcharacter>(),
			ModContent.ProjectileType<Lcharacter>(),
			ModContent.ProjectileType<Echaracter>(),
			ModContent.ProjectileType<Tcharacter>() ];
			position = position.OffsetPosition(velocity, 40f);
			Vector2 speed = velocity.RotatedByRandom(MathHelper.ToRadians(4));
			type = player.direction == 1 ? CharacterType[5 - counter] : CharacterType[counter];

			position.Y -= 10;
			Projectile.NewProjectile(source, position, speed, type, damage, knockback, player.whoAmI);

			counter++;
			if (counter == 6)
			{
				counter = 0;
			}
			return false;
		}
		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(ItemID.Handgun, 3);
			recipe.AddIngredient(ItemID.HoneyBlock, 50);
			recipe.AddTile(TileID.Anvils);
			recipe.Register();
		}
	}
}
