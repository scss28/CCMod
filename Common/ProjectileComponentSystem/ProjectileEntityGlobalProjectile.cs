using Microsoft.Xna.Framework;
using Mono.Cecil;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;

namespace CCMod.Common.ProjectileComponentSystem
{
    internal class ProjectileEntityGlobalProjectile : GlobalProjectile
    {
        public override bool InstancePerEntity => true;
        private ProjectileEntityHooks ProjectileHooks { get; } = new ProjectileEntityHooks();

		public void AddComponent(IProjectileComponent projectileComponent)
		{
			projectileComponent.RegisterHooks(ProjectileHooks);
		}

		private IEntitySource source;
        public override void OnSpawn(Projectile projectile, IEntitySource source)
        {
            this.source = source;
        }

        private bool runOnSpawnHook = false;
        public override void AI(Projectile projectile)
        {
            if (!runOnSpawnHook)
            {
                ProjectileHooks.InvokeOnSpawnEvent(projectile, source);
                runOnSpawnHook = true;
            }

            ProjectileHooks.InvokeAIEvent(projectile);
        }

        public override void ModifyHitNPC(Projectile projectile, NPC target, ref NPC.HitModifiers modifiers)
        {
            ProjectileHooks.InvokeModifyHitNPCEvent(projectile, target, ref modifiers);
        }

        public override void PostDraw(Projectile projectile, Color lightColor)
        {
            ProjectileHooks.InvokePostDrawEvent(projectile, lightColor);
        }

        public override bool OnTileCollide(Projectile projectile, Vector2 oldVelocity)
        {
            return ProjectileHooks.OnTileCollide?.Invoke(projectile, oldVelocity) ?? base.OnTileCollide(projectile, oldVelocity);
        }

        public override Color? GetAlpha(Projectile projectile, Color lightColor)
        {
            return ProjectileHooks.GetAlpha?.Invoke(projectile, lightColor) ?? base.GetAlpha(projectile, lightColor);
        }

        public override bool PreDraw(Projectile projectile, ref Color lightColor)
        {
            return ProjectileHooks.PreDraw?.Invoke(projectile, ref lightColor) ?? base.PreDraw(projectile, ref lightColor);
        }
    }

    internal static class ProjectileEntityExtensions
    {
        public static Projectile AddComponent(this Projectile projectile, IProjectileComponent projectileComponent)
        {
            projectile.GetGlobalProjectile<ProjectileEntityGlobalProjectile>().AddComponent(projectileComponent);
            return projectile;
        }

        public static ModProjectile AddComponent(this ModProjectile modProjectile, IProjectileComponent projectileComponent)
        {
            modProjectile.Projectile.AddComponent(projectileComponent);
            return modProjectile;
        }
    }
}
