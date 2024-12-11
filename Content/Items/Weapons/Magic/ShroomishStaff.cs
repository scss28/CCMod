using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using CCMod.Utils;
using CCMod.Common;
using Terraria.DataStructures;
using Terraria.Audio;
using Terraria.GameContent.Creative;
using CCMod.Common.Attributes;

namespace CCMod.Content.Items.Weapons.Magic
{ 
	[CodedBy("Pexiltd")]
	[SpritedBy("Pexiltd")]
	[ConceptBy("Pexiltd")]
	public class ShroomishStaff : ModItem
	{// feel free to use on the condition you try to undnerstand who it does what it does lol, dont gotta but pls try
		public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
		{
			Projectile.NewProjectile(source, position, velocity, ProjectileID.FlaironBubble, 30, 3, player.whoAmI);
			for (int i = 0; i < 4; i++)
			{
				Vector2 vec = velocity.NextVector2RotatedByRandom(20f, 30, i);
				Projectile.NewProjectile(source, position, vec, type, damage, knockback, player.whoAmI);
			}
			return base.Shoot(player, source, position, velocity, type, damage, knockback);
		}

		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Shroomish Staff");
			// Tooltip.SetDefault("[Turns out the spear isnt so differnt from a staff.]);
			// to set a tool tip and display name go to en-US.JSON and look foryour item, might not be there so build then look again if it isnt
			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
			Item.staff[Item.type] = true;
		}
		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(ItemID.MushroomSpear, 1);
			recipe.AddIngredient(ItemID.ManaCrystal, 1);
			recipe.AddTile(TileID.WorkBenches);
			recipe.Register();
		}
		public override void SetDefaults()
		{
			// Most here is self explanitory if you can try to figure it out
			Terraria.Audio.SoundStyle item1 = SoundID.Item1;
			Item.UseSound = item1;
			Item.SetDefaultMagic(9, 9, 20, 6, 14, 14, ItemUseStyleID.Shoot, ProjectileID.Mushroom, 60, 4, true);
			//I think adding reforgesis a nicesmall feature that goes the long way, I trty to find ones that make sense for the weapon
			Item.CanRollPrefix(PrefixID.Mythical);
			Item.CanRollPrefix(PrefixID.Taboo);
			Item.CanRollPrefix(PrefixID.Celestial);
			Item.CanRollPrefix(PrefixID.Unhappy);
			//2 good and 2 bad imo is what I thinkshould be the minimium fr what I make lol, you can google reforges and look at wiki to see them
		}
	}
}

