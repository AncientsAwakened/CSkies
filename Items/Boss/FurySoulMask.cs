using System.Collections.Generic;
using Terraria.ModLoader;

namespace CSkies.Items.Boss
{
    [AutoloadEquip(EquipType.Head)]
	public class FurySoulMask : ModItem
    {
        public override void SetStaticDefaults()
        {
            base.SetStaticDefaults();
            DisplayName.SetDefault("Fury Soul Mask");
		}

        public override void SetDefaults()
        {
            item.width = 22;
            item.height = 26;
            item.rare = 11;
            item.vanity = true;
            item.expertOnly = true;
            item.expert = true;
        }

        public override void ModifyTooltips(List<TooltipLine> list)
        {
            foreach (TooltipLine line2 in list)
            {
                if (line2.mod == "Terraria" && line2.Name == "ItemName")
                {
                    line2.overrideColor = Colors.Rarity12;
                }
            }
        }
    }
}