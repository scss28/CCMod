using CCMod.Content.Items.Weapons.Magic;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace CCMod.Content.Items.Weapons.Magic
{
    public class StaffOfBloodRain : ModItem
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("temp");
            Item.staff[Item.type] = true;
        }

        public override void SetDefaults()
        {
            Item.damage = 65;
            Item.DamageType = DamageClass.Magic;
            Item.mana = 9;
            Item.width = 34;
            Item.height = 34;
            Item.useTime = 5;
            Item.useAnimation = 15;
            Item.reuseDelay = 30;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.noMelee = true;
            Item.knockBack = 0;
            Item.value = 10000;
            Item.rare = ItemRarityID.Red;
            Item.UseSound = SoundID.Item67;
            Item.autoReuse = true;
            Item.shoot = ModContent.ProjectileType<BloodBolt>();
            Item.shootSpeed = 12f;
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            Projectile.NewProjectile(source, position, velocity.RotatedByRandom(MathHelper.ToRadians(120)), type, damage, knockback, player.whoAmI);
            return false;
        }
    }
}