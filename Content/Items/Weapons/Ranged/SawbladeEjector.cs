using CCMod.Common.Attributes;
using CCMod.Common.ModPlayers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.IO;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.GameContent.Creative;
using Terraria.GameInput;
using Terraria.ID;
using Terraria.ModLoader;

namespace CCMod.Content.Items.Weapons.Ranged
{
	[CodedBy("sucss")]
	[SpritedBy("mayhemm")]
	[ChestLoot(ChestType.LockedGoldChest, 8)]
	public class SawbladeEjector : ModItem
	{

		public override void SetStaticDefaults()
		{
			/* Tooltip.SetDefault($"[c/{Color.Lerp(Color.Red, Color.Gray, 0.6f).Hex3()}:Hold LMB to charge up a sawblade]\n" +
				$"[c/{Color.Lerp(Color.Red, Color.LightSteelBlue, 0.6f).Hex3()}:Charge speed scales with attack speed]"); */
			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
		}

		public override void SetDefaults()
		{
			Item.width = 38;
			Item.height = 30;
			Item.damage = 4;
			Item.knockBack = 5.5f;
			Item.useTime = Item.useAnimation = 25;
			Item.DamageType = DamageClass.Ranged;
			Item.useStyle = ItemUseStyleID.Shoot;
			Item.noMelee = true;
			Item.autoReuse = true;
			Item.value = Item.buyPrice(gold: 1);
			Item.rare = ItemRarityID.Blue;
			Item.shoot = ModContent.ProjectileType<SawbladeEjectorHeldProj>();
			Item.shootSpeed = 9;
			Item.channel = true;
			Item.noUseGraphic = true;
		}

		public override bool CanUseItem(Player player)
		{
			return player.ownedProjectileCounts[Item.shoot] < 1;
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
				player.altFunctionUse
				);

			return false;
		}
	}

	public class SawbladeEjectorHeldProj : ModProjectile
	{
		Player Player => Main.player[Projectile.owner];

		public override void SetDefaults()
		{
			Projectile.width = 8;
			Projectile.height = 8;
			Projectile.aiStyle = -1;
			Projectile.penetrate = -1;
			Projectile.ignoreWater = false;
			Projectile.tileCollide = false;
			Projectile.friendly = false;
			Projectile.hostile = false;
			Projectile.timeLeft = 999;

			Projectile.extraUpdates = 2;
		}

		SawbladeEjectorProjectile SawProjectile;
		public override void OnSpawn(IEntitySource source)
		{
			SawProjectile = Projectile.NewProjectileDirect(
				source,
				Projectile.Center,
				Vector2.Zero,
				ModContent.ProjectileType<SawbladeEjectorProjectile>(),
				Projectile.damage,
				Projectile.knockBack,
				Player.whoAmI
				).ModProjectile as SawbladeEjectorProjectile;
		}

		void UpdateCenterRot()
		{
			// Get player shoulder pos
			Projectile.Center = Player.RotatedRelativePoint(Player.MountedCenter) + new Vector2(-4 * Player.direction, -2);

			directionToMouse = Projectile.Center.DirectionTo(Main.MouseWorld);

			Projectile.Center += directionToMouse.RotatedBy(-MathHelper.PiOver2 * Player.direction) * 8;
			directionToMouse = Projectile.Center.DirectionTo(Main.MouseWorld);
		}

		Vector2 directionToMouse;
		Vector2 recoil;
		public override void AI()
		{
			if (Player.HeldItem.type != ModContent.ItemType<SawbladeEjector>())
			{
				Projectile.Kill();
				return;
			}

			Player.heldProj = Projectile.whoAmI;

			if (Player.whoAmI == Main.myPlayer)
			{
				Player.direction = MathF.Sign(Main.MouseWorld.X - Player.Center.X);
				UpdateCenterRot();

				if (SawProjectile is not null)
				{
					Projectile.timeLeft = 60;
					if (PlayerInput.Triggers.Current.MouseLeft)
					{
						SawProjectile.Projectile.Center = Projectile.Center + directionToMouse * 50;

						if (SawProjectile.RotVelocity < SawbladeEjectorProjectile.MaxRot)
						{
							SawProjectile.RotVelocity += 0.0025f * Player.GetTotalAttackSpeed(DamageClass.Generic) * Player.GetTotalAttackSpeed(DamageClass.Ranged);
						}

						SawProjectile.Projectile.netUpdate = true;  //////////
						SawProjectile.Projectile.netUpdate2 = true; // I still have no idea what this netUpdate2 does but with it, it works for some reason
					}
					else
					{
						recoil += new Vector2(10, 0.25f);
						ShootSaw();
					}
				}

				Projectile.netUpdate = true;
			}

			Projectile.rotation = directionToMouse.ToRotation() + -recoil.Y * Player.direction;

			Player.SetCompositeArmFront(true, Player.CompositeArmStretchAmount.Full, Projectile.rotation - MathHelper.PiOver2);

			recoil *= 0.96f;
		}

		public override bool ShouldUpdatePosition()
		{
			return false;
		}

		private void ShootSaw()
		{
			SawProjectile.ShotOut = true;
			SawProjectile.Projectile.velocity = directionToMouse * MathHelper.Lerp(0.5f, 5, SawProjectile.RotVelocity / SawbladeEjectorProjectile.MaxRot);
			SawProjectile.Projectile.netUpdate = true;
			SawProjectile.Projectile.netUpdate2 = true;
			SawProjectile = null;

			SoundEngine.PlaySound(SoundID.Item102, Player.Center);
		}

		public override void OnKill(int timeLeft)
		{
			if (SawProjectile is not null)
			{
				ShootSaw();
			}
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

		public override bool PreDraw(ref Color lightColor)
		{
			Texture2D texture = TextureAssets.Projectile[Type].Value;
			var normOrigin = new Vector2(-1 + recoil.X, 12);

			Main.spriteBatch.Draw(
				texture,
				Projectile.Center - Main.screenPosition,
				null,
				lightColor,
				Projectile.rotation + (Player.direction == -1 ? MathHelper.Pi : 0),
				Player.direction == -1 ? new Vector2(texture.Width - normOrigin.X, normOrigin.Y) : normOrigin,
				Projectile.scale,
				Player.direction == 1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally,
				0
				);

			return false;
		}
	}

	public class SawbladeEjectorProjectile : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			Main.projFrames[Type] = 2;
		}

		public override void SetDefaults()
		{
			Projectile.width = 26;
			Projectile.height = 26;
			Projectile.aiStyle = -1;
			Projectile.penetrate = -1;
			Projectile.ignoreWater = false;
			Projectile.tileCollide = false;
			Projectile.friendly = true;
			Projectile.hostile = false;
			Projectile.timeLeft = 100; // this wont change shit, change time left in ai thing

			Projectile.extraUpdates = 4;

			Projectile.usesLocalNPCImmunity = true;
			Projectile.localNPCHitCooldown = 5;
		}

		public const float MaxRot = MathHelper.PiOver4 * 1.25f;

		Player Player => Main.player[Projectile.owner];
		public bool ShotOut { get; set; }
		public ref float RotVelocity => ref Projectile.ai[0];
		float rotVelDir;
		public override bool ShouldUpdatePosition()
		{
			return ShotOut || hitNPC is null;
		}

		public override void AI()
		{
			if (ShotOut)
			{
				Projectile.tileCollide = true;
				if (hitNPC is not null)
				{
					Projectile.Center = hitNPC.Center + hitNPCOffset;

					if (hitNPC.life <= 0 || !hitNPC.active)
					{
						hitNPC = null;
					}

					Projectile.netUpdate = true;
				}
				else
				{
					Projectile.velocity.Y += 0.01f;
				}
			}
			else
			{
				rotVelDir = RotVelocity * Player.direction;
				Projectile.timeLeft = 3600;
			}

			Projectile.localNPCHitCooldown = (int)MathHelper.Lerp(15, 120, (MaxRot - RotVelocity) / MaxRot);
			Projectile.rotation += rotVelDir / 5;

			Projectile.frame = RotVelocity >= MaxRot ? 1 : 0;
		}

		public override void ModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers)
		{
			if (!ShotOut)
			{
				modifiers.HitDirectionOverride = MathF.Sign(target.Center.X - Player.Center.X);
			}
		}

		public override void SendExtraAI(BinaryWriter writer)
		{
			writer.Write(ShotOut);
		}

		public override void ReceiveExtraAI(BinaryReader reader)
		{
			ShotOut = reader.ReadBoolean();
		}

		NPC hitNPC;
		Vector2 hitNPCOffset;
		public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
		{
			if (ShotOut && hitNPC is null)
			{
				hitNPC = target;
				hitNPCOffset = Projectile.Center - target.Center;
			}

			Player.GetModPlayer<ScreenShakePlayer>().ShakeScreen(1f, 0.85f);

			Vector2 dirToTarget = Projectile.Center.DirectionTo(target.Center);
			for (int i = 0; i < Main.rand.Next(2, 4); i++)
			{
				Vector2 dir = dirToTarget.RotatedByRandom(MathHelper.PiOver4);
				Vector2 position = Projectile.Center + dir * 12;
				dir = dir.RotatedBy(MathHelper.PiOver2 * MathF.Sign(rotVelDir));
				Dust.NewDustPerfect(position, DustID.Blood, dir * Main.rand.NextFloat(3, 6), Scale: Main.rand.NextFloat(0.85f, 1.75f));
			}
		}

		public override void OnKill(int timeLeft)
		{
			if (!Main.dedServ)
			{
				Vector2 dir = Main.rand.NextVector2Unit() * 7;
				for (int i = 0; i < 2; i++)
				{
					var gore = Gore.NewGoreDirect(Projectile.GetSource_Death(), Projectile.Center + dir, Vector2.Zero, Mod.Find<ModGore>(Name + $"_Gore{1 - i}").Type);
					gore.velocity = Projectile.velocity;
					gore.rotation = dir.ToRotation() + MathHelper.Pi * i;
					gore.position -= new Vector2(gore.Width, gore.Height) * 0.5f;
					dir = dir.RotatedBy(MathHelper.Pi);
				}

				for (int i = 0; i < Main.rand.Next(3, 5); i++)
				{
					Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.Asphalt, Projectile.velocity.X, Projectile.velocity.Y);
				}
			}
		}
	}
}