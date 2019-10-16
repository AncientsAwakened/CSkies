using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CSkies
{
    public class CGlobalTile : GlobalTile
    {
        public static int glowTick = 0;
        public static int glowMax = 100;

        public override void AnimateTile()
        {
            glowTick++;
            if (glowTick >= glowMax)
            {
                glowTick = 0;
            }
        }

        public static Color GetBlankColor(Color color, float min, float max, bool clamp) => GetTimedColor(Color.White, color, min, max, clamp);
        public static Color GetBlankColorDark(Color color) => GetBlankColor(color, 0f, .6f, false);
        public static Color GetBlankColorDim(Color color) => GetBlankColor(color, 0.4f, 1f, false);
        public static Color GetBlankColorBright(Color color) => GetBlankColor(color, 0.6f, 1f, false);
        public static Color GetBlankColorBrightInvert(Color color) => GetBlankColor(color, 1f, 0.6f, true);

        public override bool Drop(int i, int j, int type)
        {
            if (type == TileID.Dirt && TileID.Sets.BreakableWhenPlacing[TileID.Dirt]) //placing grass
            {
                return false;
            }

            if (type == TileID.Mud && TileID.Sets.BreakableWhenPlacing[TileID.Mud]) //placing grass
            {
                return false;
            }

            return base.Drop(i, j, type);
        }

        public static Color GetTimedColor(Color tColor, Color color, float min, float max, bool clamp)
        {
            Color glowColor = BaseUtility.ColorMult(tColor, BaseUtility.MultiLerp(glowTick / (float)glowMax, min, max, min));

            if (clamp)
            {
                if (color.R > glowColor.R) { glowColor.R = color.R; }
                if (color.G > glowColor.G) { glowColor.G = color.G; }
                if (color.B > glowColor.B) { glowColor.B = color.B; }
            }

            return glowColor;
        }

        public static Color GetGradientColor(Color tColor1, Color tColor2, Color color, bool clamp)
        {
            Color glowColor = Color.Lerp(tColor1, tColor2, BaseUtility.MultiLerp(glowTick / (float)glowMax, 0f, 1f, 0f));

            if (clamp)
            {
                if (color.R > glowColor.R)
                {
                    glowColor.R = color.R;
                }

                if (color.G > glowColor.G)
                {
                    glowColor.G = color.G;
                }

                if (color.B > glowColor.B)
                {
                    glowColor.B = color.B;
                }
            }

            return glowColor;
        }
    }
}

