using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CCMod.Common.ECS;
using CCMod.Common.ECS.Projectiles;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using static CCMod.Utils.CCModTool;

namespace CCMod.Content.ProjectileModifiers
{
	internal class PowerOfLight : ProjectileModifier
	{
		public PowerOfLight(IEntity entity) : base(entity)
		{
		}
		public float R = 1f;
		public float G = 1f;
		public float B = 1f;
		public override void Install()
		{
			RegisterDelegation(Hook.AI, AI);
			RegisterDelegation(Hook.GetAlpha, GetAlpha);
			RegisterDelegation(Hook.ModifyHitNPC, new ModifyHitNPCDelegate(ModifyHitNPC));
		}
		public override void AI(Projectile projectile)
		{
			base.AI(projectile);
			Lighting.AddLight(projectile.position, R, G, B);
		}
		public override Color? GetAlpha(Projectile projectile, Color lightColor)
		{
			return lightColor * 0.3f;
		}
		public override void ModifyHitNPC(Projectile projectile, NPC target, ref NPC.HitModifiers refModifiers)
		{
			refModifiers.SetCrit();
			refModifiers.CritDamage *= 2;
		}
	}

	/*
		public class ECSTest : ModItem
		{
			public override string Texture => $"Terraria/Images/Item_{ItemID.Ale}";
			public override void SetDefaults()
			{
				Item.DefaultToMagicWeapon(ProjectileID.ChlorophyteArrow, 24, 6, true);
				Item.damage = 10;
			}
			public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
			{
				var manager = (IEntity)Projectile.NewProjectileDirect(source, position, velocity, type, damage, knockback).GetGlobalProjectile<ProjectileModifierManager>();
				PowerOfLight pol = new PowerOfLight(manager);
				pol.R = 2;
				manager.InstallComponent(pol);
				return false;
			}
		}
	*/
}
