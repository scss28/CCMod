using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Reflection;
using Terraria;
using Terraria.ModLoader;

namespace CCMod.Common.Attributes
{
	[AttributeUsage(AttributeTargets.Class)]
	internal class ExampleItemAttribute : Attribute { }

	internal class ExampleItemGlobalItem : GlobalItem
	{
		public override void ModifyTooltips(Item item, List<TooltipLine> tooltips)
		{
			if (item.ModItem is not null && item.ModItem.GetType().GetCustomAttribute<ExampleItemAttribute>() is not null)
			{
				tooltips.Add(new TooltipLine(Mod, "ExampleItem", $"[[c/{Color.OrangeRed.Hex3()}:Example Item]]"));
			}
		}
	}
}
