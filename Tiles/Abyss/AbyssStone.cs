using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace CSkies.Tiles.Abyss
{
    public class AbyssStone : ModTile
    {
        public override void SetDefaults()
        {
            Main.tileSolid[Type] = true;
			Main.tileMergeDirt[Type] = true;
            Main.tileBlockLight[Type] = true;
            soundType = SoundID.Tink;
            dustType = ModContent.DustType<Dusts.VoidDust>();
            AddMapEntry(new Color(20, 20, 50));
			minPick = 225;
        }
    }
}