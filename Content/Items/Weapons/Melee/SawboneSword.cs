using Terraria;
using Terraria.ID;
using CCMod.Utils;
using CCMod.Common;
using Terraria.ModLoader;
using Terraria.GameContent;
using Microsoft.Xna.Framework;
using Terraria.DataStructures;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System;

namespace CCMod.Content.Items.Weapons.Melee
{
    internal class SawboneSword : ModItem, IMadeBy
    {
        public string CodedBy => "LowQualityTrash-Xinim";
        public string SpritedBy => "razorxt";

        public override void SetDefaults()
        {
            Item.width = 54;
            Item.height = 66;
            Item.DefaultToSword(30, 15, 5, true);
            Item.rare = ItemRarityID.Orange;
            Item.shoot = ProjectileID.Ale;
        }
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            Vector2 direction = new Vector2(70 * player.direction, 0);
            for (int i = 0; i < 3; i++)
            {
                Projectile.NewProjectile(Item.GetSource_FromThis(), position + direction * (i + 1)
                    , Vector2.Zero, ModContent.ProjectileType<SawboneSwordSpawnSpikeP>(), damage, knockback, player.whoAmI, i, player.direction);
            }
            return false;
        }
        public override void MeleeEffects(Player player, Rectangle hitbox)
        {
            Vector2 hitboxCenter = new Vector2(hitbox.X, hitbox.Y);

            int dust = Dust.NewDust(hitboxCenter, hitbox.Width, hitbox.Height, DustID.Blood, 0, 0, 0, Color.Red, Main.rand.NextFloat(1.25f, 1.75f));
            Main.dust[dust].noGravity = true;
        }
    }
    public class SawboneSwordSpawnSpikeP : ModProjectile
    {
        public override string Texture => "CCMod/Content/Items/Weapons/Melee/SawboneSword";
        public override void SetDefaults()
        {
            Projectile.width = 30;
            Projectile.height = 30;
            Projectile.penetrate = -1;
            Projectile.hide = true;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = true;
        }
        int timer = 0;
        public override void AI()
        {
            if (timer >= 20 + 10 * Projectile.ai[0])
            {
                Projectile.velocity.Y = 10;
            }
            else
            {
                timer++;
            }
        }
        public override void Kill(int timeLeft)
        {
            for (int i = 0; i < 50; i++)
            {
                int dust = Dust.NewDust(Projectile.Center + new Vector2(0, 20), 0, 0, DustID.Blood, 0, 0, 0, default, Main.rand.NextFloat(1.3f, 2.35f));
                Main.dust[dust].noGravity = true;
                Vector2 dustVelocity = Main.rand.NextVector2Unit(-MathHelper.PiOver2, MathHelper.PiOver4 * (Main.rand.NextFloat(.5f, .7f) + i * .03f)) * Main.rand.Next(3, 15);
                dustVelocity.X *= Projectile.ai[1];
                dustVelocity.Y *= 1 + Projectile.ai[0] * .1f;
                Main.dust[dust].velocity = dustVelocity;
            }
            Projectile.NewProjectile(Projectile.GetSource_FromThis(), 
                Projectile.Center + new Vector2(0, 25 /*+ (25 * Projectile.ai[0])*/),
                new Vector2(0, -2f /*+ (-2 * .25f * Projectile.ai[0])*/ ), 
                ModContent.ProjectileType<SawboneSwordP>(), 
                Projectile.damage, 0f, Projectile.owner, Projectile.ai[0], Projectile.ai[1]);
        }
    }
    public class SawboneSwordP : ModProjectile
    {
        public override string Texture => "CCMod/Content/Items/Weapons/Melee/SawboneSword";
        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.DontAttachHideToAlpha[Projectile.type] = true;
        }
        public override void SetDefaults()
        {
            Projectile.width = 30;
            Projectile.height = 70;
            Projectile.penetrate = -1;
            Projectile.friendly = true;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
            Projectile.timeLeft = 30;
            Projectile.DamageType = DamageClass.Melee;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 20;
            Projectile.hide = true;
        }
        public override void DrawBehind(int index, List<int> behindNPCsAndTiles, List<int> behindNPCs, List<int> behindProjectiles, List<int> overPlayers, List<int> overWiresUI)
        {
            behindNPCsAndTiles.Add(index);
        }
        public override void AI()
        {
            Projectile.velocity -= Projectile.velocity * .1f;
            Projectile.spriteDirection = directionTo;
        }
        int firstframe = 0;
        int directionTo = 1;
        public override bool PreAI()
        {
            if (firstframe == 0)
            {
                directionTo = (int)Projectile.ai[1];
                //This is scaling projectile size, but it is so stupid that it somehow fuck the projectile position, i probably know why, but too lazy to fix it
                //Projectile.scale += Projectile.ai[0] * .25f;
                //Projectile.Hitbox = new Rectangle(
                //    (int)(Projectile.position.X + (Projectile.width - Projectile.scale * Projectile.width)),
                //    (int)(Projectile.position.Y + (Projectile.height - Projectile.scale * Projectile.height) * 2f),
                //    (int)(Projectile.width * Projectile.scale),
                //    (int)(Projectile.height * Projectile.scale));
                //DrawOffsetX = (int)(10 * Projectile.scale) * directionTo;
                firstframe++;
            }
            float pi = directionTo == 1 ? MathHelper.PiOver4 : MathHelper.PiOver2 + MathHelper.PiOver4;
            Projectile.rotation = Projectile.velocity.ToRotation() + pi;
            return base.PreAI();
        }
        public override void Kill(int timeLeft)
        {
            for (int i = 0; i < 30; i++)
            {
                Dust.NewDust(Projectile.Center - new Vector2(0, 60), 30, 60, DustID.Blood, 0, .5f, 0, Color.Red, Main.rand.NextFloat(1.25f, 1.75f));
                Dust.NewDust(Projectile.Center - new Vector2(0, 60), 30, 60, DustID.Bone, 0, .5f, 0, default, Main.rand.NextFloat(1f, 1.25f));
            }
        }
    }
}
