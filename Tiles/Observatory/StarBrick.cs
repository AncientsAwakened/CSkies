using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace CSkies.Tiles.Observatory
{
    public class StarBrick : ModTile
    {
        public override void SetDefaults()
        {
            Main.tileSolid[Type] = true;
            Main.tileMergeDirt[Type] = true;
            Main.tileBlockLight[Type] = true;
            Main.tileBlendAll[Type] = true;
            soundType = SoundID.Tink;
            drop = mod.ItemType("StarBrick");
            dustType = mod.DustType("Stardust");
            AddMapEntry(new Color(110, 142, 142));
			minPick = 200;
        }
    }

    public class StarBrickUnsafe : ModTile
    {
        public override void SetDefaults()
        {
            Main.tileSolid[Type] = true;
            Main.tileMergeDirt[Type] = true;
            Main.tileBlockLight[Type] = true;
            Main.tileBlendAll[Type] = true;
            soundType = SoundID.Tink;
            drop = mod.ItemType("StarBrick");
            dustType = mod.DustType("Stardust");
            AddMapEntry(new Color(110, 142, 142));
            minPick = 200;
        }

        public override bool CanExplode(int i, int j)
        {
            return false;
        }

        public override bool CanKillTile(int i, int j, ref bool blockDamaged)
        {
            return CWorld.downedEnigma;
        }
    }
}