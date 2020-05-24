using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CSkies.Items.Star
{
    public class Railscope : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Railscope");
			Tooltip.SetDefault(@"Allows you to view slightly further by moving your mouse to the edge of the screen");
		}

		public override void SetDefaults()
        {
            item.autoReuse = true;
            item.useStyle = ItemUseStyleID.HoldingOut;
            item.useAnimation = 50;
            item.useTime = 50;
            item.width = 24;
            item.height = 28;
            item.UseSound = SoundID.Item12;
            item.knockBack = 0.75f;
            item.damage = 20;
            item.shootSpeed = 5f;
            item.noMelee = true;
            item.rare = ItemRarityID.Pink;
            item.ranged = true;
            item.value = Item.sellPrice(0, 2, 0, 0);
            item.shoot = ModContent.ProjectileType<Projectiles.Star.Starlaser>();
        }

        public override void HoldItem(Player player)
        {
            player.scope = true;
        }

        public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.SpaceGun);
            recipe.AddIngredient(mod, "Stelarite", 5);
            recipe.AddIngredient(mod, "CosmicStar", 2);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.SetResult(this);
			recipe.AddRecipe();
		}
		
		public override Vector2? HoldoutOffset()
		{
			return new Vector2(-12, -6);
		}
    }
}
