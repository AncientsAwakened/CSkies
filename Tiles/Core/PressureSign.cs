using System;
using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ObjectData;
using Terraria.ModLoader;
namespace CSkies.Tiles.Core
{
	public class PressureSign : ModTile
	{
		public override void SetDefaults()
		{
			Main.tileFrameImportant[Type] = true;
            Main.tileLavaDeath[Type] = false;
			TileObjectData.newTile.Width = 4;
			TileObjectData.newTile.Height = 3;
			TileObjectData.newTile.Origin = new Point16(0, 0);
			TileObjectData.newTile.AnchorWall = true;
			TileObjectData.newTile.UsesCustomCanPlace = true;
			TileObjectData.newTile.CoordinateHeights = new int[]{ 16, 16, 16 };
			TileObjectData.newTile.CoordinateWidth = 16;
			TileObjectData.newTile.CoordinatePadding = 2;
			TileObjectData.newTile.LavaDeath = false;				
            TileObjectData.addTile(Type);
			dustType = 7;
			disableSmartCursor = true;
			BaseTile.AddMapEntry(this, new Color(120, 164, 94), "Pressure Gague");
		}

		public override void AnimateTile(ref int frame, ref int frameCounter)
		{
			if (++frameCounter >= 8f)
			{
				frameCounter = 0;

				if (CWorld.ValveBroken)
				{
					if (++frame < 4 || frame >= 6) frame = 4;
				}
				else
				{
					if (++frame >= 4) frame = 0;
				}
			}
		}

	}
}