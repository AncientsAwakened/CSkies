using Terraria.ID;
using Terraria.ModLoader;

namespace CSkies.Items.Comet
{
    public class CometDagger : ModItem
	{
		public override void SetDefaults()
		{
            item.damage = 20;            
            item.ranged = true;
            item.width = 20;
            item.height = 20;
			item.useTime = 14;
            item.useAnimation = 14;
            item.maxStack = 999;
            item.noUseGraphic = true;
            item.useStyle = 1;
			item.knockBack = 1;
			item.value = 10;
			item.rare = 2;
			item.shootSpeed = 12f;
			item.shoot = mod.ProjectileType("CometKnife");
			item.UseSound = SoundID.Item1;
			item.autoReuse = true;
            item.consumable = true;
            item.noMelee = true;
        }

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Comet Dagger");
            Tooltip.SetDefault("");
        }
        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(null, "CometBar", 1);
            recipe.AddIngredient(null, "CometFragment", 1);
            recipe.AddTile(TileID.Anvils);
            recipe.SetResult(this, 50);
            recipe.AddRecipe();
		}
    }
}
