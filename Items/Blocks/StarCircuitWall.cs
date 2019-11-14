using Terraria.ModLoader;
using Terraria.ID;

namespace CSkies.Items.Blocks
{
    public class StarCircuitWall : ModItem
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
            item.createWall = mod.WallType("StarCircuitWall");
        }

        
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("StarcircuitWall Wall");
        }

        public override void AddRecipes()
        {
            ModRecipe recipe;
            recipe = new ModRecipe(mod);
            recipe.AddIngredient(null, "StarCircuitBlock");
            recipe.AddTile(TileID.WorkBenches);
            recipe.SetResult(this, 4);
            recipe.AddRecipe();
            recipe = new ModRecipe(mod);
            recipe.AddIngredient(this, 4);
            recipe.AddTile(TileID.WorkBenches);
            recipe.SetResult(null, "StarCircuitBlock");
            recipe.AddRecipe();
        }
    }
}
