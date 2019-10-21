using Microsoft.Xna.Framework;
using Terraria.ModLoader;

namespace CSkies.Tiles.Abyss
{
    public class AbyssSandstoneWall : ModWall
	{
		public override void SetDefaults()
		{
            dustType = mod.DustType("VoidDust");
			AddMapEntry(new Color(22, 10, 35));
            Terraria.ID.WallID.Sets.Conversion.Sandstone[Type] = true;
        }

        public override void NumDust(int i, int j, bool fail, ref int num)
		{
			num = fail ? 1 : 3;
		}
    }
}