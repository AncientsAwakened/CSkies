using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace CSkies.Items.Armor.Starsteel
{
    [AutoloadEquip(EquipType.Body)]
	public class StarsteelPlate : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Starsteel Chestplate");
			Tooltip.SetDefault(@"5% increased damage
4% Increased critical strike chance");
		}

		public override void SetDefaults()
		{
			item.width = 22;
			item.height = 24;
			item.value = Item.sellPrice (0, 2, 40, 0);
			item.rare = ItemRarityID.LightRed;
			item.defense = 16;
		}
		
		public override void UpdateEquip(Player player)
		{
			player.allDamage += .05f;
			player.GetModPlayer<CPlayer>().CritAll(5);
		}
		
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(null, "Stelarite", 10);
            recipe.AddTile(TileID.MythrilAnvil);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
}