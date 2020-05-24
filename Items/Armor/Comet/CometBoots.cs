using Terraria.ID;
using Terraria.ModLoader;

namespace CSkies.Items.Armor.Comet
{
    [AutoloadEquip(EquipType.Legs)]
	public class CometBoots : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Cometsteel Boots");
            Tooltip.SetDefault("7% increased ranged damage");
		}

		public override void SetDefaults()
		{
			item.width = 22;
			item.height = 18;
			item.value = 5000;
			item.rare = ItemRarityID.Green;
			item.defense = 4;
		}

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(null, "CometBar", 20);
			recipe.AddTile(TileID.Anvils);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
	}
}