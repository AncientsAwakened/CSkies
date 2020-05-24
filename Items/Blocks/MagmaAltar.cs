using Terraria.ModLoader;
using Terraria.ID;

namespace CSkies.Items.Blocks
{
    public class MagmaAltar : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Magma Altar");
        }

        public override void SetDefaults()
		{
			item.useStyle = ItemUseStyleID.SwingThrow;
			item.useTurn = true;
			item.useAnimation = 15;
			item.useTime = 10;
			item.autoReuse = true;
			item.consumable = true;
			item.createTile = mod.TileType("HeartAltar");
			item.width = 24;
			item.height = 24;
            item.rare = ItemRarityID.Purple;
            item.value = 10000;
			item.accessory = true;
		}

        public override void ModifyTooltips(System.Collections.Generic.List<TooltipLine> list)
        {
            foreach (TooltipLine line2 in list)
            {
                if (line2.mod == "Terraria" && line2.Name == "ItemName")
                {
                    line2.overrideColor = Colors.Rarity12;
                }
            }
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.LifeCrystal, 5);
            recipe.AddIngredient(ItemID.Meteorite, 20);
            recipe.AddIngredient(null, "MoltenHeart", 1);
            recipe.AddTile(TileID.LunarCraftingStation);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}
