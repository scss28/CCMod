using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CCMod.Common.ECS
{
	internal abstract class Component : IComponent
	{
		public IEntity Entity { get; set; }
		public Component(IEntity entity)
		{
			Entity = entity;
		}

		public virtual List<Type> GetDependency()
		{
			return null;
		}

		public abstract void Install();

		public virtual void OnOthersInstall(List<IComponent> components) { }

		public virtual void Uninstall() { }

		public virtual IComponent Clone(IEntity entity)
		{
			return PrimitiveClone(entity);
		}

		public virtual IComponent PerfectClone(IEntity entity)
		{
			return (IComponent)MemberwiseClone();
		}

		public virtual IComponent PrimitiveClone(IEntity entity)
		{
			return (IComponent)Activator.CreateInstance(GetType(), entity);
		}
	}
}
