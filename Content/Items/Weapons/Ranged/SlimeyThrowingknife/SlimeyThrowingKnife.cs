using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.DataStructures;
using Microsoft.Xna.Framework;
using CCMod.Common;
using System.Collections.Generic;
using CCMod.Common.ProjectileAI;

namespace CCMod.Content.Items.Weapons.Ranged.SlimeyThrowingknife
{
    internal class SlimeyThrowingKnife : ModItem, IMadeBy
    {
        public string CodedBy => "LowQualityTrash-Xinim";
        public string SpritedBy => "PixelGaming";

        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("W.I.P");
        }
        public override void SetDefaults()
        {
            Item.width = 10;
            Item.height = 24;

            Item.useTime = 15;
            Item.useAnimation = 15;

            Item.damage = 15;
            Item.knockBack = 2f;

            Item.rare = ItemRarityID.Pink;

            Item.shoot = ModContent.ProjectileType<SlimeyThrowingKnifeP>();
            Item.shootSpeed = 15;

            Item.useStyle = ItemUseStyleID.Swing;
            Item.DamageType = DamageClass.Ranged;

            Item.noUseGraphic = true;
            Item.noMelee = true;
            Item.autoReuse = true;
        }
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            return base.Shoot(player, source, position, velocity, type, damage, knockback);
        }
        public override void AddRecipes()
        {
            CreateRecipe(30)
                .AddIngredient(ItemID.Gel, 10)
                .AddIngredient(ItemID.Shuriken)
                .AddTile(TileID.Solidifier)
                .Register();
        }
    }

    class SlimeyThrowingKnifeP : ModProjectile
    {
        public override string Texture => "CCMod/Content/Items/Weapons/Ranged/SlimeyThrowingknife/SlimeyThrowingKnife";

        public override void SetDefaults()
        {
            Projectile.width = 10;
            Projectile.height = 10;
            Projectile.penetrate = 3;
            Projectile.timeLeft = 75;
            Projectile.friendly = true;
            Projectile.tileCollide = true;
            DrawOffsetX = -2;
            DrawOriginOffsetY = -10;
            Projectile.DamageType = DamageClass.Ranged;
        }

        int timerBeforeRotate = 0;
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
                    Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver2;
                    Projectile.velocity.Y += timerBeforeRotate >= 10 && Projectile.velocity.Y <= 18 ? .75f : 0;
                }
                timerBeforeRotate++;
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
        public override void Kill(int timeLeft)
        {
            Vector2 KnifeTip = (Projectile.rotation + MathHelper.PiOver2).ToRotationVector2() * 7;
            Vector2 KnifeHandle = -KnifeTip;
            for (int i = 0; i < 15; i++)
            {
                int dust = Dust.NewDust(Projectile.Center + KnifeTip * Main.rand.NextFloat(), 0, 0, DustID.t_Slime, 0, 0, 125, new Color(0, 153, 255, 125), Main.rand.NextFloat(.75f, 1));
                int dust2 = Dust.NewDust(Projectile.Center + KnifeHandle * Main.rand.NextFloat(), 0, 0, DustID.t_Slime, 0, 0, 125, new Color(0, 153, 255, 125), Main.rand.NextFloat(.75f, 1));
                Main.dust[dust].noGravity = true; Main.dust[dust2].noGravity = true;
                Main.dust[dust].velocity = new Vector2(Main.rand.NextFloat(-1, 1), Main.rand.NextFloat(1.5f)); Main.dust[dust2].velocity = new Vector2(Main.rand.NextFloat(-1, 1), Main.rand.NextFloat(1.5f));
            }
        }
        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
            return StickToEnemyAI.CollisionBetweenEnemyAndProjectile(projHitbox, targetHitbox);
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
            Projectile.DrawBehindNPCandOtherProj(IsStickingToTarget, index, behindNPCsAndTiles, behindNPCs, behindProjectiles);
        }
        public override void ModifyHitNPC(NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
        {
            Projectile.OnHitNPCwithProjectile(target, out bool IsStickingToTarget, out int TargetWhoAmI);
            this.IsStickingToTarget = IsStickingToTarget;
            this.TargetWhoAmI = TargetWhoAmI;
        }
    }
}
