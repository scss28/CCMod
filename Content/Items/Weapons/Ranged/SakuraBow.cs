using CCMod.Common;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace CCMod.Content.Items.Weapons.Ranged
{
    // Bad weapon so feel free to rework
    public class SakuraBow : ModItem, IMadeBy
    {
        public string CodedBy => "sucss";

        public string SpritedBy => "person_";

        public override void SetDefaults()
        {
            Item.width = 46;
            Item.height = 46;

            Item.crit = 7;
            Item.damage = 17;
            Item.knockBack = 4f;
            Item.DamageType = DamageClass.Ranged;

            Item.UseSound = SoundID.Item102;
            Item.useTime = 25;
            Item.useAnimation = Item.useTime;
            Item.autoReuse = true;

            Item.useStyle = ItemUseStyleID.Shoot;

            Item.noMelee = true;

            Item.value = Item.sellPrice(0, 0, 60, 0);
            Item.rare = ItemRarityID.Pink;

            Item.shoot = ProjectileID.WoodenArrowFriendly;
            Item.useAmmo = AmmoID.Arrow;
            Item.shootSpeed = 16f;
        }

        public override Vector2? HoldoutOffset()
        {
            return Vector2.UnitX * -5;
        }
    }

    public class SakuraBowProjectile : GlobalProjectile
    {
        public override bool InstancePerEntity => true;

        bool shotFromSakura;
        public override void OnSpawn(Projectile projectile, IEntitySource source)
        {
            if (source is EntitySource_ItemUse_WithAmmo itemSource && itemSource.Item.type == ModContent.ItemType<SakuraBow>())
                shotFromSakura = true;
        }

        public override void OnHitNPC(Projectile projectile, NPC target, int damage, float knockback, bool crit)
        {
            if (shotFromSakura)
            {
                float rad = (target.width + target.height);
                Vector2 normVel = projectile.velocity.SafeNormalize(Vector2.Zero);
                Vector2 position = projectile.Center - normVel * rad * 0.5f;
                Projectile.NewProjectile(
                    projectile.GetSource_FromThis(),
                    position,
                    normVel * rad,
                    ModContent.ProjectileType<SakuraSlashProjectile>(),
                    (int)(projectile.damage * 0.5f),
                    projectile.knockBack * 0.5f,
                    projectile.owner
                    );
            }
        }
    }
}
