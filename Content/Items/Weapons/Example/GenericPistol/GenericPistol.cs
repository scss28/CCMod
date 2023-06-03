using Terraria;
using CCMod.Utils;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Terraria.DataStructures;
using CCMod.Common;

namespace CCMod.Content.Items.ExampleWeapon.GenericPistol
{
	internal class GenericPistol : ModItem, IMadeBy
	{
		public string CodedBy => "Xinim";

		public string SpritedBy => "Xinim";

		public override void SetDefaults()
		{
			Item.CCModItemSetDefaultRange(34, 19, 20, 1f, 20, 20, ItemUseStyleID.Shoot, ProjectileID.Bullet, 10, false, AmmoID.Bullet);
		}
		public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
		{
			for (int i = 0; i < 5; i++)
			{
				Vector2 vec = velocity.Vector2EvenArchSpread(5, 30, i);
				Projectile.NewProjectile(source, position, vec, type, damage, knockback, player.whoAmI);
			}
			return false;
		}
	}
}