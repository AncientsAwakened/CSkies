using Terraria.ModLoader;
using Terraria.ID;

namespace CSkies.Items.Boss
{
    [AutoloadEquip(EquipType.Head)]
	public class ObserverMask : ModItem
    {
        public override void SetStaticDefaults()
        {
            base.SetStaticDefaults();
            DisplayName.SetDefault("Observer Mask");
		}

        public override void SetDefaults()
        {
            item.width = 22;
            item.height = 26;
            item.rare = ItemRarityID.Purple;
            item.vanity = true;
        }
    }
}