using Microsoft.Xna.Framework;
using Terraria.ModLoader;
using Terraria;
using Terraria.Localization;
using System.Collections.Generic;
using Terraria.ID;

namespace CSkies.Items.Dev
{
    public class NovacoreReader : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Novacore Reader");
            Tooltip.SetDefault(@"Displays Novacore's current AI integer");
        }

        public override void SetDefaults()
        {
            item.width = 20;
            item.height = 20;
            item.rare = ItemRarityID.Red;
        }

        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
            string AI = CWorld.NovacoreAI.ToString();
            string Counter = CWorld.NovacoreCounter.ToString();
            TooltipLine AITooltip = new TooltipLine(mod, "AI Type", AI + @"
" + Counter);
            tooltips.Add(AITooltip);

            base.ModifyTooltips(tooltips);
        }
    }
}