using Terraria.ModLoader;
using Terraria.ID;

namespace CSkies.Items.Boxes
{
    public class HCBox : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Heartcore Music Box");
            Tooltip.SetDefault(@"Plays 'Morpho Knight Battle' from Kirby Star Allies");
        }

        public override void SetDefaults()
		{
			item.useStyle = ItemUseStyleID.SwingThrow;
			item.useTurn = true;
			item.useAnimation = 15;
			item.useTime = 10;
			item.autoReuse = true;
			item.consumable = true;
			item.createTile = mod.TileType("HCBox");
			item.width = 24;
			item.height = 24;
            item.rare = ItemRarityID.LightRed;
			item.value = 10000;
			item.accessory = true;
		}
        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.MusicBox);
            recipe.AddIngredient(null, "HeartSoul", 5);
            recipe.AddTile(TileID.Sawmill);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}
