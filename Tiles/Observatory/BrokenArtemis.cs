using Microsoft.Xna.Framework;
using Terraria.Enums;
using Terraria.DataStructures;
using Terraria;
using Terraria.ModLoader;
using Terraria.ObjectData;

namespace CSkies.Tiles.Observatory
{
    public class BrokenArtemis : ModTile
	{
		public override void SetDefaults()
		{
			Main.tileFrameImportant[Type] = true;
			Main.tileSolid[Type] = false;
			minPick = 200;
			TileObjectData.newTile.Height = 6;
			TileObjectData.newTile.Width = 10;
			TileObjectData.newTile.Origin = new Point16(0, 0);
            TileObjectData.newTile.AnchorTop = new AnchorData(AnchorType.SolidTile, TileObjectData.newTile.Width, 0);
            TileObjectData.newTile.AnchorBottom = default;
			TileObjectData.newTile.LavaDeath = false;
			TileObjectData.newTile.CoordinateHeights = new int[] { 16, 16, 16, 16, 16, 16};
			TileObjectData.newTile.CoordinateWidth = 16;
			TileObjectData.newTile.CoordinatePadding = 2;
			TileObjectData.addTile(Type);
			AddMapEntry(new Color(52, 75, 88));

		}

		public override void NumDust(int i, int j, bool fail, ref int num)
		{
			num = fail ? 1 : 3;
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