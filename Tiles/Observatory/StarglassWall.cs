using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace CSkies.Tiles.Observatory
{
    public class StarglassWall : ModWall
    {
        public override void SetDefaults()
        {
            Main.wallHouse[Type] = true;
            drop = mod.ItemType("StarglassWall");
            AddMapEntry(new Color(31, 82, 72));
        }
    }

    public class StarglassWallUnsafe : ModWall
    {
        public override void SetDefaults()
        {
            Main.wallHouse[Type] = false;
            drop = mod.ItemType("StarglassWall");
            AddMapEntry(new Color(11, 62, 52));
        }

        public override void KillWall(int i, int j, ref bool fail)
        {
            fail = !CWorld.downedEnigma;
        }
    }
}