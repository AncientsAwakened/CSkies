using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CSkies.Items.Comet
{
    public class CometTablet : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Comet Tablet");	
            BaseUtility.AddTooltips(item, new string[] { "Brings forth a travelling star to your world" });	
		}

        public override void SetDefaults()
        {
            item.width = 20;
            item.height = 20;
            item.maxStack = 30;
            item.rare = ItemRarityID.Green;
            item.useAnimation = 45;
            item.useTime = 45;
            item.consumable = true;
            item.useStyle = ItemUseStyleID.SwingThrow;
        }

        public override bool CanUseItem(Player player)
        {
            int num = 0;
            float num2 = Main.maxTilesX / 4200;
            int num3 = (int)(400f * num2);
            for (int j = 5; j < Main.maxTilesX - 5; j++)
            {
                int num4 = 5;
                while (num4 < Main.worldSurface)
                {
                    if (Main.tile[j, num4].active() && Main.tile[j, num4].type == (ushort)ModContent.TileType<Tiles.CometOre>())
                    {
                        num++;
                        if (num > num3)
                        {
                            BaseUtility.Chat("Another comet exists in this world in some form or another already...", new Color(136, 151, 255), true);
                            return false;
                        }
                    }
                    num4++;
                }
            }
            CWorld.DropMeteor();
            return true;
        }


        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(null, "CometShard", 9);
            recipe.AddTile(TileID.Anvils);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }

        public override void PostDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, float rotation, float scale, int whoAmI)
        {
            Texture2D texture = Main.itemTexture[item.type];
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