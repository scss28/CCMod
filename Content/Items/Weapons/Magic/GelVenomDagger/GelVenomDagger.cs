using CCMod.Common.Attributes;
using CCMod.Utils;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;

namespace CCMod.Content.Items.Weapons.Magic.GelVenomDagger
{
	[CodedBy("LowQualityTrash-Xinim")]
	[SpritedBy("PixelGaming")]
	[ConceptBy("LowQualityTrash-Xinim", "PixelGaming")]
	internal class GelVenomDagger : ModItem
	{
		public override void SetStaticDefaults()
		{
			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
		}

		public override void SetDefaults()
		{
			Item.width = 12;
			Item.height = 32;

			Item.useTime = 14;
			Item.useAnimation = 14;

			Item.damage = 59;
			Item.knockBack = 3f;
			Item.crit = 8;
			Item.value = 4000;

			Item.rare = ItemRarityID.Pink;

			Item.shoot = ModContent.ProjectileType<GelVenomDaggerProjectile>();
			Item.shootSpeed = 23;

			Item.mana = 7;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.DamageType = DamageClass.Magic;

			Item.noUseGraphic = true;
			Item.noMelee = true;
			Item.autoReuse = true;
		}

		int ThrownCounter = 0;
		public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
		{
			int proj = Projectile.NewProjectile(source, position, velocity, type, damage, knockback, player.whoAmI);
			ThrownCounter++;
			if (ThrownCounter % 3 == 0)
			{
				Main.projectile[proj].penetrate = 2;
				Main.projectile[proj].ai[0] = 1;
			}

			if (ThrownCounter % 5 == 0)
			{
				Main.projectile[proj].penetrate = 5;
			}

			return false;
		}

		public override void AddRecipes()
		{
			CreateRecipe()
				.AddIngredient(ItemID.SoulofNight, 10)
				.AddIngredient(ItemID.SoulofSight, 10)
				.AddIngredient(ItemID.Gel, 200)
				.AddIngredient(ItemID.VialofVenom, 3)
				.AddIngredient(ItemID.MagicDagger)
				.AddIngredient(ItemID.ThrowingKnife, 100)
				.AddTile(TileID.MythrilAnvil)
				.Register();
		}
	}

	class GelVenomDaggerProjectile : ModProjectile
	{
		public override string Texture => "CCMod/Content/Items/Weapons/Magic/GelVenomDagger/GelVenomDagger";

		public override void SetDefaults()
		{
			Projectile.width = 20;
			Projectile.height = 20;
			Projectile.penetrate = 1;
			Projectile.timeLeft = 300;
			Projectile.friendly = true;
			Projectile.tileCollide = true;
			Projectile.DamageType = DamageClass.Magic;
			Projectile.scale = .75f;
			DrawOriginOffsetY -= 16;
			Projectile.usesLocalNPCImmunity = true;
			Projectile.localNPCHitCooldown = 20;
		}

		float ai1 = 0, ai2 = 0;
		// Are we sticking to a target?
		public bool IsStickingToTarget
		{
			get => ai1 == 1f;
			set => ai1 = value ? 1f : 0f;
		}

		// Index of the current target
		public int TargetWhoAmI
		{
			get => (int)ai2;
			set => ai2 = value;
		}

		int timerBeforeRotate = 0;
		public override void AI()
		{
			int dust = Dust.NewDust(Projectile.Center, 0, 0, DustID.UnholyWater, 0, 0, 0, default, Main.rand.NextFloat(.8f, 1.2f));
			Main.dust[dust].noGravity = true;
			if (Projectile.ai[0] == 1)
			{
				if (IsStickingToTarget)
				{
					Projectile.StickyAI(TargetWhoAmI);
					return;
				}
			}

			Projectile.alpha += 3;
			Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver2;
			Projectile.velocity.Y += timerBeforeRotate >= 10 && Projectile.velocity.Y <= 18 ? .75f : 0;
			timerBeforeRotate++;
		}

		public override void ModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers)
		{
			if (Projectile.ai[0] == 1)
			{
				Projectile.OnHitNPCwithProjectile(target, out bool IsStickingToTarget, out int TargetWhoAmI);
				this.IsStickingToTarget = IsStickingToTarget;
				this.TargetWhoAmI = TargetWhoAmI;
			}
		}

		public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
		{
			return CCModUtils.CollisionBetweenEnemyAndProjectile(projHitbox, targetHitbox);
		}

		public override void DrawBehind(int index, List<int> behindNPCsAndTiles, List<int> behindNPCs, List<int> behindProjectiles, List<int> overPlayers, List<int> overWiresUI)
		{
			if (Projectile.ai[0] == 1)
			{
				CCModUtils.DrawBehindNPCandOtherProj(IsStickingToTarget, TargetWhoAmI, index, behindNPCsAndTiles, behindNPCs, behindProjectiles);
			}
		}

		public override bool OnTileCollide(Vector2 oldVelocity)
		{
			Collision.TileCollision(Projectile.position, Projectile.velocity, Projectile.width, Projectile.height);
			if (Projectile.velocity.X != oldVelocity.X)
			{
				Projectile.velocity.X = -oldVelocity.X;
			}

			if (Projectile.velocity.Y != oldVelocity.Y)
			{
				Projectile.velocity.Y = -oldVelocity.Y;
			}

			return false;
		}

		public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
		{
			if (Main.rand.NextBool(7))
			{
				Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, Main.rand.NextVector2CircularEdge(-MathHelper.PiOver4, MathHelper.PiOver2) * 2, ModContent.ProjectileType<ToxicBubbleProjectile>(), Projectile.damage, 0, Projectile.owner);
			}
		}

		public override void OnKill(int timeLeft)
		{
			for (int i = 0; i < 10; i++)
			{
				int dust = Dust.NewDust(Projectile.Center, 0, 0, DustID.UnholyWater, 0, 0, 0, default, Main.rand.NextFloat(.8f, 1.2f));
				Main.dust[dust].noGravity = true;
				Main.dust[dust].velocity = Main.rand.NextVector2Circular(4, 4);
			}

			if (Main.rand.NextBool(7))
			{
				Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, Main.rand.NextVector2CircularEdge(-MathHelper.PiOver4, MathHelper.PiOver2) * 2, ModContent.ProjectileType<ToxicBubbleProjectile>(), Projectile.damage, 0, Projectile.owner);
			}
		}
	}

	public class ToxicBubbleProjectile : ModProjectile
	{
		const int MAX_TIME_LEFT = 300;
		public override void SetDefaults()
		{
			Projectile.alpha = 255;
			Projectile.scale = 0;

			Projectile.width = 32;
			Projectile.height = 32;
			Projectile.light = .2f;
			Projectile.timeLeft = MAX_TIME_LEFT;
			Projectile.friendly = true;
			Projectile.tileCollide = true;
			Projectile.DamageType = DamageClass.Magic;
		}

		public override void OnSpawn(IEntitySource source)
		{
			Projectile.velocity.Y = -3f;
			Projectile.velocity.X = Main.rand.NextFloat(-3f, 3f);
		}

		public override bool? CanDamage()
		{
			return false;
		}

		const int EXPLODING_TIME = 30;
		bool BonusProjectiles => Projectile.ai[0] == 1;
		public override void AI()
		{
			if (Main.rand.NextBool(24))
			{
				CCModUtils.NewDustCircular(
					Projectile.Center,
					Main.rand.Next(7, 19),
					i => Main.rand.NextFromList(DustID.UnholyWater, DustID.Water),
					Main.rand.Next(6),
					Main.rand.NextFloat(MathHelper.TwoPi),
					(4, 6)
					);
			}

			for (int i = 0; i < 2; i++)
			{
				int dust = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.UnholyWater, 0, 0, 0, default, Main.rand.NextFloat(.3f, 1f));
				Main.dust[dust].noGravity = false;
			}

			if (Projectile.timeLeft < MAX_TIME_LEFT - 30)
			{
				if (CCModTool.LookForProjectile(Projectile.Center, ModContent.ProjectileType<GelVenomDaggerProjectile>(), 20))
				{
					Projectile.ai[0] = 1;
					Projectile.Kill();
				}
			}

			if (Projectile.alpha > 120)
			{
				Projectile.alpha -= 10;
			}

			Projectile.alpha += (int)(MathF.Sin(Projectile.timeLeft * 0.15f) * 15);

			Projectile.scale = Projectile.timeLeft < EXPLODING_TIME ?
				1f + 0.5f * ((float)(EXPLODING_TIME - Projectile.timeLeft) / EXPLODING_TIME)
				: MathHelper.Lerp(Projectile.scale, 1f, 0.1f);

			Projectile.position.Y += MathF.Sin(Projectile.timeLeft * 0.025f) * 0.55f;

			Projectile.velocity.X *= 0.9f;
			Projectile.velocity.Y *= 0.98f;
		}

		public override void OnKill(int timeLeft)
		{
			CCModUtils.NewDustCircular(
				Projectile.Center,
				16,
				i => Main.rand.NextFromList(DustID.UnholyWater, DustID.Water),
				Main.rand.Next(15, 20),
				Main.rand.NextFloat(),
				(7, 12)
				);

			float projCount = BonusProjectiles ? 20 : 10;
			for (int i = 0; i < projCount; i++)
			{
				Vector2 direction = Main.rand.NextVector2Unit();
				Projectile.NewProjectile(
					Projectile.GetSource_FromThis(),
					Projectile.Center + direction * 15,
					direction * 3,
					ModContent.ProjectileType<ToxicDrop>(),
					(int)(Projectile.damage * .5f),
					0,
					Projectile.owner,
					Projectile.ai[0]
					);
			}
		}

		public override bool PreDraw(ref Color lightColor)
		{
			Projectile.EasyDraw(lightColor, Projectile.Center + 4f * MathF.Abs(Projectile.scale - 1f) * Main.rand.NextVector2Unit());
			return false;
		}
	}
	public class ToxicDrop : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			ProjectileID.Sets.TrailCacheLength[Projectile.type] = 40;
			ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
		}

		public override void SetDefaults()
		{
			Projectile.width = 2;
			Projectile.height = 2;
			Projectile.friendly = true;
			Projectile.tileCollide = true;
			Projectile.DamageType = DamageClass.Magic;
			Projectile.penetrate = 3;
			Projectile.extraUpdates = 6;

			Projectile.usesLocalNPCImmunity = true;
			Projectile.localNPCHitCooldown = 20;
		}

		int counter = 0;
		public override void AI()
		{
			if (counter >= 60)
			{
				Projectile.velocity.Y += .01f;
				Projectile.alpha += 1;
			}

			counter++;
		}

		public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
		{
			target.AddBuff(BuffID.Venom, Projectile.ai[0] == 1 ? 900 : 120);
		}

		public override Color? GetAlpha(Color lightColor)
		{
			return new Color(170, 20, 200, 200);
		}

		public override bool PreDraw(ref Color lightColor)
		{
			Projectile.DrawTrail(new Color(170, 20, 200, Projectile.alpha));
			return true;
		}
	}
}