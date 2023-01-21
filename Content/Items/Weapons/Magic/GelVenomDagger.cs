using Terraria;
using Terraria.ID;
using CCMod.Utils;
using CCMod.Common;
using Terraria.ModLoader;
using Terraria.GameContent;
using System.Collections.Generic;
using Terraria.DataStructures;
using Microsoft.Xna.Framework;
using CCMod.Common.ProjectileAI;
using CCMod.Content.Items.Weapons.Ranged.SlimeyThrowingknife;
using Microsoft.Xna.Framework.Graphics;

namespace CCMod.Content.Items.Weapons.Magic
{
    internal class GelVenomDagger : ModItem, IMadeBy
    {
        public string CodedBy => "LowQualityTrash-Xinim";
        public string SpritedBy => "PixelGaming";
        public string ConceptBy => "LowQualityTrash-Xinim & PixelGaming";

        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("W.I.P");
        }
        public override void SetDefaults()
        {
            Item.width = 12;
            Item.height = 32;

            Item.useTime = 14;
            Item.useAnimation = 14;

            Item.damage = 57;
            Item.knockBack = 3f;
            Item.crit = 8;

            Item.rare = ItemRarityID.Pink;

            Item.shoot = ModContent.ProjectileType<GelVenomDaggerP>();
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
                .AddIngredient(ModContent.ItemType<SlimeyThrowingKnife>(), 100)
                .AddTile(TileID.MythrilAnvil)
                .Register();
        }
    }

    class GelVenomDaggerP : ModProjectile
    {
        public override string Texture => "CCMod/Content/Items/Weapons/Magic/GelVenomDagger";

        public override void SetDefaults()
        {
            Projectile.width = 20;
            Projectile.height = 20;
            Projectile.penetrate = 1;
            Projectile.timeLeft = 100;
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
        public override void ModifyHitNPC(NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
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
            return StickToEnemyAI.CollisionBetweenEnemyAndProjectile(projHitbox, targetHitbox);
        }
        public override void DrawBehind(int index, List<int> behindNPCsAndTiles, List<int> behindNPCs, List<int> behindProjectiles, List<int> overPlayers, List<int> overWiresUI)
        {
            if (Projectile.ai[0] == 1)
            {
                Projectile.DrawBehindNPCandOtherProj(IsStickingToTarget, TargetWhoAmI, index, behindNPCsAndTiles, behindNPCs, behindProjectiles);
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

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            if (Main.rand.NextBool(7))
            {
                Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, Main.rand.NextVector2CircularEdge(-MathHelper.PiOver4, MathHelper.PiOver2) * 2, ModContent.ProjectileType<ToxicBubbleP>(), Projectile.damage, 0, Projectile.owner);
            }
        }

        public override void Kill(int timeLeft)
        {
            for (int i = 0; i < 10; i++)
            {
                int dust = Dust.NewDust(Projectile.Center, 0, 0, DustID.UnholyWater, 0, 0, 0, default, Main.rand.NextFloat(.8f, 1.2f));
                Main.dust[dust].noGravity = true;
                Main.dust[dust].velocity = Main.rand.NextVector2Circular(4, 4);
            }
            if (Main.rand.NextBool(7))
            {
                Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, Main.rand.NextVector2CircularEdge(-MathHelper.PiOver4, MathHelper.PiOver2) * 2, ModContent.ProjectileType<ToxicBubbleP>(), Projectile.damage, 0, Projectile.owner);
            }
        }
    }

    public class ToxicBubbleP : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.alpha = 255;
            Projectile.width = 20;
            Projectile.height = 20;
            Projectile.timeLeft = 300;
            Projectile.friendly = true;
            Projectile.tileCollide = true;
            Projectile.DamageType = DamageClass.Magic;
        }
        public override bool? CanDamage()
        {
            return false;
        }
        int counter = 0;
        public void FadeIn()
        {
            if (Projectile.alpha > 55)
            {
                Projectile.alpha -= 5;
            }
        }
        public override void AI()
        {
            for (int i = 0; i < 2; i++)
            {
                int dust = Dust.NewDust(Projectile.Center, 10, 10, DustID.UnholyWater, 0, 0, 0, default, Main.rand.NextFloat(.8f, 1.2f));
                Main.dust[dust].noGravity = false;
            }
            FadeIn();
            if (counter >= 30)
            {
                if (CCModUtils.LookForProjectile(Projectile.Center, ModContent.ProjectileType<GelVenomDaggerP>(), 30))
                {
                    Projectile.ai[0]++;
                    Projectile.Kill();
                }
            }
            counter++;
            Projectile.velocity.X -= Projectile.velocity.X * .1f;
            Projectile.velocity.Y = -1;
        }
        public override void Kill(int timeLeft)
        {
            for (int i = 0; i < 30; i++)
            {
                int dust = Dust.NewDust(Projectile.Center, 0, 0, DustID.UnholyWater, 0, 0, 0, default, Main.rand.NextFloat(.8f, 1.2f));
                Main.dust[dust].noGravity = true;
                Main.dust[dust].velocity = Main.rand.NextVector2Circular(5, 5);
            }
            float num = 15;
            if (Projectile.ai[0] == 1)
            {
                num *= 2;
            }
            for (int i = 0; i < num; i++)
            {
                Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, Main.rand.NextVector2Circular(3, 3), ModContent.ProjectileType<ToxicDrop>(), (int)(Projectile.damage * .25f), 0, Projectile.owner, Projectile.ai[0]);
            }
        }
    }
    public class ToxicDrop : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.width = 2;
            Projectile.height = 2;
            Projectile.friendly = true;
            Projectile.tileCollide = true;
            Projectile.DamageType = DamageClass.Magic;
            Projectile.penetrate = 3;
            Projectile.extraUpdates = 6;
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 40;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
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
        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            target.AddBuff(BuffID.Venom, Projectile.ai[0] == 1 ? 900 : 120);
        }
        public override Color? GetAlpha(Color lightColor)
        {
            return new Color(170, 20, 200);
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Main.instance.LoadProjectile(Projectile.type);
            Texture2D texture = TextureAssets.Projectile[Projectile.type].Value;

            Vector2 origin = new Vector2(texture.Width * 0.5f, Projectile.height * 0.5f);
            for (int k = 1; k < Projectile.oldPos.Length + 1; k++)
            {
                Vector2 drawPos = Projectile.oldPos[k - 1] - Main.screenPosition + origin + new Vector2(Projectile.gfxOffY);
                Color color = new Color(170, 20, 200, Projectile.alpha);
                Main.EntitySpriteDraw(texture, drawPos, null, color, Projectile.rotation, origin, Projectile.scale, SpriteEffects.None, 0);
            }
            return true;
        }
    }
}