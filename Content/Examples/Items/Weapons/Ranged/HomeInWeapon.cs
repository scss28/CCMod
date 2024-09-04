using CCMod.Common.Attributes;
using CCMod.Utils;
using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace CCMod.Content.Examples.Items.Weapons.Ranged
{
	/// <summary>
	/// This will teach you how to make a home in projectile
	/// </summary>
	[ExampleItem]
	[CodedBy("Xinim")]
	internal class HomeInWeapon : ModItem
	{
		public override string Texture => CCModTool.GetVanillaTexture<Item>(ItemID.Musket);
		public override void SetDefaults()
		{
			Item.SetDefaultRanged(1, 1, 30, 1, 30, 30, ItemUseStyleID.Shoot, ProjectileID.Bullet, 20, true, AmmoID.Bullet);
			Item.UseSound = SoundID.Item38;
		}
		public override bool AltFunctionUse(Player player)
		{
			return true;
		}
		public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
		{
			int number = 0;
			if (player.altFunctionUse == 2)
			{
				number = 1;
			}
			Projectile.NewProjectile(source, position.OffsetPosition(velocity, 50f), velocity, ModContent.ProjectileType<HomeInProjectile>(), damage, knockback, player.whoAmI, number);
			return false;
		}
		public override void AddRecipes()
		{
			CreateRecipe()
				.AddIngredient(ItemID.Musket, 1)
				.AddIngredient(ItemID.FallenStar, 10)
				.Register();
		}
	}
	//This is where we will be making our home in projectile
	public class HomeInProjectile : ModProjectile
	{
		public override string Texture => CCModTool.GetVanillaTexture<Item>(ItemID.WoodenArrow);
		public override void SetStaticDefaults()
		{
			ProjectileID.Sets.TrailCacheLength[Type] = 10;
			ProjectileID.Sets.TrailingMode[Type] = 3;
		}
		public override void SetDefaults()
		{
			Projectile.width = Projectile.height = 8;
			Projectile.friendly = true;
			Projectile.tileCollide = true;
			Projectile.timeLeft = 500;
			Projectile.penetrate = 1;
		}
		public override void AI()
		{
			Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver2 + MathHelper.Pi;
			//We use a already existing tool that find the nearest npc that isn't a dummy
			if (CCModTool.ClosestNPCWithinRange(Projectile.Center, out NPC npc, 600f))
			{
				//This is a standard way to do home in
				//You don't need to do the check if(Projectile.ai[0] == 0)
				//This is just to demonstrate the different between the standard one and the "advanded" one
				if (Projectile.ai[0] == 0)
				{
					//We set the velocity to be equal to the normalized distance between the projectile and npc
					//Remember to do it in order like this ( "position that it need to go" - "it current position ") and then we multiply by speed
					//I use safe normalized cause I like to play safe
					float speed = 20;
					Projectile.velocity = (npc.Center - Projectile.Center).SafeNormalize(Vector2.Zero) * speed;
					return;
				}
				//Below are the more "advanced" way
				//This way of making is subjective, there are better way or different way to do this but this is the way for me to go usually
				//I won't explain this way, it is best you figure out on your own
				if (Projectile.ai[0] == 1)
				{
					//Delay the home in by 10 tick
					if (Projectile.ai[1] < 10)
					{
						Projectile.ai[1]++;
						return;
					}
					Vector2 distance = npc.Center - Projectile.Center;
					float length = distance.Length();
					if (length > 5)
					{
						length = 5;
					}
					Projectile.velocity -= Projectile.velocity * .08f;
					Projectile.velocity += distance.SafeNormalize(Vector2.Zero) * length;
					Projectile.velocity = Projectile.velocity.LimitingVelocity(20);
				}
			}
		}
		public override bool PreDraw(ref Color lightColor)
		{
			//Easy draw trail, if you want it to scale correctly, do 1 / ProjectileID.Sets.TrailCacheLength[Type]
			Projectile.DrawTrail(lightColor, .1f);
			return base.PreDraw(ref lightColor);
		}
	}
}