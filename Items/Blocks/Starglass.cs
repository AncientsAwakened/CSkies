using Terraria.ModLoader;
using Terraria.ID;

namespace CSkies.Items.Blocks
{
    public class Starglass : ModItem
    {
        public override void SetDefaults()
        {
            item.width = 16;
            item.height = 16;
            item.maxStack = 999;
            item.useTurn = true;
            item.autoReuse = true;
            item.useAnimation = 15;
            item.useTime = 10;
            item.useStyle = ItemUseStyleID.SwingThrow;
            item.consumable = true;
            item.rare = ItemRarityID.Lime;
            item.createTile = mod.TileType("Starglass");
        }

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Starglass");
        }

    }
}
