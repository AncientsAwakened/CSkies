using System.Collections.Generic;

using Microsoft.Xna.Framework;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.World.Generation;

namespace CSkies.Worldgen
{
    public class Vault : MicroBiome
    {
        public override bool Place(Point origin, StructureMap structures)
        {
            Mod mod = CSkies.inst;

            Dictionary<Color, int> colorToTile = new Dictionary<Color, int>
            {
                [new Color(0, 0, 255)] = mod.TileType("AbyssBricks"),
                [new Color(0, 255, 0)] = mod.TileType("AbyssDoor"),
                [new Color(255, 255, 255)] = -2,
                [Color.Black] = -1
            };

            Dictionary<Color, int> colorToWall = new Dictionary<Color, int>
            {
                [new Color(255, 255, 255)] = mod.WallType("AbyssWall"),
                [Color.Black] = -1
            };

            TexGen gen = BaseWorldGenTex.GetTexGenerator(mod.GetTexture("Worldgen/AbyssVault"), colorToTile, mod.GetTexture("Worldgen/AbyssVaultWall"), colorToWall, null, mod.GetTexture("Worldgen/AbyssVaultSlopes"));

            gen.Generate(origin.X, origin.Y, true, true);

            if (CWorld.VaultCount == 0)
            {
                WorldGen.PlaceObject(origin.X + 29, origin.Y + 31, mod.TileType("HeartAltar2"));
                NetMessage.SendObjectPlacment(-1, origin.X + 29, origin.Y + 31, mod.TileType("HeartAltar2"), 0, 0, -1, -1);
            }
            else if (CWorld.VaultCount == 1)
            {
                WorldGen.PlaceObject(origin.X + 13, origin.Y + 14, mod.TileType("HeartAltar3"));
                NetMessage.SendObjectPlacment(-1, origin.X + 29, origin.Y + 31, mod.TileType("HeartAltar3"), 0, 0, -1, -1);
            }
            else if (CWorld.VaultCount == 2)
            {
                WorldGen.PlaceObject(origin.X + 13, origin.Y + 14, mod.TileType("HeartAltar4"));
                NetMessage.SendObjectPlacment(-1, origin.X + 29, origin.Y + 31, mod.TileType("HeartAltar4"), 0, 0, -1, -1);
            }

            return true;
        }

    }
    public class VaultPlanet : MicroBiome
    {
        public override bool Place(Point origin, StructureMap structures)
        {
            Mod mod = CSkies.inst;

            Dictionary<Color, int> colorToTile = new Dictionary<Color, int>
            {
                [new Color(0, 0, 255)] = mod.TileType("AbyssBricks"),
                [new Color(0, 255, 0)] = mod.TileType("AbyssDoor"),
                [new Color(255, 0, 0)] = TileID.Grass,
                [new Color(255, 0, 255)] = TileID.Dirt,
                [new Color(255, 255, 255)] = -2,
                [Color.Black] = -1
            };

            Dictionary<Color, int> colorToWall = new Dictionary<Color, int>
            {
                [new Color(0, 0, 255)] = mod.WallType("AbyssWall"),
                [Color.Black] = -1
            };

            TexGen gen = BaseWorldGenTex.GetTexGenerator(mod.GetTexture("Worldgen/AbyssVaultPlanet"), colorToTile, mod.GetTexture("Worldgen/AbyssVaultPlanetWall"), colorToWall, null, mod.GetTexture("Worldgen/AbyssVaultPlanetSlopes"));

            gen.Generate(origin.X, origin.Y, true, true);

            WorldGen.PlaceObject(origin.X + 32, origin.Y + 34, mod.TileType("HeartAltar1"));
            NetMessage.SendObjectPlacment(-1, origin.X + 17, origin.Y + 18, mod.TileType("HeartAltar1"), 0, 0, -1, -1);

            return true;
        }

    }
}