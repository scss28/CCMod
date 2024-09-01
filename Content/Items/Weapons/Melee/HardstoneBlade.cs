using CCMod.Common;
using CCMod.Common.Attributes;
using CCMod.Utils;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;

namespace CCMod.Content.Items.Weapons.Melee
{
	[CodedBy("sucss")]
	[SpritedBy("person_", "AnUncreativeName")]
	public class HardstoneBlade : ModItem
	{
		public override void SetStaticDefaults()
		{
			// Tooltip.SetDefault("[c/4bad5e:Shoots stone chunks that have a tiny chance to drop emeralds on hit.]");
			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
		}

		public override void SetDefaults()
		{
			Item.width = 54;
			Item.height = 54;

			Item.crit = 7;
			Item.damage = 12;
			Item.knockBack = 4f;
			Item.DamageType = DamageClass.Melee;

			Item.UseSound = SoundID.Item1;
			Item.useTime = 25;
			Item.useAnimation = Item.useTime;
			Item.autoReuse = true;

			Item.useStyle = ItemUseStyleID.Swing;

			Item.noMelee = true;

			Item.value = Item.sellPrice(0, 0, 20, 0);
			Item.rare = ItemRarityID.Green;

			Item.shoot = ModContent.ProjectileType<HardstoneBladeHeldProj>();
			Item.shootSpeed = 0f;

			Item.noUseGraphic = true;
		}

		public override void AddRecipes()
		{
			CreateRecipe()
				.AddIngredient(ItemID.StoneBlock, 25)
				.AddIngredient(ItemID.Emerald, 4)
				.AddTile(TileID.Anvils)
				.Register();
		}
	}

	public class HardstoneBladeHeldProj : ModProjectile
	{
		public override string Texture => base.Texture.Replace("HeldProj", string.Empty);

		public override void SetDefaults()
		{
			Projectile.width = 0;
			Projectile.height = 0;

			Projectile.aiStyle = -1;

			Projectile.DamageType = DamageClass.Melee;

			Projectile.penetrate = -1;

			Projectile.friendly = true;
			Projectile.hostile = false;

			Projectile.ignoreWater = true;
			Projectile.tileCollide = false;

			Projectile.timeLeft = 9999;

			Projectile.extraUpdates = 2;

			Projectile.usesLocalNPCImmunity = true;
			Projectile.localNPCHitCooldown = 999;
		}

		Player Player => Main.player[Projectile.owner];
		ref float RotationToMouse => ref Projectile.ai[0];
		static int swingDirection = 1;
		public override void AI()
		{
			if (Player.ItemAnimationEndingOrEnded || Player.HeldItem.type != ModContent.ItemType<HardstoneBlade>())
			{
				Projectile.Kill();
				return;
			}

			Player.heldProj = Projectile.whoAmI;

			// Here we set the center around which the sword is gonna rotate and add some offset to it to set it where the Player's shoulder is.
			Projectile.Center = Player.RotatedRelativePoint(Player.MountedCenter) + new Vector2(Player.direction * -3, -1);

			// Now we want to get the rotation of direction to mouse but we only want this to happen for the Player holding this projectile.
			if (Main.myPlayer == Player.whoAmI)
			{
				RotationToMouse = Player.Center.DirectionTo(Main.MouseWorld).ToRotation();
				Projectile.netUpdate = true;
			}

			// Here using some math we calculate the current rotation of the projectile depending on the progress of the item animation (you can use desmos.com or any other graphing calculator to visualise how this is gonna behave).
			// ps. Player.itemAnimation goes down from Player.itemAnimationMax to 0 during item use.
			float arc = MathHelper.Pi * 1.5f;
			Projectile.rotation = RotationToMouse - Player.direction * swingDirection * (0.5f * arc - 0.25f * arc * MathF.Pow(MathF.Cos(MathHelper.Pi * Player.itemAnimation / Player.itemAnimationMax) + 1, 2));

			// Set player arm rotation.
			Player.SetCompositeArmFront(true, Player.CompositeArmStretchAmount.Full, Projectile.rotation - MathHelper.PiOver2);

			// Spawn projectiles
			if (Main.myPlayer == Player.whoAmI && Player.itemAnimation < Player.itemAnimationMax * 0.4f && Player.itemAnimation > Player.itemAnimationMax * 0.3f)
			{
				Projectile.NewProjectile(
					Projectile.GetSource_FromAI(),
					Projectile.Center + Projectile.rotation.ToRotationVector2() * (6 + swordLength * Main.rand.NextFloat()),
					Player.Center.DirectionTo(Main.MouseWorld) * 9 * Main.rand.NextFloat(0.85f, 1f),
					ModContent.ProjectileType<HardstoneBladeProjectile>(),
					(int)(Projectile.damage * 0.3f),
					0.5f,
					Projectile.owner
					);
			}
		}

		public override void OnKill(int timeLeft)
		{
			swingDirection = -swingDirection;
		}

		// Sword length in this case is around the lenght of the diagonal of the texture.
		const float swordLength = 82f;
		const float swordWidth = 12f;
		public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
		{
			Vector2 rotationVector = Projectile.rotation.ToRotationVector2();

			// Offset the start by a bit because of the drawOrigin.
			Vector2 collisionStart = Projectile.Center + rotationVector * 6;
			Vector2 collisionEnd = collisionStart + swordLength * rotationVector;

			// This needs to be like this if we want to specify lineWidth for some reason...
			float x = 0;
			return Collision.CheckAABBvLineCollision(targetHitbox.TopLeft(), targetHitbox.Size(), collisionStart, collisionEnd, swordWidth, ref x);
		}

		public override bool PreDraw(ref Color lightColor)
		{
			Texture2D texture = TextureAssets.Projectile[Type].Value;

			// Here we set draw origin to somewhere outside the texture so that Player "holds" the handle correctly.
			// You should experiment with this to get the best result.
			Vector2 drawOrigin = new Vector2(Player.direction == 1 ? -6 : 60, 60);

			// Here we get drawRotation by adding an offset based on where the blade is pointing and player's direction to the projectile's rotation.
			float drawRotation = Projectile.rotation + (Player.direction == 1 ? MathHelper.PiOver4 : MathHelper.Pi * 0.75f);

			// Draw the sword sprite.
			Main.spriteBatch.Draw(
				texture,
				Projectile.Center - Main.screenPosition,
				null,
				lightColor,
				drawRotation,
				drawOrigin,
				Projectile.scale,
				Player.direction == 1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally,
				0
				);

			return false;
		}
	}

	public class HardstoneBladeProjectile : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			ProjectileID.Sets.TrailCacheLength[Type] = 4;
			ProjectileID.Sets.TrailingMode[Type] = 2;
		}

		public override void SetDefaults()
		{
			Projectile.width = 20;
			Projectile.height = 20;

			Projectile.aiStyle = -1;

			Projectile.DamageType = DamageClass.Melee;

			Projectile.penetrate = 1;

			Projectile.friendly = true;
			Projectile.hostile = false;

			Projectile.ignoreWater = false;
			Projectile.tileCollide = true;

			Projectile.timeLeft = 500;
		}

		float rotationSpeed;
		public override void OnSpawn(IEntitySource source)
		{
			rotationSpeed = Main.rand.NextFloat(0.3f);
		}

		public override void AI()
		{
			Projectile.rotation += rotationSpeed;
			Projectile.velocity.Y += 0.16f;

			if (Main.rand.NextBool(3))
				Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, DustID.GemEmerald, Scale: 0.6f).noGravity = true;
		}

		public override void OnKill(int timeLeft)
		{
			for (int i = 0; i < 5; i++)
				Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.GreenMoss, -Projectile.velocity.X * 0.3f, -Projectile.velocity.Y * 0.3f);

			if (Main.rand.NextBool(300))
			{
				Item.NewItem(Projectile.GetSource_Death(), Projectile.Center, ItemID.Emerald);
			}

			SoundEngine.PlaySound(SoundID.Dig, Projectile.Center);
		}

		public override bool PreDraw(ref Color lightColor)
		{
			Projectile.EasyDrawAfterImage();
			Projectile.EasyDraw(lightColor);
			return false;
		}
	}
}