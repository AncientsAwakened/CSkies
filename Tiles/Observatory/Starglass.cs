using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace CSkies.Tiles.Observatory
{
    public class Starglass : ModTile
    {
        public override void SetDefaults()
        {
            Main.tileSolid[Type] = true;
			Main.tileMergeDirt[Type] = false;
            Main.tileBlockLight[Type] = true;
            drop = mod.ItemType("Starglass");
            AddMapEntry(new Color(100, 200, 100));
        }
    }

    public class StarglassUnsafe : ModTile
    {
        public override void SetDefaults()
        {
            Main.tileSolid[Type] = true;
            Main.tileMergeDirt[Type] = false;
            Main.tileBlockLight[Type] = true;
            drop = mod.ItemType("Starglass");
            AddMapEntry(new Color(100, 200, 100));
        }

        public override bool CanKillTile(int i, int j, ref bool blockDamaged)
        {
            return CWorld.downedEnigma;
        }

        public override bool CanExplode(int i, int j)
        {
            return false;
        }
    }
}