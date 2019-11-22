using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.World.Generation;
using CSkies.Tiles.Abyss;
using CSkies.Tiles.Observatory;
using CSkies.Tiles;

namespace CSkies.Worldgen
{
    public class Vault : MicroBiome
    {
        public override bool Place(Point origin, StructureMap structures)
        {
            Mod mod = CSkies.inst;
            ushort tileGrass = (ushort)ModContent.TileType<AbyssGrass>(), tileStone = (ushort)ModContent.TileType<AbyssStone>(), tileIce = (ushort)ModContent.TileType<Abice>(),
            tileSand = (ushort)ModContent.TileType<AbyssSand>(), tileSandHardened = (ushort)ModContent.TileType<HardenedAbyssSand>(), tileSandstone = (ushort)ModContent.TileType<AbyssSandstone>(), 
            tileMoss = (ushort)ModContent.TileType<AbyssMoss>();

            byte StoneWall = (byte)ModContent.WallType<AbyssStoneWall>(), SandstoneWall = (byte)ModContent.WallType<AbyssSandstoneWall>(), HardenedSandWall = (byte)ModContent.WallType<HardenedAbyssSandWall>(),
            GrassWall = (byte)ModContent.WallType<AbyssLeafWall>();

            int biomeRadius = 89;

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

            Point newOrigin = new Point(origin.X, origin.Y);

            WorldUtils.Gen(newOrigin, new Shapes.Circle(biomeRadius), Actions.Chain(new GenAction[] //grass...
			{
                new InWorld(),
                new Modifiers.OnlyTiles(new ushort[]{ TileID.Grass, TileID.CorruptGrass, TileID.FleshGrass, TileID.HallowedGrass }),
                new Modifiers.RadialDither(biomeRadius - 5, biomeRadius),
                new SetModTile(tileGrass, true, true)
            }));
            WorldUtils.Gen(newOrigin, new Shapes.Circle(biomeRadius), Actions.Chain(new GenAction[] //moss...
			{
                new InWorld(),
                new Modifiers.OnlyTiles(new ushort[]{ TileID.BlueMoss, TileID.BrownMoss, TileID.GreenMoss, TileID.LavaMoss, TileID.LongMoss, TileID.PurpleMoss, TileID.RedMoss }),
                new Modifiers.RadialDither(biomeRadius - 5, biomeRadius),
                new SetModTile(tileMoss, true, true)
            }));
            WorldUtils.Gen(newOrigin, new Shapes.Circle(biomeRadius), Actions.Chain(new GenAction[] //stone...
			{
                new InWorld(),
                new Modifiers.OnlyTiles(new ushort[]{ TileID.Stone, TileID.Ebonstone, TileID.Crimstone, TileID.Pearlstone }),
                new Modifiers.RadialDither(biomeRadius - 5, biomeRadius),
                new SetModTile(tileStone, true, true)
            }));
            WorldUtils.Gen(newOrigin, new Shapes.Circle(biomeRadius), Actions.Chain(new GenAction[] //ice...
			{
                new InWorld(),
                new Modifiers.OnlyTiles(new ushort[]{ TileID.IceBlock, TileID.CorruptIce, TileID.FleshIce }),
                new Modifiers.RadialDither(biomeRadius - 5, biomeRadius),
                new SetModTile(tileIce, true, true)
            }));
            WorldUtils.Gen(newOrigin, new Shapes.Circle(biomeRadius), Actions.Chain(new GenAction[] //sand...
			{
                new InWorld(),
                new Modifiers.OnlyTiles(new ushort[]{ TileID.Sand, TileID.Ebonsand, TileID.Crimsand }),
                new Modifiers.RadialDither(biomeRadius - 5, biomeRadius),
                new SetModTile(tileSand, true, true)
            }));
            WorldUtils.Gen(newOrigin, new Shapes.Circle(biomeRadius), Actions.Chain(new GenAction[] //hardened sand...
			{
                new InWorld(),
                new Modifiers.OnlyTiles(new ushort[]{ TileID.HardenedSand, TileID.CorruptHardenedSand, TileID.CrimsonHardenedSand }),
                new Modifiers.RadialDither(biomeRadius - 5, biomeRadius),
                new SetModTile(tileSandHardened, true, true)
            }));
            WorldUtils.Gen(newOrigin, new Shapes.Circle(biomeRadius), Actions.Chain(new GenAction[] //...and sandstone.
			{
                new InWorld(),
                new Modifiers.OnlyTiles(new ushort[]{ TileID.Sandstone, TileID.CorruptSandstone, TileID.CrimsonSandstone }),
                new Modifiers.RadialDither(biomeRadius - 5, biomeRadius),
                new SetModTile(tileSandstone, true, true)
            }));
            WorldUtils.Gen(newOrigin, new Shapes.Circle(biomeRadius), Actions.Chain(new GenAction[] //Walls
			{
                new InWorld(),
                new Modifiers.OnlyWalls(new byte[]{ WallID.Stone, WallID.EbonstoneUnsafe, WallID.CrimstoneUnsafe }),
                new Modifiers.RadialDither(biomeRadius - 5, biomeRadius),
                new PlaceModWall(StoneWall, true)
            }));
            WorldUtils.Gen(newOrigin, new Shapes.Circle(biomeRadius), Actions.Chain(new GenAction[] //Walls
			{
                new InWorld(),
                new Modifiers.OnlyWalls(new byte[]{ WallID.Sandstone, WallID.CorruptSandstone, WallID.CrimsonSandstone }),
                new Modifiers.RadialDither(biomeRadius - 5, biomeRadius),
                new PlaceModWall(SandstoneWall, true)
            }));
            WorldUtils.Gen(newOrigin, new Shapes.Circle(biomeRadius), Actions.Chain(new GenAction[] //Walls
			{
                new InWorld(),
                new Modifiers.OnlyWalls(new byte[]{ WallID.HardenedSand, WallID.CorruptHardenedSand, WallID.CrimsonHardenedSand }),
                new Modifiers.RadialDither(biomeRadius - 5, biomeRadius),
                new PlaceModWall(HardenedSandWall, true)
            }));
            WorldUtils.Gen(newOrigin, new Shapes.Circle(biomeRadius), Actions.Chain(new GenAction[] //Walls
			{
                new InWorld(),
                new Modifiers.OnlyWalls(new byte[]{ WallID.HardenedSand, WallID.CorruptHardenedSand, WallID.CrimsonHardenedSand }),
                new Modifiers.RadialDither(biomeRadius - 5, biomeRadius),
                new PlaceModWall(HardenedSandWall, true)
            }));
            WorldUtils.Gen(newOrigin, new Shapes.Circle(biomeRadius), Actions.Chain(new GenAction[] //Walls
			{
                new InWorld(),
                new Modifiers.OnlyWalls(new byte[]{ WallID.GrassUnsafe, WallID.CorruptGrassUnsafe, WallID.CrimsonGrassUnsafe }),
                new Modifiers.RadialDither(biomeRadius - 5, biomeRadius),
                new PlaceModWall(GrassWall, true)
            }));

            int genX = origin.X - (gen.width / 2);
            int genY = origin.Y - 30;
            gen.Generate(genX, genY, true, true);

            if (CWorld.VaultCount == 0)
            {
                CUtils.ObectPlace(genX + 29, genY + 31, mod.TileType("HeartAltar2"));
            }
            else if (CWorld.VaultCount == 1)
            {
                CUtils.ObectPlace(genX + 29, genY + 31, mod.TileType("HeartAltar3"));
            }
            else if (CWorld.VaultCount == 2)
            {
                CUtils.ObectPlace(genX + 29, genY + 31, mod.TileType("HeartAltar4"));
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
                [new Color(0, 0, 255)] = ModContent.TileType<AbyssBricks>(),
                [new Color(0, 255, 0)] = ModContent.TileType<AbyssDoor>(),
                [new Color(255, 0, 0)] = ModContent.TileType<AbyssStone>(),
                [new Color(255, 0, 255)] = ModContent.TileType<AbyssGrass>(),
                [new Color(255, 255, 255)] = -2,
                [Color.Black] = -1
            };

            Dictionary<Color, int> colorToWall = new Dictionary<Color, int>
            {
                [new Color(0, 0, 255)] = ModContent.WallType<AbyssWall>(),
                [Color.Black] = -1
            };

            TexGen gen = BaseWorldGenTex.GetTexGenerator(mod.GetTexture("Worldgen/AbyssVaultPlanet"), colorToTile, mod.GetTexture("Worldgen/AbyssVaultPlanetWall"), colorToWall, null, mod.GetTexture("Worldgen/AbyssVaultPlanetSlopes"));

            gen.Generate(origin.X, origin.Y, true, true);

            CUtils.ObectPlace(origin.X + 32, origin.Y + 34, mod.TileType("HeartAltar1"));

            return true;
        }
    }

    public class Observatory : MicroBiome
    {
        public override bool Place(Point origin, StructureMap structures)
        {
            Mod mod = CSkies.inst;

            Dictionary<Color, int> colorToTile = new Dictionary<Color, int>
            {
                [new Color(0, 255, 0)] = ModContent.TileType<StarBrickUnsafe>(),
                [new Color(255, 0, 0)] = ModContent.TileType<StarglassUnsafe>(),
                [new Color(0, 0, 255)] = ModContent.TileType<StarCircuitUnsafe>(),
                [new Color(255, 255, 255)] = -2,
                [Color.Black] = -1
            };

            Dictionary<Color, int> colorToWall = new Dictionary<Color, int>
            {
                [new Color(0, 255, 0)] = ModContent.WallType<StarBrickWallUnsafe>(),
                [new Color(255, 0, 0)] = ModContent.WallType<StarglassWallUnsafe>(),
                [new Color(0, 0, 255)] = ModContent.WallType<StarCircuitWallUnsafe>(),
                [Color.Black] = -1
            };

            TexGen gen = BaseWorldGenTex.GetTexGenerator(mod.GetTexture("Worldgen/Observatory"), colorToTile, mod.GetTexture("Worldgen/ObservatoryWalls"), colorToWall, null, mod.GetTexture("Worldgen/ObservatorySlopes"));

            gen.Generate(origin.X, origin.Y, true, true);

            #region Decorations

            CUtils.ObectPlace(origin.X + 145, origin.Y + 118, mod.TileType("BrokenArtemis"));

            #region Lanterns
            CUtils.ObectPlace(origin.X + 134, origin.Y + 57, mod.TileType("ObservatoryLantern"));
            CUtils.ObectPlace(origin.X + 165, origin.Y + 57, mod.TileType("ObservatoryLantern"));
            CUtils.ObectPlace(origin.X + 54, origin.Y + 63, mod.TileType("ObservatoryLantern"));
            CUtils.ObectPlace(origin.X + 59, origin.Y + 63, mod.TileType("ObservatoryLantern"));
            CUtils.ObectPlace(origin.X + 240, origin.Y + 63, mod.TileType("ObservatoryLantern"));
            CUtils.ObectPlace(origin.X + 245, origin.Y + 63, mod.TileType("ObservatoryLantern"));
            CUtils.ObectPlace(origin.X + 41, origin.Y + 66, mod.TileType("ObservatoryLantern"));
            CUtils.ObectPlace(origin.X + 258, origin.Y + 66, mod.TileType("ObservatoryLantern"));
            CUtils.ObectPlace(origin.X + 129, origin.Y + 70, mod.TileType("ObservatoryLantern"));
            CUtils.ObectPlace(origin.X + 170, origin.Y + 70, mod.TileType("ObservatoryLantern"));
            CUtils.ObectPlace(origin.X + 40, origin.Y + 75, mod.TileType("ObservatoryLantern"));
            CUtils.ObectPlace(origin.X + 70, origin.Y + 75, mod.TileType("ObservatoryLantern"));
            CUtils.ObectPlace(origin.X + 78, origin.Y + 75, mod.TileType("ObservatoryLantern"));
            CUtils.ObectPlace(origin.X + 81, origin.Y + 75, mod.TileType("ObservatoryLantern"));
            CUtils.ObectPlace(origin.X + 218, origin.Y + 75, mod.TileType("ObservatoryLantern"));
            CUtils.ObectPlace(origin.X + 221, origin.Y + 75, mod.TileType("ObservatoryLantern"));
            CUtils.ObectPlace(origin.X + 229, origin.Y + 75, mod.TileType("ObservatoryLantern"));
            CUtils.ObectPlace(origin.X + 259, origin.Y + 75, mod.TileType("ObservatoryLantern"));
            CUtils.ObectPlace(origin.X + 89, origin.Y + 80, mod.TileType("ObservatoryLantern"));
            CUtils.ObectPlace(origin.X + 210, origin.Y + 80, mod.TileType("ObservatoryLantern"));
            CUtils.ObectPlace(origin.X + 71, origin.Y + 88, mod.TileType("ObservatoryLantern"));
            CUtils.ObectPlace(origin.X + 76, origin.Y + 88, mod.TileType("ObservatoryLantern"));
            CUtils.ObectPlace(origin.X + 223, origin.Y + 88, mod.TileType("ObservatoryLantern"));
            CUtils.ObectPlace(origin.X + 228, origin.Y + 88, mod.TileType("ObservatoryLantern"));
            CUtils.ObectPlace(origin.X + 33, origin.Y + 91, mod.TileType("ObservatoryLantern"));
            CUtils.ObectPlace(origin.X + 37, origin.Y + 91, mod.TileType("ObservatoryLantern"));
            CUtils.ObectPlace(origin.X + 44, origin.Y + 91, mod.TileType("ObservatoryLantern"));
            CUtils.ObectPlace(origin.X + 255, origin.Y + 91, mod.TileType("ObservatoryLantern"));
            CUtils.ObectPlace(origin.X + 262, origin.Y + 91, mod.TileType("ObservatoryLantern"));
            CUtils.ObectPlace(origin.X + 266, origin.Y + 91, mod.TileType("ObservatoryLantern"));
            CUtils.ObectPlace(origin.X + 34, origin.Y + 99, mod.TileType("ObservatoryLantern"));
            CUtils.ObectPlace(origin.X + 37, origin.Y + 99, mod.TileType("ObservatoryLantern"));
            CUtils.ObectPlace(origin.X + 262, origin.Y + 99, mod.TileType("ObservatoryLantern"));
            CUtils.ObectPlace(origin.X + 265, origin.Y + 99, mod.TileType("ObservatoryLantern"));
            CUtils.ObectPlace(origin.X + 67, origin.Y + 102, mod.TileType("ObservatoryLantern"));
            CUtils.ObectPlace(origin.X + 70, origin.Y + 102, mod.TileType("ObservatoryLantern"));
            CUtils.ObectPlace(origin.X + 229, origin.Y + 102, mod.TileType("ObservatoryLantern"));
            CUtils.ObectPlace(origin.X + 232, origin.Y + 102, mod.TileType("ObservatoryLantern"));
            CUtils.ObectPlace(origin.X + 37, origin.Y + 111, mod.TileType("ObservatoryLantern"));
            CUtils.ObectPlace(origin.X + 79, origin.Y + 111, mod.TileType("ObservatoryLantern"));
            CUtils.ObectPlace(origin.X + 220, origin.Y + 111, mod.TileType("ObservatoryLantern"));
            CUtils.ObectPlace(origin.X + 262, origin.Y + 111, mod.TileType("ObservatoryLantern"));
            CUtils.ObectPlace(origin.X + 93, origin.Y + 112, mod.TileType("ObservatoryLantern"));
            CUtils.ObectPlace(origin.X + 96, origin.Y + 112, mod.TileType("ObservatoryLantern"));
            CUtils.ObectPlace(origin.X + 99, origin.Y + 112, mod.TileType("ObservatoryLantern"));
            CUtils.ObectPlace(origin.X + 200, origin.Y + 112, mod.TileType("ObservatoryLantern"));
            CUtils.ObectPlace(origin.X + 203, origin.Y + 112, mod.TileType("ObservatoryLantern"));
            CUtils.ObectPlace(origin.X + 206, origin.Y + 112, mod.TileType("ObservatoryLantern"));

            CUtils.ObectPlace(origin.X + 79, origin.Y + 123, mod.TileType("ObservatoryLantern"));
            CUtils.ObectPlace(origin.X + 82, origin.Y + 123, mod.TileType("ObservatoryLantern"));
            CUtils.ObectPlace(origin.X + 85, origin.Y + 123, mod.TileType("ObservatoryLantern"));
            CUtils.ObectPlace(origin.X + 88, origin.Y + 123, mod.TileType("ObservatoryLantern"));
            CUtils.ObectPlace(origin.X + 211, origin.Y + 123, mod.TileType("ObservatoryLantern"));
            CUtils.ObectPlace(origin.X + 214, origin.Y + 123, mod.TileType("ObservatoryLantern"));
            CUtils.ObectPlace(origin.X + 217, origin.Y + 123, mod.TileType("ObservatoryLantern"));
            CUtils.ObectPlace(origin.X + 220, origin.Y + 123, mod.TileType("ObservatoryLantern"));

            #endregion

            #region Chandeliers
            CUtils.ObectPlace(origin.X + 140, origin.Y + 48, mod.TileType("ObservatoryChandelier"));
            CUtils.ObectPlace(origin.X + 159, origin.Y + 48, mod.TileType("ObservatoryChandelier"));
            CUtils.ObectPlace(origin.X + 31, origin.Y + 66, mod.TileType("ObservatoryChandelier"));
            CUtils.ObectPlace(origin.X + 269, origin.Y + 66, mod.TileType("ObservatoryChandelier"));
            CUtils.ObectPlace(origin.X + 100, origin.Y + 80, mod.TileType("ObservatoryChandelier"));
            CUtils.ObectPlace(origin.X + 106, origin.Y + 80, mod.TileType("ObservatoryChandelier"));
            CUtils.ObectPlace(origin.X + 112, origin.Y + 80, mod.TileType("ObservatoryChandelier"));
            CUtils.ObectPlace(origin.X + 118, origin.Y + 80, mod.TileType("ObservatoryChandelier"));
            CUtils.ObectPlace(origin.X + 181, origin.Y + 80, mod.TileType("ObservatoryChandelier"));
            CUtils.ObectPlace(origin.X + 187, origin.Y + 80, mod.TileType("ObservatoryChandelier"));
            CUtils.ObectPlace(origin.X + 193, origin.Y + 80, mod.TileType("ObservatoryChandelier"));
            CUtils.ObectPlace(origin.X + 199, origin.Y + 80, mod.TileType("ObservatoryChandelier"));
            CUtils.ObectPlace(origin.X + 40, origin.Y + 83, mod.TileType("ObservatoryChandelier"));
            CUtils.ObectPlace(origin.X + 45, origin.Y + 83, mod.TileType("ObservatoryChandelier"));
            CUtils.ObectPlace(origin.X + 254, origin.Y + 83, mod.TileType("ObservatoryChandelier"));
            CUtils.ObectPlace(origin.X + 259, origin.Y + 83, mod.TileType("ObservatoryChandelier"));
            CUtils.ObectPlace(origin.X + 6, origin.Y + 93, mod.TileType("ObservatoryChandelier"));
            CUtils.ObectPlace(origin.X + 293, origin.Y + 93, mod.TileType("ObservatoryChandelier"));
            CUtils.ObectPlace(origin.X + 103, origin.Y + 90, mod.TileType("ObservatoryChandelier"));
            CUtils.ObectPlace(origin.X + 196, origin.Y + 90, mod.TileType("ObservatoryChandelier"));
            CUtils.ObectPlace(origin.X + 95, origin.Y + 91, mod.TileType("ObservatoryChandelier"));
            CUtils.ObectPlace(origin.X + 203, origin.Y + 91, mod.TileType("ObservatoryChandelier"));
            CUtils.ObectPlace(origin.X + 49, origin.Y + 99, mod.TileType("ObservatoryChandelier"));
            CUtils.ObectPlace(origin.X + 250, origin.Y + 99, mod.TileType("ObservatoryChandelier"));
            CUtils.ObectPlace(origin.X + 63, origin.Y + 102, mod.TileType("ObservatoryChandelier"));
            CUtils.ObectPlace(origin.X + 73, origin.Y + 102, mod.TileType("ObservatoryChandelier"));
            CUtils.ObectPlace(origin.X + 225, origin.Y + 102, mod.TileType("ObservatoryChandelier"));
            CUtils.ObectPlace(origin.X + 236, origin.Y + 102, mod.TileType("ObservatoryChandelier"));
            CUtils.ObectPlace(origin.X + 15, origin.Y + 103, mod.TileType("ObservatoryChandelier"));
            CUtils.ObectPlace(origin.X + 20, origin.Y + 103, mod.TileType("ObservatoryChandelier"));
            CUtils.ObectPlace(origin.X + 278, origin.Y + 103, mod.TileType("ObservatoryChandelier"));
            CUtils.ObectPlace(origin.X + 284, origin.Y + 103, mod.TileType("ObservatoryChandelier"));
            CUtils.ObectPlace(origin.X + 29, origin.Y + 107, mod.TileType("ObservatoryChandelier"));
            CUtils.ObectPlace(origin.X + 270, origin.Y + 107, mod.TileType("ObservatoryChandelier"));
            CUtils.ObectPlace(origin.X + 44, origin.Y + 111, mod.TileType("ObservatoryChandelier"));
            CUtils.ObectPlace(origin.X + 50, origin.Y + 111, mod.TileType("ObservatoryChandelier"));
            CUtils.ObectPlace(origin.X + 133, origin.Y + 111, mod.TileType("ObservatoryChandelier"));
            CUtils.ObectPlace(origin.X + 166, origin.Y + 111, mod.TileType("ObservatoryChandelier"));
            CUtils.ObectPlace(origin.X + 249, origin.Y + 111, mod.TileType("ObservatoryChandelier"));
            CUtils.ObectPlace(origin.X + 254, origin.Y + 111, mod.TileType("ObservatoryChandelier"));
            CUtils.ObectPlace(origin.X + 16, origin.Y + 112, mod.TileType("ObservatoryChandelier"));
            CUtils.ObectPlace(origin.X + 283, origin.Y + 112, mod.TileType("ObservatoryChandelier"));
            CUtils.ObectPlace(origin.X + 128, origin.Y + 121, mod.TileType("ObservatoryChandelier"));
            CUtils.ObectPlace(origin.X + 171, origin.Y + 121, mod.TileType("ObservatoryChandelier"));
            #endregion

            #endregion

            return true;
        }
    }

    public class AbyssBiome : MicroBiome
    {
        public override bool Place(Point origin, StructureMap structures)
        {
            ushort tileGrass = (ushort)ModContent.TileType<AbyssGrass>(), tileStone = (ushort)ModContent.TileType<AbyssStone>(), tileIce = (ushort)ModContent.TileType<Abice>(),
            tileSand = (ushort)ModContent.TileType<AbyssSand>(), tileSandHardened = (ushort)ModContent.TileType<HardenedAbyssSand>(), tileSandstone = (ushort)ModContent.TileType<AbyssSandstone>(),
            tileMoss = (ushort)ModContent.TileType<AbyssMoss>();

            byte StoneWall = (byte)ModContent.WallType<AbyssStoneWall>(), SandstoneWall = (byte)ModContent.WallType<AbyssSandstoneWall>(), HardenedSandWall = (byte)ModContent.WallType<HardenedAbyssSandWall>(),
            GrassWall = (byte)ModContent.WallType<AbyssLeafWall>();

            int biomeRadius  = 100;

            Point newOrigin = new Point(origin.X - biomeRadius, origin.Y - 10);

            WorldUtils.Gen(newOrigin, new Shapes.Circle(biomeRadius), Actions.Chain(new GenAction[] //grass...
			{
                new InWorld(),
                new Modifiers.OnlyTiles(new ushort[]{ TileID.Grass, TileID.CorruptGrass, TileID.FleshGrass, TileID.HallowedGrass }),
                new Modifiers.RadialDither(biomeRadius - 5, biomeRadius),
                new SetModTile(tileGrass, true, true)
            }));
            WorldUtils.Gen(newOrigin, new Shapes.Circle(biomeRadius), Actions.Chain(new GenAction[] //moss...
			{
                new InWorld(),
                new Modifiers.OnlyTiles(new ushort[]{ TileID.BlueMoss, TileID.BrownMoss, TileID.GreenMoss, TileID.LavaMoss, TileID.LongMoss, TileID.PurpleMoss, TileID.RedMoss }),
                new Modifiers.RadialDither(biomeRadius - 5, biomeRadius),
                new SetModTile(tileMoss, true, true)
            }));
            WorldUtils.Gen(newOrigin, new Shapes.Circle(biomeRadius), Actions.Chain(new GenAction[] //stone...
			{
                new InWorld(),
                new Modifiers.OnlyTiles(new ushort[]{ TileID.Stone, TileID.Ebonstone, TileID.Crimstone, TileID.Pearlstone }),
                new Modifiers.RadialDither(biomeRadius - 5, biomeRadius),
                new SetModTile(tileStone, true, true)
            }));
            WorldUtils.Gen(newOrigin, new Shapes.Circle(biomeRadius), Actions.Chain(new GenAction[] //ice...
			{
                new InWorld(),
                new Modifiers.OnlyTiles(new ushort[]{ TileID.IceBlock, TileID.CorruptIce, TileID.FleshIce }),
                new Modifiers.RadialDither(biomeRadius - 5, biomeRadius),
                new SetModTile(tileIce, true, true)
            }));
            WorldUtils.Gen(newOrigin, new Shapes.Circle(biomeRadius), Actions.Chain(new GenAction[] //sand...
			{
                new InWorld(),
                new Modifiers.OnlyTiles(new ushort[]{ TileID.Sand, TileID.Ebonsand, TileID.Crimsand }),
                new Modifiers.RadialDither(biomeRadius - 5, biomeRadius),
                new SetModTile(tileSand, true, true)
            }));
            WorldUtils.Gen(newOrigin, new Shapes.Circle(biomeRadius), Actions.Chain(new GenAction[] //hardened sand...
			{
                new InWorld(),
                new Modifiers.OnlyTiles(new ushort[]{ TileID.HardenedSand, TileID.CorruptHardenedSand, TileID.CrimsonHardenedSand }),
                new Modifiers.RadialDither(biomeRadius - 5, biomeRadius),
                new SetModTile(tileSandHardened, true, true)
            }));
            WorldUtils.Gen(newOrigin, new Shapes.Circle(biomeRadius), Actions.Chain(new GenAction[] //...and sandstone.
			{
                new InWorld(),
                new Modifiers.OnlyTiles(new ushort[]{ TileID.Sandstone, TileID.CorruptSandstone, TileID.CrimsonSandstone }),
                new Modifiers.RadialDither(biomeRadius - 5, biomeRadius),
                new SetModTile(tileSandstone, true, true)
            }));
            WorldUtils.Gen(newOrigin, new Shapes.Circle(biomeRadius), Actions.Chain(new GenAction[] //Walls
			{
                new InWorld(),
                new Modifiers.OnlyWalls(new byte[]{ WallID.Stone, WallID.EbonstoneUnsafe, WallID.CrimstoneUnsafe }),
                new Modifiers.RadialDither(biomeRadius - 5, biomeRadius),
                new PlaceModWall(StoneWall, true)
            }));
            WorldUtils.Gen(newOrigin, new Shapes.Circle(biomeRadius), Actions.Chain(new GenAction[] //Walls
			{
                new InWorld(),
                new Modifiers.OnlyWalls(new byte[]{ WallID.Sandstone, WallID.CorruptSandstone, WallID.CrimsonSandstone }),
                new Modifiers.RadialDither(biomeRadius - 5, biomeRadius),
                new PlaceModWall(SandstoneWall, true)
            }));
            WorldUtils.Gen(newOrigin, new Shapes.Circle(biomeRadius), Actions.Chain(new GenAction[] //Walls
			{
                new InWorld(),
                new Modifiers.OnlyWalls(new byte[]{ WallID.HardenedSand, WallID.CorruptHardenedSand, WallID.CrimsonHardenedSand }),
                new Modifiers.RadialDither(biomeRadius - 5, biomeRadius),
                new PlaceModWall(HardenedSandWall, true)
            }));
            WorldUtils.Gen(newOrigin, new Shapes.Circle(biomeRadius), Actions.Chain(new GenAction[] //Walls
			{
                new InWorld(),
                new Modifiers.OnlyWalls(new byte[]{ WallID.HardenedSand, WallID.CorruptHardenedSand, WallID.CrimsonHardenedSand }),
                new Modifiers.RadialDither(biomeRadius - 5, biomeRadius),
                new PlaceModWall(HardenedSandWall, true)
            }));
            WorldUtils.Gen(newOrigin, new Shapes.Circle(biomeRadius), Actions.Chain(new GenAction[] //Walls
			{
                new InWorld(),
                new Modifiers.OnlyWalls(new byte[]{ WallID.GrassUnsafe, WallID.CorruptGrassUnsafe, WallID.CrimsonGrassUnsafe }),
                new Modifiers.RadialDither(biomeRadius - 5, biomeRadius),
                new PlaceModWall(GrassWall, true)
            }));

            return true;
        }

        public static int GetWorldSize()
        {
            if (Main.maxTilesX == 4200) { return 1; }
            else if (Main.maxTilesX == 6300) { return 2; }
            else if (Main.maxTilesX == 8400) { return 3; }
            return 1; //unknown size, assume small
        }
    }

    public class InWorld : GenAction
    {
        public InWorld()
        {
        }

        public override bool Apply(Point origin, int x, int y, params object[] args)
        {
            if (x < 0 || x > Main.maxTilesX || y < 0 || y > Main.maxTilesY)
                return Fail();
            return UnitApply(origin, x, y, args);
        }
    }
}