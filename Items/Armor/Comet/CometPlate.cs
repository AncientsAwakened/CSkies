using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CSkies.Items.Armor.Comet
{
    [AutoloadEquip(EquipType.Body)]
	public class CometPlate : ModItem
	{
		public override void SetStaticDefaults()
		{
			base.SetStaticDefaults();
			DisplayName.SetDefault("Cometsteel Platemail");
			Tooltip.SetDefault("6% increased ranged damage");
		}

		public override void SetDefaults()
		{
			item.width = 24;
			item.height = 24;
			item.value = 6000;
			item.rare = ItemRarityID.Green;
			item.defense = 5;
		}

		public override void UpdateEquip(Player player)
		{
			player.rangedDamage += .06f;
        }

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(null, "CometBar", 25);
            recipe.AddTile(TileID.Anvils);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
}