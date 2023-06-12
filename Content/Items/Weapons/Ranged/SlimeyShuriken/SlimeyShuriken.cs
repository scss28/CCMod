using CCMod.Common.Attributes;
using CCMod.Utils;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;

namespace CCMod.Content.Items.Weapons.Ranged.SlimeyShuriken
{
	[CodedBy("LowQualityTrash-Xinim")]
	[SpritedBy("PixelGaming")]
	[ConceptBy("Cohozuna Jr.", "LowQualityTrash-Xinim")]
	internal class SlimeyShuriken : ModItem
	{
		public override void SetStaticDefaults()
		{
			// Tooltip.SetDefault("The Blade Feels Like Putty");
			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 5;

		}

		public override void SetDefaults()
		{
			Item.width = 24;
			Item.height = 24;

			Item.useTime = 15;
			Item.useAnimation = 15;

			Item.damage = 19;
			Item.knockBack = .5f;
			Item.value = 15;

			Item.rare = ItemRarityID.Green;

			Item.shoot = ModContent.ProjectileType<SlimeyShurikenProjectile>();
			Item.shootSpeed = 17;

			Item.useStyle = ItemUseStyleID.Swing;
			Item.DamageType = DamageClass.Ranged;

			Item.noUseGraphic = true;
			Item.noMelee = true;
			Item.autoReuse = true;
			Item.consumable = true;
			Item.maxStack = 999;
		}

		public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
		{
			Projectile.NewProjectile(source, position, velocity, type, damage, knockback, player.whoAmI, default, player.direction);
			return false;
		}

		public override void AddRecipes()
		{
			CreateRecipe(50)
				.AddIngredient(ItemID.Gel, 10)
				.AddIngredient(ItemID.Shuriken)
				.AddTile(TileID.Solidifier)
				.Register();
		}
	}

	class SlimeyShurikenProjectile : ModProjectile
	{
		public override string Texture => "CCMod/Content/Items/Weapons/Ranged/SlimeyShuriken/SlimeyShuriken";

		public override void SetDefaults()
		{
			Projectile.width = 24;
			Projectile.height = 24;
			Projectile.penetrate = 3;
			Projectile.timeLeft = 175;
			Projectile.friendly = true;
			Projectile.tileCollide = true;
			Projectile.DamageType = DamageClass.Ranged;
			Projectile.usesLocalNPCImmunity = true;
			Projectile.localNPCHitCooldown = 175;
		}

		int TimerBeforeGravity = 0;
		bool hittile = false;
		public override void AI()
		{
			if (IsStickingToTarget)
			{
				Projectile.StickyAI(TargetWhoAmI);
			}
			else
			{
				TargetWhoAmI++;
				if (!hittile)
				{
					Projectile.rotation = MathHelper.ToRadians(TimerBeforeGravity * TimerBeforeGravity * .6f * Projectile.ai[1]);
					Projectile.velocity.Y += TimerBeforeGravity >= 10 && Projectile.velocity.Y <= 18 ? 1f : 0;
				}

				TimerBeforeGravity++;
			}
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

		public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
		{
			return CCModUtils.CollisionBetweenEnemyAndProjectile(projHitbox, targetHitbox);
		}

		public override bool OnTileCollide(Vector2 oldVelocity)
		{
			if (!hittile)
			{
				Projectile.position += Projectile.velocity;
			}

			hittile = true;
			Collision.HitTiles(Projectile.Center, Projectile.velocity, Projectile.width, Projectile.height);
			Projectile.velocity.X = 0;
			Projectile.velocity.Y = 0;
			return false;
		}

		public override Color? GetAlpha(Color lightColor)
		{
			return new Color(255, 255, 255, 175);
		}

		public override void DrawBehind(int index, List<int> behindNPCsAndTiles, List<int> behindNPCs, List<int> behindProjectiles, List<int> overPlayers, List<int> overWiresUI)
		{
			CCModUtils.DrawBehindNPCandOtherProj(IsStickingToTarget, TargetWhoAmI, index, behindNPCsAndTiles, behindNPCs, behindProjectiles);
		}

		public override void ModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers)
		{
			target.AddBuff(BuffID.Oiled, 120);
			Projectile.OnHitNPCwithProjectile(target, out bool IsStickingToTarget, out int TargetWhoAmI, false, false);
			this.IsStickingToTarget = IsStickingToTarget;
			this.TargetWhoAmI = TargetWhoAmI;
		}
	}
}