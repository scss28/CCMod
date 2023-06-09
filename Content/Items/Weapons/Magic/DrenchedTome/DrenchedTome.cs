using CCMod.Common.Attributes;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;

namespace CCMod.Content.Items.Weapons.Magic.DrenchedTome
{
	[CodedBy("LowQualityTrash-Xinim")]
	[SpritedBy("Kyoru")]
	internal class DrenchedTome : ModItem
	{
		public override void SetStaticDefaults()
		{
			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;

		}

		public override void SetDefaults()
		{
			Item.width = 39;
			Item.height = 44;

			Item.useTime = 10;
			Item.useAnimation = 10;

			Item.damage = 9;
			Item.knockBack = 1f;
			Item.crit = 2;
			Item.mana = 4;

			Item.DamageType = DamageClass.Magic;
			Item.value = Item.sellPrice(silver: 3);
			Item.useStyle = ItemUseStyleID.Shoot;
			Item.rare = ItemRarityID.Green;
			Item.autoReuse = true;
			Item.noMelee = true;

			Item.shoot = ModContent.ProjectileType<SlimeSpike>();
			Item.shootSpeed = 9;
		}
		public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
		{
			for (int i = 0; i < 3; i++)
			{
				Vector2 randomSpread = velocity.RotatedByRandom(MathHelper.ToRadians(15));
				Projectile.NewProjectile(source, position, randomSpread, type, damage, knockback, player.whoAmI);
			}

			return false;
		}
		public override void AddRecipes()
		{
			CreateRecipe()
				.AddIngredient(ItemID.Gel, 250)
				// .AddRecipeGroup("4thPHMBar", 10) Not sure what this is but the recipe group doesn't exist
				.AddIngredient(ItemID.Book)
				.AddTile(TileID.Solidifier)
				.Register();
		}
	}
	class SlimeSpike : ModProjectile
	{
		public override void SetDefaults()
		{
			Projectile.width = 10;
			Projectile.height = 10;
			Projectile.penetrate = 1;
			Projectile.tileCollide = true;
			Projectile.friendly = true;
			Projectile.DamageType = DamageClass.Magic;
		}
		public override void AI()
		{
			int dust = Dust.NewDust(Projectile.Center, 0, 0, DustID.t_Slime, 0, 0, 0, new Color(0, 153, 255, 125), Main.rand.NextFloat(.85f, 1));
			Main.dust[dust].noGravity = true;
			Main.dust[dust].fadeIn = 1;
			Projectile.rotation = Projectile.velocity.ToRotation();
			if (Projectile.velocity.Y <= 10)
			{
				Projectile.velocity.Y += .25f;
			}
		}
	}
}