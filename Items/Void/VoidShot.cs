using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CSkies.Items.Void
{
    public class VoidShot : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Abyss Tome");
			Tooltip.SetDefault("Releases a homing singularity that drags in enemies and explodes on contact");
		}

		public override void SetDefaults()
		{
			item.mana = 35;
			item.damage = 160;
			item.useStyle = ItemUseStyleID.HoldingOut;
			item.shootSpeed = 9f;
			item.shoot = mod.ProjectileType("VoidMagic");
			item.width = 28;
			item.height = 30;
			item.UseSound = SoundID.Item117;
			item.useAnimation = 35;
			item.useTime = 35;
			item.autoReuse = true;
			item.noMelee = true;
			item.knockBack = 8f;
			item.rare = ItemRarityID.Purple;
			item.value = Item.sellPrice(0, 10, 0, 0);
			item.magic = true;
			item.noUseGraphic = true;
		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(null, "Skyshot", 1);
            recipe.AddIngredient(null, "VoidFragment", 5);
            recipe.AddTile(TileID.LunarCraftingStation);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
}
