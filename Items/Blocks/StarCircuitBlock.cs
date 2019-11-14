using Terraria.ModLoader;

namespace CSkies.Items.Blocks
{
    public class StarCircuitBlock : ModItem
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
            item.useStyle = 1;
            item.consumable = true;
            item.rare = 7;
            item.createTile = mod.TileType("StarCircuit");
        }

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Starcircuit Block");
        }

    }
}
