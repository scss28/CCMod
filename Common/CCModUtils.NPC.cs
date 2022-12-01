using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CCMod.Common
{
    public partial class CCModUtils
    {
        public static void ForeachNPCInRange(Vector2 center, float range, Action<NPC> npcAction)
        {
            foreach (NPC npc in Main.npc)
            {
                Vector2 dir = center.DirectionTo(npc.Center);
                if (Collision.CheckAABBvLineCollision(npc.TopLeft, npc.Size, center, center + dir * range))
                {
                    npcAction.Invoke(npc);
                }
            }
        }
    }
}
