using CCMod.Common;
using CCMod.Common.ModSystems;
using CCMod.Utils;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;

namespace CCMod.Content.Items.Weapons.Magic
{
	public class HexedSkyBlades : ModItem, IMadeBy, IChestItem
	{
		public string CodedBy => "sucss";
		public string SpritedBy => "person_";
		// public string ConceptBy => "person_"; ???

		public int ChestType => 21; // 21 is one of the chest types
		public int ChestStyle => 13; // 13 is the skyware chest style
		public int Stack => 1;
		public bool SpawnChance => Main.rand.NextBool(3); // 1/3 chance to spawn in the chest

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Hexed Skyblades");
			Tooltip.SetDefault("[c/6dc7d1:The blades deal heavy damage] [c/e84343:but can also hurt the player...]\n[c/f3fa4f:\"Do you believe in \'gravity\'?\"]");
			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
		}

		public override void SetDefaults()
		{
			Item.width = 46;
			Item.height = 46;

			Item.crit = 7;
			Item.damage = 18;
			Item.knockBack = 4f;
			Item.DamageType = DamageClass.Magic;

			Item.UseSound = SoundID.Item63;
			Item.useTime = 8;
			Item.useAnimation = Item.useTime;
			Item.autoReuse = true;

			Item.useStyle = ItemUseStyleID.Shoot;

			Item.noMelee = true;

			Item.value = Item.sellPrice(0, 0, 65, 0);
			Item.rare = ItemRarityID.Blue;

			Item.shoot = ModContent.ProjectileType<HexedSkyBladesProjectile>();
			Item.shootSpeed = 30f;

			Item.noUseGraphic = true;
		}

		static int direction = 1;
		public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
		{
			position += direction * velocity.RotatedBy(MathHelper.PiOver2).Normalized() * 65 - velocity.Normalized() * 35;
			direction = -direction;

			velocity = position.DirectionTo(Main.MouseWorld) * Item.shootSpeed;
		}
	}

	public class HexedSkyBladesProjectile : ModProjectile
	{
		public override string Texture => base.Texture.Replace("Projectile", string.Empty);

		public override void SetDefaults()
		{
			Projectile.width = 1;
			Projectile.height = 1;

			Projectile.aiStyle = -1;

			Projectile.DamageType = DamageClass.Magic;
			Projectile.penetrate = -1;

			Projectile.friendly = true;
			Projectile.hostile = false;

			Projectile.ignoreWater = true;
			Projectile.tileCollide = false;

			Projectile.netImportant = true;

			Projectile.timeLeft = 500;

			Projectile.usesLocalNPCImmunity = true;
			Projectile.localNPCHitCooldown = 999;
		}

		bool onSpawnStuff = true;
		public override void AI()
		{
			if (onSpawnStuff)
			{
				onSpawnStuff = false;

				Projectile.rotation = Projectile.velocity.ToRotation();
				Projectile.hostile = true;
				Projectile.netUpdate = true;

				CCModUtils.NewDustCircular(Projectile.Center, 10, DustID.SilverFlame, 16, minMaxSpeedFromCenter: (6, 6), dustAction: d => d.noGravity = true);
			}

			Projectile.velocity *= 0.87f;

			float lSQ = Projectile.velocity.LengthSquared();
			if (lSQ < 500f)
			{
				if ((Projectile.alpha += 4) >= 255)
					Projectile.Kill();
			}

			if (Projectile.timeLeft % 3 == 0)
			{
				Dust.NewDustDirect(Projectile.Center + Projectile.rotation.ToRotationVector2() * 32 * Main.rand.NextFloatDirection(), 0, 0, DustID.SilverFlame).noGravity = true;
			}
		}

		public override bool? CanHitNPC(NPC target)
		{
			return target.friendly ? false : null;
		}

		public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
		{
			Projectile.velocity *= 0.3f;
		}

		public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
		{
			Vector2 rotVector = Projectile.rotation.ToRotationVector2();
			float x = 0;
			return Collision.CheckAABBvLineCollision(targetHitbox.TopLeft(), targetHitbox.Size(), Projectile.Center - rotVector * 32, Projectile.Center + rotVector * 32, 10, ref x);
		}

		public override bool PreDraw(ref Color lightColor)
		{
			Projectile.EasyDraw(lightColor, rotation: Projectile.rotation + MathHelper.PiOver4);
			return false;
		}
	}
}