﻿using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using CCMod.Common;
using System.Collections.Generic;
using CCMod.Common.ProjectileAI;
using Terraria.DataStructures;

namespace CCMod.Content.Items.Weapons.Ranged.SlimeyShuriken
{
    internal class SlimeyShuriken : ModItem, IMadeBy
    {
        public string CodedBy => "LowQualityTrash-Xinim";
        public string SpritedBy => "PixelGaming";
        public string ConceptBy => "Cohozuna Jr.";

        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("The Blade Feels Like Putty");
        }
        public override void SetDefaults()
        {
            Item.width = 24;
            Item.height = 24;

            Item.useTime = 15;
            Item.useAnimation = 15;

            Item.damage = 19;
            Item.knockBack = .5f;

            Item.rare = ItemRarityID.Green;

            Item.shoot = ModContent.ProjectileType<SlimeyShurikenP>();
            Item.shootSpeed = 17;

            Item.useStyle = ItemUseStyleID.Swing;
            Item.DamageType = DamageClass.Ranged;

            Item.noUseGraphic = true;
            Item.noMelee = true;
            Item.autoReuse = true;
        }
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            Projectile.NewProjectile(source, position, velocity, type, damage, knockback, player.whoAmI,default, player.direction);
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

    class SlimeyShurikenP : ModProjectile
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
                    Projectile.rotation = MathHelper.ToRadians(TimerBeforeGravity * TimerBeforeGravity * .3f * Projectile.ai[1]);
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
            Projectile.DrawBehindNPCandOtherProj(IsStickingToTarget, TargetWhoAmI, index, behindNPCsAndTiles, behindNPCs, behindProjectiles);
        }
        public override void ModifyHitNPC(NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
        {
            target.AddBuff(BuffID.Oiled, 120);
            Projectile.OnHitNPCwithProjectile(target, out bool IsStickingToTarget, out int TargetWhoAmI);
            this.IsStickingToTarget = IsStickingToTarget;
            this.TargetWhoAmI = TargetWhoAmI;
        }
    }
}
