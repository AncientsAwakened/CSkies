using Terraria.ModLoader;
using Terraria.ID;

namespace CSkies.Items.Boss.Starcore
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
            item.rare = ItemRarityID.LightRed;
            item.vanity = true;
        }
    }
}