using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CSkies.Items.Boss.Heartcore
{
    public class Sol : ModItem
	{
		public override void SetDefaults()
		{
			item.damage = 250;
			item.melee = true;
			item.width = 48;
			item.height = 48;
			item.useTime = 20;
			item.useAnimation = 20;
			item.useStyle = ItemUseStyleID.SwingThrow;
			item.shoot = mod.ProjectileType("FirePro");
			item.shootSpeed = 9f;
			item.knockBack = 7;
            item.value = Item.sellPrice(0, 12, 0, 0);
            item.rare = ItemRarityID.Yellow;
            item.UseSound = SoundID.Item15;
            item.autoReuse = true;
		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(null, "Starsaber", 1);
            recipe.AddIngredient(null, "HeartSoul", 8);
            recipe.AddTile(TileID.LunarCraftingStation);
            recipe.SetResult(this);
			recipe.AddRecipe();
		}

        public override void PostDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, float rotation, float scale, int whoAmI)
        {
            Texture2D texture = mod.GetTexture("Glowmasks/Sol_Glow");
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
        
        public override void ModifyTooltips(System.Collections.Generic.List<TooltipLine> list)
        {
            foreach (TooltipLine line2 in list)
            {
                if (line2.mod == "Terraria" && line2.Name == "ItemName")
                {
                    line2.overrideColor = Colors.Rarity12;
                }
            }
        }

    }
}
