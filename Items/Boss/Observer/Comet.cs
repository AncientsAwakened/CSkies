using Terraria.ID;
using Terraria.ModLoader;

namespace CSkies.Items.Boss.Observer
{
    public class Comet : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("The Comet");			
		}		
		
        public override void SetDefaults()
        {
            item.width = 30;
            item.height = 30;
            item.maxStack = 1;
            item.rare = ItemRarityID.Green;
            item.value = BaseUtility.CalcValue(0, 0, 90, 50);
            item.useStyle = ItemUseStyleID.HoldingOut;
            item.useAnimation = 45;
            item.useTime = 45;
            item.UseSound = SoundID.Item1;
            item.damage = 24;
            item.knockBack = 7;
            item.melee = true;
            item.shoot = ModContent.ProjectileType<Projectiles.Comet.Comet>();
            item.shootSpeed = 12;
            item.noUseGraphic = true;
            item.noMelee = true;
            item.channel = true;		
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(null, "CometFragment", 3);
            recipe.AddIngredient(null, "CometBar", 5);
            recipe.AddIngredient(ItemID.BlueMoon, 1);
            recipe.AddTile(TileID.Hellforge);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}