using Terraria.ModLoader;

namespace CSkies.Items.Comet
{
    public class CometOre : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Comet Ore");
        }

        public override void SetDefaults()
        {
            item.width = 22;
            item.height = 22;
            item.maxStack = 99;
            item.rare = 1;
            item.maxStack = 999;
            item.useTurn = true;
            item.autoReuse = true;
            item.useAnimation = 15;
            item.useTime = 10;
            item.useStyle = 1;
            item.rare = 1;
            item.value = Terraria.Item.sellPrice(0, 0, 8, 0);
            item.consumable = true;
            item.createTile = mod.TileType("CometOre");
        }
    }
}
