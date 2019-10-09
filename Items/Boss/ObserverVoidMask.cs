using Terraria.ModLoader;

namespace CSkies.Items.Boss
{
    [AutoloadEquip(EquipType.Head)]
    public class ObserverVoidMask : ModItem
    {
        public override void SetStaticDefaults()
        {
            base.SetStaticDefaults();
            DisplayName.SetDefault("Observer Void Mask");
        }

        public override void SetDefaults()
        {
            item.width = 22;
            item.height = 26;
            item.rare = 11;
            item.vanity = true;
        }
    }
}