using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using CCMod.Common;
using CCMod.Utils;
using System.Collections.Generic;

namespace CCMod.Content.Items.Weapons.Melee
{
    internal class SawboneSword : ModItem, IMadeBy
    {
        public string CodedBy => "LowQualityTrash-Xinim";

        public string SpritedBy => "razorxt";

        public override void SetDefaults()
        {
            Item.width = 54;
            Item.height = 66;
            Item.DefaultToSword(30,15,5,true);
        }

        public override void MeleeEffects(Player player, Rectangle hitbox)
        {
            Vector2 hitboxCenter = new Vector2(hitbox.X, hitbox.Y);

            int dust = Dust.NewDust(hitboxCenter, hitbox.Width, hitbox.Height, DustID.Blood, 0, 0, 0, Color.Black, Main.rand.NextFloat(1.25f, 1.75f));
            Main.dust[dust].noGravity = true;
        }
    }

    public class SawboneSwordP : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.DontAttachHideToAlpha[Projectile.type] = true;
        }
        public override void SetDefaults()
        {
            Projectile.width = 38;
            Projectile.height = 154;
            Projectile.friendly = true;
            Projectile.penetrate = -1;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
            Projectile.timeLeft = 90;
            Projectile.DamageType = DamageClass.Ranged;
            Projectile.hide = true;
        }
        public override void DrawBehind(int index, List<int> behindNPCsAndTiles, List<int> behindNPCs, List<int> behindProjectiles, List<int> overPlayers, List<int> overWiresUI)
        {
            behindNPCsAndTiles.Add(index);
        }
        public override void AI()
        {
            if (Projectile.velocity.Y >= 5f)
            {
                Projectile.velocity.Y = 5f;
            }
            Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver2;

            Projectile.ai[0] += 1f;
            if (Projectile.ai[0] == 3)
            {
                Projectile.netUpdate = true;
                Projectile.ai[0] = 0f;
                Projectile.velocity -= Projectile.velocity * 0.4f;
            }
        }
    }
}
