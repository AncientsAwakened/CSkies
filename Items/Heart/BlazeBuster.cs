using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CSkies.Items.Heart
{
    public class BlazeBuster : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Blaze Breaker");
			Tooltip.SetDefault("Fires an immensely powerful piercing laser that explodes on contact with enemies");
		}

		public override void SetDefaults()
        {
            item.autoReuse = true;
            item.useStyle = 5;
            item.useAnimation = 50;
            item.useTime = 50;
            item.width = 46;
            item.height = 28;
            item.UseSound = SoundID.Item14;
            item.knockBack = 0.75f;
            item.damage = 180;
            item.shootSpeed = 16f;
            item.noMelee = true;
            item.rare = 8;
            item.ranged = true;
            item.value = Item.sellPrice(0, 12, 0, 0);
            item.shoot = ModContent.ProjectileType<Projectiles.Star.Starlaser>();
        }

        public override void HoldItem(Player player)
        {
            player.scope = true;
        }

        public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(null, "Railscope");
            recipe.AddIngredient(null, "HeartSoul", 8);
            recipe.AddTile(TileID.LunarCraftingStation);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
		
		public override Vector2? HoldoutOffset()
		{
			return new Vector2(-12, -6);
		}
    }
}
