using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CSkies.Items.Boss.Void
{
    public class VoidJavelin : ModItem
	{
		public override void SetDefaults()
		{
			item.damage = 250;
			item.melee = true;
			item.width = 22;
			item.noUseGraphic = true;
			item.maxStack = 1;
			item.consumable = false;
			item.height = 44;
			item.useTime = 15;
			item.useAnimation = 15;
			item.shoot = mod.ProjectileType("VoidJavelin");
			item.shootSpeed = 16;
			item.useStyle = ItemUseStyleID.SwingThrow;
			item.knockBack = 4;
			item.value = Item.sellPrice(0, 10, 0, 0);
			item.rare = ItemRarityID.Purple;
			item.UseSound = SoundID.Item1;
			item.autoReuse = true;
			item.crit = 3;
		}

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Void Javelin");
			Tooltip.SetDefault("");
		}

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(null, "CometJavelin", 1);
            recipe.AddIngredient(null, "VoidFragment" ,5);
            recipe.AddTile(TileID.Anvils);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }

        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{
			Vector2 perturbedSpeed = new Vector2(speedX, speedY).RotatedByRandom(MathHelper.ToRadians(02));
			speedX = perturbedSpeed.X;
			speedY = perturbedSpeed.Y;
			return true;
		}
	}
}
