using CCMod.Common;
using CCMod.Common.Attributes;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace CCMod.Content.Items.Weapons.Melee
{
	[CodedBy("Jon Arbuckle")]
	[SpritedBy("Garfield")]
	[CommonNPCDrop(NPCID.WallofFlesh, 3)]
	internal class HotRoller : ModItem
	{
		public override void SetDefaults()
		{
			Item.DamageType = DamageClass.Melee;
			Item.damage = 62;
			Item.useTime = 30;
			Item.useAnimation = 30;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.noMelee = true;
			Item.noUseGraphic = true;
			Item.rare = ItemRarityID.Orange;
		}

		public override bool CanUseItem(Player player)
		{
			return player.ownedProjectileCounts[ModContent.ProjectileType<HotRollerProjectile>()] <= 0;
		}

		public override void AddRecipes()
		{

		}

		public override bool? UseItem(Player player)
		{
			Projectile.NewProjectile(Projectile.GetSource_None(), player.Center, Vector2.Zero, ModContent.ProjectileType<HotRollerProjectile>(), Item.damage, Item.knockBack, player.whoAmI);

			SoundEngine.PlaySound(SoundID.Item116, player.Center);

			return true;
		}
	}

	internal class HotRollerProjectile : ModProjectile
	{
		public Player Owner => Main.player[Projectile.owner];

		public float Timer => 60 - Projectile.timeLeft;

		public override void SetStaticDefaults()
		{
			ProjectileID.Sets.TrailCacheLength[Projectile.type] = 20;
			ProjectileID.Sets.TrailingMode[Projectile.type] = 2;
		}

		public override void SetDefaults()
		{
			Projectile.friendly = true;
			Projectile.width = 120;
			Projectile.height = 120;
			Projectile.timeLeft = 60;
			Projectile.tileCollide = false;
			Projectile.penetrate = -1;
		}

		public override void AI()
		{
			Owner.heldProj = Projectile.whoAmI;

			Projectile.Center = Owner.Center + Vector2.UnitY * Owner.gfxOffY; //stick to the player

			Projectile.rotation -= -0.2f * Owner.direction;

			if (Timer < 10)
			{
				Owner.velocity += Vector2.Normalize(Main.MouseWorld - Owner.Center) * 3;

				if (Owner.velocity.Length() > 20)
				{
					Owner.velocity = Vector2.Normalize(Owner.velocity) * 19.99f;
				}
			}

			Owner.direction = Owner.velocity.X > 0 ? 1 : -1;

			if (Owner.velocity.Y == 0 && Projectile.timeLeft > 10)
			{
				Projectile.timeLeft = 10;
			}

			float dustRot = Main.rand.NextFloat(6.28f);
			var d = Dust.NewDustPerfect(Projectile.Center + Vector2.One.RotatedBy(dustRot) * 50f, ModContent.DustType<Dusts.Cinder>(), Vector2.UnitX.RotatedBy(dustRot + 1.57f * Owner.direction) * Main.rand.NextFloat(6, 16), Owner.direction, new Color(255, Main.rand.Next(150, 255), 150), Main.rand.NextFloat(0.5f, 1.75f));
			d.customData = Owner;

			var d2 = Dust.NewDustPerfect(Projectile.Center + Vector2.One.RotatedBy(dustRot) * 50f, ModContent.DustType<Dusts.Cinder>(), Vector2.UnitX.RotatedBy(dustRot + 1.57f * Owner.direction) * Main.rand.NextFloat(4, 8), Owner.direction, new Color(255, 100, 0), Main.rand.NextFloat(1.0f, 1.75f));
			d2.customData = Owner;

			Lighting.AddLight(Projectile.Center, new Vector3(1, 0.5f, 0.2f));
		}

		public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
		{
			for (int k = 0; k < 20; k++)
			{
				float dustRot = (target.Center - Owner.Center).ToRotation() + Main.rand.NextFloat(1);
				var d2 = Dust.NewDustPerfect(Projectile.Center + Vector2.One.RotatedBy(dustRot) * 50f, ModContent.DustType<Dusts.Cinder>(), Vector2.UnitX.RotatedBy(dustRot) * Main.rand.NextFloat(4, 8), 0, new Color(255, 100, 0), Main.rand.NextFloat(1.0f, 1.75f));
				d2.customData = Owner;
			}

			if (Projectile.timeLeft > 10)
			{
				Projectile.timeLeft = 10;
				Owner.velocity *= -0.75f;
			}

			SoundEngine.PlaySound(SoundID.Item71, Owner.Center);
			SoundEngine.PlaySound(SoundID.DD2_ExplosiveTrapExplode, Owner.Center);
		}

		public override bool PreDraw(ref Color lightColor)
		{
			Texture2D tex = ModContent.Request<Texture2D>("CCMod/Content/Items/Weapons/Melee/HotRollerProjectile").Value;
			Texture2D tex2 = ModContent.Request<Texture2D>("CCMod/Content/Items/Weapons/Melee/HotRollerProjectileGlow").Value;
			Texture2D tex3 = ModContent.Request<Texture2D>("CCMod/Content/Items/Weapons/Melee/HotRoller").Value;

			float fade = 1f;

			if (Timer < 10)
			{
				fade = Timer / 10f;
			}

			if (Timer > 50)
			{
				fade = 1 - (Timer - 50) / 10f;
			}

			Color color = Color.White * fade;
			color.A = 0;

			Color color2 = new Color(255, 100, 0) * MathF.Sin(Timer * 0.05f);
			color2.A = 0;

			float scale = 1.15f + MathF.Sin(Timer * 0.05f) * 0.05f;

			for (int k = 0; k < Projectile.oldPos.Length; k++)
			{
				float progress = 1 - k / (float)Projectile.oldPos.Length;
				Color trailColor = Color.Lerp(Color.White, new Color(255, 100, 0), 1 - progress) * progress * 0.05f * fade;
				trailColor.A = 0;
				Main.spriteBatch.Draw(tex2, Projectile.oldPos[k] + Projectile.Size / 2 - Main.screenPosition, null, trailColor, Projectile.oldRot[k], tex2.Size() / 2, scale, default, default);
			}

			Main.spriteBatch.Draw(tex2, Projectile.Center - Main.screenPosition, null, color2, Projectile.rotation, tex2.Size() / 2, scale, 0, 0);

			Main.spriteBatch.Draw(tex, Projectile.Center - Main.screenPosition, null, Color.White * fade, Projectile.rotation, tex.Size() / 2, 1, 0, 0);

			Main.spriteBatch.Draw(tex2, Projectile.Center - Main.screenPosition, null, color, Projectile.rotation, tex2.Size() / 2, 1, 0, 0);

			SpriteEffects effects = Owner.direction == -1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None;
			Vector2 origin = Owner.direction == -1 ? tex3.Size() : Vector2.UnitY * tex3.Height;

			Main.spriteBatch.Draw(tex3, Projectile.Center - Main.screenPosition, null, Color.White * fade, Projectile.rotation * 2, origin, 1, effects, 0);

			for (int k = 0; k < 5; k++)
			{
				float progress = 1 - k / 5f;
				Color trailColor2 = Color.Lerp(Color.White, new Color(255, 100, 0), 1 - progress) * progress * 0.5f * fade;
				Main.spriteBatch.Draw(tex3, Projectile.Center - Main.screenPosition, null, trailColor2, Projectile.oldRot[k] * 2, origin, 1, effects, 0);
			}

			return false;
		}
	}
}
