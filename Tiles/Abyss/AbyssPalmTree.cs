using Microsoft.Xna.Framework.Graphics;
using Terraria.ModLoader;

namespace CSkies.Tiles.Abyss
{
    class AbyssPalmTree : ModPalmTree
    {
        private Mod mod => ModLoader.GetMod("CSkies");

        public override int DropWood()
        {
            return mod.ItemType("AbyssWood");
        }

        public override Texture2D GetTexture()
        {
            
            return mod.GetTexture("Tiles/AbyssPalmTree");
        }

        public override Texture2D GetTopTextures()
        {
            return mod.GetTexture("Tiles/AbyssPalmTreetops");
        }
    }
}
