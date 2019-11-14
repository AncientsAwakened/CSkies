using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;

namespace CSkies.Tiles.Observatory
{
    public class StarCircuit : ModTile
    {
        public override void SetDefaults()
        {
            Main.tileSolid[Type] = true;
            Main.tileMergeDirt[Type] = true;
            Main.tileBlockLight[Type] = true;
            Main.tileBlendAll[Type] = true;
            soundType = 21;
            drop = mod.ItemType("StarCircuit");
            dustType = mod.DustType("Stardust");
            AddMapEntry(new Color(110, 142, 142));
			minPick = 225;
        }

        public static Color C(Color a)
        {
            return Color.White;
        }

        public override void PostDraw(int x, int y, SpriteBatch sb)
        {
            int xPos = Main.tile[x, y].frameX;
            int yPos = Main.tile[x, y].frameY;
            Texture2D glowTex = mod.GetTexture("Glowmasks/StarCircuit_Glow");
            Color GlowColor = new Color(120, 120, 120);
            Vector2 zero = Main.drawToScreen ? Vector2.Zero : new Vector2(Main.offScreenRange);
            Vector2 drawOffset = new Vector2(x * 16 - Main.screenPosition.X, y * 16 - Main.screenPosition.Y) + zero;
            Tile tile = Main.tile[x, y];
            if (!(tile.halfBrick() && tile.slope() == 0))
            {
                Main.spriteBatch.Draw(glowTex, drawOffset, new Rectangle?(new Rectangle(xPos, yPos, 18, 18)), GlowColor, 0.0f, Vector2.Zero, 1f, SpriteEffects.None, 0.0f);
            }
            else if (tile.halfBrick())
            {
                Main.spriteBatch.Draw(glowTex, drawOffset + new Vector2(0f, 8f), new Rectangle?(new Rectangle(xPos, yPos, 18, 8)), GlowColor, 0.0f, Vector2.Zero, 1f, SpriteEffects.None, 0.0f);
            }
        }
    }

    public class StarCircuitUnsafe : ModTile
    {
        public override void SetDefaults()
        {
            Main.tileSolid[Type] = true;
            Main.tileMergeDirt[Type] = true;
            Main.tileBlockLight[Type] = true;
            Main.tileBlendAll[Type] = true;
            soundType = 21;
            drop = mod.ItemType("StarCircuit");
            dustType = mod.DustType("Stardust");
            AddMapEntry(new Color(110, 142, 142));
            minPick = 225;
        }

        public static Color C(Color a)
        {
            return new Color(200, 200, 200);
        }

        public override void PostDraw(int x, int y, SpriteBatch sb)
        {
            int xPos = Main.tile[x, y].frameX;
            int yPos = Main.tile[x, y].frameY;
            Texture2D glowTex = mod.GetTexture("Glowmasks/StarCircuit_Glow");
            Color GlowColor = new Color(120, 120, 120);
            Vector2 zero = Main.drawToScreen ? Vector2.Zero : new Vector2(Main.offScreenRange);
            Vector2 drawOffset = new Vector2(x * 16 - Main.screenPosition.X, y * 16 - Main.screenPosition.Y) + zero;
            Tile tile = Main.tile[x, y];
            if (!(tile.halfBrick() && tile.slope() == 0))
            {
                Main.spriteBatch.Draw(glowTex, drawOffset, new Rectangle?(new Rectangle(xPos, yPos, 18, 18)), GlowColor, 0.0f, Vector2.Zero, 1f, SpriteEffects.None, 0.0f);
            }
            else if (tile.halfBrick())
            {
                Main.spriteBatch.Draw(glowTex, drawOffset + new Vector2(0f, 8f), new Rectangle?(new Rectangle(xPos, yPos, 18, 8)), GlowColor, 0.0f, Vector2.Zero, 1f, SpriteEffects.None, 0.0f);
            }
        }

        public override bool CanExplode(int i, int j)
        {
            return false;
        }

        public override bool CanKillTile(int i, int j, ref bool blockDamaged)
        {
            return CWorld.downedEnigma;
        }
    }
}