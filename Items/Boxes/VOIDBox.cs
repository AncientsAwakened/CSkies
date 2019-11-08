using Terraria.ModLoader;
using Terraria.ID;

namespace CSkies.Items.Boxes
{
    public class VOIDBox : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("VOID Music Box");
            Tooltip.SetDefault(@"Plays 'Dark Matter/Miracle Matter Battle' from Kirby Star Allies");
        }

        public override void SetDefaults()
		{
			item.useStyle = 1;
			item.useTurn = true;
			item.useAnimation = 15;
			item.useTime = 10;
			item.autoReuse = true;
			item.consumable = true;
			item.createTile = mod.TileType("VOIDBox");
			item.width = 24;
			item.height = 24;
            item.rare = 4;
			item.value = 10000;
			item.accessory = true;
		}
        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.MusicBox);
            recipe.AddIngredient(null, "ObserverVoidEye");
            recipe.AddTile(TileID.Sawmill);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}
