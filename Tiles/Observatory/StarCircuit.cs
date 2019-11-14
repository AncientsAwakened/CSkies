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
            soundType = 21;
            drop = mod.ItemType("StarCircuit");
            dustType = mod.DustType("Stardust");
            AddMapEntry(new Color(110, 142, 142));
			minPick = 225;
        }

        public override void ModifyLight(int x, int y, ref float r, ref float g, ref float b)
        {
            Color color = BaseUtility.ColorMult(Colors.COLOR_GLOWPULSE, 0.7f);
            r = color.R / 255f; g = color.G / 255f; b = color.B / 255f;
        }

        public static Color C(Color a)
        {
            return Color.White;
        }

        public override void PostDraw(int i, int j, SpriteBatch spriteBatch)
        {
            Tile tile = Main.tile[i, j];
            Vector2 zero = new Vector2(Main.offScreenRange, Main.offScreenRange);
            if (Main.drawToScreen)
            {
                zero = Vector2.Zero;
            }
            int height = tile.frameY == 36 ? 18 : 16;
            Texture2D glowTex = mod.GetTexture("Glowmasks/StarCircuit_Glow");
            Main.spriteBatch.Draw(glowTex, new Vector2((i * 16) - (int)Main.screenPosition.X, (j * 16) - (int)Main.screenPosition.Y) + zero, new Rectangle(tile.frameX, tile.frameY, 16, height), Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
        }
    }

    public class StarCircuitUnsafe : ModTile
    {
        public override void SetDefaults()
        {
            Main.tileSolid[Type] = true;
            Main.tileMergeDirt[Type] = true;
            Main.tileBlockLight[Type] = true;
            soundType = 21;
            drop = mod.ItemType("StarCircuit");
            dustType = mod.DustType("Stardust");
            AddMapEntry(new Color(110, 142, 142));
            minPick = 225;
        }

        public override void ModifyLight(int x, int y, ref float r, ref float g, ref float b)
        {
            Color color = BaseUtility.ColorMult(Colors.COLOR_GLOWPULSE, 0.7f);
            r = color.R / 255f; g = color.G / 255f; b = color.B / 255f;
        }

        public static Color C(Color a)
        {
            return Color.White;
        }

        public override void PostDraw(int i, int j, SpriteBatch spriteBatch)
        {
            Tile tile = Main.tile[i, j];
            Vector2 zero = new Vector2(Main.offScreenRange, Main.offScreenRange);
            if (Main.drawToScreen)
            {
                zero = Vector2.Zero;
            }
            int height = tile.frameY == 36 ? 18 : 16;
            Texture2D glowTex = mod.GetTexture("Glowmasks/StarCircuit_Glow");
            Main.spriteBatch.Draw(glowTex, new Vector2((i * 16) - (int)Main.screenPosition.X, (j * 16) - (int)Main.screenPosition.Y) + zero, new Rectangle(tile.frameX, tile.frameY, 16, height), Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
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