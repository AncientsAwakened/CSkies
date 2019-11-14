using Microsoft.Xna.Framework;
using Terraria.ModLoader;

namespace CSkies.Tiles
{
    public class AbyssBrickWall : ModWall
	{
		public override void SetDefaults()
		{
			dustType = ModContent.DustType<Dusts.VoidDust>();
            AddMapEntry(new Color(10, 10, 20));
            soundType = 21;
        }

		public override void NumDust(int i, int j, bool fail, ref int num)
		{
			num = fail ? 1 : 3;
		}
    }
}