using Microsoft.Xna.Framework;
using Terraria.ModLoader;

namespace CSkies.Tiles.Abyss
{
    public class HardenedAbyssSandWall : ModWall
	{
		public override void SetDefaults()
		{
            dustType = mod.DustType("VoidDust");
			AddMapEntry(new Color(12, 10, 25));
            Terraria.ID.WallID.Sets.Conversion.HardenedSand[Type] = true;
        }

        public override void NumDust(int i, int j, bool fail, ref int num)
		{
			num = fail ? 1 : 3;
		}
        
    }
}