using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

using Terraria;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace CCMod.Content.Items.Weapons.Melee
{
    public class HardstoneBladeHeldProj : ModProjectile
    {
        public override string Texture => base.Texture.Replace("HeldProj", string.Empty);

        public override void SetDefaults()
        {
            Projectile.width = 0;
            Projectile.height = 0;

            Projectile.aiStyle = -1;

            Projectile.DamageType = DamageClass.Melee;

            Projectile.penetrate = -1;

            Projectile.friendly = true;
            Projectile.hostile = false;

            Projectile.ignoreWater = true;
            Projectile.tileCollide = false;

            Projectile.timeLeft = 9999;

            Projectile.netImportant = true;
            Projectile.extraUpdates = 2;

            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 999;
        }

        Player Player => Main.player[Projectile.owner];
        ref float RotationToMouse => ref Projectile.ai[0];
        static int swingDirection = 1;
        public override void AI()
        {
            if (Player.ItemAnimationEndingOrEnded || Player.HeldItem.type != ModContent.ItemType<HardstoneBlade>())
            {
                swingDirection = -swingDirection;
                Projectile.Kill();
                return;
            }

            Player.heldProj = Projectile.whoAmI;

            // Here we set the center around which the sword is gonna rotate and add some offset to it to set it where the Player's shoulder is.
            Projectile.Center = Player.RotatedRelativePoint(Player.MountedCenter) + new Vector2(Player.direction * -3, -1);

            // Now we want to get the rotation of direction to mouse but we only want this to happen for the Player holding this projectile.
            if (Main.myPlayer == Player.whoAmI)
                RotationToMouse = Player.Center.DirectionTo(Main.MouseWorld).ToRotation();

            // Here using some math we calculate the current rotation of the projectile depending on the progress of the item animation (you can use desmos.com or any other graphing calculator to visualise how this is gonna behave).
            // ps. Player.itemAnimation goes down from Player.itemAnimationMax to 0 during item use.
            float arc = MathHelper.Pi * 1.5f;
            Projectile.rotation = RotationToMouse - Player.direction * swingDirection * (0.5f * arc - 0.25f * arc * MathF.Pow(MathF.Cos(MathHelper.Pi * Player.itemAnimation / Player.itemAnimationMax) + 1, 2));

            // Set player arm rotation.
            Player.SetCompositeArmFront(true, Player.CompositeArmStretchAmount.Full, Projectile.rotation - MathHelper.PiOver2);

            // Spawn projectiles
            if (Main.myPlayer == Player.whoAmI && Player.itemAnimation < Player.itemAnimationMax * 0.4f && Player.itemAnimation > Player.itemAnimationMax * 0.3f)
                Projectile.NewProjectile(
                    Projectile.GetSource_FromAI(), 
                    Projectile.Center + Projectile.rotation.ToRotationVector2() * (6 + swordLength * Main.rand.NextFloat()),
                    Player.Center.DirectionTo(Main.MouseWorld) * 9 * Main.rand.NextFloat(0.85f, 1f),
                    ModContent.ProjectileType<HardstoneBladeProjectile>(),
                    (int)(Projectile.damage * 0.3f),
                    0.5f,
                    Projectile.owner
                    );
        }

        // Sword length in this case is around the lenght of the diagonal of the texture.
        const float swordLength = 82f;
        const float swordWidth = 12f;
        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
            Vector2 rotationVector = Projectile.rotation.ToRotationVector2();

            // Offset the start by a bit because of the drawOrigin.
            Vector2 collisionStart = Projectile.Center + rotationVector * 6;
            Vector2 collisionEnd = collisionStart + swordLength * rotationVector;

            // This needs to be like this if we want to specify lineWidth for some reason...
            float x = 0;
            return Collision.CheckAABBvLineCollision(targetHitbox.TopLeft(), targetHitbox.Size(), collisionStart, collisionEnd, swordWidth, ref x);
        }

        
        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D texture = TextureAssets.Projectile[Type].Value;

            // Here we set draw origin to somewhere outside the texture so that Player "holds" the handle correctly.
            // You should experiment with this to get the best result.
            Vector2 drawOrigin = new Vector2(Player.direction == 1 ? -6 : 60, 60);

            // Here we get drawRotation by adding an offset based on where the blade is pointing and player's direction to the projectile's rotation.
            float drawRotation = Projectile.rotation + (Player.direction == 1 ? MathHelper.PiOver4 : MathHelper.Pi * 0.75f);

            // Draw the sword sprite.
            Main.spriteBatch.Draw(
                texture,
                Projectile.Center - Main.screenPosition,
                null,
                lightColor,
                drawRotation,
                drawOrigin,
                Projectile.scale,
                Player.direction == 1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally,
                0
                );

            return false;
        }
    }
}
