using Terraria.ID;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using Terraria.DataStructures;
using CCMod.Content.Items.Material;
using CCMod.Common.Attributes;
using CCMod.Utils;

namespace CCMod.Content.Items.Weapons.Ranged.BowOfLight
{
	//I port this from my old mod lol
	[CodedBy("LowQualityTrash-Xinim")]
	[SpritedBy("LowQualityTrash-Xinim")]
	[ConceptBy("LowQualityTrash-Xinim")]
	public class BowOfLight : ModItem
	{
		public override void SetDefaults()
		{
			Item.damage = 28;
			Item.crit = 15;
			Item.DamageType = DamageClass.Ranged;
			Item.width = 52;
			Item.height = 98;
			Item.useTime = Item.useAnimation = 25;
			Item.useStyle = ItemUseStyleID.Shoot;
			Item.noMelee = true;
			Item.knockBack = 2;
			Item.value = 10000;
			Item.rare = ItemRarityID.LightRed;
			Item.UseSound = SoundID.Item5;
			Item.autoReuse = true;
			Item.shoot = ProjectileID.WoodenArrowFriendly;
			Item.shootSpeed = 10f;
			Item.useAmmo = AmmoID.Arrow;
			Item.scale = .55f;
		}

		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(ModContent.ItemType<PureSilver>(), 4);
			recipe.AddIngredient(ItemID.Pearlwood, 60);
			recipe.AddIngredient(ItemID.Topaz, 15);
			recipe.AddTile(TileID.MythrilAnvil);
			recipe.Register();
		}

		public override bool AltFunctionUse(Player player) => true;

		public override bool CanUseItem(Player player)
		{
			if (player.altFunctionUse == 2)
			{
				Item.useTime = Item.useAnimation = 45;
			}
			else
			{
				Item.useTime = Item.useAnimation = 25;
			}
			return base.CanUseItem(player);
		}

		public override Vector2? HoldoutOffset()
		{
			return new();
		}

		public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
		{
			if (player.altFunctionUse == 2)
			{
				Projectile.NewProjectile(source, position, velocity, ProjectileID.DD2PhoenixBowShot, damage * 3, knockback, player.whoAmI);
				return false;
			}
			else
			{
				{
					int[] Arrow = { type, ProjectileID.CursedArrow, ProjectileID.FrostburnArrow, ProjectileID.FireArrow };
					float velocityLength = velocity.Length();
					float amount = Main.rand.Next(2,5);
					for (int i = 0; i < amount; i++)
					{
						Vector2 perturbedSpeed = velocity.RotatedByRandom(MathHelper.ToRadians(11)) * Main.rand.NextFloat(.75f, 1.25f);
						type = Main.rand.Next(Arrow);
						Projectile.NewProjectile(source, position + Main.rand.NextVector2Circular(15,15), perturbedSpeed, type, damage, knockback, player.whoAmI);
					}
				}
				return true;
			}
		}
	}
}
