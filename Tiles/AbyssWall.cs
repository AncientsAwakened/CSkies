using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;

namespace CSkies.Tiles
{
    public class AbyssWall : ModWall
	{
		public override void SetDefaults()
		{
			dustType = ModContent.DustType<Dusts.VoidDust>();
            AddMapEntry(new Color(10, 10, 30));
            soundType = 21;
            Main.wallLargeFrames[Type] = 2;
        }

		public override void NumDust(int i, int j, bool fail, ref int num)
		{
			num = fail ? 1 : 3;
		}

        public static Color Glow(Color c) => Colors.Flash;

        public override void PostDraw(int i, int j, SpriteBatch spriteBatch)
        {
            Tile tile = Main.tile[i, j];
            Vector2 zero = new Vector2(Main.offScreenRange, Main.offScreenRange);
            if (Main.drawToScreen)
            {
                zero = Vector2.Zero;
            }
            int height = tile.frameY == 36 ? 18 : 16;
            BaseDrawing.DrawWallTexture(spriteBatch, mod.GetTexture("Glowmasks/DoomsdayWall_Glow"), i, j, false, Glow);
        }
    }
}