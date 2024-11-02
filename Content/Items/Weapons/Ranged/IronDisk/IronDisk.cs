using Terraria.ID;
using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Terraria.DataStructures;
using CCMod.Utils;
using CCMod.Common.Attributes;

namespace CCMod.Content.Items.Weapons.Ranged.IronDisk
{
	[CodedBy("LowQualityTrash-Xinim")]
	[SpritedBy("LowQualityTrash-Xinim")]
	[ConceptBy("LowQualityTrash-Xinim")]
	public class IronDisk : ModItem
	{
		//public override void SetStaticDefaults()
		//{
		//	Tooltip.SetDefault("Iron Disc\nAlt click to fire out 4 disks deal 25% of original damage");
		//}
		public override void SetDefaults()
		{
			Item.DamageType = DamageClass.Ranged;
			Item.autoReuse = true;
			Item.noMelee = true;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.shootSpeed = 12.5f;
			Item.shoot = ModContent.ProjectileType<IronDiskProjectile>();
			Item.damage = 14;
			Item.knockBack = 2.5f;
			Item.width = Item.height = 50;
			Item.UseSound = SoundID.Item1;
			Item.useAnimation = Item.useTime = 25;
			Item.noUseGraphic = true;
			Item.rare = ItemRarityID.Blue;
			Item.value = 500;
		}
		public override bool AltFunctionUse(Player player) => true;

		public override bool CanUseItem(Player player)
		{
			if (player.altFunctionUse == 2)
			{
				Item.useTime = Item.useAnimation = 50;
			}
			else
			{
				Item.useAnimation = Item.useTime = 25;
			}
			return base.CanUseItem(player);
		}

		public override void AddRecipes()
		{
			CreateRecipe()
				.AddIngredient(ItemID.IronBar, 7)
				.AddTile(TileID.HeavyWorkBench)
				.Register();
		}
		public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
		{
			if (player.altFunctionUse == 2)
			{
				float rotation = MathHelper.ToRadians(30);
				float numProjectile = 4;
				Vector2 posOffSet = position.OffsetPosition(velocity, 10f);
				for (int i = 0; i < numProjectile; i++)
				{
					Vector2 NewSpeed = velocity.RotatedBy(MathHelper.Lerp(-rotation, rotation, i / (numProjectile - 1)));
					Projectile.NewProjectile(source, position, NewSpeed, type, (int)(damage * .55f), knockback, player.whoAmI);
				}
			}
			else
			{
				Projectile.NewProjectile(source, position, velocity, type, damage, knockback, player.whoAmI);
			}
			return false;
		}
	}
}
