using Microsoft.Xna.Framework.Graphics;
using Terraria.ModLoader;

namespace CSkies.Tiles.Abyss
{
    public class Abysstus : ModCactus
	{
        private Mod Mod => ModLoader.GetMod("CSkies");

        public override Texture2D GetTexture()
		{
			return Mod.GetTexture("Tiles/Abysstus");
		}
    }
}