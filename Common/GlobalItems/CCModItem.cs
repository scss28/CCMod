using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CCMod.Common.GlobalItems
{
    public class CCModItem : GlobalItem
    {
        public override bool InstancePerEntity => true;

        Color spriterColor;
        Color coderColor;
        public override void SetDefaults(Item item)
        {
            if (item.ModItem is not null && item.ModItem.Mod is CCMod)
            {
                //spriterColor = new Color(Main.rand.NextFloat(0.6f, 1f), Main.rand.NextFloat(0.6f, 1f), Main.rand.NextFloat(0.6f, 1f));
                //coderColor = new Color(Main.rand.NextFloat(0.6f, 1f), Main.rand.NextFloat(0.6f, 1f), Main.rand.NextFloat(0.6f, 1f));

                spriterColor = RandomColorWithBrightness(0.95f);
                coderColor = RandomColorWithBrightness(0.95f);
            }
        }

        static Color RandomColorWithBrightness(float brightness)
        {
            brightness = Math.Clamp(brightness, 0f, 1f);

            float r = Main.rand.NextFloat(0f, brightness);
            float g = Main.rand.NextFloat(0f, brightness - r);
            float b = brightness - r - g;

            return new Color(r, g, b) * 3;
        }

        public override void ModifyTooltips(Item item, List<TooltipLine> tooltips)
        {
            if (item.ModItem is not null && item.ModItem.Mod is CCMod)
            {
                //string bracesHex = Color.LightSteelBlue.Hex3();
                string titleHex = Color.LightGoldenrodYellow.Hex3();

                tooltips.Add(new TooltipLine(Mod, "ModTitle", $"    [c/{titleHex}:Community Item]"));

                string spriterName = "?";
                string coderName = "?";

                if (item.ModItem is IMadeBy madeByItem)
                {
                    spriterName = madeByItem.SpritedBy == string.Empty ? "?" : madeByItem.SpritedBy;
                    coderName = madeByItem.CodedBy == string.Empty ? "?" : madeByItem.CodedBy;
                }

                string spriterHex = spriterColor.Hex3();
                string spriterHexDarker = (spriterColor * 0.75f).Hex3();

                string coderHex = coderColor.Hex3();
                string coderHexDarker = (coderColor * 0.75f).Hex3();

                tooltips.Add(
                        new TooltipLine(
                            Mod,
                            "CreditLine",
                            $"∘ [c/{spriterHexDarker}:Sprited by:] [c/{spriterHex}:{spriterName}]\n" +
                            $"∘ [c/{coderHexDarker}:Coded by:] [c/{coderHex}:{coderName}]"
                        )
                        );
            }
        }
    }
}
