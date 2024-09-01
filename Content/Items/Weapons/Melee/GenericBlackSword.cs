using CCMod.Common.Attributes;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.IO;
using Terraria;
using Terraria.GameContent;
using Terraria.GameContent.Creative;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace CCMod.Content.Items.Weapons.Melee
{
	[CodedBy("LowQualityTrash-Xinim")]
	[SpritedBy("LowQualityTrash-Xinim")]
	internal class GenericBlackSword : ModItem
	{
		public override void SetStaticDefaults()
		{
			// Tooltip.SetDefault("it is just a generic sword");
			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
		}
		public override void SetDefaults()
		{
			Item.width = 40;
			Item.height = 40;

			Item.damage = 27;
			Item.knockBack = 2f;
			Item.useTime = 14;
			Item.useAnimation = 14;

			Item.rare = ItemRarityID.White;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.value = Item.sellPrice(copper: 1);
			Item.DamageType = DamageClass.Melee;
			Item.autoReuse = true;
			Item.scale = 1.5f;
			Item.shoot = ModContent.ProjectileType<GenericBlackSwordSlash>();

			Item.UseSound = SoundID.Item1;
		}

		public override void ModifyTooltips(List<TooltipLine> tooltips)
		{
			Player player = Main.LocalPlayer;
			if (player.name == "LowQualityTrashXinim" || player.GetModPlayer<GenericBlackSwordPlayer>().HowDIDyouFigureThatOut > 0)
			{
				foreach (TooltipLine item in tooltips)
				{
					if (item.Text == "Generic Black Sword")
					{
						item.OverrideColor = Main.DiscoColor;
					}
				}
			}
		}
		public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
		{
			if (!player.GetModPlayer<GenericBlackSwordPlayer>().VoidSlash)
			{
				type = ProjectileID.None;
			}

			velocity = player.direction == 1 ? new Vector2(5, 0) : new Vector2(-5, 0);
			position.Y -= 30;
		}

		public override void OnHitNPC(Player player, NPC target, NPC.HitInfo hit, int damageDone)
		{
			if (player.GetModPlayer<GenericBlackSwordPlayer>().VoidBladeProtection)
			{
				player.GetModPlayer<GenericBlackSwordPlayer>().VoidCount++;
			}
		}

		public override void MeleeEffects(Player player, Rectangle hitbox)
		{
			Vector2 hitboxCenter = new Vector2(hitbox.X, hitbox.Y);

			int dust = Dust.NewDust(hitboxCenter, hitbox.Width, hitbox.Height, DustID.t_Granite, 0, 0, 0, Color.Black, Main.rand.NextFloat(1.25f, 1.75f));
			Main.dust[dust].noGravity = true;
		}
	}

	internal class GenericBlackSwordProjectileBlade : ModProjectile
	{
		public override string Texture => "CCMod/Content/Items/Weapons/Melee/GenericBlackSword";
		public override void SetDefaults()
		{
			Projectile.width = 40;
			Projectile.height = 40;
			Projectile.penetrate = -1;
			Projectile.light = 1;
			Projectile.friendly = true;
			Projectile.tileCollide = false;
			Projectile.extraUpdates = 6;
			ProjectileID.Sets.TrailCacheLength[Projectile.type] = 50;
			ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
		}
		public void Behavior(Player player, float offSet, int Counter, float Distance = 150)
		{
			Vector2 Rotate = new Vector2(1, 1).RotatedBy(MathHelper.ToRadians(offSet));
			Vector2 NewCenter = player.Center + Rotate.RotatedBy(Counter * 0.01f) * Distance;
			Projectile.Center = NewCenter;
			if (Counter == 0 && Check2 == 0)
			{
				for (int i = 0; i < 90; i++)
				{
					Vector2 randomSpeed = Main.rand.NextVector2CircularEdge(5, 5);
					int dust = Dust.NewDust(NewCenter, 0, 0, DustID.Granite, randomSpeed.X, randomSpeed.Y, 0, Color.Black, 1f);
					Main.dust[dust].noGravity = true;
				}

				Check2++;
			}
		}
		int Counter = 0;
		int Multiplier = 1;
		int Check = 0;
		int Check2 = 0;
		public override void AI()
		{
			Player player = Main.player[Projectile.owner];
			if (player.GetModPlayer<GenericBlackSwordPlayer>().YouGotHitLMAO)
			{
				Projectile.ai[0]++;
				if (Projectile.ai[0] == 150)
				{
					Projectile.penetrate = 1;
					Projectile.netUpdate = true;

					float distance = 1500;

					NPC closestNPC = FindClosestNPC(distance);
					if (closestNPC != null && Check == 0)
					{
						Projectile.damage *= 5;
						Projectile.velocity = (closestNPC.Center - Projectile.Center).SafeNormalize(Vector2.Zero) * 10f;
						Projectile.timeLeft = 100;
						Check++;
					}

					Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver4;
				}
				else
				{
					Projectile.rotation = MathHelper.PiOver4 + (Main.MouseWorld - Projectile.Center).SafeNormalize(Vector2.UnitX).ToRotation();
				}
			}
			else
			{
				Projectile.timeLeft = 200;
				if (player.dead || !player.active)
				{
					Projectile.Kill();
				}

				if (Projectile.ai[0] == 0)
				{
					switch (Projectile.velocity.X)
					{
						case 1:
							Multiplier = 1;
							break;
						case 2:
							Multiplier = 2;
							break;
						case 3:
							Multiplier = 3;
							break;
						case 4:
							Multiplier = 4;
							break;
						case 5:
							Multiplier = 5;
							break;
					}

					Projectile.velocity = Vector2.Zero;
					Projectile.ai[0]++;
				}

				if (Main.rand.NextBool(3))
				{
					int dust = Dust.NewDust(Projectile.position, 10, 10, DustID.t_Granite, 0, 0, 0, Color.Black, Main.rand.NextFloat(.8f, 1f));
					Main.dust[dust].noGravity = true;
				}

				Projectile.rotation = MathHelper.PiOver4 + MathHelper.ToRadians(72 * Multiplier) - MathHelper.ToRadians(Counter);
				Behavior(player, 72 * Multiplier, Counter);
				if (Math.Abs(Counter) == MathHelper.TwoPi * 100 + 1)
				{
					Counter = 1;
				}

				if (player.direction == 1)
				{
					Counter++;
				}
				else
				{
					Counter--;
				}
			}
		}

		public NPC FindClosestNPC(float maxDetectDistance)
		{
			NPC closestNPC = null;
			float sqrMaxDetectDistance = maxDetectDistance * maxDetectDistance;
			for (int k = 0; k < Main.maxNPCs; k++)
			{
				NPC target = Main.npc[k];
				if (target.CanBeChasedBy())
				{
					// The DistanceSquared function returns a squared distance between 2 points, skipping relatively expensive square root calculations
					float sqrDistanceToTarget = Vector2.DistanceSquared(target.Center, Projectile.Center);

					// Check if it is within the radius
					if (sqrDistanceToTarget < sqrMaxDetectDistance)
					{
						sqrMaxDetectDistance = sqrDistanceToTarget;
						closestNPC = target;
					}
				}
			}

			return closestNPC;
		}

		public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
		{
			target.immune[Projectile.owner] = 5;
			if (Main.player[Projectile.owner].GetModPlayer<GenericBlackSwordPlayer>().YouGotHitLMAO)
			{
				target.immune[Projectile.owner] = 0;
			}
		}

		public override void OnKill(int timeLeft)
		{
			for (int i = 0; i < 40; i++)
			{
				Vector2 randomSpeed = Main.rand.NextVector2CircularEdge(3, 3);
				Dust.NewDust(Projectile.position, 0, 0, DustID.t_Granite, randomSpeed.X, randomSpeed.Y, 0, Color.Black, Main.rand.NextFloat(1f, 1.25f));
			}
		}

		public override bool PreDraw(ref Color lightColor)
		{
			Main.instance.LoadProjectile(Projectile.type);
			Texture2D texture = TextureAssets.Projectile[Projectile.type].Value;

			Vector2 origin = new Vector2(texture.Width * 0.5f, Projectile.height * 0.5f);
			for (int k = 1; k < Projectile.oldPos.Length + 1; k++)
			{
				Vector2 drawPos = Projectile.oldPos[k - 1] - Main.screenPosition + origin + new Vector2(Projectile.gfxOffY);
				Color color = new Color(0, 0, 0, 255 / k);
				Main.EntitySpriteDraw(texture, drawPos, null, color, Projectile.rotation, origin, Projectile.scale - (k - 1) * 0.02f, SpriteEffects.None, 0);
			}

			return true;
		}
	}

	internal class GenericBlackSwordProjectile : ModProjectile
	{
		public override string Texture => "CCMod/Content/Items/Weapons/Melee/GenericBlackSword";
		public override void SetDefaults()
		{
			Projectile.hide = true;
			Projectile.width = 20;
			Projectile.height = 20;
			Projectile.penetrate = -1;
			Projectile.light = 1;
			Projectile.friendly = true;
			Projectile.timeLeft = 500;
			Projectile.tileCollide = false;
		}

		public override void AI()
		{
			Projectile.ai[0] += 1f;
			if (Projectile.ai[0] > 50)
			{
				Projectile.penetrate = 1;
				Projectile.netUpdate = true;

				float distance = 1500;

				NPC closestNPC = FindClosestNPC(distance);
				if (closestNPC != null)
				{
					Projectile.velocity += (closestNPC.Center - Projectile.Center).SafeNormalize(Vector2.Zero);
					if (Projectile.timeLeft % 50 == 0)
					{
						Projectile.velocity = (closestNPC.Center - Projectile.Center).SafeNormalize(Vector2.UnitX) * 10;
					}

					if (Projectile.velocity.X > 10)
					{
						Projectile.velocity.X = 10;
					}
					else if (Projectile.velocity.X < -10)
					{
						Projectile.velocity.X = -10;
					}

					if (Projectile.velocity.Y > 10)
					{
						Projectile.velocity.Y = 10;
					}
					else if (Projectile.velocity.Y < -10)
					{
						Projectile.velocity.Y = -10;
					}
				}
				else
				{
					Projectile.Kill();
				}
			}

			int dust = Dust.NewDust(Projectile.Center, 5, 5, DustID.t_Granite, 0, 0, 0, Color.Black, Main.rand.NextFloat(1f, 1.5f));
			Main.dust[dust].noGravity = true;
		}

		public NPC FindClosestNPC(float maxDetectDistance)
		{
			NPC closestNPC = null;
			float sqrMaxDetectDistance = maxDetectDistance * maxDetectDistance;
			for (int k = 0; k < Main.maxNPCs; k++)
			{
				NPC target = Main.npc[k];
				if (target.CanBeChasedBy())
				{
					// The DistanceSquared function returns a squared distance between 2 points, skipping relatively expensive square root calculations
					float sqrDistanceToTarget = Vector2.DistanceSquared(target.Center, Projectile.Center);

					// Check if it is within the radius
					if (sqrDistanceToTarget < sqrMaxDetectDistance)
					{
						sqrMaxDetectDistance = sqrDistanceToTarget;
						closestNPC = target;
					}
				}
			}

			return closestNPC;
		}
	}

	public class GenericBlackSwordSlash : ModProjectile
	{
		public override void SetDefaults()
		{
			Projectile.width = 68;
			Projectile.height = 112;
			Projectile.friendly = true;
			Projectile.penetrate = -1;
			Projectile.DamageType = DamageClass.Melee;
			Projectile.tileCollide = false;
			Projectile.timeLeft = 50;
			Projectile.light = 0.5f;
			Projectile.extraUpdates = 6;
			ProjectileID.Sets.TrailCacheLength[Projectile.type] = 40;
			ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
		}
		Player player => Main.player[Projectile.owner];
		public override void AI()
		{
			Projectile.rotation = Projectile.velocity.ToRotation();

			Vector2 BetterTop = new Vector2(Projectile.Center.X, Projectile.Center.Y - Projectile.height * 0.5f);
			Dust.NewDust(BetterTop, Projectile.width, Projectile.height, DustID.t_Granite, Projectile.velocity.X, 0, 0, Color.Black, Main.rand.NextFloat(0.55f, 1f));
			if (player.GetModPlayer<GenericBlackSwordPlayer>().VoidSlashUpgrade)
			{
				if (Main.rand.NextBool(20))
				{
					Vector2 circle = Main.rand.NextVector2Circular(50, 50);
					Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center + circle, Vector2.Zero, ModContent.ProjectileType<GenericBlackSwordProjectile>(), (int)(Projectile.damage * .5f), 0, Projectile.owner);
				}
			}
		}
		public override void OnKill(int timeLeft)
		{
			Vector2 BetterTop = new Vector2(Projectile.Center.X, Projectile.Center.Y - Projectile.height * 0.5f);
			for (int i = 0; i < 20; i++)
			{
				Dust.NewDust(BetterTop, Projectile.width, Projectile.height, DustID.t_Granite, Projectile.velocity.X, 0, 0, Color.Black, Main.rand.NextFloat(0.5f, 1f));
			}
		}
		public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
		{
			if (player.GetModPlayer<GenericBlackSwordPlayer>().VoidBladeProtection)
			{
				player.GetModPlayer<GenericBlackSwordPlayer>().VoidCount++;
			}

			target.immune[Projectile.owner] = 7;
		}
		public override bool PreDraw(ref Color lightColor)
		{
			Main.instance.LoadProjectile(Projectile.type);
			Texture2D texture = TextureAssets.Projectile[Projectile.type].Value;

			Vector2 origin = new Vector2(texture.Width * 0.5f, Projectile.height * 0.5f);
			for (int k = 1; k < Projectile.oldPos.Length + 1; k++)
			{
				Vector2 drawPos = Projectile.oldPos[k - 1] - Main.screenPosition + origin + new Vector2(Projectile.gfxOffY);
				Color color = new Color(0, 0, 0, 255 / k);
				Main.EntitySpriteDraw(texture, drawPos, null, color, Projectile.rotation, origin, Projectile.scale, SpriteEffects.None, 0);
			}

			return true;
		}
	}
	internal class GenericBlackSwordPlayer : ModPlayer
	{
		public int VoidCount = 0;
		public int HowDIDyouFigureThatOut = 0;
		public bool YouGotHitLMAO = false;

		public bool VoidSlash = false;
		public bool VoidSlashUpgrade = false;
		public bool VoidBladeProtection = false;
		public bool VoidBladeProtectionUpgrade = false;
		public override void ResetEffects()
		{
			VoidSlash = NPC.downedBoss2;
			VoidSlashUpgrade = NPC.downedBoss3;
			VoidBladeProtection = NPC.downedMechBossAny;
			VoidBladeProtectionUpgrade = NPC.downedPlantBoss;
		}
		public override void PostUpdate()
		{
			if (VoidCount >= 10)
			{
				if (Player.ownedProjectileCounts[ModContent.ProjectileType<GenericBlackSwordProjectileBlade>()] < 1)
				{
					int PostUpdateDamage = Player.HeldItem.damage;
					if (HowDIDyouFigureThatOut >= 1 || Player.name == "LowQualityTrashXinim")
					{
						PostUpdateDamage *= 10;
					}

					YouGotHitLMAO = false;
					for (int i = 0; i < 5; i++)
					{
						Projectile.NewProjectile(Player.GetSource_FromThis(), Player.Center, new Vector2(1 + i, 0), ModContent.ProjectileType<GenericBlackSwordProjectileBlade>(), PostUpdateDamage, 0, Player.whoAmI);
					}
				}
			}
		}

		public override void OnHitNPCWithProj(Projectile proj, NPC target, NPC.HitInfo hit, int damageDone)/* tModPorter If you don't need the Projectile, consider using OnHitNPC instead */
		{
			if (proj.type == ModContent.ProjectileType<GenericBlackSwordProjectile>() || proj.type == ModContent.ProjectileType<GenericBlackSwordProjectileBlade>())
			{
				for (int i = 0; i < 35; i++)
				{
					Vector2 randomSpeed = Main.rand.NextVector2CircularEdge(10, 10);
					int dust = Dust.NewDust(proj.position, 0, 0, DustID.t_Granite, randomSpeed.X, randomSpeed.Y, 0, Color.Black, 1.2f);
					Main.dust[dust].noGravity = true;
				}
			}
		}

		public override void ModifyWeaponDamage(Item item, ref StatModifier damage)
		{
			if ((HowDIDyouFigureThatOut >= 1 || Player.name == "LowQualityTrashXinim") && item.type == ModContent.ItemType<GenericBlackSword>())
			{
				if (NPC.downedBoss2)
				{
					damage += 1;
				}

				if (NPC.downedBoss3)
				{
					damage += 1;
				}

				if (Main.hardMode)
				{
					damage += 3;
				}

				if (NPC.downedMechBossAny)
				{
					damage += 1;
				}

				if (NPC.downedPlantBoss)
				{
					damage += 2;
				}

				if (NPC.downedGolemBoss)
				{
					damage += 3;
				}

				if (NPC.downedMoonlord)
				{
					damage += 5;
				}
			}
		}

		public override void OnHurt(Player.HurtInfo info)
		{
			VoidCount = 0;
			YouGotHitLMAO = true;
		}

		public override void SyncPlayer(int toWho, int fromWho, bool newPlayer)
		{
			ModPacket packet = Mod.GetPacket();
			packet.Write((byte)CCMod.MessageType.GenericBlackSwordPlayer);
			packet.Write((byte)Player.whoAmI);
			packet.Write(HowDIDyouFigureThatOut);
			packet.Send(toWho, fromWho);
		}

		public void ReceivePlayerSync(BinaryReader reader)
		{
			HowDIDyouFigureThatOut = reader.ReadByte();
		}

		public override void CopyClientState(ModPlayer clientClone)/* tModPorter Suggestion: Replace Item.Clone usages with Item.CopyNetStateTo */
		{
			GenericBlackSwordPlayer clone = clientClone as GenericBlackSwordPlayer;
			clone.HowDIDyouFigureThatOut = HowDIDyouFigureThatOut;
		}

		public override void SendClientChanges(ModPlayer clientPlayer)
		{
			GenericBlackSwordPlayer clone = clientPlayer as GenericBlackSwordPlayer;

			if (HowDIDyouFigureThatOut != clone.HowDIDyouFigureThatOut)
				SyncPlayer(toWho: -1, fromWho: Main.myPlayer, newPlayer: false);
		}

		public override void SaveData(TagCompound tag)
		{
			tag["HowDIDyouFigureThatOut"] = HowDIDyouFigureThatOut;
		}

		public override void LoadData(TagCompound tag)
		{
			HowDIDyouFigureThatOut = (int)tag["HowDIDyouFigureThatOut"];
		}
	}

	public class DropGenericBlackSword : GlobalNPC
	{
		public override void ModifyNPCLoot(NPC npc, NPCLoot npcLoot)
		{
			if (npc.type == NPCID.EyeofCthulhu)
			{
				npcLoot.Add(ItemDropRule.ByCondition(new GenericBlackSwordConditionRule(), ModContent.ItemType<GenericBlackSword>()));
			}
		}

		public override void OnKill(NPC npc)
		{
			Player player = Main.LocalPlayer;
			if (player.ZoneGraveyard && player.ZoneUnderworldHeight && npc.type == NPCID.EyeofCthulhu && Main.masterMode)
			{
				player.GetModPlayer<GenericBlackSwordPlayer>().HowDIDyouFigureThatOut++;//Don't ask me why this is set to int, bool didn't work
			}
		}
	}

	public class GenericBlackSwordConditionRule : IItemDropRuleCondition
	{
		public bool CanDrop(DropAttemptInfo info)
		{
			if (!info.IsInSimulation)
			{
				return
					info.player.ZoneGraveyard;
			}

			return false;
		}

		public bool CanShowItemDropInUI()
		{
			return true;
		}

		public string GetConditionDescription()
		{
			return "Beat Eye of Cthulhu in Graveyard";
		}
	}
}