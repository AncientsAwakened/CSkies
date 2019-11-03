using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace CSkies.Tiles.Abyss
{
    public class HardenedAbyssSand : ModTile
    {
        public override void SetDefaults()
        {
            Main.tileSolid[Type] = true;
            Terraria.ID.TileID.Sets.Conversion.HardenedSand[Type] = true;
            Main.tileBlendAll[Type] = true;
            Main.tileBlockLight[Type] = true;
            dustType = mod.DustType("VoidDust");
            drop = mod.ItemType("HardenedAbyssSand");   //put your CustomBlock name
            AddMapEntry(new Color(30, 17, 50));
            minPick = 65;
        }
    }
}