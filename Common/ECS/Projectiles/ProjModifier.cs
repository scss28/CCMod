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
	internal abstract class ProjModifier : Component
	{
		protected ProjModifier(IEntity entity) : base(entity)
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
		/// Additional parameter: <para>PreDraw_LightColor</para>
		/// </summary>
		/// <param name="projectile"></param>
		/// <returns></returns>
		public virtual bool PreDraw(Projectile projectile) { return true; }
		public virtual void PostDraw(Projectile projectile, Color lightColor) { }
		public virtual Color? GetAlpha(Projectile projectile, Color lightColor) { return null; }
		public virtual bool OnTileCollide(Projectile projectile, Vector2 oldVelocity) { return true; }
		/// <summary>
		/// Additional parameter: <para>ModifyHitNPC_Modifiers</para>
		/// </summary>
		/// <param name="projectile"></param>
		/// <param name="target"></param>
		/// <param name="modifiers"></param>
		public virtual void ModifyHitNPC(Projectile projectile, NPC target) { }
		/// <summary>
		/// Additional parameter: <para>ModifyHitPlayer_Modifiers</para>
		/// </summary>
		/// <param name="projectile"></param>
		/// <param name="target"></param>
		public virtual void ModifyHitPlayer(Projectile projectile, Player target) { }
		public virtual void OnHitNPC(Projectile projectile, NPC target, NPC.HitInfo hit, int damageDone) { }
		public virtual void OnHitPlayer(Projectile projectile, Player target, Player.HurtInfo info) { }

	}
}
