using Terraria.ModLoader;

namespace CSkies.Items.Observatory
{
    public class ObservatoryChandelier : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Large Observatory Cieling Lamp");
        }

        public override void SetDefaults()
        {
            item.width = 36;
            item.height = 22;
            item.maxStack = 99;
            item.useTurn = true;
            item.autoReuse = true;
            item.useAnimation = 15;
            item.useTime = 10;
            item.useStyle = 1;
            item.consumable = true;
            item.value = 250;
            item.createTile = mod.TileType("ObservatoryChandelier");
        }
    }
}