using Terraria;
using Terraria.ModLoader;
using CCMod.Common.GlobalItems;
using CCMod.Utils;
using Terraria.ID;
using Microsoft.Xna.Framework;

namespace CCMod.Content.Items.Weapons.Example
{
	internal class ExampleBouncyProjectile : ModProjectile, IBouncyProjectile
	{
		public override string Texture => CCModTool.GetVanillaTexture<Item>(ItemID.Acorn);
		public int BounceTime { get => 10; set => BounceTime = value; }
		public int ChangeVelocityPerBounce => 1;
		public override void SetDefaults()
		{
			Projectile.width = Projectile.height = 32;
			Projectile.friendly = true;
			Projectile.timeLeft = 900;
		}
		public override void AI()
		{
			base.AI();
			Projectile.rotation += MathHelper.ToRadians(20);
		}
	}
	class ExampleShootBouncyProjectile : ModItem
	{
		public override string Texture => CCModTool.GetVanillaTexture<Item>(ItemID.Acorn);
		public override void SetDefaults()
		{
		}
	}
}
