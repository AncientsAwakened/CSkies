using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace CSkies.Items.Galaxium.Dragon
{
    [AutoloadEquip(EquipType.Head)]
	public class DragonHelmet : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Dragon Helmet");
			Tooltip.SetDefault("15% increased thrown damage");
		}

		public override void SetDefaults()
		{
			item.width = 20;
			item.height = 24;
			item.value = Item.sellPrice(0, 5, 0, 0);
			item.rare = ItemRarityID.Red;
			item.defense = 8;
		}

		public override bool IsArmorSet(Item head, Item body, Item legs)
		{
			return body.type == mod.ItemType("DragonBreastplate") && legs.type == mod.ItemType("DragonGreaves");
		}
		
		public override void UpdateEquip(Player player)
		{
			player.thrownDamage += 0.15f;
		}

		public override void UpdateArmorSet(Player player)
		{
			player.moveSpeed += 0.20f; 
			player.setBonus = "+20% movement speed";
		}
		
		public override void AddRecipes()
        {
            /*ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(null, "Dragon_Scale", 6);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.SetResult(this);
            recipe.AddRecipe();*/
        }
	}
}