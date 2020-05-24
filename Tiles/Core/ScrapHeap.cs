using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CSkies.Tiles.Core
{
    public class ScrapHeap : ModTile
    {
        public override void SetDefaults()
        {
            Main.tileSolid[Type] = true;
            Main.tileMergeDirt[Type] = false;
            Main.tileBlockLight[Type] = true;
            Main.tileBlendAll[Type] = false;
            soundType = SoundID.Tink;
            dustType = DustID.Fire;
            AddMapEntry(new Color(142, 110, 110));
			minPick = 200;
        }

        public override bool CanExplode(int i, int j)
        {
            return false;
        }

        public override bool CanKillTile(int i, int j, ref bool blockDamaged)
        {
            return CWorld.downedRegulator;
        }

        public override void PostDraw(int x, int y, SpriteBatch sb)
        {
            int xPos = Main.tile[x, y].frameX;
            int yPos = Main.tile[x, y].frameY;
            Texture2D glowTex = mod.GetTexture("Glowmasks/Scrap_Glow");
            Vector2 zero = Main.drawToScreen ? Vector2.Zero : new Vector2(Main.offScreenRange);
            Vector2 drawOffset = new Vector2(x * 16 - Main.screenPosition.X, y * 16 - Main.screenPosition.Y) + zero;
            Tile tile = Main.tile[x, y];
            if (!(tile.halfBrick() && tile.slope() == 0))
            {
                Main.spriteBatch.Draw(glowTex, drawOffset, new Rectangle?(new Rectangle(xPos, yPos, 18, 18)), Colors.FlashInverse, 0.0f, Vector2.Zero, 1f, SpriteEffects.None, 0.0f);
            }
            else if (tile.halfBrick())
            {
                Main.spriteBatch.Draw(glowTex, drawOffset + new Vector2(0f, 8f), new Rectangle?(new Rectangle(xPos, yPos, 18, 8)), Colors.FlashInverse, 0.0f, Vector2.Zero, 1f, SpriteEffects.None, 0.0f);
            }
        }
    }
}