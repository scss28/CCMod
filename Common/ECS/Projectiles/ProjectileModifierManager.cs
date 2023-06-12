using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.DataStructures;
using Terraria.ModLoader;
using Terraria;
using System.Reflection;
using Mono.Cecil;
using Microsoft.Xna.Framework;
using static CCMod.Common.ECS.Projectiles.ProjectileModifier;

namespace CCMod.Common.ECS.Projectiles
{
	internal class ProjectileModifierManager : GlobalProjectile, IEntity
	{
		public override bool InstancePerEntity => true;

		public object Source { get; set; }
		public Dictionary<string, List<IComponent>> Components { get; set; } = new Dictionary<string, List<IComponent>>();
		public Dictionary<string, Dictionary<IComponent, Delegate>> Delegations { get; set; } = new Dictionary<string, Dictionary<IComponent, Delegate>>();

		#region Delegations Invoke
		public override void OnSpawn(Projectile projectile, IEntitySource source)
		{
			string hookName = nameof(OnSpawn);
			if (Components.ContainsKey(hookName))
			{
				Components[hookName].ForEach(component => Delegations[hookName][component].DynamicInvoke(projectile, source));
			}
		}

		public override bool PreAI(Projectile projectile)
		{
			bool result = true;
			string hookName = nameof(PreAI);
			if (Components.ContainsKey(hookName))
			{
				Components[hookName].ForEach(component => result = (bool)Delegations[hookName][component].DynamicInvoke(projectile));
			}
			return result;
		}

		public override void AI(Projectile projectile)
		{
			string hookName = nameof(AI);
			if (Components.ContainsKey(hookName))
			{
				Components[hookName].ForEach(component => Delegations[hookName][component].DynamicInvoke(projectile));
			}
		}

		public override void PostAI(Projectile projectile)
		{
			string hookName = nameof(PostAI);
			if (Components.ContainsKey(hookName))
			{
				Components[hookName].ForEach(component => Delegations[hookName][component].DynamicInvoke(projectile));
			}
		}

		public override bool PreDraw(Projectile projectile, ref Color lightColor)
		{
			Color refColor = lightColor;
			bool result = true;
			string hookName = nameof(PreDraw);
			if (Components.ContainsKey(hookName))
			{
				Components[hookName].ForEach(component => result = ((PreDrawDelegate)Delegations[hookName][component]).Invoke(projectile, ref refColor));
			}
			lightColor = refColor;
			return result;
		}

		public override void PostDraw(Projectile projectile, Color lightColor)
		{
			string hookName = nameof(PostDraw);
			if (Components.ContainsKey(hookName))
			{
				Components[hookName].ForEach(component => Delegations[hookName][component].DynamicInvoke(projectile, lightColor));
			}
		}

		public override Color? GetAlpha(Projectile projectile, Color lightColor)
		{
			Color? result = null;
			string hookName = nameof(GetAlpha);
			if (Components.ContainsKey(hookName))
			{
				Components[hookName].ForEach(component => result = (Color?)Delegations[hookName][component].DynamicInvoke(projectile, lightColor));
			}
			return result;
		}

		public override bool OnTileCollide(Projectile projectile, Vector2 oldVelocity)
		{
			bool result = true;
			string hookName = nameof(OnTileCollide);
			if (Components.ContainsKey(hookName))
			{
				Components[hookName].ForEach(component => result = (bool)Delegations[hookName][component].DynamicInvoke(projectile, oldVelocity));
			}
			return result;
		}

		public override void ModifyHitNPC(Projectile projectile, NPC target, ref NPC.HitModifiers modifiers)
		{
			NPC.HitModifiers refModifiers = modifiers;
			string hookName = nameof(ModifyHitNPC);
			if (Components.ContainsKey(hookName))
			{
				Components[hookName].ForEach(component => ((ModifyHitNPCDelegate)Delegations[hookName][component]).Invoke(projectile, target, ref refModifiers));
			}
			modifiers = refModifiers;
		}

		public override void ModifyHitPlayer(Projectile projectile, Player target, ref Player.HurtModifiers modifiers)
		{
			Player.HurtModifiers refModifiers = modifiers;
			string hookName = nameof(ModifyHitPlayer);
			if (Components.ContainsKey(hookName))
			{
				Components[hookName].ForEach(component => ((ModifyHitPlayerDelegate)Delegations[hookName][component]).Invoke(projectile, target, ref refModifiers));
			}
			modifiers = refModifiers;
		}

		public override void OnHitNPC(Projectile projectile, NPC target, NPC.HitInfo hit, int damageDone)
		{
			string hookName = nameof(OnHitNPC);
			if (Components.ContainsKey(hookName))
			{
				Components[hookName].ForEach(component => Delegations[hookName][component].DynamicInvoke(projectile, target, hit, damageDone));
			}
		}

		public override void OnHitPlayer(Projectile projectile, Player target, Player.HurtInfo info)
		{
			string hookName = nameof(OnHitPlayer);
			if (Components.ContainsKey(hookName))
			{
				Components[hookName].ForEach(component => Delegations[hookName][component].DynamicInvoke(projectile, target, info));
			}
		}

		#endregion

		public IEntity Clone()
		{
			return PerfectClone();
		}

		public IEntity PerfectClone()
		{
			return (IEntity)MemberwiseClone();
		}

		public IEntity PrimitiveClone()
		{
			return new ProjectileModifierManager();
		}
	}
}
