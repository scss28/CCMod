using Terraria;
using System.IO;
using Terraria.ID;
using CCMod.Common;
using Terraria.ModLoader;
using Terraria.GameContent;
using Terraria.ModLoader.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.GameContent.ItemDropRules;
using System.Collections.Generic;

namespace CCMod.Content.Items.Weapons.Melee
{
    internal class GenericBlackSword : ModItem,IMadeBy
    {
        public string CodedBy => "LowQualityTrash-Xinim";

        public string SpritedBy => "LowQualityTrash-Xinim";

        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("it is just a generic sword");
        }
        public override void SetDefaults()
        {
            Item.width = 40;
            Item.height = 40;

            Item.damage = 20;
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
        }

        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
            Player player = Main.LocalPlayer;
            if(player.name == "LowQualityTrashXinim" || player.GetModPlayer<GenericBlackSwordPlayer>().HowDIDyouFigureThatOut > 0)
            {
                foreach (TooltipLine item in tooltips)
                {
                    if(item.Text == "Generic Black Sword")
                    {
                        item.OverrideColor = Main.DiscoColor;
                    }
                }
            }
        }
        public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
        {
            velocity = player.direction == 1 ? new Vector2(5, 0) : new Vector2(-5, 0);
            position.Y = position.Y - 30;
        }

        public override void OnHitNPC(Player player, NPC target, int damage, float knockBack, bool crit)
        {
            player.GetModPlayer<GenericBlackSwordPlayer>().VoidCount++;
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
            if (Counter == 0)
            {
                for (int i = 0; i < 12; i++)
                {
                    Vector2 randomSpeed = Main.rand.NextVector2Circular(1, 1);
                    Dust.NewDust(NewCenter, 0, 0, DustID.Smoke, randomSpeed.X, randomSpeed.Y, 0, Color.Black, Main.rand.NextFloat(2f, 2.5f));
                }
            }
        }
        int Counter = 0;
        int Multiplier = 1;
        int Check = 0;
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
                    if (closestNPC != null || Check == 0)
                    {
                        Projectile.damage *= 3;
                        Projectile.velocity = (closestNPC.Center - Projectile.Center).SafeNormalize(Vector2.Zero) * 10f;
                        Projectile.timeLeft = 100;
                        Check++;
                    }
                }
                Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver4;
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
                Projectile.rotation = MathHelper.PiOver4 + MathHelper.ToRadians(72 * Multiplier) - MathHelper.ToRadians(Counter);
                Behavior(player, 72 * Multiplier, Counter);
                if (Counter == MathHelper.TwoPi * 100 + 1) { Counter = 1; }
                Counter++;
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

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            target.immune[Projectile.owner] = 5;
            if(Main.player[Projectile.owner].GetModPlayer<GenericBlackSwordPlayer>().YouGotHitLMAO)
            {
                target.immune[Projectile.owner] = 0;
            }
        }

        public override void Kill(int timeLeft)
        {
            for (int i = 0; i < 32; i++)
            {
                Vector2 randomSpeed = Main.rand.NextVector2Circular(3, 3);
                Dust.NewDust(Projectile.position, 0, 0, DustID.Smoke, randomSpeed.X, randomSpeed.Y, 0, Color.Black, Main.rand.NextFloat(1.4f, 1.9f));
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
            }
            int dust = Dust.NewDust(Projectile.Center, 5, 5, DustID.t_Granite, 0, 0, 0, Color.Black, Main.rand.NextFloat(1.55f, 2f));
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
        public override void AI()
        {
            Projectile.rotation = Projectile.velocity.ToRotation();
        }
        public override void Kill(int timeLeft)
        {
            Vector2 BetterTop = new Vector2(Projectile.Center.X, Projectile.Center.Y - Projectile.height * 0.5f);
            for (int i = 0; i < 30; i++)
            {
                int dust = Dust.NewDust(BetterTop, Projectile.width, Projectile.height, DustID.t_Granite, 0, 0, 0, Color.Black, Main.rand.NextFloat(2.25f, 2.75f));
                Main.dust[dust].noGravity = true;
            }
        }
        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            Player player = Main.player[Projectile.owner];
                player.GetModPlayer<GenericBlackSwordPlayer>().VoidCount++;
            
            target.immune[Projectile.owner] = 7;
            for (int i = 0; i < player.GetModPlayer<GenericBlackSwordPlayer>().VoidCount/4 + 1; i++)
            {
                Vector2 RanVel = Main.rand.NextVector2CircularEdge(10, 10);
                Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, RanVel, ModContent.ProjectileType<GenericBlackSwordProjectile>(), Projectile.damage, 0, Projectile.owner);
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
                Main.EntitySpriteDraw(texture, drawPos, null, color, Projectile.rotation, origin, Projectile.scale , SpriteEffects.None, 0);
            }
            return true;
        }
    }
    internal class GenericBlackSwordPlayer : ModPlayer
    {
        public int VoidCount = 0;
        public int HowDIDyouFigureThatOut = 0;
        public bool YouGotHitLMAO = false;
        public override IEnumerable<Item> AddStartingItems(bool mediumCoreDeath)
        {
            List<Item> items = new List<Item>();
            if (Player.name == "LowQualityTrashXinim")
            {
                items.Add(new Item(ModContent.ItemType<GenericBlackSword>()));
            }
            return items;
        }
        public override void PostUpdate()
        {
            if (VoidCount >= 10)
            {
                if (Player.ownedProjectileCounts[ModContent.ProjectileType<GenericBlackSwordProjectileBlade>()] < 1)
                {
                    YouGotHitLMAO = false;
                    for (int i = 0; i < 5; i++)
                    {
                        Projectile.NewProjectile(Player.GetSource_FromThis(), Player.Center, new Vector2(1 + i, 1 + i), ModContent.ProjectileType<GenericBlackSwordProjectileBlade>(), Player.HeldItem.damage, 0, Player.whoAmI);
                    }
                }
            }
        }

        public override void ModifyWeaponDamage(Item item, ref StatModifier damage)
        {
            if (HowDIDyouFigureThatOut >= 1 || Player.name == "LowQualityTrashXinim") { damage += 10; }
        }

        public override void Hurt(bool pvp, bool quiet, double damage, int hitDirection, bool crit, int cooldownCounter)
        {
            VoidCount = 0;
            YouGotHitLMAO = true;
        }
        public override void SyncPlayer(int toWho, int fromWho, bool newPlayer)
        {
            ModPacket packet = Mod.GetPacket();
            packet.Write((byte)GenericBlackSwordNetCodeHandle.MessageType.ItIsMEXinim);
            packet.Write((byte)Player.whoAmI);
            packet.Write(HowDIDyouFigureThatOut);
            packet.Send(toWho, fromWho);
        }
        public void ReceivePlayerSync(BinaryReader reader)
        {
            HowDIDyouFigureThatOut = reader.ReadByte();
        }
        public override void clientClone(ModPlayer clientClone)
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
    partial class GenericBlackSwordNetCodeHandle
    {
        internal enum MessageType : byte
        {
            ItIsMEXinim
        }
        public void HandlePacket(BinaryReader reader, int whoAmI)
        {
            MessageType msgType = (MessageType)reader.ReadByte();
            if (msgType == MessageType.ItIsMEXinim)
            {
                byte playernumber = reader.ReadByte();
                GenericBlackSwordPlayer HowDIDyouTho = Main.player[playernumber].GetModPlayer<GenericBlackSwordPlayer>();
                HowDIDyouTho.HowDIDyouFigureThatOut = reader.ReadInt32();
                if (Main.netMode == NetmodeID.Server)
                {
                    HowDIDyouTho.SyncPlayer(-1, whoAmI, false);
                }
            }
        }
    }
    public class DropGenericBlackSword : GlobalNPC
    {
        public override void ModifyNPCLoot(NPC npc, NPCLoot npcLoot)
        {
            if(npc.type == NPCID.KingSlime)
            {
                npcLoot.Add(ItemDropRule.ByCondition(new GenericBlackSwordConditionRule(), ModContent.ItemType<GenericBlackSword>()));
            }
        }

        public override void OnKill(NPC npc)
        {
            Player player = Main.LocalPlayer;
            if (player.poisoned && player.ZoneOldOneArmy && player.ZoneGraveyard && player.ZoneUnderworldHeight && npc.type == NPCID.KingSlime && Main.masterMode)
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
                    info.player.poisoned 
                    && info.player.ZoneOldOneArmy 
                    && info.player.ZoneGraveyard 
                    && info.player.ZoneUnderworldHeight 
                    && info.IsMasterMode;
            }
            return false;
        }
        public bool CanShowItemDropInUI() => true;
        public string GetConditionDescription() => "Secret condition";
    }
}