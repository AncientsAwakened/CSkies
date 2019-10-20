using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CSkies.Tiles
{
    public class CelestialGrass : ModTile
	{
		public static int _type;

		public override void SetDefaults()
		{
			Main.tileSolid[Type] = true;
            //SetModTree(new CTree());
            Main.tileBlendAll[Type] = true;
            TileID.Sets.NeedsGrassFraming[Type] = true;
            dustType = mod.DustType("CGrassDust");
			AddMapEntry(new Color(90, 30, 120));
		}
        
		/*public override int SaplingGrowthType(ref int style)
		{
			style = 0;
			return mod.TileType("");
		}*/
    }
}