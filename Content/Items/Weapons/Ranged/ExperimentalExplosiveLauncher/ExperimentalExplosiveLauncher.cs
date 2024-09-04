using CCMod.Common;
using CCMod.Common.Attributes;
using CCMod.Common.ModPlayers;
using CCMod.Content.Dusts;
using CCMod.Utils;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using System;
using System.IO;
using System.Linq;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;

namespace CCMod.Content.Items.Weapons.Ranged.ExperimentalExplosiveLauncher
{
	[CodedBy("sucss")]
	[SpritedBy("mayhemm")]
	public class ExperimentalExplosiveLauncher : ModItem
	{
		public override void SetStaticDefaults()
		{
			// Tooltip.SetDefault($"[c/{Color.LightSteelBlue.Hex3()}:Spits out 5 grenades that blow up after a while]\n[c/{(Color.LightSteelBlue * 0.9f).Hex3()}:Use Right Click to shoot and detonate the explosives for higher damage]");
			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
		}

		public override void SetDefaults()
		{
			Item.width = 38;
			Item.height = 30;
			Item.damage = 54;
			Item.useTime = Item.useAnimation = 25;
			Item.DamageType = DamageClass.Ranged;
			Item.useStyle = ItemUseStyleID.Shoot;
			Item.noMelee = true;
			Item.autoReuse = true;
			Item.value = Item.buyPrice(gold: 1);
			Item.rare = ItemRarityID.Blue;
			Item.shoot = ModContent.ProjectileType<ExperimentalExplosiveLauncherHeldProj>();
			Item.shootSpeed = 0;
			Item.channel = true;
			Item.noUseGraphic = true;
		}

		public override bool AltFunctionUse(Player player)
		{
			return true;
		}

		public override bool CanUseItem(Player player)
		{
			if (player.altFunctionUse == 2)
			{
				Item.useTime = Item.useAnimation = 25;
			}
			else
			{
				Item.useTime = Item.useAnimation = 60;
			}

			return base.CanUseItem(player);
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
				Math.Clamp(player.altFunctionUse - 1, 0, 1) // so that its either 0 or 1
				);

			return false;
		}

		public override void AddRecipes()
		{
			CreateRecipe()
				.AddIngredient(ItemID.SoulofFright, 5)
				.AddIngredient(ItemID.HallowedBar, 12)
				.AddIngredient(ItemID.Wire, 25)
				.AddIngredient(ItemID.Grenade, 4)
				.AddIngredient(ItemID.IllegalGunParts)
				.AddTile(TileID.MythrilAnvil)
				.Register();
		}
	}

	public class ExperimentalExplosiveLauncherHeldProj : ModProjectile, IDrawAdditive
	{
		Player Player => Main.player[Projectile.owner];

		public override void SetDefaults()
		{
			Projectile.width = 8;
			Projectile.height = 8;
			Projectile.aiStyle = -1;
			Projectile.penetrate = -1;
			Projectile.ignoreWater = true;
			Projectile.tileCollide = false;
			Projectile.friendly = false;
			Projectile.hostile = false;
			Projectile.timeLeft = 999;
		}

		void UpdateCenterRot()
		{
			// Get player shoulder pos
			Projectile.Center = Player.RotatedRelativePoint(Player.MountedCenter) + new Vector2(-4 * Player.direction, -2);

			directionToMouse = Projectile.Center.DirectionTo(Main.MouseWorld);

			Projectile.Center += directionToMouse.RotatedBy(-MathHelper.PiOver2 * Player.direction) * (AttackType == 0 ? -4 : 7);
			directionToMouse = Projectile.Center.DirectionTo(Main.MouseWorld);
		}

		public override void OnSpawn(IEntitySource source)
		{
			if (AttackType == 1)
			{
				recoil += new Vector2(-8, -0.35f) * Player.direction;
				UpdateCenterRot();

				SoundEngine.PlaySound(SoundManager.Sounds["EEL_Shoot2"] with { Pitch = Main.rand.NextFloat(0, 0.4f) }, Projectile.Center);

				CCModUtils.NewDustCircular(MuzzlePosition, 3, d => Main.rand.NextFromList(DustID.Smoke, DustID.Shadowflame, ModContent.DustType<BlueGlowDust>()), 10, 0, (0.5f, 1), d =>
				{
					d.velocity += directionToMouse * 10;
					d.noGravity = true;
					d.scale = Main.rand.NextFloat(0.6f, 1f);
				});
				Projectile.NewProjectile(source, MuzzlePosition, directionToMouse * 6, ModContent.ProjectileType<EELBullet>(), Projectile.damage, Projectile.knockBack, Projectile.owner);

				if (!Main.dedServ)
					Lighting.AddLight(MuzzlePosition, TorchID.White);
			}
		}

		Vector2 MuzzlePosition => Projectile.Center + directionToMouse * (AttackType == 0 ? 52 : 42);

		ref float AttackType => ref Projectile.ai[0];
		Vector2 recoil;
		float Rotation => directionToMouse.ToRotation() + recoil.Y;
		Vector2 directionToMouse;

		int projectileTimer;
		public override void AI()
		{
			if (Player.ItemAnimationEndingOrEnded || Player.HeldItem.type != ModContent.ItemType<ExperimentalExplosiveLauncher>())
			{
				Projectile.Kill();
				return;
			}

			Player.heldProj = Projectile.whoAmI;

			if (Main.myPlayer == Player.whoAmI)
			{
				UpdateCenterRot();

				if (AttackType == 0)
				{
					if (projectileTimer++ > Player.itemAnimationMax * 0.15f)
					{
						projectileTimer = 0;

						recoil.X += -Player.direction * 7f;
						Projectile.NewProjectile(Projectile.GetSource_FromAI(), MuzzlePosition, directionToMouse.RotatedByRandom(0.25f) * Main.rand.NextFloat(10, 16), ModContent.ProjectileType<Guhnade>(), Projectile.damage, 5, Projectile.owner);

						SoundEngine.PlaySound(SoundManager.Sounds["EEL_Guh"], Projectile.Center);
					}
				}

				Projectile.netUpdate = true;
			}

			Projectile.rotation = Rotation;

			Player.SetCompositeArmFront(true, Player.CompositeArmStretchAmount.Full, Projectile.rotation - MathHelper.PiOver2);

			recoil *= 0.85f;
		}

		public override void SendExtraAI(BinaryWriter writer)
		{
			writer.WriteVector2(directionToMouse);
			writer.WriteVector2(recoil);
		}

		public override void ReceiveExtraAI(BinaryReader reader)
		{
			directionToMouse = reader.ReadVector2();
			recoil = reader.ReadVector2();
		}

		public override bool ShouldUpdatePosition()
		{
			return false;
		}

		float whiteAlpha;

		public override bool PreDraw(ref Color lightColor)
		{
			Texture2D texture = TextureAssets.Projectile[Type].Value;
			Rectangle rect = new Rectangle(0, 22 * (int)AttackType, 46, 22 + 4 * (int)AttackType);
			Vector2 normOrigin = (AttackType == 0 ? new Vector2(6, 11) : new Vector2(13, 6)) - Vector2.UnitX * (15 + recoil.X * Player.direction);

			Main.spriteBatch.Draw(
				texture,
				Projectile.Center - Main.screenPosition,
				rect,
				lightColor,
				Projectile.rotation + (Player.direction == -1 ? MathHelper.Pi : 0),
				Player.direction == -1 ? new Vector2(texture.Width - normOrigin.X, normOrigin.Y) : normOrigin,
				Projectile.scale,
				Player.direction == 1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally,
				0
				);

			Texture2D glowTex = ModContent.Request<Texture2D>(Texture + "_Glow", AssetRequestMode.ImmediateLoad).Value;
			Main.spriteBatch.Draw(
				glowTex,
				Projectile.Center - Main.screenPosition,
				rect,
				Color.White,
				Projectile.rotation + (Player.direction == -1 ? MathHelper.Pi : 0),
				Player.direction == -1 ? new Vector2(texture.Width - normOrigin.X, normOrigin.Y) : normOrigin,
				Projectile.scale,
				Player.direction == 1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally,
				0
				);

			Texture2D whiteTex = ModContent.Request<Texture2D>(Texture + "_White", AssetRequestMode.ImmediateLoad).Value;
			whiteAlpha = MathF.Abs(recoil.X) * 0.3f;
			Main.spriteBatch.Draw(
				whiteTex,
				Projectile.Center - Main.screenPosition,
				rect,
				Color.White * whiteAlpha,
				Projectile.rotation + (Player.direction == -1 ? MathHelper.Pi : 0),
				Player.direction == -1 ? new Vector2(texture.Width - normOrigin.X, normOrigin.Y) : normOrigin,
				Projectile.scale,
				Player.direction == 1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally,
				0
				);

			return false;
		}

		public void DrawAdditive(Color lightColor)
		{
			Texture2D bloomTex = ModContent.Request<Texture2D>("CCMod/Assets/FX/Glow2").Value;
			Main.spriteBatch.Draw(
				bloomTex,
				MuzzlePosition - Main.screenPosition,
				null,
				Color.White * whiteAlpha * 0.18f,
				0,
				bloomTex.Size() * 0.5f,
				whiteAlpha * 0.35f,
				SpriteEffects.None,
				0
				);
		}
	}

	public class EELBullet : ModProjectile, IDrawAdditive
	{
		public override void SetStaticDefaults()
		{
			ProjectileID.Sets.TrailCacheLength[Type] = 90;
			ProjectileID.Sets.TrailingMode[Type] = 2;
		}

		public override void SetDefaults()
		{
			Projectile.width = 7;
			Projectile.height = 7;
			Projectile.aiStyle = -1;
			Projectile.penetrate = -1;
			Projectile.friendly = true;
			Projectile.ignoreWater = true;
			Projectile.tileCollide = false;
			Projectile.timeLeft = 850;
			Projectile.extraUpdates = 13;
			Projectile.DamageType = DamageClass.Ranged;
		}

		public override void AI()
		{
			if (!Main.dedServ)
				Lighting.AddLight(Projectile.Center, 0.5f, 0.5f, 0.5f);

			if (Projectile.timeLeft % 30 == 0)
				Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, ModContent.DustType<BlueGlowDust>());

			Projectile.rotation = Projectile.velocity.ToRotation();

			int guhnadeType = ModContent.ProjectileType<Guhnade>();
			Projectile[] guhnadeProj = Main.projectile.Where(p => p.type == guhnadeType).ToArray();

			int setTimeLeft = 25;
			foreach (Projectile proj in guhnadeProj)
			{
				if (Projectile.Hitbox.Intersects(proj.Hitbox) && proj.timeLeft > setTimeLeft)
				{
					proj.timeLeft = setTimeLeft;
					proj.damage *= 2;

					Projectile closest = null;
					float closestDist = float.MaxValue;
					foreach (Projectile proj2 in guhnadeProj)
					{
						float dist = proj2.DistanceSQ(Projectile.Center);
						if (proj2.timeLeft > setTimeLeft && dist < closestDist)
						{
							closest = proj2;
							closestDist = dist;
						}
					}

					if (closest is not null)
					{
						float projSpeed = Projectile.velocity.Length();
						Projectile.velocity = Projectile.Center.DirectionTo(closest.Center) * projSpeed;
					}

					SoundEngine.PlaySound(SoundManager.Sounds["EEL_Bounce"] with { Pitch = Main.rand.NextFloat(0, 0.4f) });

					break;
				}
			}
		}

		public override bool PreDraw(ref Color lightColor)
		{
			return false;
		}

		public void DrawAdditive(Color lightColor)
		{
			float alpha = 0.85f;
			Projectile.EasyDrawAfterImage(Color.MediumPurple * alpha);
			Projectile.EasyDraw(Color.Purple * alpha);
		}
	}

	public class Guhnade : ModProjectile, IDrawAdditive
	{
		public override void SetStaticDefaults()
		{
			Main.projFrames[Type] = 2;

			ProjectileID.Sets.TrailCacheLength[Type] = 5;
			ProjectileID.Sets.TrailingMode[Type] = 2;
		}

		public override void SetDefaults()
		{
			Projectile.width = 22;
			Projectile.height = 22;
			Projectile.aiStyle = -1;
			Projectile.penetrate = -1;
			Projectile.friendly = false;
			Projectile.ignoreWater = true;
			Projectile.tileCollide = true;
			Projectile.timeLeft = 240;
			Projectile.DamageType = DamageClass.Ranged;

			Projectile.frame = Main.rand.Next(2);
		}

		int dir;
		public override void AI()
		{
			if (dir == 0)
				dir = MathF.Sign(Projectile.velocity.X);

			Projectile.velocity *= 0.96f;
			Projectile.rotation += Projectile.velocity.LengthSquared() * 0.02f * dir;

			if (Projectile.timeLeft < 180)
			{
				blinkProg *= 0.8f;
				/*
				if (--blinkTimer <= 0)
				{
					blinkProg = 1f;
					blinkDiff -= 2.96f;
					blinkTimer = blinkDiff;
				}*/

				if (++blinkTimer > Projectile.timeLeft / 7f)
				{
					blinkTimer = 0;
					blinkProg = 1;
				}

				Projectile.scale = 1 + 0.2f * (150 - Projectile.timeLeft) / 150f;
			}
		}

		public override bool OnTileCollide(Vector2 oldVelocity)
		{
			return false;
		}

		public override bool PreDraw(ref Color lightColor)
		{
			Projectile.EasyDrawAfterImage(Color.Black * 0.25f, altTex: ModContent.Request<Texture2D>(Texture + "_White").Value);

			Projectile.EasyDraw(lightColor);
			Projectile.EasyDraw(Color.White, altTex: ModContent.Request<Texture2D>(Texture + "_Glow").Value);

			return false;
		}

		public override void OnKill(int timeLeft)
		{
			SoundEngine.PlaySound(SoundManager.Sounds["EEL_GuhEX2"] with { Pitch = Main.rand.NextFloat(0, 0.4f) }, Projectile.Center);
			CCModUtils.ForeachNPCInRange(Projectile.Center, 130, npc =>
			{
				if (!npc.friendly && npc.active && npc.life > 0)
					Main.player[Projectile.owner].ApplyDamageToNPC(npc, Projectile.damage, Projectile.knockBack, MathF.Sign(Projectile.Center.X - npc.Center.X), Main.rand.NextBool(4));
			});
			CCModUtils.NewDustCircular(Projectile.Center, 50, d => Main.rand.NextFromList(DustID.Shadowflame, DustID.Smoke, DustID.Electric), Main.rand.Next(5, 9), Main.rand.NextFloat(), (6, 9), d => d.noGravity = true);

			Main.LocalPlayer.GetModPlayer<ScreenShakePlayer>().ShakeScreen(7, 0.75f);
		}

		float blinkProg = 0f;
		float blinkTimer = 30;
		public void DrawAdditive(Color lightColor)
		{
			Texture2D bloomTex = ModContent.Request<Texture2D>("CCMod/Assets/FX/Glow2").Value;
			Main.spriteBatch.Draw(
				bloomTex,
				Projectile.Center - Main.screenPosition,
				null,
				Projectile.timeLeft < 2 ? Color.White * 0.7f : Color.White * blinkProg * 0.25f,
				0,
				bloomTex.Size() * 0.5f,
				(Projectile.timeLeft < 3 ? new Vector2(Main.rand.NextFloat(1, 5), Main.rand.NextFloat(1, 5)) * Main.rand.NextFloat(0.75f, 1f) : blinkProg * 1.5f * Vector2.One) * Main.rand.NextFloat(0.75f, 1.75f),
				SpriteEffects.None,
				0
				);

			Projectile.EasyDraw(Color.White * blinkProg, altTex: ModContent.Request<Texture2D>(Texture + "_White").Value);
		}
	}
}