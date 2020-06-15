using Microsoft.Xna.Framework;
using Terraria.DataStructures;
using Terraria;
using Terraria.ModLoader;
using Terraria.ObjectData;

namespace CSkies.Tiles.Deco
{
    public class Console : ModTile
    {
        bool off = true;

		public override void SetDefaults()
		{
			Main.tileFrameImportant[Type] = true;
			Main.tileLavaDeath[Type] = false;
			TileObjectData.newTile.CopyFrom(TileObjectData.Style3x2);
            TileObjectData.newTile.Origin = new Point16(0, 0);
            TileObjectData.addTile(Type);
            Main.tileHammer[Type] = true;
			disableSmartCursor = true;
			ModTranslation name = CreateMapEntryName();
			name.SetDefault("Console");
            dustType = mod.DustType("StarDust");
            AddMapEntry(new Color(0, 0, 100), name);
        }

        public override void ModifyLight(int x, int y, ref float r, ref float g, ref float b)
        {
            if (off)
            {
                r = 0.25f;
                g = 0.1f;
                b = 0.0f;
            }
            else
            {
                r = 0;
                g = 0.1f;
                b = 0.25f;
            }
        }

        public override void AnimateIndividualTile(int type, int i, int j, ref int frameXOffset, ref int frameYOffset)
        {
            Tile tile = Main.tile[i, j];

            if (off)
            {
                tile.frameX = 36;
            }
            else
            {
                tile.frameX = 0;
            }
        }
    }
}