using Microsoft.Xna.Framework;
using Steamworks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.DataStructures;

namespace CCMod.Common.ECS.Projectiles
{
	internal abstract class ProjectileModifier : Component
	{
		protected ProjectileModifier(IEntity entity) : base(entity)
		{
		}
		public enum Hook
		{
			OnSpawn,
			PreAI,
			AI,
			PostAI,
			PreDraw,
			PostDraw,
			GetAlpha,
			OnTileCollide,
			ModifyHitNPC,
			ModifyHitPlayer,
			OnHitNPC,
			OnHitPlayer
		}
		public void RegisterDelegation(Hook hook, Delegate delegation)
		{
			Entity?.RegisterDelegation(this, hook.ToString(), delegation);
		}
		public virtual void OnSpawn(Projectile projectile, IEntitySource source) { }
		public virtual bool PreAI(Projectile projectile) { return true; }
		public virtual void AI(Projectile projectile) { }
		public virtual void PostAI(Projectile projectile) { }
		/// <summary>
		/// While registering this method, the parameter 'delegation' should be <code>new PreDrawDelegate(PreDraw)</code>
		/// </summary>
		/// <param name="projectile"></param>
		/// <param name="lightColor"></param>
		/// <returns></returns>
		public virtual bool PreDraw(Projectile projectile, ref Color lightColor) { return true; }
		public virtual void PostDraw(Projectile projectile, Color lightColor) { }
		public virtual Color? GetAlpha(Projectile projectile, Color lightColor) { return null; }
		public virtual bool OnTileCollide(Projectile projectile, Vector2 oldVelocity) { return true; }
		/// <summary>
		/// While registering this method, the parameter 'delegation' should be <code>new ModifyHitNPCDelegate(ModifyHitNPC)</code>
		/// </summary>
		/// <param name="projectile"></param>
		/// <param name="target"></param>
		/// <param name="modifiers"></param>
		public virtual void ModifyHitNPC(Projectile projectile, NPC target, ref NPC.HitModifiers modifiers) { }
		/// <summary>
		/// While registering this method, the parameter 'delegation' should be <code>new ModifyHitPlayerDelegate(ModifyHitPlayer)</code>
		/// </summary>
		/// <param name="projectile"></param>
		/// <param name="target"></param>
		/// <param name="modifiers"></param>
		public virtual void ModifyHitPlayer(Projectile projectile, Player target, ref Player.HurtModifiers modifiers) { }
		public virtual void OnHitNPC(Projectile projectile, NPC target, NPC.HitInfo hit, int damageDone) { }
		public virtual void OnHitPlayer(Projectile projectile, Player target, Player.HurtInfo info) { }

		public delegate bool PreDrawDelegate(Projectile projectile, ref Color lightColor);
		public delegate void ModifyHitNPCDelegate(Projectile projectile, NPC target, ref NPC.HitModifiers modifiers);
		public delegate void ModifyHitPlayerDelegate(Projectile projectile, Player target, ref Player.HurtModifiers modifiers);

	}
}
