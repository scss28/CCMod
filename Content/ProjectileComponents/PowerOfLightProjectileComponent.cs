using CCMod.Common.ProjectileComponentSystem;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;

namespace CCMod.Content.ProjectileComponents
{
    internal record PowerOfLightProjectileComponent(float R = 1f, float G = 1f, float B = 1f) : IProjectileComponent
    {
        public void RegisterHooks(ProjectileEntityHooks hooks)
        {
            hooks.AI += (projectile) => Lighting.AddLight(projectile.position, R, G, B);

            hooks.GetAlpha = (projectile, lightColor) => lightColor * 0.3f;

            hooks.ModifyHitNPC += (Projectile projectile, NPC target, ref NPC.HitModifiers modifiers) =>
            {
                modifiers.SetCrit();
                modifiers.CritDamage *= 2;
            };
        }
    }
}
