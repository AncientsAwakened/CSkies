using Terraria.ModLoader;
using Terraria.ID;

namespace CSkies.Items.Boss
{
    public class StarcoreTrophy : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Starcore Trophy");
		}

        public override void SetDefaults()
		{
			item.width = 32;
			item.height = 32;
			item.maxStack = 99;
			item.useTurn = true;
			item.autoReuse = true;
			item.useAnimation = 15;
			item.useTime = 10;
            item.useStyle = ItemUseStyleID.SwingThrow;
			item.consumable = true;
			item.value = 2000;
			item.rare = ItemRarityID.Blue;
			item.createTile = mod.TileType("StarcoreTrophy");
		}
    }
}