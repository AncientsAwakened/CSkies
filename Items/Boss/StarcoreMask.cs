using System.Collections.Generic;
using Terraria.ModLoader;

namespace CSkies.Items.Boss
{
    [AutoloadEquip(EquipType.Head)]
	public class StarcoreMask : ModItem
    {
        public override void SetStaticDefaults()
        {
            base.SetStaticDefaults();
            DisplayName.SetDefault("Starcore Mask");
		}

        public override void SetDefaults()
        {
            item.width = 22;
            item.height = 26;
            item.rare = 4;
            item.vanity = true;
        }
    }
}