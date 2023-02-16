using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using CCMod.Common;

namespace CCMod.Content.Items.Accessories
{
    internal class EnergizedAcceleratorAttachment : ModItem, IMadeBy
    {
        public string CodedBy => "LowQualityTrash-Xinim";
        public string ConceptBy => "Tim";
        public string SpritedBy => "RockyStan";
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("Every 5th bullet/arrow shot deal extra 15% damage");
        }
        public override void SetDefaults()
        {
            Item.width = 20;
            Item.height = 20;
            Item.rare = ItemRarityID.LightRed;
            Item.accessory = true;
        }
        public override void UpdateEquip(Player player)
        {
            player.GetModPlayer<EnergizedAcceleratorAttachmentPlayer>().EnergizedAcceleratorAttachment = true;
        }
        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.Bone, 10)
                .AddIngredient(ItemID.FallenStar, 5)
                .AddIngredient(ItemID.IllegalGunParts)
                .AddTile(TileID.Anvils)
                .Register();
        }
    }
    public class EnergizedAcceleratorAttachmentPlayer : ModPlayer
    {
        public bool EnergizedAcceleratorAttachment = false;
        int counter;

        public override void ResetEffects()
        {
            EnergizedAcceleratorAttachment = false;
        }

        public override void ModifyShootStats(Item item, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
        {
            if (!EnergizedAcceleratorAttachment) return;
            if (item.useAmmo == AmmoID.Bullet || item.useAmmo == AmmoID.Arrow)
            {
                if (++counter == 5)
                {
                    damage = (int)(damage * 1.15f);
                    counter = 0;
                }
                else if (counter == 4)
                {
                    for (int i = 0; i < 5; i++)
                    {
                        Vector2 resize = Vector2.One * 30;
                        Dust.NewDust(Player.position - resize / 2, Player.width + (int)resize.X, Player.height + (int)resize.Y, DustID.Electric);
                    }
                }
            }
        }
    }
}