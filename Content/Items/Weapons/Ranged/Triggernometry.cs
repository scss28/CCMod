using CCMod.Common;
using CCMod.Common.Attributes;
using CCMod.Utils;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.IO;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.GameContent.Creative;
using Terraria.GameInput;
using Terraria.ID;
using Terraria.ModLoader;

namespace CCMod.Content.Items.Weapons.Ranged
{
	[CodedBy("sucss")]
	[SpritedBy("Pick")]
	[ConceptBy("Pick", "sucss")]
	public class Triggernometry : ModItem
	{
		public override void SetStaticDefaults()
		{
			// Tooltip.SetDefault($"[c/{Color.YellowGreen.Hex3()}:Redirects bullets with a ruler for bonus damage]");
			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
		}

		public override void SetDefaults()
		{
			Item.width = 46;
			Item.height = 46;

			Item.crit = 7;
			Item.damage = 14;
			Item.knockBack = 4f;
			Item.DamageType = DamageClass.Ranged;

			Item.UseSound = SoundID.Item40;
			Item.useTime = Item.useAnimation = 18;

			Item.autoReuse = true;

			Item.useStyle = ItemUseStyleID.Shoot;

			Item.noMelee = true;

			Item.value = Item.sellPrice(0, 0, 60, 0);
			Item.rare = ItemRarityID.Pink;

			Item.shoot = ModContent.ProjectileType<TriggernometryHeldProj>();
			Item.useAmmo = AmmoID.Bullet;
			Item.shootSpeed = 21f;
			Item.noUseGraphic = true;
		}

		public override bool CanUseItem(Player player)
		{
			return true;
		}

		public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
		{
			Projectile.NewProjectile(
				source,
				position,
				velocity,
				Item.shoot,
				damage,
				knockback,
				player.whoAmI,
				type
				);
			return false;
		}

		public override void HoldItem(Player player)
		{
			int rulerType = ModContent.ProjectileType<TriggernometryRuler>();
			if (player.ownedProjectileCounts[rulerType] <= 0)
			{
				Projectile.NewProjectile(
					player.GetSource_FromThis(),
					player.Center,
					Vector2.Zero,
					rulerType,
					0,
					0,
					player.whoAmI
					);
			}
		}

		public override void AddRecipes()
		{
			CreateRecipe()
				.AddIngredient(ItemID.IllegalGunParts)
				.AddIngredient(ItemID.Musket)
				.AddIngredient(ItemID.Ruler)
				.AddTile(TileID.Anvils)
				.Register();

			CreateRecipe()
				.AddIngredient(ItemID.IllegalGunParts)
				.AddIngredient(ItemID.FlintlockPistol)
				.AddIngredient(ItemID.Ruler)
				.AddTile(TileID.Anvils)
				.Register();
		}
	}

	public class TriggernometryHeldProj : ModProjectile
	{
		public override string Texture => base.Texture.Replace("HeldProj", string.Empty);
		Player Player => Main.player[Projectile.owner];

		public override void SetDefaults()
		{
			Projectile.width = 0;
			Projectile.height = 0;
			Projectile.aiStyle = -1;
			Projectile.penetrate = -1;
			Projectile.ignoreWater = true;
			Projectile.tileCollide = false;
			Projectile.friendly = false;
			Projectile.hostile = false;
			Projectile.timeLeft = 999;
		}

		public override void OnSpawn(IEntitySource source)
		{
			UpdateCenterRot();

			Vector2 muzzlePos = Projectile.Center + directionToMouse * 40;

			Projectile.NewProjectile(
				source,
				muzzlePos,
				directionToMouse * Projectile.velocity.Length(),
				(int)Projectile.ai[0],
				Projectile.damage,
				Projectile.knockBack,
				Player.whoAmI
				);
		}

		void UpdateCenterRot()
		{
			// Get player shoulder pos
			Projectile.Center = Player.RotatedRelativePoint(Player.MountedCenter) + new Vector2(-4 * Player.direction, -2);

			directionToMouse = Projectile.Center.DirectionTo(Main.MouseWorld);

			Projectile.Center += directionToMouse.RotatedBy(-MathHelper.PiOver2 * Player.direction) * 10;
			directionToMouse = Projectile.Center.DirectionTo(Main.MouseWorld);

			Projectile.netUpdate = true;
		}

		Vector2 directionToMouse;
		float recoil = -1;
		public override void AI()
		{
			if (Player.ItemAnimationEndingOrEnded || Player.HeldItem.type != ModContent.ItemType<Triggernometry>())
			{
				Projectile.Kill();
				return;
			}

			Player.heldProj = Projectile.whoAmI;

			if (Player.whoAmI == Main.myPlayer)
				UpdateCenterRot();

			if (recoil < 0)
			{
				Vector2 muzzlePos = Projectile.Center + directionToMouse * 40;

				CCModUtils.NewDustCircular(muzzlePos, 0.2f, d => Main.rand.NextFromList(DustID.TreasureSparkle, DustID.Smoke, DustID.YellowTorch), Main.rand.Next(7, 14), 0, (5, 8), d =>
				{
					d.noGravity = true;
					d.scale = Main.rand.NextFloat(0.2f, 1.4f);
					d.velocity = directionToMouse.RotatedByRandom(MathHelper.PiOver4 * 0.125f) * Main.rand.NextFloat(8, 25);
				});

				recoil += 10;
			}

			Projectile.rotation = directionToMouse.ToRotation() - recoil * 0.04f * Player.direction;
			Player.SetCompositeArmFront(true, Player.CompositeArmStretchAmount.Full, Projectile.rotation - MathHelper.PiOver2);

			recoil *= 0.85f;
		}

		public override void SendExtraAI(BinaryWriter writer)
		{
			writer.WriteVector2(directionToMouse);
		}

		public override void ReceiveExtraAI(BinaryReader reader)
		{
			directionToMouse = reader.ReadVector2();
		}

		public override bool ShouldUpdatePosition()
		{
			return false;
		}

		public override bool PreDraw(ref Color lightColor)
		{
			Vector2 origin = new Vector2(8 + recoil, 6);
			Texture2D tex = TextureAssets.Projectile[Type].Value;

			Main.spriteBatch.Draw(
				tex,
				Projectile.Center - Main.screenPosition,
				null,
				lightColor,
				Projectile.rotation + (Player.direction == -1 ? MathHelper.Pi : 0),
				Player.direction == -1 ? new Vector2(tex.Width - origin.X, origin.Y) : origin,
				Projectile.scale,
				Player.direction == 1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally,
				0
				);

			return false;
		}
	}

	public class TriggernometryRuler : ModProjectile, IDrawAdditive
	{
		public override void SetDefaults()
		{
			Projectile.width = 0;
			Projectile.height = 0;
			Projectile.aiStyle = -1;
			Projectile.penetrate = -1;
			Projectile.ignoreWater = true;
			Projectile.tileCollide = false;
			Projectile.friendly = false;
			Projectile.hostile = false;
			Projectile.timeLeft = 2;
			Projectile.extraUpdates = 10;
			Projectile.netImportant = true;
		}

		public override void OnSpawn(IEntitySource source)
		{
			Dir = 1;
		}

		Player Player => Main.player[Projectile.owner];
		ref float Dir => ref Projectile.ai[0];
		ref float alphaHit => ref Projectile.ai[1];
		bool spawnImpactDust;
		public override void AI()
		{
			if (Player.HeldItem.type == ModContent.ItemType<Triggernometry>())
				Projectile.timeLeft = 2;

			if (spawnImpactDust)
			{
				CCModUtils.NewDustCircular(Projectile.Center, 4, DustID.Pixie, Main.rand.Next(4, 7), 0, (2, 5), d =>
				{
					d.noGravity = true;
					d.scale = Main.rand.NextFloat(0.6f, 1.2f);
					d.velocity = -Projectile.rotation.ToRotationVector2().RotatedBy(MathHelper.PiOver4 * Dir).RotatedByRandom(MathHelper.Pi * 0.3) * Main.rand.NextFloat(3, 13);
				});

				spawnImpactDust = false;
			}

			if (Main.myPlayer == Player.whoAmI)
			{
				Vector2 dirToMouse = Player.Center.DirectionTo(Main.MouseWorld);

				Projectile.Center = Main.MouseWorld;
				Projectile.rotation = dirToMouse.ToRotation();
				Projectile.Center += new Vector2(2, 2 * Dir).RotatedBy(Projectile.rotation);

				if (PlayerInput.Triggers.JustPressed.MouseRight)
				{
					Dir *= -1;
				}

				alphaHit *= 0.997f;

				Projectile.netUpdate = true;
			}

			float len = 55;
			float thicc = 350;
			Vector2 dirRot = (Projectile.rotation + MathHelper.PiOver2).ToRotationVector2();
			Vector2 start = Projectile.Center + Projectile.rotation.ToRotationVector2() * thicc * 0.5f + dirRot * len * 0.5f;
			Vector2 end = start - dirRot * len;
			float x = 0;
			float sqToRuler = Projectile.Center.DistanceSQ(Player.Center);
			foreach (Projectile proj in Main.projectile)
			{
				if (proj.active &&
					proj.owner == Projectile.owner &&
					proj.TryGetGlobalProjectile(out RulerKingBullet rulerKingBullet) &&
					rulerKingBullet.RulerShot &&
					Collision.CheckAABBvLineCollision(proj.TopLeft + proj.velocity * proj.extraUpdates, proj.Size, start, end, thicc, ref x) /*(proj.Center + proj.velocity * (1 + proj.extraUpdates)).DistanceSQ(Player.Center) >= sqToRuler*/)
				{
					alphaHit = 5f;

					proj.velocity = (Projectile.rotation + -Dir * MathHelper.PiOver2).ToRotationVector2() * proj.velocity.Length();
					proj.Center = Projectile.Center;
					proj.damage = (int)(proj.damage * 1.75f);
					proj.netUpdate = true;
					proj.netUpdate2 = true;

					spawnImpactDust = true;
					Projectile.netUpdate2 = true;

					rulerKingBullet.RulerShot = false;
				}
			}
		}

		public override void SendExtraAI(BinaryWriter writer)
		{
			writer.Write(Projectile.rotation);
			writer.Write(spawnImpactDust);
		}

		public override void ReceiveExtraAI(BinaryReader reader)
		{
			Projectile.rotation = reader.ReadSingle();
			spawnImpactDust = reader.ReadBoolean();
		}

		public override bool PreDraw(ref Color lightColor)
		{
			Projectile.EasyDraw(Color.White * Math.Clamp(0.1f + alphaHit, 0f, 1f),
				rotation: Projectile.rotation + (Dir == -1 ? MathHelper.Pi : 0),
				spriteEffects: Dir == 1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally,
				origin: Dir == 1 ? new Vector2(46, 21) : new Vector2(10, 21)
				);
			return false;
		}

		public void DrawAdditive(Color lightColor)
		{

		}
	}

	public class RulerKingBullet : GlobalProjectile
	{
		public override bool InstancePerEntity => true;

		public bool RulerShot { get; set; }
		public override void OnSpawn(Projectile projectile, IEntitySource source)
		{
			if (source is EntitySource_ItemUse_WithAmmo itemSource && itemSource.Item.type == ModContent.ItemType<Triggernometry>())
				RulerShot = true;
		}
	}
}