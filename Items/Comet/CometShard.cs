using Terraria;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.ModLoader;
using Terraria.ID;

namespace CSkies.Items.Comet
{
    public class CometShard : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Comet Shard");
        }
        public override void SetDefaults()
        {
            item.width = 22;
            item.height = 22;
			item.maxStack = 99;
            item.rare = ItemRarityID.Green;
            item.value = 100;
        }

        public override void Update(ref float gravity, ref float maxFallSpeed)
        {
            Lighting.AddLight((int)((item.position.X + item.width) / 16f), (int)((item.position.Y + item.height / 2) / 16f), 0.1f, 0.7f, 0.8f);
            if (Main.rand.Next(25) == 0)
            {
                Dust.NewDust(item.position, item.width, item.height, 58, item.velocity.X * 0.5f, item.velocity.Y * 0.5f, 150, Color.Blue, 1.2f);
            }
            if (Main.rand.Next(50) == 0)
            {
                Gore.NewGore(item.position, new Vector2(item.velocity.X * 0.2f, item.velocity.Y * 0.2f), Main.rand.Next(16, 18), 1f);
            }
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
