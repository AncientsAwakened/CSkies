using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace CSkies.Tiles.Observatory
{
    public class StarBrickWall : ModWall
	{
		public override void SetDefaults()
        {
            Main.wallHouse[Type] = true;
            dustType = ModContent.DustType<Dusts.StarDust>();
            AddMapEntry(new Color(10, 70, 30));
            soundType = 21;
            Main.wallLargeFrames[Type] = 2;
        }

		public override void NumDust(int i, int j, bool fail, ref int num)
		{
			num = fail ? 1 : 3;
		}
    }

    public class StarBrickWallUnsafe : ModWall
    {
        public override void SetDefaults()
        {
            Main.wallHouse[Type] = false;
            dustType = ModContent.DustType<Dusts.StarDust>();
            AddMapEntry(new Color(10, 70, 30));
            soundType = 21;
            Main.wallLargeFrames[Type] = 2;
        }

        public override void NumDust(int i, int j, bool fail, ref int num)
        {
            num = fail ? 1 : 3;
        }

        public override void KillWall(int i, int j, ref bool fail)
        {
            fail = !CWorld.downedEnigma;
        }
    }
}