using CCMod.Common.ProjectileComponentSystem;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;

namespace CCMod.Content.ProjectileComponents
{
    internal record HomingProjectileComponent(float MinimumHomingDistance = 500, float HomingStrenght = 0.05f) : IProjectileComponent
    {
        public void RegisterHooks(ProjectileEntityHooks hooks)
        {
			float homingStrenght = Math.Clamp(HomingStrenght, 0f, 1f);
            float minimumHomingDistanceSquared = MinimumHomingDistance * MinimumHomingDistance;
            hooks.AI += (projectile) => {
                NPC chasedNPC = null;
                float minimumDistance = float.MaxValue;
                foreach (NPC npc in Main.npc)
                {
                    float distanceToNPC = projectile.Center.DistanceSQ(npc.Center);
                    if (!npc.CanBeChasedBy() || distanceToNPC > minimumHomingDistanceSquared || distanceToNPC > minimumDistance)
                    {
                        continue;
                    }

                    chasedNPC = npc;
                    minimumDistance = distanceToNPC;
                }

                if (chasedNPC is null)
                {
                    return;
                }

                projectile.velocity = Vector2.Lerp(
                    projectile.velocity.SafeNormalize(Vector2.Zero), 
                    projectile.Center.DirectionTo(chasedNPC.Center),
					homingStrenght
				) * projectile.velocity.Length();
            };
        }
    }
}
