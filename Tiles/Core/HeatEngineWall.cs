using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CSkies.Tiles.Core
{
    public class HeatEngineWall : ModWall
	{
		public override void SetDefaults()
        {
            Main.wallHouse[Type] = true;
            dustType = DustID.Fire;
            AddMapEntry(new Color(10, 70, 30));
            soundType = 21;
        }

		public override void NumDust(int i, int j, bool fail, ref int num)
		{
			num = fail ? 1 : 3;
		}
    }

    public class HeatEngineWallUnsafe : ModWall
    {
        public override void SetDefaults()
        {
            Main.wallHouse[Type] = false;
            dustType = ModContent.DustType<Dusts.StarDust>();
            AddMapEntry(new Color(10, 70, 30));
            soundType = 21;
        }

        public override void NumDust(int i, int j, bool fail, ref int num)
        {
            num = fail ? 1 : 3;
        }

        public override void KillWall(int i, int j, ref bool fail)
        {
            fail = true; ;
        }
    }
}