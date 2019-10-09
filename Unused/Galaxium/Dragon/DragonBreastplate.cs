using Terraria;
using Terraria.ModLoader;

namespace CSkies.Items.Galaxium.Dragon
{
    [AutoloadEquip(EquipType.Body)]
	public class DragonBreastplate : ModItem
	{
		public override void SetStaticDefaults()
		{
			base.SetStaticDefaults();
			DisplayName.SetDefault("Dragon Breastplate");
			Tooltip.SetDefault("+24% thrown damage"
				+ "\n+30% thrown velocity"
				+ "\nThe hellstone charred the scales and now they have a shiny black look to them!");
		}

		public override void SetDefaults()
		{
			item.width = 26;
			item.height = 20;
			item.value = Item.sellPrice(0, 4, 0, 0);
			item.rare = 10;
			item.defense = 16;
		}

		public override void UpdateEquip(Player player)
		{
			player.thrownDamage += 0.24f;
			player.thrownVelocity += 0.30f;
		}
		
		public override void AddRecipes()
        {
            /*ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(null, "Dragon_Scale", 10);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.SetResult(this);
            recipe.AddRecipe();*/
        }
    }
}