using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System;

namespace CCMod.Common.ProjectileAI
{
    /// <summary>
    /// for those who want to use this spagetti code i took from example javaline code and try to make it easy to use
    /// add this<br/>
    /// float ai1 = 0,ai2 = 0;
    ///public bool IsStickingToTarget<br/>
    ///{<br/>
    ///    get => ai1 == 1f;<br/>
    ///    set => ai1 = value ? 1f : 0f;<br/>
    ///}<br/>
    ///public int TargetWhoAmI<br/>
    ///{<br/>
    ///    get => (int)ai2;<br/>
    ///    set => ai2 = value;<br/>
    ///}<br/>
    ///The example javalin projectile code despite have a lot of comment explaining, they are still very hard to understand<br/>
    ///Thank to weird naming and weird casting, if anyone smarter than me or know more in and out, please fix this<br/>
    ///I have near to 0 idea what happen here, I will update this time to time to make it easier to use or to understand<br/>
    ///For now, deal with messy code
    /// </summary>
    static class StickToEnemyAI
    {
        /// <summary>
        /// add this to draw behind hook
        /// </summary>
        /// <param name="projectile"></param>
        /// <param name="isStickingToTarget"></param>
        /// <param name="index"></param>
        /// <param name="behindNPCsAndTiles"></param>
        /// <param name="behindNPCs"></param>
        /// <param name="behindProjectiles"></param>
        public static void DrawBehindNPCandOtherProj(this Projectile projectile, bool isStickingToTarget, int index, List<int> behindNPCsAndTiles, List<int> behindNPCs, List<int> behindProjectiles)
        {
            if (isStickingToTarget) // or if(isStickingToTarget) since we made that helper method.
            {
                int npcIndex = (int)projectile.ai[1];
                if (npcIndex >= 0 && npcIndex < 200 && Main.npc[npcIndex].active)
                {
                    if (Main.npc[npcIndex].behindTiles)
                    {
                        behindNPCsAndTiles.Add(index);
                    }
                    else
                    {
                        behindNPCs.Add(index);
                    }

                    return;
                }
            }
            // Since we aren't attached, add to this list
            behindProjectiles.Add(index);
        }
        /// <summary>
        /// add this to colliding hook
        /// </summary>
        /// <param name="projHitbox"></param>
        /// <param name="targetHitbox"></param>
        /// <returns></returns>
        public static bool CollisionBetweenEnemyAndProjectile(Rectangle projHitbox, Rectangle targetHitbox)
        {
            // Inflate some target hitboxes if they are beyond 8,8 size
            if (targetHitbox.Width > 8 && targetHitbox.Height > 8)
            {
                targetHitbox.Inflate(-targetHitbox.Width / 8, -targetHitbox.Height / 8);
            }
            // Return if the hitboxes intersects, which means the javelin collides or not
            return projHitbox.Intersects(targetHitbox);
        }

        private const int MAX_STICKY_JAVELINS = 6; // This is the max. amount of javelins being able to attach
        private static readonly Point[] _stickingJavelins = new Point[MAX_STICKY_JAVELINS]; // The point array holding for sticking javelins
        /// <summary>
        /// add this to ModifyHitNPC
        /// </summary>
        /// <param name="projectile"></param>
        /// <param name="target"></param>
        /// <param name="IsStickingToTarget"></param>
        /// <param name="TargetWhoAmI"></param>
        public static void OnHitNPCwithProjectile(this Projectile projectile, NPC target, out bool IsStickingToTarget, out int TargetWhoAmI)
        {
            IsStickingToTarget = true; // we are sticking to a target
            TargetWhoAmI = target.whoAmI; // Set the target whoAmI
            projectile.velocity = (target.Center - projectile.Center) * 0.75f; // Change velocity based on delta center of targets (difference between entity centers)
            projectile.netUpdate = true; // netUpdate this javelin
            projectile.damage = 0; 
            // It is recommended to split your code into separate methods to keep code clean and clear
            projectile.UpdateStickyJavelins(IsStickingToTarget, TargetWhoAmI ,target);
        }

        private static void UpdateStickyJavelins(this Projectile projectile,bool IsStickingToTarget, int TargetWhoAmI , NPC target)
        {
            int currentJavelinIndex = 0; // The javelin index
            for (int i = 0; i < Main.maxProjectiles; i++) // Loop all projectiles
            {
                Projectile currentProjectile = Main.projectile[i];
                if (i != projectile.whoAmI // Make sure the looped projectile is not the current javelin
                    && currentProjectile.active // Make sure the projectile is active
                    && currentProjectile.owner == Main.myPlayer // Make sure the projectile's owner is the client's player
                    && currentProjectile.type == projectile.type // Make sure the projectile is of the same type as this javelin
                    && IsStickingToTarget // the previous pattern match allows us to use our properties
                    && TargetWhoAmI == target.whoAmI)
                {
                    _stickingJavelins[currentJavelinIndex++] = new Point(i, currentProjectile.timeLeft); // Add the current projectile's index and timeleft to the point array
                    if (currentJavelinIndex >= _stickingJavelins.Length)  // If the javelin's index is bigger than or equal to the point array's length, break
                        break;
                }
            }

            // Remove the oldest sticky javelin if we exceeded the maximum
            if (currentJavelinIndex >= MAX_STICKY_JAVELINS)
            {
                int oldJavelinIndex = 0;
                // Loop our point array
                for (int i = 1; i < MAX_STICKY_JAVELINS; i++)
                {
                    // Remove the already existing javelin if it's timeLeft value (which is the Y value in our point array) is smaller than the new javelin's timeLeft
                    if (_stickingJavelins[i].Y < _stickingJavelins[oldJavelinIndex].Y)
                    {
                        oldJavelinIndex = i; // Remember the index of the removed javelin
                    }
                }
                // Remember that the X value in our point array was equal to the index of that javelin, so it's used here to kill it.
                Main.projectile[_stickingJavelins[oldJavelinIndex].X].Kill();
            }
        }
        /// <summary>
        /// can be add to ModifyHitNPC or AI hook
        /// </summary>
        /// <param name="projectile"></param>
        /// <param name="TargetWhoAmI"></param>
        public static void StickyAI(this Projectile projectile, int TargetWhoAmI)
        {
            // These 2 could probably be moved to the ModifyNPCHit hook, but in vanilla they are present in the AI
            projectile.ignoreWater = true; // Make sure the projectile ignores water
            projectile.tileCollide = false; // Make sure the projectile doesn't collide with tiles anymore
            const int aiFactor = 15; // Change this factor to change the 'lifetime' of this sticking javelin
            projectile.localAI[0] += 1f;

            // Every 30 ticks, the javelin will perform a hit effect
            bool hitEffect = projectile.localAI[0] % 30f == 0f;
            if (projectile.localAI[0] >= 60 * aiFactor || TargetWhoAmI < 0 || TargetWhoAmI >= 200)
            { // If the index is past its limits, kill it
                projectile.Kill();
            }
            else if (Main.npc[TargetWhoAmI].active && !Main.npc[TargetWhoAmI].dontTakeDamage)
            { // If the target is active and can take damage
              // Set the projectile's position relative to the target's center
                projectile.Center = Main.npc[TargetWhoAmI].Center - projectile.velocity * 2f;
                projectile.gfxOffY = Main.npc[TargetWhoAmI].gfxOffY;
                if (hitEffect)
                { // Perform a hit effect here
                    Main.npc[TargetWhoAmI].HitEffect(0, 1.0);
                }
            }
            else
            { // Otherwise, kill the projectile
                projectile.Kill();
            }
        }
    }
}
