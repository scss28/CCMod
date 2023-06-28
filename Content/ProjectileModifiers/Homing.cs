using CCMod.Common.Attributes;
using CCMod.Common.ECS;
using CCMod.Common.ECS.Projectiles;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace CCMod.Content.ProjectileModifiers
{
	internal class Homing : ProjectileModifier
	{
		public Homing(IEntity entity, int range, float strength = 0.25f) : base(entity)
		{
			Range = range;
			Strength = strength;
		}
		public Homing(IEntity entity) : base(entity)
		{

		}
		public int Range;
		public float Strength;
		public override void Install()
		{
			RegisterDelegation(Hook.AI, AI);
		}
		public override void AI(Projectile projectile)
		{
			int index = projectile.FindTargetWithLineOfSight(Range);
			NPC target = index < 0 ? null : Main.npc[index];
			if (target == null)
			{
				return;
			}
			projectile.velocity = Vector2.Lerp(
					projectile.velocity.SafeNormalize(Vector2.Zero),
					projectile.Center.DirectionTo(target.Center),
					Strength
				) * projectile.velocity.Length();
		}
	}

	[ExampleItem]
	public class HomingTest : ModItem
	{
		public override string Texture => $"Terraria/Images/Item_{ItemID.AshWoodBow}";
		public override void SetDefaults()
		{
			Item.DefaultToMagicWeapon(ProjectileID.Bullet, 24, 6, true);
			Item.damage = 10;
		}
		public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
		{
			var manager = (IEntity)Projectile.NewProjectileDirect(source, position, velocity * 2, type, damage, knockback).GetGlobalProjectile<ProjectileModifierManager>();
			manager.InstallComponent(new Homing(manager, 800, 0.6f));
			return false;
		}
	}
}
