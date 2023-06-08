using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.DataStructures;
using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;

namespace CCMod.Common.ProjectileComponentSystem
{
    internal class ProjectileEntityHooks
    {
        #region MULTIPLE

        public delegate void OnSpawnHook(Projectile projectile, IEntitySource source);
        public event OnSpawnHook OnSpawn;
        public void InvokeOnSpawnEvent(Projectile projectile, IEntitySource source)
        {
            OnSpawn?.Invoke(projectile, source);
        }

        public delegate void AIHook(Projectile projectile);
        public event AIHook AI;
        public void InvokeAIEvent(Projectile projectile)
        {
            AI?.Invoke(projectile);
        }

        public delegate void ModifyHitNPCHook(Projectile projectile, NPC target, ref NPC.HitModifiers modifiers);
        public event ModifyHitNPCHook ModifyHitNPC;
        public void InvokeModifyHitNPCEvent(Projectile projectile, NPC target, ref NPC.HitModifiers modifiers)
        {
            ModifyHitNPC?.Invoke(projectile, target, ref modifiers);
        }

        public delegate void PostDrawHook(Projectile projectile, Color lightColor);
        public event PostDrawHook PostDraw;
        public void InvokePostDrawEvent(Projectile projectile, Color lightColor)
        {
            PostDraw?.Invoke(projectile, lightColor);
        }

        #endregion

        #region SINGULAR

        public delegate bool OnTileCollideHook(Projectile projectile, Vector2 oldVelocity);
        public OnTileCollideHook OnTileCollide { get; set; }

        public delegate Color? GetAlphaHook(Projectile projectile, Color lightColor);
        public GetAlphaHook GetAlpha { get; set; }

        public delegate bool PreDrawHook(Projectile projectile, ref Color lightColor);
        public PreDrawHook PreDraw { get; set; }

        #endregion
    }
}
