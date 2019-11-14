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
                CUtils.ObectPlace(origin.X + 29, origin.Y + 31, mod.TileType("HeartAltar2"));
            }
            else if (CWorld.VaultCount == 1)
            {
                CUtils.ObectPlace(origin.X + 29, origin.Y + 31, mod.TileType("HeartAltar3"));
            }
            else if (CWorld.VaultCount == 2)
            {
                CUtils.ObectPlace(origin.X + 29, origin.Y + 31, mod.TileType("HeartAltar4"));
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
                [new Color(255, 255, 0)] = ModContent.TileType<StarBrickUnsafe>(),
                [new Color(0, 255, 255)] = ModContent.TileType<StarglassUnsafe>(),
                [new Color(128, 128, 128)] = ModContent.TileType<StarCircuitUnsafe>(),
                [new Color(0, 0, 255)] = ModContent.TileType<AbyssBricks>(),
                [new Color(0, 255, 0)] = ModContent.TileType<AbyssDoor>(),
                [new Color(255, 0, 0)] = ModContent.TileType<AbyssStone>(),
                [new Color(255, 0, 255)] = ModContent.TileType<AbyssGrass>(),
                [new Color(255, 255, 255)] = -2,
                [Color.Black] = -1
            };

            Dictionary<Color, int> colorToWall = new Dictionary<Color, int>
            {
                [new Color(0, 255, 0)] = ModContent.WallType<StarBrickWallUnsafe>(),
                [new Color(255, 0, 0)] = ModContent.WallType<StarglassWallUnsafe>(),
                [new Color(255, 0, 255)] = ModContent.WallType<StarCircuitWallUnsafe>(),
                [new Color(0, 0, 255)] = ModContent.WallType<AbyssWall>(),
                [Color.Black] = -1
            };

            TexGen gen = BaseWorldGenTex.GetTexGenerator(mod.GetTexture("Worldgen/Observatory"), colorToTile, mod.GetTexture("Worldgen/ObservatoryWalls"), colorToWall, null, mod.GetTexture("Worldgen/ObservatorySlopes"));

            gen.Generate(origin.X, origin.Y, true, true);

            CUtils.ObectPlace(origin.X + 277, origin.Y + 51, mod.TileType("HeartAltar1"));

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