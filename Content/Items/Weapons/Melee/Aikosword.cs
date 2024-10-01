using CCMod.Utils;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace CCMod.Content.Items.Weapons.Melee
{
	public class Aikosword : ModItem
	{
		public override void SetDefaults()
		{
			Item.width = Item.height = 84;
			Item.DefaultToSword(40, 35, 4, true);
			Item.useStyle = ItemUseStyleID.Thrust;
			Item.shoot = ModContent.ProjectileType<AikoswordProjectile>();
			Item.shootSpeed = 3;
			Item.noUseGraphic = true;
			Item.noMelee = true;
		}
		public override void AddRecipes()
		{
			CreateRecipe()
				.AddIngredient(ItemID.AshWoodSword)
				.AddIngredient(ItemID.CrimtaneBar, 12)
				.AddIngredient(ItemID.TissueSample, 10)
				.AddIngredient(ItemID.Bone, 10)
				.AddTile(TileID.DemonAltar)
				.Register();
		}
	}
	public class AikoswordProjectile : ModProjectile
	{
		public override string Texture => CCModTool.GetSameTextureAs<Aikosword>();
		public override void SetDefaults()
		{
			Projectile.width = Projectile.height = 84;
			Projectile.friendly = true;
			Projectile.tileCollide = false;
			Projectile.timeLeft = 999;
			Projectile.penetrate = -1;
			Projectile.usesIDStaticNPCImmunity = true;
			Projectile.idStaticNPCHitCooldown = 20;
		}
		protected virtual float HoldoutRangeMin => 20f;
		protected virtual float HoldoutRangeMax => 60f;
		public override bool PreAI()
		{
			Dust dust = Dust.NewDustDirect(Projectile.Center + Main.rand.NextVector2Circular(50, 50), 0, 0, DustID.GemRuby);
			dust.noGravity = true;
			dust.velocity = Vector2.Zero;
			Dust dust2 = Dust.NewDustDirect(Projectile.Center + Main.rand.NextVector2Circular(50, 50), 0, 0, DustID.Wraith);
			dust2.noGravity = true;
			dust2.velocity = Vector2.Zero;
			Player player = Main.player[Projectile.owner];
			AikoswordPlayer modplayer = player.GetModPlayer<AikoswordPlayer>();
			if (player.itemAnimation == player.itemAnimationMax)
			{
				if (modplayer.SwingType >= 3)
				{
					modplayer.SwingType = 1;
				}
				else
				{
					modplayer.SwingType = Math.Clamp(modplayer.SwingType + 1, 1, 3);
				}
			}
			return base.PreAI();
		}
		public override void AI()
		{
			Player player = Main.player[Projectile.owner];
			AikoswordPlayer modplayer = player.GetModPlayer<AikoswordPlayer>();
			if (modplayer.SwingType == 3)
			{
				int duration = player.itemAnimationMax;
				player.heldProj = Projectile.whoAmI;
				if (Projectile.timeLeft > duration)
				{
					Projectile.timeLeft = duration;
				}
				Projectile.velocity = Vector2.Normalize(Projectile.velocity);
				Projectile.rotation = Projectile.velocity.ToRotation() - MathHelper.PiOver2;
				float halfDuration = duration * 0.5f;
				float progress;

				if (Projectile.timeLeft < halfDuration)
				{
					progress = Projectile.timeLeft / halfDuration;
				}
				else
				{
					progress = (duration - Projectile.timeLeft) / halfDuration;
				}
				Vector2 vel = Vector2.SmoothStep(Projectile.velocity * HoldoutRangeMin, Projectile.velocity * HoldoutRangeMax, progress);
				Projectile.Center = player.MountedCenter + vel;
				Projectile.rotation += Projectile.spriteDirection == -1 ? MathHelper.PiOver4 : MathHelper.PiOver4 + MathHelper.PiOver2;
				return;
			}
			CCModUtils.ProjectileSwordSwingAI(Projectile, player, modplayer.SwingType, 145, 12);
		}
		public override void ModifyDamageHitbox(ref Rectangle hitbox)
		{
			Player player = Main.player[Projectile.owner];
			AikoswordPlayer modplayer = player.GetModPlayer<AikoswordPlayer>();
			if (modplayer.SwingType <= 2)
			{
				CCModUtils.ModifyProjectileDamageHitbox(ref hitbox, player, 84, 84, 30);
			}
		}
		public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
		{
			if (Projectile.ai[1] <= 0)
			{
				Projectile.ai[1] = 1;

				Player player = Main.player[Projectile.owner];
				for (int i = 0; i < 3; i++)
				{
					Vector2 vel = (Vector2.UnitX * -player.direction).NextVector2RotatedByRandom(20, 1, 1);
					Projectile.NewProjectile(Projectile.GetSource_FromThis(), player.Center.OffsetPosition(vel, 30f), vel, ModContent.ProjectileType<AikoswordSubProjectile>(), (int)(Projectile.damage * .55f), Projectile.knockBack * .4f, Projectile.owner);
				}
			}
		}
	}
	public class AikoswordSubProjectile : ModProjectile
	{
		public override string Texture => CCModTool.GetVanillaTexture<Item>(ItemID.GoldBroadsword);
		public override void SetStaticDefaults()
		{
			ProjectileID.Sets.TrailCacheLength[Type] = 50;
			ProjectileID.Sets.TrailingMode[Type] = 0;
		}
		public override void SetDefaults()
		{
			Projectile.width = Projectile.height = 32;
			Projectile.friendly = true;
			Projectile.tileCollide = false;
			Projectile.timeLeft = 1000;
			Projectile.penetrate = 1;
			Projectile.extraUpdates = 10;
		}
		public override void AI()
		{
			if (Main.rand.NextBool(5))
			{
				Dust dust = Dust.NewDustDirect(Projectile.Center + Main.rand.NextVector2Circular(22, 22), 0, 0, DustID.GemRuby);
				dust.noGravity = true;
				dust.velocity = Vector2.Zero;
				Dust dust2 = Dust.NewDustDirect(Projectile.Center + Main.rand.NextVector2Circular(22, 22), 0, 0, DustID.Wraith);
				dust2.noGravity = true;
				dust2.velocity = Vector2.Zero;
			}
			if (Projectile.velocity.IsVelocityLimitReached(2))
			{
				Projectile.velocity *= .995f;
			}
			Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver4;
			if (++Projectile.ai[0] <= 300)
			{
				return;
			}
			if (Projectile.Center.ClosestNPCWithinRange(out NPC target, 2500))
			{
				Projectile.timeLeft = 1000;
				if (++Projectile.ai[1] <= 250)
				{
					Projectile.velocity += (target.Center - Projectile.Center).SafeNormalize(Vector2.Zero) * 0.01f;
					Projectile.ai[2] = Projectile.velocity.Length();
				}
				else
				{
					Projectile.velocity = (target.Center - Projectile.Center).SafeNormalize(Vector2.Zero) * Projectile.ai[2];
				}
			}
		}
		public override bool PreDraw(ref Color lightColor)
		{
			Projectile.DrawTrailWithoutColorAdjustment(new Color(255, 0, 0, 30), .02f);
			Projectile.ProjectileDefaultDrawInfo(out Texture2D texture, out Vector2 origin);
			for (int k = 0; k < Projectile.oldPos.Length; k++)
			{
				Vector2 drawPos = Projectile.oldPos[k] - Main.screenPosition + origin + new Vector2(0f, Projectile.gfxOffY);
				Main.EntitySpriteDraw(texture, drawPos, null, new Color(0, 0, 0), Projectile.rotation, origin, (Projectile.scale - k * 0.02f) * .5f, SpriteEffects.None, 0);
			}
			return false;
		}
	}
	public class AikoswordPlayer : ModPlayer
	{
		public int SwingType = 0;
	}
}
