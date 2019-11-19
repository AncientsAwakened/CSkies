using Microsoft.Xna.Framework;
using Terraria.Enums;
using Terraria.DataStructures;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ObjectData;
using Terraria.ModLoader.IO;
using System.IO;
using System.Collections.Generic;

namespace CSkies.Tiles.Observatory.Doors
{
	public class ObservatoryDoorEntrance : ModTile
	{
		public override void SetDefaults()
		{
			Main.tileFrameImportant[Type] = true;
			Main.tileSolid[Type] = false;
			minPick = 200;
			TileObjectData.newTile.Height = 3;
			TileObjectData.newTile.Width = 1;
			TileObjectData.newTile.Origin = new Point16(0, 0);
            TileObjectData.newTile.AnchorBottom = new AnchorData(AnchorType.SolidTile, TileObjectData.newTile.Width, 0);
			TileObjectData.newTile.LavaDeath = false;
			TileObjectData.newTile.CoordinateHeights = new int[] { 16, 16, 16 };
			TileObjectData.newTile.CoordinateWidth = 16;
			TileObjectData.newTile.CoordinatePadding = 2;
			TileObjectData.addTile(Type);
			AddMapEntry(new Color(62, 85, 98));
		}

        public override void AnimateTile(ref int frame, ref int frameCounter)
        {
            if (DoorWorld.Door1)
            {
                frameCounter = 0;
                if (++frame >= 9) frame = 8;
            }
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

    public class DoorWorld : ModWorld
    {
        public static bool Door1 = false;
        public static bool Door2 = false;
        public static bool Door3 = false;
        public static bool Door4 = false;

        #region saving/loading
        public override TagCompound Save()
        {
            var open = new List<string>();
            if (Door1) open.Add("1");
            if (Door2) open.Add("2");
            if (Door3) open.Add("3");
            if (Door4) open.Add("4");

            return new TagCompound
            {
                {"open", open},
            };
        }
        public override void Load(TagCompound tag)
        {
            var open = tag.GetList<string>("open");
            Door1 = open.Contains("1");
            Door2 = open.Contains("2");
            Door3 = open.Contains("3");
            Door4 = open.Contains("4");
        }
        public override void NetSend(BinaryWriter writer)
        {
            BitsByte doors = new BitsByte();
            doors[0] = Door1;
            doors[1] = Door2;
            doors[2] = Door3;
            doors[3] = Door4;
            writer.Write(doors);
        }
        public override void NetReceive(BinaryReader reader)
        {
            BitsByte doors = reader.ReadByte();
            Door1 = doors[0];
            Door2 = doors[1];
            Door3 = doors[2];
            Door4 = doors[3];
        }
        #endregion
    }
}