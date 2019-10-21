using Microsoft.Xna.Framework.Graphics;
using Terraria.ModLoader;

namespace CSkies.Tiles.Abyss
{
    class AbyssTree : ModTree
    {
        private Mod mod => ModLoader.GetMod("CSkies");

        public override int DropWood()
        {
            return mod.ItemType("AbyssWood");
        }

        public override Texture2D GetTexture()
        {
            return mod.GetTexture("Tiles/AbyssTree");
        }

        public override Texture2D GetBranchTextures(int i, int j, int trunkOffset, ref int frame)
        {
            return mod.GetTexture("Tiles/AbyssTreeBranches");
        }

        public override Texture2D GetTopTextures(int i, int j, ref int frame, ref int frameWidth, ref int frameHeight, ref int xOffsetLeft, ref int yOffset)
        {
            return mod.GetTexture("Tiles/AbyssTreetop");
        }
    }
}
