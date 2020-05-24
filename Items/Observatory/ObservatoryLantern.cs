using Terraria.ModLoader;
using Terraria.ID;

namespace CSkies.Items.Observatory
{
    public class ObservatoryLantern : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Observatory Cieling Lamp");
        }

        public override void SetDefaults()
        {
            item.width = 14;
            item.height = 22;
            item.maxStack = 99;
            item.useTurn = true;
            item.autoReuse = true;
            item.useAnimation = 15;
            item.useTime = 10;
            item.useStyle = ItemUseStyleID.SwingThrow;
            item.consumable = true;
            item.value = 250;
            item.createTile = mod.TileType("ObservatoryLantern");
        }
    }
}