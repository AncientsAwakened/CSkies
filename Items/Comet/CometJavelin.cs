using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework.Graphics;

namespace CSkies.Items.Comet
{
    public class CometJavelin : ModItem
	{
		public override void SetDefaults()
		{

			item.damage = 20;
			item.melee = true;
			item.width = 22;
			item.noUseGraphic = true;
			item.maxStack = 1;
			item.consumable = false;
			item.height = 44;
			item.useTime = 30;
			item.useAnimation = 30;
			item.shoot = mod.ProjectileType("CometJavelin");
			item.shootSpeed = 12;
			item.useStyle = 1;
			item.knockBack = 2;
			item.value = Item.sellPrice(0, 1, 0, 0);
			item.rare = 2;
			item.UseSound = SoundID.Item1;
			item.autoReuse = true;
			item.crit = 3;
		}

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Comet Javelin");
			Tooltip.SetDefault("");
		}

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(null, "CometBar", 5);
            recipe.AddIngredient(null, "CometFragment", 8);
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
		
		public override void PostDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, float rotation, float  scale, int whoAmI)
		{
			Texture2D texture = mod.GetTexture("Glowmasks/CometJavelin_Glow");
			spriteBatch.Draw
			(
				texture,
				new Vector2
				(
					item.position.X - Main.screenPosition.X + item.width * 0.5f,
					item.position.Y - Main.screenPosition.Y + item.height - texture.Height * 0.5f + 2f
				),
				new Rectangle(0, 0, texture.Width, texture.Height),
				Color.White,
				rotation,
				texture.Size() * 0.5f,
				scale, 
				SpriteEffects.None, 
				0f
			);
		}
	}
}
