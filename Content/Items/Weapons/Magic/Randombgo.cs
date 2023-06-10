using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using CCMod.Utils;
using CCMod.Common;
using Terraria.DataStructures;

namespace CCMod.Content.Items.Weapons.Magic

{
	public class Randombsgo : ModItem, IMadeBy
	{
		public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
		{
			Projectile.NewProjectile(source, position, velocity, ProjectileID.GoldenShowerFriendly, 100, 0, player.whoAmI);
			Projectile.NewProjectile(source, position, velocity, ProjectileID.VampireKnife, 150, 0, player.whoAmI);
			Projectile.NewProjectile(source, position, velocity, ProjectileID.CursedFlameFriendly, 50, 0, player.whoAmI);
			Projectile.NewProjectile(source, position, velocity, ProjectileID.SandnadoFriendly, 100, 0, player.whoAmI);
			Projectile.NewProjectile(source, position, velocity, ProjectileID.IchorDart, 100, 0, player.whoAmI);
			Projectile.NewProjectile(source, position, velocity, ProjectileID.InfernoFriendlyBolt, 150, 5, player.whoAmI);
			Projectile.NewProjectile(source, position, velocity, ProjectileID.ZapinatorLaser, 200, 1, player.whoAmI);
			Projectile.NewProjectile(source, position, velocity, ProjectileID.UnholyTridentFriendly, 50, 3, player.whoAmI);
			Projectile.NewProjectile(source, position, velocity, ProjectileID.ShadowBeamFriendly, 75, 1, player.whoAmI);
			Projectile.NewProjectile(source, position, velocity, ProjectileID.LightBeam, 75, 1, player.whoAmI);
			for (int i = 0; i < 5; i++)
			{
				Vector2 vec = velocity.NextVector2RotatedByRandom(25f, 40, i);
				Projectile.NewProjectile(source, position, vec, type, damage, knockback, player.whoAmI);
			}
			return base.Shoot(player, source, position, velocity, type, damage, knockback);
		}
		public string CodedBy => "Pexiltd";

		public string SpritedBy => "Pexiltd";

		public override void SetStaticDefaults()
		{
			Main.RegisterItemAnimation(Item.type, new DrawAnimationVertical(15, 12));
			Item.staff[Item.type] = true;
		}
		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(ItemID.LunarBar, 1);
			recipe.AddIngredient(ItemID.SoulofFright, 15);
			recipe.AddIngredient(ItemID.SoulofSight, 15);
			recipe.AddIngredient(ItemID.BeetleHusk, 3);
			recipe.AddIngredient(ItemID.WoodBreastplate, 3);
			recipe.AddTile(TileID.MythrilAnvil);
			recipe.AddTile(TileID.AlchemyTable);
			recipe.Register();
		}
		public override void SetDefaults()
		{
			Item.SetDefaultMagic(9, 9, 25, 6, 13, 13, ItemUseStyleID.Shoot, ProjectileID.Mushroom, 40, 10, true);
		}
	}
}

