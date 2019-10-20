using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace CSkies.Tiles
{
    public class CelestialBrick : ModTile
    {
        public override void SetDefaults()
        {
            Main.tileSolid[Type] = true;
			Main.tileMergeDirt[Type] = true;
            Main.tileBlockLight[Type] = true;
            soundType = 21;
            dustType = ModContent.DustType<Dusts.CDust>();
            AddMapEntry(new Color(80, 30, 100));
			minPick = 225;
        }
    }
}