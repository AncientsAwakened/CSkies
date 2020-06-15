using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.ModLoader;

namespace CSkies.Tiles.Core
{
    public class HeatCircuitWall : ModWall
	{
        public override void SetDefaults()
        {
            AddMapEntry(new Color(82, 30, 30));
        }

        public static Color C(Color a)
        {
            return new Color(128, 128, 128);
        }

        public override void PostDraw(int x, int y, SpriteBatch sb)
        {
            Texture2D glowTex = mod.GetTexture("Glowmasks/HeatCircuitWall_Glow");
            if (!CWorld.downedRegulator) BaseDrawing.DrawWallTexture(sb, glowTex, x, y, false, C);
        }

        public override void KillWall(int i, int j, ref bool fail)
        {
            fail = !CWorld.downedRegulator;
        }
    }
}