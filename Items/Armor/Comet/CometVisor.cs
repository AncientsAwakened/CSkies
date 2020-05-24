using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Localization;


namespace CSkies.Items.Armor.Comet
{
    [AutoloadEquip(EquipType.Head)]
	public class CometVisor : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Cometsteel Visor");
			Tooltip.SetDefault(@"6% increased ranged damage");
        }

		public override void SetDefaults()
		{
			item.width = 24;
			item.height = 20;
			item.value = 10000;
			item.rare = ItemRarityID.Green;
			item.defense = 4;
		}

        public override bool IsArmorSet(Item head, Item body, Item legs)
		{
            return body.type == mod.ItemType("CometPlate") && legs.type == mod.ItemType("CometBoots");
        }

		public override void UpdateArmorSet(Player player)
		{
			player.setBonus = "Ranged projectiles have a chance to inflict cometspark on targets";
            player.GetModPlayer<CPlayer>().CometSet = true;
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(null, "CometBar", 15);
			recipe.AddTile(TileID.Anvils);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
	}
}