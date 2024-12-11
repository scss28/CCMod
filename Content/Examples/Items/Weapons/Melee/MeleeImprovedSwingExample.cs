using CCMod.Utils;
using Terraria.ID;
using Terraria.ModLoader;
using CCMod.Common.GlobalItems;
using CCMod.Content.Items.Weapons.Melee;
using Terraria;
using CCMod.Common.Attributes;

namespace CCMod.Content.Examples.Items.Weapons.Melee
{
	[ExampleItem]
	[CodedBy("Xinim")]
	[SpritedBy("Xinim")]
	internal class MeleeImprovedSwingExample : ModItem, IMeleeWeaponWithImprovedSwing
	{
		public float SwingDegree => 170;
		public override string Texture => CCModTool.GetSameTextureAs<GenericBlackSword>();

		public override void SetDefaults()
		{
			Item.SetDefaultMelee(40, 40, 30, 4f, 30, 30, ItemUseStyleID.Swing, true);
			Item.Set_MeleeIFrame(1);
		}
		public override void OnHitNPC(Player player, NPC target, NPC.HitInfo hit, int damageDone)
		{
			CCModTool.LifeStealOnHit(player.whoAmI, target.whoAmI, 5, 10, 1, 3);
			//The math for life steal : Hp life steal amount = ( 5 + Main.rand.Next(10 + 1) ) * ( 1 + Main.rand.NextFloat(3) )
			//Assumed Main.rand.Next(10 + 1) return : 5
			//Assumed Main.rand.NextFloat(3) return : 1.5
			// Hp life steal amount = ( 5 + 5 ) * ( 1 + 1.5 )
			// Hp life steal amount = 10 * 2.5
			// Hp life steal amount = 25
		}
	}
}