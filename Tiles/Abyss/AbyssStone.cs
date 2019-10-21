using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace CSkies.Tiles.Abyss
{
    public class AbyssStone : ModTile
    {
        public override void SetDefaults()
        {
            Main.tileSolid[Type] = true;
			Main.tileMergeDirt[Type] = true;
            Main.tileBlockLight[Type] = true;
            soundType = 21;
            dustType = ModContent.DustType<Dusts.VoidDust>();
            AddMapEntry(new Color(20, 20, 50));
			minPick = 225;
        }
    }
}