using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;

namespace CSkies.Tiles.Observatory
{
    public class StarCircuitWallUnsafe : ModWall
	{
        public override void SetDefaults()
        {
            drop = mod.ItemType("StarCircuitWall");
            AddMapEntry(new Color(31, 82, 72));
        }

        public static Color C(Color a)
        {
            return new Color(128, 128, 128);
        }

        public override void PostDraw(int x, int y, SpriteBatch sb)
        {
            Texture2D glowTex = mod.GetTexture("Glowmasks/StarCircuitWall_Glow");
            BaseDrawing.DrawWallTexture(sb, glowTex, x, y, false, C);
        }

        public override void KillWall(int i, int j, ref bool fail)
        {
            fail = !CWorld.downedEnigma;
        }
    }

    public class StarCircuitWall : ModWall
    {
        public override void SetDefaults()
        {
            Main.wallHouse[Type] = true;
            drop = mod.ItemType("StarCircuitWall");
            AddMapEntry(new Color(31, 82, 72));
        }

        public override void PostDraw(int x, int y, SpriteBatch sb)
        {
            Texture2D glowTex = mod.GetTexture("Glowmasks/StarCircuitWall_Glow");
            BaseDrawing.DrawWallTexture(sb, glowTex, x, y, false, StarCircuitWallUnsafe.C);
        }
    }
}