using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

using Terraria;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.PlayerDrawLayer;

namespace CCMod.Utils
{
    partial class CCModUtils
    {
        /// <summary>
        /// Array segment skipping the last projectile in <see cref="Main.projectile"/>.<br/>
        /// Can be used in <see langword="foreach"/> statements.
        /// </summary>
        public static ArraySegment<Projectile> ProjectileForeach => new ArraySegment<Projectile>(Main.projectile, 0, Main.projectile.Length - 1);
        public static void EasyDraw(this Projectile projectile, Color color, Vector2? position = null, float? rotation = null, Vector2? origin = null, float? scale = null, SpriteEffects? spriteEffects = null, Texture2D altTex = null)
        {
            Texture2D tex = altTex ?? TextureAssets.Projectile[projectile.type].Value;

            int frameHeight = tex.Height / Main.projFrames[projectile.type];
            Rectangle rect = new Rectangle(0, frameHeight * projectile.frame, tex.Width, frameHeight);

            Main.spriteBatch.Draw(
                tex,
                (position ?? projectile.Center) - Main.screenPosition,
                rect,
                color * ((255f - Math.Clamp(projectile.alpha, 0, 255)) / 255f),
                rotation ?? projectile.rotation,
                origin ?? (rect.Size() * 0.5f),
                scale ?? projectile.scale,
                spriteEffects ?? (projectile.spriteDirection == -1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None),
                0
                );
        }
        public static void EasyDrawAfterImage(this Projectile projectile, Color? color = null, Vector2[] oldPos = null, Vector2? origin = null, SpriteEffects? spriteEffects = null, Texture2D altTex = null)
        {
            Texture2D tex = altTex ?? TextureAssets.Projectile[projectile.type].Value;

            int frameHeight = tex.Height / Main.projFrames[projectile.type];
            Rectangle rect = new Rectangle(0, frameHeight * projectile.frame, tex.Width, frameHeight);

            Vector2[] positions = oldPos ?? projectile.oldPos;
            for (int i = 0; i < positions.Length; i++)
            {
                Vector2 position = positions[i];

                Main.spriteBatch.Draw(
                    tex,
                    position + (oldPos is null ? projectile.Size * 0.5f : Vector2.Zero) - Main.screenPosition,
                    rect,
                    (color ?? Color.White) * ((float)(positions.Length - (i + 1)) / positions.Length),
                    projectile.rotation,
                    origin ?? rect.Size() * 0.5f,
                    projectile.scale,
                    spriteEffects ?? (projectile.spriteDirection == -1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None),
                    0
                );
            }
        }

        //LowQualityTrash-Xinim Code start
        public static void EvenEasierDrawAfterImage(this Projectile projectile, Color lightColor, float ScaleAccordinglyToLength = 0)
        {
            Main.instance.LoadProjectile(projectile.type);
            Texture2D texture = TextureAssets.Projectile[projectile.type].Value;
            Vector2 origin = new Vector2(texture.Width * 0.5f, projectile.height * 0.5f);
            for (int k = 0; k < projectile.oldPos.Length; k++)
            {
                Vector2 drawPos = projectile.oldPos[k] - Main.screenPosition + origin + new Vector2(0f, projectile.gfxOffY);
                Color color = projectile.GetAlpha(lightColor) * ((projectile.oldPos.Length - k) / (float)projectile.oldPos.Length);
                Main.EntitySpriteDraw(texture, drawPos, null, color, projectile.rotation, origin, projectile.scale - k * ScaleAccordinglyToLength, SpriteEffects.None, 0);
            }
        }
        public static Vector2 LimitingVelocity(this Vector2 velocity, float limited)
        {
            velocity.X = Math.Clamp(velocity.X, -limited, limited);
            velocity.Y = Math.Clamp(velocity.Y, -limited, limited);
            return velocity;
        }

        public static bool IsVelocityLimitReached(this Vector2 velocity, float limited)
        {
            if (Math.Abs(Math.Clamp(velocity.X, -limited, limited)) >= limited) return true;
            if (Math.Abs(Math.Clamp(velocity.Y, -limited, limited)) >= limited) return true;
            return false;
        }

        public static Vector2 Vector2EvenlyDistribute(this Vector2 Vec2ToRotate, float ProjectileAmount, float rotation, int i)
        {
            if (ProjectileAmount > 1)
            {
                rotation = MathHelper.ToRadians(rotation);
                return Vec2ToRotate.RotatedBy(MathHelper.Lerp(rotation * .5f, rotation * -.5f, i / (ProjectileAmount - 1f)));
            }
            return Vec2ToRotate;
        }

        /// <summary>
        /// This method will go through and look for active enemy, recommend to not over use this
        /// </summary>
        /// <param name="position">current position of a object</param>
        /// <param name="distance">the search distance</param>
        /// <returns>True or False</returns>
        public static bool LookforHostileNPC(Vector2 position, float distance)
        {
            for (int i = 0; i < Main.maxNPCs; i++)
            {
                if (Main.npc[i] != null && Main.npc[i].active)
                {
                    if (CompareSquareFloatValue(position, Main.npc[i].Center,distance))
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        /// <summary>
        /// This method will go through and look for active enemy, recommend to not over use this
        /// </summary>
        /// <param name="position">current position of a object</param>
        /// <param name="distance">the search distance</param>
        /// <param name="enemyPos">enemy position, if no enemy is found then return Vector2(0,0)</param>
        /// <returns>True or false and enemy position</returns>
        public static bool LookforHostileNPC(Vector2 position, float distance, out Vector2 enemyPos)
        {
            for (int i = 0; i < Main.maxNPCs; i++)
            {
                if (Main.npc[i] != null && Main.npc[i].active)
                {
                    if (CompareSquareFloatValue(position, Main.npc[i].Center, distance))
                    {
                        enemyPos = Main.npc[i].Center;
                        return true;
                    }
                }
            }
            enemyPos = Vector2.Zero;
            return false;
        }

        /// <summary>
        /// Go through all projectiles in Main.projectile[] <br/>
        /// Check if the projectile is the type that you looking for<br/>
        /// and if it's position to parameter position is smaller than parameter distance
        /// </summary>
        /// <param name="position">position want to check</param>
        /// <param name="type">type of projectile</param>
        /// <param name="distance">distance to check</param>
        /// <returns>Return true if it's type equal type you look for and its distance to the position is smaller than distance<br/>
        /// otherwise return false</returns>
        public static bool LookForProjectile(Vector2 position, int type, float distance)
        {
            for (int i = 0; i < Main.maxProjectiles; i++)
            {
                if (Main.projectile[i] != null
                    && Main.projectile[i].active
                    && Main.projectile[i].type == type)
                {
                    if (CompareSquareFloatValue(Main.projectile[i].Center,position,distance)) return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Go through all projectiles in Main.projectile[] <br/>
        /// Look for itself and check if there are 2 of its that the distance between 2 of its is smaller than parameter distance
        /// </summary>
        /// <param name="projectile"></param>
        /// <param name="distance">distance to check</param>
        /// <param name="amountofItself">amount require</param>
        /// <returns>Return true if there are more than 2 and its distance to the other it is smaller than distance<br/>
        /// otherwise return false</returns>
        public static bool LookForProjectile(this Projectile projectile, float distance, int amountofItself = 0)
        {
            int count = 0;
            for (int i = 0; i < Main.maxProjectiles; i++)
            {
                if (Main.projectile[i] != null
                    && Main.projectile[i].active
                    && Main.projectile[i].type == projectile.type)
                {
                    if (CompareSquareFloatValue(projectile.Center, Main.projectile[i].Center, distance)) count++;
                    if (count >= 2 + amountofItself) return true;
                }
            }
            return false;
        }
        private static bool CompareSquareFloatValue(Vector2 pos1, Vector2 pos2, float Distance) => Vector2.DistanceSquared(pos1, pos2) <= Distance*Distance;
        //LowQualityTrash-Xinim Code end
    }
}
