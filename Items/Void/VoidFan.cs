using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework.Graphics;

namespace CSkies.Items.Void
{
    public class VoidFan : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Abyss Fan");
		}
		
		public override void SetDefaults()
		{
			item.rare = ItemRarityID.Purple;
			item.noUseGraphic = true;
			item.UseSound = SoundID.Item1;
			item.useStyle = ItemUseStyleID.SwingThrow;
			item.damage = 110;
			item.useAnimation = 16;
			item.useTime = 16;
			item.width = 26;
			item.height = 26;
			item.shoot = mod.ProjectileType("VoidKnife");
			item.shootSpeed = 18f;
			item.knockBack = 0f;
			item.ranged = true;
			item.value = Item.sellPrice(0, 10, 0, 0);
			item.autoReuse = true;
			item.crit = 20;
		}

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(null, "CometFan", 1);
            recipe.AddIngredient(null, "VoidFragment", 5);
            recipe.AddTile(TileID.LunarCraftingStation);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }

        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{
			int numberProjectiles = 2 + Main.rand.Next(3); // 2 or 4 shots
			for (int i = 0; i < numberProjectiles; i++)
			{
				Vector2 perturbedSpeed = new Vector2(speedX, speedY).RotatedByRandom(MathHelper.ToRadians(14));
				Projectile.NewProjectile(position.X, position.Y, perturbedSpeed.X, perturbedSpeed.Y, type, damage, knockBack, player.whoAmI);
			}
			return false;
		}
		
		public override void PostDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, float rotation, float  scale, int whoAmI)
		{
			Texture2D texture = mod.GetTexture("Glowmasks/CometFan_Glow");
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