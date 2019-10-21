using Microsoft.Xna.Framework;
using Terraria.ModLoader;

namespace CSkies.Tiles.Abyss
{
    public class AbyssLeafWall : ModWall
	{
		public override void SetDefaults()
		{
			dustType = mod.DustType("RazeleafDust");
			AddMapEntry(new Color(10, 5, 30));
            Terraria.ID.WallID.Sets.Conversion.Grass[Type] = true;
        }

		public override void NumDust(int i, int j, bool fail, ref int num)
		{
			num = fail ? 1 : 3;
		}
    }
}