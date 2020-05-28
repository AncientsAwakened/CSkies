using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CSkies.Items.Boss.Void
{
    public class Singularity : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("The Singularity");
            Tooltip.SetDefault(@"Throws a vortex on a chain that attracts enemies towards it");
        }

        public override void SetDefaults()
        {
            item.autoReuse = true;
            item.useStyle = ItemUseStyleID.HoldingOut;
            item.useAnimation = 24;
            item.useTime = 24;
            item.knockBack = 15f;
            item.width = 20;
            item.height = 20;
            item.damage = 150;
            item.shoot = mod.ProjectileType("Singularity");
            item.shootSpeed = 14f;
            item.UseSound = SoundID.Item10;
            item.rare = ItemRarityID.Purple;
            item.value = Item.sellPrice(0, 20, 0, 0);
            item.melee = true;
            item.noMelee = true;
            item.noUseGraphic = true;
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(null, "Comet", 1);
            recipe.AddIngredient(null, "VoidFragment", 5);
            recipe.AddTile(TileID.LunarCraftingStation);
            recipe.SetResult(this, 1);
            recipe.AddRecipe();
        }
    }
}