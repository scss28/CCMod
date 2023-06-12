using CCMod.Common.Attributes;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.IO;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;

namespace CCMod.Content.Items.Weapons.Ranged.ExperimentalPlasmacaster
{
	[CodedBy("mayhemm")]
	[SpritedBy("mayhemm")]
	public class ExperimentalPlasmacaster : ModItem
	{
		public override Vector2? HoldoutOffset()
		{
			return new Vector2(-0.5f, 0);
		}

		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Experimental Plasmacaster");
			// Tooltip.SetDefault("Fires unstable orbs of plasma\nProjectile behaviour varies based on time spent charging\n'What's the worst that could happen?'");
			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Item.type] = 1;
		}

		public override void SetDefaults()
		{
			Item.width = 38;
			Item.height = 30;
			Item.damage = 86;
			Item.useTime = 10;
			Item.useAnimation = 10;
			Item.DamageType = DamageClass.Ranged;
			Item.useStyle = ItemUseStyleID.Shoot;
			Item.noMelee = true;
			Item.autoReuse = false;
			Item.value = Item.buyPrice(gold: 1);
			Item.rare = ItemRarityID.Green;
			Item.shoot = ModContent.ProjectileType<ExperimentalPlasmachargerHeld>();
			Item.shootSpeed = 0;
			Item.channel = true;
			Item.noUseGraphic = true;
		}

		public override void AddRecipes()
		{
			CreateRecipe()
			.AddIngredient(ItemID.SoulofSight, 5)
			.AddIngredient(ItemID.HallowedBar, 12)
			.AddIngredient(ItemID.Wire, 25)
			.AddIngredient(ItemID.BlackLens)
			.AddIngredient(ItemID.IllegalGunParts)
			.AddTile(TileID.MythrilAnvil)
			.Register();
		}
	}

	public class ExperimentalPlasmachargerHeld : ModProjectile
	{
		public override string Texture => "CCMod/Content/Items/Weapons/Ranged/ExperimentalPlasmacaster/ExperimentalPlasmacaster";

		public override void Load()
		{
			Terraria.On_Main.DrawProjectiles += DrawMuzzleEffect;
		}

		public override void Unload()
		{
			Terraria.On_Main.DrawProjectiles -= DrawMuzzleEffect;
		}

		Player owner => Main.player[Projectile.owner];

		float ChargeTimer { get => Projectile.ai[0]; set => Projectile.ai[0] = value; }

		int aimDir => aimNormal.X > 0 ? 1 : -1;

		public bool BeingHeld => Main.player[Projectile.owner].channel && !Main.player[Projectile.owner].noItems && !Main.player[Projectile.owner].CCed;

		private static readonly List<MuzzleEffect> mzFx = new();

		public Vector2 aimNormal;

		public float flashAlpha = 0;
		public float recoilAmount = 0;

		public int chargeLevel = 0;

		public bool charging = true;

		public override void SetDefaults()
		{
			Projectile.width = 38;
			Projectile.height = 30;
			Projectile.aiStyle = -1;
			Projectile.penetrate = -1;
			Projectile.ignoreWater = true;
			Projectile.tileCollide = false;
			Projectile.timeLeft = 2;
		}

		public override void OnSpawn(IEntitySource source)
		{
			Projectile.originalDamage = Projectile.damage;
			Projectile.damage = 0;
			ChargeTimer = 20;
		}

		public override bool PreAI()
		{
			owner.heldProj = Projectile.whoAmI;
			owner.itemAnimation = 2;
			owner.itemTime = 2;

			if (Main.myPlayer == Projectile.owner)
			{
				//aimNormal is a normal vector pointing from the player at the mouse cursor
				aimNormal = Vector2.Normalize(Main.MouseWorld - owner.MountedCenter + new Vector2(owner.direction * -3, -1));
			}

			owner.ChangeDir(aimDir);
			return true;
		}

		public override void AI()
		{
			Projectile.netUpdate = true;

			flashAlpha *= 0.85f; //constantly decrease alpha of the charge increase visual effect
			recoilAmount *= 0.85f; //constantly decrease the value of the variable that controls the recoil-esque visual effect of the gun's position

			//set projectile center to be at the player, slightly offset towards the aim normal, with adjustments for the recoil visual effect
			Projectile.Center = owner.MountedCenter + new Vector2(owner.direction * -3, -1) + (aimNormal * 20).RotatedBy(-(recoilAmount * 0.2f * aimDir));
			//set projectile rotation to point towards the aim normal, with adjustments for the recoil visual effect
			Projectile.rotation = aimNormal.ToRotation() - recoilAmount * 0.4f * aimDir;

			//set fancy player arm rotation
			owner.SetCompositeArmFront(true, Player.CompositeArmStretchAmount.Full, (aimNormal * 20).RotatedBy(-(recoilAmount * 0.2f * aimDir)).ToRotation() - MathHelper.PiOver2 + 0.3f * aimDir);

			if (BeingHeld)
			{
				Projectile.timeLeft = 10; //constantly set timeLeft to greater than zero to allow projectile to remain infinitely as long as player channels weapon

				if (charging == true)
				{
					ChargeTimer--;

					if (ChargeTimer <= 0 && chargeLevel < 8) //increment charge level and play charge increase visual effects (white flash + loading click sound)
					{
						chargeLevel++;

						SoundEngine.PlaySound(new SoundStyle("CCMod/Assets/Audio/SFX/loadingclick") with { Pitch = (float)chargeLevel / 12 }, Projectile.Center);
						flashAlpha = 1;
						ChargeTimer = 15;

						if (chargeLevel == 8)
						{
							SoundEngine.PlaySound(new SoundStyle("CCMod/Assets/Audio/SFX/somethingweird"), Projectile.Center); //play full charge telegraph when charge level is max
						}
					}
				}
			}
			else
			{
				if (charging == true) //run for a single frame when player stops channeling weapon
				{
					if (chargeLevel < 1) //fire normal projectile if gun is completely uncharged (basic tap-fire)
					{
						Fire();
					}
					else if (chargeLevel < 8) //initiate burst fire if gun is charged but not fully
					{
						//set timeLeft to a high enough value to allow entire burst fire to happen
						Projectile.timeLeft = chargeLevel * 3 + 14;
						//ChargeTimer has use changed from delay between charge increments to a timer to control burst fire. this is done solely to use less variables
						ChargeTimer = chargeLevel * 3 + 8;
					}
					else //fire slow moving, piercing, Flying Nightmare-esque projectile if gun is fully charged
					{
						FireNightmare();

						Projectile.timeLeft = 20;
					}
				}
				else //runs every frame from when the player stops channeling until projectile despawns
				{
					if (chargeLevel >= 1 && chargeLevel < 8)
					{
						ChargeTimer--;

						if (ChargeTimer % 3 == 0 && Projectile.timeLeft >= 10) //fire plasma ball every 3 frames until ChargeTimer reaches 0
						{
							Fire();
						}
					}
				}

				charging = false;
			}
		}

		private void Fire() //method to fire regular projectile
		{
			float instability = (float)chargeLevel / 8;

			//increase recoil value, make gun appear like it's actually firing with some force
			recoilAmount += Main.rand.NextFloat(0.5f, 0.8f);
			SoundEngine.PlaySound(new SoundStyle("CCMod/Assets/Audio/SFX/laserping") with { PitchVariance = 0.2f, Volume = 2f }, Projectile.Center);

			//where the projectile should spawn, modified so the projectile actually looks like it's coming out of the barrel
			Vector2 shootOrigin = Projectile.Center + aimNormal.RotatedBy(-MathHelper.PiOver2 * aimDir) * 7;

			//spawn weird glow ring effect to act as a sort of muzzle flash
			mzFx.Add(new MuzzleEffect(shootOrigin + aimNormal * 22, aimNormal.RotatedByRandom(0.1f).RotatedBy(-recoilAmount * 0.2f * aimDir) * Main.rand.NextFloat(4, 6)));

			if (Main.myPlayer == Projectile.owner)
			{
				float speed = 12 + instability * 12;
				int damage = (int)(Projectile.originalDamage + instability * 48);
				Projectile.NewProjectile(Projectile.GetSource_FromThis(), shootOrigin, aimNormal.RotatedByRandom(instability * 0.2f) * speed, ModContent.ProjectileType<PlasmaOrb>(), damage, 2, Projectile.owner);
			}
		}

		private void FireNightmare() //method to fire flying nightmare projectile
		{
			//more recoil because big bullet
			recoilAmount += 2;
			SoundEngine.PlaySound(new SoundStyle("CCMod/Assets/Audio/SFX/nightmareshot"), Projectile.Center);

			//where the projectile should spawn, modified so the projectile actually looks like it's coming out of the barrel
			Vector2 shootOrigin = Projectile.Center + aimNormal.RotatedBy(-MathHelper.PiOver2 * aimDir) * 7;

			for (int i = 0; i < 3; i++) //spawns 3 glow ring "particles" simultaneously, with 3 different sizes and speeds
			{
				var m = new MuzzleEffect(shootOrigin + aimNormal * 22, aimNormal * 2 * (i + 1))
				{
					scale = 0.3f * (i + 1)
				};
				mzFx.Add(m);
			}

			if (Main.myPlayer == Projectile.owner)
			{
				Projectile.NewProjectile(Projectile.GetSource_FromThis(), shootOrigin, aimNormal * 8, ModContent.ProjectileType<FlyingNightmare>(), 102, 2, Projectile.owner);
			}
		}

		public override bool PreDraw(ref Color lightColor)
		{
			return false;
		}

		public override void PostDraw(Color lightColor)
		{
			Texture2D main = TextureAssets.Projectile[Type].Value;
			Texture2D glow = ModContent.Request<Texture2D>("CCMod/Content/Items/Weapons/Ranged/ExperimentalPlasmacaster/ExperimentalPlasmacaster_glow").Value;
			Texture2D flash = ModContent.Request<Texture2D>("CCMod/Content/Items/Weapons/Ranged/ExperimentalPlasmacaster/ExperimentalPlasmacaster_extra1").Value;

			Vector2 position = Projectile.Center - Main.screenPosition;
			Vector2 origin = Projectile.Size / 2;
			SpriteEffects flip = (aimDir == 1) ? SpriteEffects.None : SpriteEffects.FlipVertically;

			Main.EntitySpriteDraw(main, position, null, lightColor, Projectile.rotation, origin, 1, flip, 0);
			Main.EntitySpriteDraw(glow, position, null, Color.White, Projectile.rotation, origin, 1, flip, 0);
			Main.EntitySpriteDraw(flash, position, null, Color.White * flashAlpha, Projectile.rotation, origin, 1, flip, 0);
		}

		public override void SendExtraAI(BinaryWriter writer) //important because mouse cursor logic is really unstable in multiplayer if done wrong
		{
			writer.WriteVector2(aimNormal);
		}

		public override void ReceiveExtraAI(BinaryReader reader)
		{
			aimNormal = reader.ReadVector2();
		}

		private static void DrawMuzzleEffect(Terraria.On_Main.orig_DrawProjectiles orig, Main self) //i'd much rather make a standalone particle system to do this but i dont have time to fit an entire library mod into this mod
		{
			orig(self);

			Texture2D effect = ModContent.Request<Texture2D>("CCMod/Content/Items/Weapons/Ranged/ExperimentalPlasmacaster/ExperimentalPlasmacaster_extra2").Value;

			MuzzleEffect[] mzFxA = mzFx.ToArray(); //i don't like lists and they don't like me either

			for (int i = 0; i < mzFx.Count; i++)
			{
				Vector2 drawPos = mzFxA[i].position - Main.screenPosition;
				Vector2 drawOrigin = effect.Size() / 2;

				mzFxA[i].position += mzFxA[i].velocity;
				mzFxA[i].velocity *= 0.85f;
				mzFxA[i].alpha *= 0.92f;
				mzFxA[i].scale += 0.05f;

				if (mzFxA[i].alpha < 0.1f)
					mzFx.Remove(mzFxA[i]);

				Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.Additive, Main.DefaultSamplerState, DepthStencilState.None, Main.Rasterizer, null, Main.GameViewMatrix.TransformationMatrix);

				Main.EntitySpriteDraw(effect, drawPos, null, new Color(48, 234, 255, 255) * mzFxA[i].alpha, mzFxA[i].velocity.ToRotation() + MathHelper.Pi, drawOrigin, mzFxA[i].scale, SpriteEffects.None, 0);

				Main.spriteBatch.End();
			}
		}

		private class MuzzleEffect //class for glow ring "particle" so multiple of them can exist at once independantly (this should probably be a struct but i can't be bothered)
		{
			public float alpha;
			public float scale;

			public Vector2 position;
			public Vector2 velocity;

			public MuzzleEffect(Vector2 initPosition, Vector2 initVelocity)
			{
				alpha = 1;
				scale = 0.5f;
				position = initPosition;
				velocity = initVelocity;
			}
		}
	}

	public class PlasmaOrb : ModProjectile
	{
		public override string Texture => "CCMod/Content/Items/Weapons/Ranged/ExperimentalPlasmacaster/ExperimentalPlasmacaster_proj";

		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Plasma Orb");

			ProjectileID.Sets.TrailCacheLength[Projectile.type] = 18;
			ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
		}

		public override void SetDefaults()
		{
			Projectile.width = 16;
			Projectile.height = 16;
			Projectile.aiStyle = -1;
			Projectile.DamageType = DamageClass.Ranged;
			Projectile.penetrate = 1;
			Projectile.friendly = true;
			Projectile.hostile = false;
			Projectile.ignoreWater = false;
			Projectile.tileCollide = true;
			Projectile.timeLeft = 500;
		}

		public override void OnSpawn(IEntitySource source)
		{
			Projectile.localAI[0] = (float)Main.timeForVisualEffects;
		}

		public override void AI()
		{
			if (Projectile.timeLeft <= 1 && Projectile.localAI[1] == 0)
				Die();

			if (Projectile.localAI[1] != 0)
				Projectile.localAI[1]++; //death anim is active when localai is not equal to 0
		}

		public override bool OnTileCollide(Vector2 oldVelocity)
		{
			Projectile.position += oldVelocity;
			Die();
			return false;
		}

		public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
		{
			Die();
		}

		private void Die() //make the projectile effectively intangible and start death animation;
		{
			Projectile.damage = 0;
			Projectile.penetrate = -1;
			Projectile.tileCollide = false;
			Projectile.timeLeft = 15;
			Projectile.localAI[1] = 1;
			Projectile.velocity *= 0;

			SoundEngine.PlaySound(new SoundStyle("CCMod/Assets/Audio/SFX/plasmaimpact") with { PitchVariance = 0.2f, Volume = 1.5f }, Projectile.Center);
		}

		public override bool PreDraw(ref Color lightColor)
		{
			return false;
		}

		public override void PostDraw(Color lightColor)
		{
			Texture2D main = TextureAssets.Projectile[Type].Value;
			Texture2D bloom = ModContent.Request<Texture2D>("CCMod/Content/Items/Weapons/Ranged/ExperimentalPlasmacaster/ExperimentalPlasmacaster_proj_bloom").Value;
			Texture2D explosion = ModContent.Request<Texture2D>("CCMod/Content/Items/Weapons/Ranged/ExperimentalPlasmacaster/ExperimentalPlasmacaster_proj_extra1").Value;

			Vector2 position = Projectile.Center - Main.screenPosition;
			Vector2 mainOrigin = Projectile.Size / 2;
			Vector2 bloomOrigin = bloom.Size() / 2;
			Vector2 explOrigin = explosion.Size() / 2;

			float sin = (float)Math.Sin((float)(Main.timeForVisualEffects - Projectile.localAI[0]) / 7);
			Vector2 scale = new Vector2(1 + sin * 0.15f, 1 - sin * 0.15f);

			Main.spriteBatch.End(); //This code is bad. Do not do this
			Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.Additive, Main.DefaultSamplerState, DepthStencilState.None, Main.Rasterizer, null, Main.GameViewMatrix.TransformationMatrix); //set the spritebatch to additive blending mode, which allows for transparency in sprites to render properly

			Main.EntitySpriteDraw(bloom, position, null, new Color(48, 234, 255, 40) * ((15f - Projectile.localAI[1]) / 15), Projectile.rotation, bloomOrigin, 0.7f, SpriteEffects.None, 0);

			if (Projectile.localAI[1] == 0) //disable drawing trail projectile if death animation is active
			{
				for (int i = 0; i < Projectile.oldPos.Length; i++)
				{
					float factor = (float)(Projectile.oldPos.Length - i) / Projectile.oldPos.Length;

					Vector2 pos = Projectile.oldPos[i] + Projectile.Size / 2 - Main.screenPosition;

					Main.EntitySpriteDraw(bloom, pos, null, new Color(48, 234, 255, 50) * factor, Projectile.rotation, bloomOrigin, factor * 0.6f, SpriteEffects.None, 0);
				}
			}
			else //extra "pulse" explosion-esque effect for death animation
			{
				Main.EntitySpriteDraw(explosion, position, null, new Color(48, 234, 255, 255) * ((15f - Projectile.localAI[1]) / 15), Projectile.rotation, explOrigin, (float)Math.Log(Projectile.localAI[1], 15) / 2, SpriteEffects.None, 0);
			}

			Main.spriteBatch.End();
			Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, Main.Rasterizer, null, Main.GameViewMatrix.TransformationMatrix); //but seriously, it's better to have a separate additive drawing manager and use interfaces to draw additively in entities so the spritebatch only has to stop and start a few times, but again, i dont want to force a whole library mod into this mod

			//disable drawing main projectile if death animation is active
			if (Projectile.localAI[1] == 0)
				Main.EntitySpriteDraw(main, position, null, Color.White, Projectile.rotation, mainOrigin, scale, SpriteEffects.None, 0);
		}
	}

	public class FlyingNightmare : ModProjectile
	{
		public override string Texture => "CCMod/Content/Items/Weapons/Ranged/ExperimentalPlasmacaster/ExperimentalPlasmacaster_proj";

		float Scale { get => Projectile.localAI[0]; set => Projectile.localAI[0] = value; }

		public override void SetDefaults()
		{
			Projectile.width = 96;
			Projectile.height = 96;
			Projectile.aiStyle = -1;
			Projectile.DamageType = DamageClass.Ranged;
			Projectile.penetrate = -1;
			Projectile.friendly = true;
			Projectile.hostile = false;
			Projectile.ignoreWater = true;
			Projectile.tileCollide = false;
			Projectile.timeLeft = 240;
		}

		public override void AI()
		{
			if (Main.rand.NextBool(8)) //dust because this thing looked kinda empty without it
			{
				Vector2 pos = Projectile.Center + new Vector2(Main.rand.NextFloat(0, 16)).RotatedByRandom(MathHelper.TwoPi);

				var d = Dust.NewDustDirect(pos, 0, 0, ModContent.DustType<Dusts.BlueGlowDust>());
				d.velocity = Projectile.velocity * 0.5f;
			}
		}

		public override bool PreDraw(ref Color lightColor)
		{
			return false;
		}

		public override void PostDraw(Color lightColor)
		{
			//this code makes the projectile shrink before disappearing at the end of its rather than poofing out of existance
			if (Projectile.timeLeft > 30)
			{
				if (Scale < 30)
					Scale++;
				else
					Scale--;
			}

			Texture2D main = ModContent.Request<Texture2D>("CCMod/Content/Items/Weapons/Ranged/ExperimentalPlasmacaster/ExperimentalPlasmacaster_proj2").Value;
			Texture2D outer = ModContent.Request<Texture2D>("CCMod/Content/Items/Weapons/Ranged/ExperimentalPlasmacaster/ExperimentalPlasmacaster_proj_extra1").Value;
			Texture2D bloom = ModContent.Request<Texture2D>("CCMod/Content/Items/Weapons/Ranged/ExperimentalPlasmacaster/ExperimentalPlasmacaster_proj_bloom").Value;

			Vector2 position = Projectile.Center - Main.screenPosition;
			Vector2 mainOrigin = main.Size() / 2;
			Vector2 outerOrigin = outer.Size() / 2;
			Vector2 bloomOrigin = bloom.Size() / 2;

			float sin = (float)Math.Sin((float)Main.timeForVisualEffects / 7); //sin wave for pulsating outer glow and main orb shape
			float alpha = 1 + sin * 0.2f;
			Vector2 scale = new Vector2(1 + sin * 0.15f, 1 - sin * 0.15f);

			Main.spriteBatch.End(); //again, don't do this
			Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.Additive, Main.DefaultSamplerState, DepthStencilState.None, Main.Rasterizer, null, Main.GameViewMatrix.TransformationMatrix);

			Main.EntitySpriteDraw(outer, position, null, new Color(48, 234, 255, 150) * alpha, Projectile.rotation, outerOrigin, Scale / 30, SpriteEffects.None, 0);
			Main.EntitySpriteDraw(bloom, position, null, new Color(48, 234, 255, 60), Projectile.rotation, bloomOrigin, Scale / 30, SpriteEffects.None, 0);

			Main.spriteBatch.End();
			Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, Main.Rasterizer, null, Main.GameViewMatrix.TransformationMatrix);

			Main.EntitySpriteDraw(main, position, null, Color.White, Projectile.rotation, mainOrigin, scale * (Scale / 30), SpriteEffects.None, 0);
		}
	}
}