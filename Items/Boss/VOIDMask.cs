using Terraria.ModLoader;

namespace CSkies.Items.Boss
{
    [AutoloadEquip(EquipType.Head)]
    public class VOIDMask : ModItem
    {
        public override void SetStaticDefaults()
        {
            base.SetStaticDefaults();
            DisplayName.SetDefault("VOID0 Mask");
        }

        public override void SetDefaults()
        {
            item.width = 22;
            item.height = 26;
            item.rare = 11;
            item.vanity = true;
            item.expert = true;
            item.expertOnly = true;
        }
    }
}