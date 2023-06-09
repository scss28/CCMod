using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Reflection;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace CCMod.Common.Attributes
{
	/// <summary>Give credit to coders, artists or conecept creators for their contribuiton.</summary>
	[AttributeUsage(AttributeTargets.Class)]
	internal class MadeByAttribute : Attribute
	{
		public string Coder { get; }
		public string Spriter { get; }
		public string ConceptCreator { get; }

		public MadeByAttribute(string coder, string spriter, string conceptCreator)
		{
			Coder = coder;
			Spriter = spriter;
			ConceptCreator = conceptCreator;
		}

		public MadeByAttribute(string coder, string spriter) : this(coder, spriter, coder) { }
	}

	internal class MadeByGlobalItem : GlobalItem
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

		public override void SetDefaults(Item entity)
		{
			spriterColor = Main.rand.NextFromList(DISPLAY_COLORS);
			coderColor = Main.rand.NextFromList(DISPLAY_COLORS);
			conceptCreatorColor = Main.rand.NextFromList(DISPLAY_COLORS);
		}

		public override void ModifyTooltips(Item item, List<TooltipLine> tooltips)
		{
			MadeByAttribute madeBy;
			if (item.ModItem is null || (madeBy = item.ModItem.GetType().GetCustomAttribute<MadeByAttribute>()) is null)
				return;

			tooltips.Add(
				new TooltipLine(
					Mod,
					"Credits",
					$"[i:{ItemID.Wrench}] [c/{(coderColor * 0.75f).Hex3()}:Coded by:] " +
					$"[c/{coderColor.Hex3()}:{madeBy.Coder}]\n" +
					$"[i:{ItemID.Paintbrush}] [c/{(spriterColor * 0.75f).Hex3()}:Sprited by:] " +
					$"[c/{spriterColor.Hex3()}:{madeBy.Spriter}]\n" +
					$"[i:{ItemID.Cog}] [c/{(conceptCreatorColor * 0.75f).Hex3()}:Concept by:] " +
					$"[c/{conceptCreatorColor.Hex3()}:{madeBy.ConceptCreator}]"
				)
			);
		}
	}
}