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
using Terraria.WorldBuilding;

namespace CCMod.Common.ECS.Projectiles
{
	internal class ProjModifierManager : GlobalProjectile, IEntity
	{
		public override bool InstancePerEntity => true;
		private object source;
		public object Source { get => source; set => source = value; }
		private Dictionary<string, List<IComponent>> components = new Dictionary<string, List<IComponent>>();
		public Dictionary<string, List<IComponent>> Components { get => components; set => components = value; }
		private Dictionary<string, Dictionary<IComponent, Delegate>> delegations = new Dictionary<string, Dictionary<IComponent, Delegate>>();
		public Dictionary<string, Dictionary<IComponent, Delegate>> Delegations { get => delegations; set => delegations = value; }

		#region Intermediate Variables
		public Color PreDraw_LightColor = new Color();
		public NPC.HitModifiers ModifyHitNPC_Modifiers = new NPC.HitModifiers();
		public Player.HurtModifiers ModifyHitPlayer_Modifiers = new Player.HurtModifiers();
		#endregion

		#region Delegations Invoke
		public override void OnSpawn(Projectile projectile, IEntitySource source)
		{
			string hookName = nameof(OnSpawn);
			if (Components.ContainsKey(hookName))
			{
				Components[hookName].ForEach(component =>
				{
					Delegations[hookName][component].DynamicInvoke(projectile, source);
				});
			}
		}
		public override bool PreAI(Projectile projectile)
		{
			bool result = true;
			string hookName = nameof(PreAI);
			if (Components.ContainsKey(hookName))
			{
				Components[hookName].ForEach(component =>
				{
					result = (bool)Delegations[hookName][component].DynamicInvoke(projectile);
				});
			}
			return result;
		}
		public override void AI(Projectile projectile)
		{
			base.AI(projectile);
			string hookName = nameof(AI);
			if (Components.ContainsKey(hookName))
			{
				Components[hookName].ForEach(component =>
				{
					Delegations[hookName][component].DynamicInvoke(projectile);
				});
			}
		}
		public override void PostAI(Projectile projectile)
		{
			string hookName = nameof(PostAI);
			if (Components.ContainsKey(hookName))
			{
				Components[hookName].ForEach(component =>
				{
					Delegations[hookName][component].DynamicInvoke(projectile);
				});
			}
		}
		public override bool PreDraw(Projectile projectile, ref Color lightColor)
		{
			PreDraw_LightColor = lightColor;
			bool result = true;
			string hookName = nameof(PreDraw);
			if (Components.ContainsKey(hookName))
			{
				Components[hookName].ForEach(component =>
				{
					result = (bool)Delegations[hookName][component].DynamicInvoke(projectile);
				});
			}
			lightColor = PreDraw_LightColor;
			return result;
		}
		public override void PostDraw(Projectile projectile, Color lightColor)
		{
			string hookName = nameof(PostDraw);
			if (Components.ContainsKey(hookName))
			{
				Components[hookName].ForEach(component =>
				{
					Delegations[hookName][component].DynamicInvoke(projectile, lightColor);
				});
			}
		}
		public override Color? GetAlpha(Projectile projectile, Color lightColor)
		{
			Color? result = null;
			string hookName = nameof(GetAlpha);
			if (Components.ContainsKey(hookName))
			{
				Components[hookName].ForEach(component =>
				{
					result = (Color?)Delegations[hookName][component].DynamicInvoke(projectile, lightColor);
				});
			}
			return result;
		}
		public override bool OnTileCollide(Projectile projectile, Vector2 oldVelocity)
		{
			bool result = true;
			string hookName = nameof(OnTileCollide);
			if (Components.ContainsKey(hookName))
			{
				Components[hookName].ForEach(component =>
				{
					result = (bool)Delegations[hookName][component].DynamicInvoke(projectile, oldVelocity);
				});
			}
			return result;
		}
		public override void ModifyHitNPC(Projectile projectile, NPC target, ref NPC.HitModifiers modifiers)
		{
			ModifyHitNPC_Modifiers = modifiers;
			string hookName = nameof(ModifyHitNPC);
			if (Components.ContainsKey(hookName))
			{
				Components[hookName].ForEach(component =>
				{
					Delegations[hookName][component].DynamicInvoke(projectile, target);
				});
			}
			modifiers = ModifyHitNPC_Modifiers;
		}
		public override void ModifyHitPlayer(Projectile projectile, Player target, ref Player.HurtModifiers modifiers)
		{
			ModifyHitPlayer_Modifiers = modifiers;
			string hookName = nameof(ModifyHitPlayer);
			if (Components.ContainsKey(hookName))
			{
				Components[hookName].ForEach(component =>
				{
					Delegations[hookName][component].DynamicInvoke(projectile, target);
				});
			}
			modifiers = ModifyHitPlayer_Modifiers;
		}
		public override void OnHitNPC(Projectile projectile, NPC target, NPC.HitInfo hit, int damageDone)
		{
			string hookName = nameof(OnHitNPC);
			if (Components.ContainsKey(hookName))
			{
				Components[hookName].ForEach(component =>
				{
					Delegations[hookName][component].DynamicInvoke(projectile, target, hit, damageDone);
				});
			}
		}
		public override void OnHitPlayer(Projectile projectile, Player target, Player.HurtInfo info)
		{
			string hookName = nameof(OnHitPlayer);
			if (Components.ContainsKey(hookName))
			{
				Components[hookName].ForEach(component =>
				{
					Delegations[hookName][component].DynamicInvoke(projectile, target, info);
				});
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
			return new ProjModifierManager();
		}
	}
}
