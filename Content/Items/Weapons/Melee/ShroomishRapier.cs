using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using CCMod.Utils;
using CCMod.Common;
using Terraria.DataStructures;
using Terraria.Audio;
using Terraria.GameContent.Creative;


namespace CCMod.Content.Items.Weapons.Melee
{
	internal class ShroomishRapier : ModItem, IMadeBy
	{
		public string CodedBy => "Pexiltd";

		public string SpritedBy => "Pexiltd";

		public override void OnHitNPC(Player player, NPC target, NPC.HitInfo hit, int damageDone)
		{
			base.OnHitNPC(player, target, hit, damageDone);
			target.AddBuff(BuffID.Slow, 480);
			CCModTool.LifeStealOnHit(player.whoAmI, target.whoAmI, 3, 3, 1, 3);
		}
		public override void SetDefaults()
		{
			Item.damage = 80;
			Item.DamageType = DamageClass.Melee;
			Item.width = 55;
			Item.height = 58;
			Item.useTime = 30;
			Item.useAnimation = 30;
			Item.useStyle = ItemUseStyleID.Rapier;
			Item.knockBack = 4;
			Item.CanRollPrefix(PrefixID.Legendary);
			Item.CanRollPrefix(PrefixID.Awful);
			Item.value = 50000;
			Item.rare = ItemRarityID.Green;
			Terraria.Audio.SoundStyle item1 = SoundID.Item1;
			Item.UseSound = item1;
			Item.autoReuse = true;
			Item.scale = 1.0f;
			Item.crit = 7;
		}
	}
}
