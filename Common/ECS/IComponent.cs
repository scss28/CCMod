using rail;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;

namespace CCMod.Common.ECS
{
	internal interface IComponent
	{
		public IEntity Entity { get; set; }
		public List<Type> GetDependency();
		public void Install();
		public void Uninstall();
		public void OnOthersInstall(List<IComponent> components);
		public IComponent Clone(IEntity entity);
		public IComponent PrimitiveClone(IEntity entity);
		public IComponent PerfectClone(IEntity entity);
	}
}
