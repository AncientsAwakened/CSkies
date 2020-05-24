using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace CSkies.Tiles
{
    public class AbyssDoor : ModTile
    {
        public override void SetDefaults()
        {
            Main.tileSolid[Type] = true;
			Main.tileMergeDirt[Type] = true;
            Main.tileBlockLight[Type] = true;
            Main.tileMerge[Type][ModContent.TileType<AbyssBricks>()] = true;
            soundType = SoundID.Tink;
            dustType = ModContent.DustType<Dusts.VoidDust>();
            AddMapEntry(new Color(10, 10, 50));
			minPick = 99999;
        }

        public override bool CanExplode(int i, int j)
        {
            return false;
        }

        public override bool CanKillTile(int i, int j, ref bool blockDamaged)
        {
            return false;
        }
    }
}