using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CSkies.Tiles.Abyss
{
    public class Abice : ModTile
    {
        public override void SetDefaults()
        {
            Main.tileSolid[Type] = true;
            Main.tileMergeDirt[Type] = true;
            Main.tileBlendAll[this.Type] = false;
			Main.tileMerge[TileID.SnowBlock][Type] = true;
            soundType = SoundID.Tink;
            dustType = mod.DustType("VoidDust");
            drop = mod.ItemType("BlackIce");   //put your CustomBlock name
            AddMapEntry(new Color(60, 60, 100));
            TileID.Sets.Ices[Type] = true;
            minPick = 225;
        }
    }
}