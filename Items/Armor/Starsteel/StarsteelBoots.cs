using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace CSkies.Items.Armor.Starsteel
{
    [AutoloadEquip(EquipType.Legs)]
	public class StarsteelBoots : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Starsteel Boots");
			Tooltip.SetDefault(@"4% increased damage & critical strike chance");
		}

		public override void SetDefaults()
		{
			item.width = 22;
			item.height = 24;
			item.value = Item.sellPrice(0, 1, 80, 0);
			item.rare = ItemRarityID.LightRed;
			item.defense = 11;
		}

		public override void UpdateEquip(Player player)
		{
			player.allDamage += .04f;
			player.GetModPlayer<CPlayer>().CritAll(4);
		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(null, "Stelarite", 8);
			recipe.AddTile(TileID.MythrilAnvil);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
}