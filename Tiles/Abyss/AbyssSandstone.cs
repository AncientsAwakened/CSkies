using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace CSkies.Tiles.Abyss
{
    public class AbyssSandstone : ModTile
    {
        public override void SetDefaults()
        {
            Main.tileSolid[Type] = true;
            Terraria.ID.TileID.Sets.Conversion.Sandstone[Type] = true;
            Main.tileBlendAll[Type] = true;
            Main.tileBlockLight[Type] = true;
            dustType = mod.DustType("VoidDust");
            drop = mod.ItemType("AbyssSandstone");   //put your CustomBlock name
            AddMapEntry(new Color(40, 30, 50));
            minPick = 65;
        }
    }
}