using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CCMod.Common.ECS
{
	internal interface IEntity
	{
		public object Source { get; set; }
		public Dictionary<string, List<IComponent>> Components { get; set; }
		public Dictionary<string, Dictionary<IComponent, Delegate>> Delegations { get; set; }
		public bool InstallComponent(IComponent component)
		{
			List<IComponent> pool = new List<IComponent>();
			foreach (List<IComponent> coms in Components.Values)
			{
				foreach (IComponent com in coms)
				{
					if (!pool.Contains(com))
					{
						pool.Add(com);
					}
				}
			}
			if (installComponent(component, pool))
			{
				foreach (IComponent com in pool)
				{
					com.OnOthersInstall(pool);
				}
				return true;
			}
			return false;
		}
		public bool CreateAndInstallComponent<T>( params object[] args) where T : IComponent
		{
			T component = (T)Activator.CreateInstance(typeof(T));
			return InstallComponent(component);
		}
		private bool installComponent(IComponent component, List<IComponent> pool)
		{
			if (pool.Contains(component))
			{
				return false;
			}
			List<Type> dependencies = component.GetDependency();
			bool depended;
			if (dependencies != null)
			{
				foreach (Type dependency in dependencies)
				{
					depended = false;
					foreach (IComponent p in pool)
					{
						if (dependency == p.GetType())
						{
							depended = true;
							break;
						}
					}
					if (!depended)
					{
						return false;
					}
				}
			}
			component.Entity = this;
			pool.Add(component);
			component.Install();
			return false;
		}
		private Dictionary<Type, List<IComponent>> GetDependencies()
		{
			Dictionary<Type, List<IComponent>> dependencies = new Dictionary<Type, List<IComponent>>();
			List<Type> depCache = new List<Type>();
			foreach (List<IComponent> coms in Components.Values)
			{
				foreach (IComponent com in coms)
				{
					depCache = com.GetDependency();
					if (depCache == null || depCache.Count == 0)
					{
						continue;
					}
					depCache.ForEach(depended =>
					{
						if (dependencies.ContainsKey(depended))
						{
							dependencies.Add(depended, new List<IComponent>());
						}
						dependencies[depended].Add(com);
					});
				}
			}
			return dependencies;
		}
		public bool UninstallComponent(IComponent component)
		{
			bool result = true;
			Dictionary<Type, List<IComponent>> dependencies = GetDependencies();
			if (dependencies.TryGetValue(component.GetType(), out List<IComponent> dependings))
			{
				foreach (IComponent depending in dependings)
				{
					if (!dependencies.TryGetValue(depending.GetType(), out List<IComponent> nextDependeds))
					{
						continue;
					}
					foreach (IComponent depended in nextDependeds)
					{
						if (!UninstallComponent(depended))
						{
							result = false;
						}
					}
				}
			}
			foreach (List<IComponent> coms in Components.Values)
			{
				if (!coms.Remove(component))
				{
					result = false;
				}
			}
			return result;
		}
		public bool HasComponent<T>() where T : IComponent
		{
			foreach (var coms in Components.Values)
			{
				foreach (var com in coms)
				{
					if (com is T)
					{
						return true;
					}
				}
			}
			return false;
		}
		public bool TryGetComponent<T>(out IComponent component) where T : IComponent
		{
			component = null;
			foreach (List<IComponent> coms in Components.Values)
			{
				foreach (IComponent com in coms)
				{
					if (com is T)
					{
						component = com;
						return true;
					}
				}
			}
			return false;
		}
		public void RegisterDelegation(IComponent component, string hookName, Delegate delegation)
		{
			if (delegation == null)
			{
				return;
			}
			if (!Components.ContainsKey(hookName))
			{
				Components.Add(hookName, new List<IComponent>());
			}
			if (Components[hookName].Contains(component))
			{
				return;
			}
			Components[hookName].Add(component);
			if (!Delegations.ContainsKey(hookName))
			{
				Delegations.Add(hookName, new Dictionary<IComponent, Delegate>());
			}
			Delegations[hookName].Add(component, delegation);
		}
		public IEntity Clone();
		public IEntity PrimitiveClone();
		public IEntity PerfectClone();
	}
}
