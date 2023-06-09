using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria;
using Microsoft.Xna.Framework;
using System.Reflection;

namespace CCMod.Common.Attributes
{
	[AttributeUsage(AttributeTargets.Class)]
	internal class CodedByAttribute : Attribute
	{
		public string[] Coders { get; }
		public CodedByAttribute(params string[] coders) 
		{
			Coders = coders;
		}
	}

	[AttributeUsage(AttributeTargets.Class)]
	internal class SpritedByAttribute : Attribute
	{
		public string[] Spriters { get; }
		public SpritedByAttribute(params string[] spriters)
		{
			Spriters = spriters;
		}
	}

	[AttributeUsage(AttributeTargets.Class)]
	internal class ConceptByAttribute : Attribute
	{
		public string[] ConceptCreators { get; }
		public ConceptByAttribute(params string[] conceptCreators)
		{
			ConceptCreators = conceptCreators;
		}
	}

	internal class ModItemCreditsGlobalItem : GlobalItem
	{
		public override bool InstancePerEntity => true;

		private readonly Color[] DISPLAY_COLORS =
		{
			Color.IndianRed,
			Color.Honeydew,
			Color.CadetBlue,
			Color.Beige,
			Color.BlueViolet,
			Color.Magenta,
			Color.Aqua,
			Color.Gold,
			Color.Orange,
			Color.Orchid,
			Color.OrangeRed
		};

		private Color spriterColor, coderColor, conceptCreatorColor;

		public override void SetDefaults(Item item)
		{
			if (item.ModItem is null || item.ModItem.Mod is not CCMod)
			{
				return;
			}

			spriterColor = Main.rand.NextFromList(DISPLAY_COLORS);
			coderColor = Main.rand.NextFromList(DISPLAY_COLORS);
			conceptCreatorColor = Main.rand.NextFromList(DISPLAY_COLORS);
		}

		public override void ModifyTooltips(Item item, List<TooltipLine> tooltips)
		{
			if (item.ModItem is null || item.ModItem.Mod is not CCMod)
			{
				return;
			}

			CodedByAttribute codedByAttribute;
			if ((codedByAttribute = item.ModItem.GetType().GetCustomAttribute<CodedByAttribute>()) is not null)
			{
				tooltips.Add(
					new TooltipLine(
						Mod,
						"CodedBy",
						$"[i:{ItemID.Wrench}] [c/{(coderColor * 0.75f).Hex3()}:Coded by:] " +
						$"[c/{coderColor.Hex3()}:{string.Join(", ", codedByAttribute.Coders)}]"
					)
				);
			}

			SpritedByAttribute spritedByAttribute;
			if ((spritedByAttribute = item.ModItem.GetType().GetCustomAttribute<SpritedByAttribute>()) is not null)
			{
				tooltips.Add(
					new TooltipLine(
						Mod,
						"SpritedBy",
						$"[i:{ItemID.Paintbrush}] [c/{(spriterColor * 0.75f).Hex3()}:Sprited by:] " +
						$"[c/{spriterColor.Hex3()}:{string.Join(", ", spritedByAttribute.Spriters)}]"
					)
				);
			}

			ConceptByAttribute conceptByAttribute;
			if ((conceptByAttribute = item.ModItem.GetType().GetCustomAttribute<ConceptByAttribute>()) is not null)
			{
				tooltips.Add(
					new TooltipLine(
						Mod,
						"ConceptBy",
						$"[i:{ItemID.Cog}] [c/{(conceptCreatorColor * 0.75f).Hex3()}:Concept by:] " +
						$"[c/{conceptCreatorColor.Hex3()}:{string.Join(", ", conceptByAttribute.ConceptCreators)}]"
					)
				);
			}
		}
	}
}
