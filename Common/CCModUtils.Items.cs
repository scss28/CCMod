using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CCMod.Common
{
    partial class CCModUtils
    {
        public static void DefaultToSword(this Item item, int damage, int swingTime, float knockback = 0, bool autoReuse = true)
        {
            item.useStyle = ItemUseStyleID.Swing;
            item.DamageType = DamageClass.Melee;
            item.damage = damage;
            item.useTime = item.useAnimation = swingTime;
            item.knockBack = knockback;
            item.autoReuse = autoReuse;
            item.UseSound = SoundID.Item1;
        }
    }
}
