using Microsoft.Xna.Framework;
using Terraria.DataStructures;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ObjectData;
using CSkies.Tiles.Core;

namespace CSkies.Tiles.Consoles
{
    public class Console1 : ModTile
	{
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
			name.SetDefault("Abyss Altar");
            dustType = mod.DustType("AbyssiumDust");
            AddMapEntry(new Color(0, 0 ,100), name);
            adjTiles = new int[] { TileID.DemonAltar };
        }

        public override bool CanKillTile(int i, int j, ref bool blockDamaged)
        {
            return CWorld.downedRegulator;
        }

        public override void ModifyLight(int x, int y, ref float r, ref float g, ref float b)
        {
            if (CWorld.ValveBroken)
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

        public override void AnimateTile(ref int frame, ref int frameCounter)
        {
            if (CWorld.ValveBroken)
            {
                frame = 1;
            }
            else
            {
                frame = 0;
            }
        }

        public override bool NewRightClick(int i, int j)
        {
            if (!CWorld.ValveBroken)
            {
                for (int x = 0; x < Main.maxTilesX; x++)
                {
                    for (int y = 0; y < Main.maxTilesY; y++)
                    {
                        if (Main.tile[x, y].active() && Main.tile[x, y].type == (ushort)ModContent.TileType<CoreValve>())
                        {
                            WorldGen.KillTile(x, y, false, false, true);
                        }
                    }
                }
                Main.NewText("WARNING!!! LAVA PRESSURE IS ABOVE CAPACITY! VALVE DAMAGE DETECTED!", Color.IndianRed);
                CWorld.ValveBroken = true;
            }
            return true;
        }
    }
}