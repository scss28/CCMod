using Terraria;
using Terraria.ModLoader;
using CCMod.Utils;
using Terraria.ID;
using Microsoft.Xna.Framework;
using Terraria.DataStructures;
using CCMod.Common.GlobalProjectiles;
using CCMod.Common.Attributes;

namespace CCMod.Content.Examples.Projectiles
{
	internal class ExampleBouncyProjectile : ModProjectile, IBouncyProjectile
	{
		public override string Texture => CCModTool.GetVanillaTexture<Item>(ItemID.Acorn);
		public int BounceTime => 10;
		public float ChangeVelocityPerBounce => .8f;
		public override void SetDefaults()
		{
			Projectile.width = Projectile.height = 20;
			Projectile.penetrate = 1;
			Projectile.friendly = true;
			Projectile.timeLeft = 900;
		}

		int first = 0;
		public override void AI()
		{
			base.AI();
			if (first == 0)
			{
				Projectile.rotation = MathHelper.ToRadians(Main.rand.NextFloat(180f));
				first = 1;
			}
			Projectile.rotation += MathHelper.ToRadians(20);
			if (Projectile.ai[0] > 10)
			{
				if (Projectile.velocity.Y <= 16)
				{
					Projectile.velocity.Y += .15f;
				}
			}
			Projectile.ai[0]++;
		}
	}

	[ExampleItem]
	class ExampleShootBouncyProjectile : ModItem
	{
		public override string Texture => CCModTool.GetVanillaTexture<Item>(ItemID.Acorn);
		public override void SetDefaults()
		{
			Item.SetDefaultRanged(20, 20, 5, 1f, 27, 27, ItemUseStyleID.Swing, ModContent.ProjectileType<ExampleBouncyProjectile>(), 8f, true);
			Item.noUseGraphic = true;
			Item.consumable = true;
			Item.maxStack = 999;
		}
		public override void AddRecipes()
		{
			CreateRecipe()
				.AddIngredient(ItemID.Acorn)
				.AddTile(TileID.WorkBenches)
				.Register();
		}
	}
}