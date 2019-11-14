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

        public override void PostDraw(int x, int y, SpriteBatch sb)
        {
            Tile tile = Main.tile[x, y];
            if (tile != null && tile.active() && tile.type == Type)
            {
                Texture2D glowTex = mod.GetTexture("Glowmasks/StarCircuit_Glow");
                BaseDrawing.DrawTileTexture(sb, glowTex, x, y, true, false, false, null, C);
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

        public override void PostDraw(int x, int y, SpriteBatch sb)
        {
            Tile tile = Main.tile[x, y];
            if (tile != null && tile.active() && tile.type == Type)
            {
                Texture2D glowTex = mod.GetTexture("Glowmasks/StarCircuit_Glow");
                BaseDrawing.DrawTileTexture(sb, glowTex, x, y, true, false, false, null, C);
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