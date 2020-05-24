using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace CSkies.Items.Galaxium.Dragon
{
    [AutoloadEquip(EquipType.Legs)]
	public class DragonGreaves : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Dragon Greaves");
			Tooltip.SetDefault("7% increased critical strike chance"
				+ "\n+12% thrown velocity");
		}

		public override void SetDefaults()
		{
			item.width = 22;
			item.height = 18;
			item.value = Item.sellPrice(0, 3, 0, 0);
			item.rare = ItemRarityID.Red;
			item.defense = 10;
		}

		public override void UpdateEquip(Player player)
		{
			player.thrownCrit += 7;
			player.thrownVelocity += 0.12f;
		}
		
		public override void AddRecipes()
        {
            /*ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(null, "Dragon_Scale", 8);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.SetResult(this);
            recipe.AddRecipe();*/
        }
    }
}