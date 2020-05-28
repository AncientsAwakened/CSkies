using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ObjectData;
using CSkies.NPCs.Bosses.Heartcore;

namespace CSkies.Tiles.HeartAltars
{
    public class HeartAltar3 : ModTile
    {
        public override void SetDefaults()
        {
            Main.tileSolidTop[Type] = false;
            Main.tileFrameImportant[Type] = true;
            Main.tileNoAttach[Type] = true;
            dustType = DustID.t_Meteor;
            Main.tileLavaDeath[Type] = false;
            TileObjectData.newTile.CopyFrom(TileObjectData.Style3x3);
            TileObjectData.newTile.CoordinateHeights = new int[] { 16, 16, 16 };
            TileObjectData.newTile.CoordinateWidth = 16;
            TileObjectData.newTile.CoordinatePadding = 2;
            TileObjectData.addTile(Type);
            ModTranslation name = CreateMapEntryName();
            name.SetDefault("Heart Altar");
            AddMapEntry(new Color(199, 74, 161), name);
            disableSmartCursor = true;
            animationFrameHeight = 54;
        }

        public override void ModifyLight(int x, int y, ref float r, ref float g, ref float b)
        {
            Color color = BaseUtility.ColorMult(Heartcore.Flame, 0.7f);
            r = color.R / 255f; g = color.G / 255f; b = color.B / 255f;
        }

        public override void AnimateTile(ref int frame, ref int frameCounter)
        {
            if (++frameCounter >= 10)
            {
                frameCounter = 0;
                if (++frame >= 4) frame = 0;
            }
        }

        public override bool NewRightClick(int i, int j)
        {
            if (NPC.AnyNPCs(mod.NPCType("MagmaHeart")))
            {
                return false;
            }
            if (CWorld.Altar3)
            {
                Main.NewText("The Altar has lost its glow...but is slowly gaining it back. Maybe it will have returned by morning?");
            }
            Player player = Main.LocalPlayer;
            int type = ModContent.ItemType<Items.Boss.Void.VoidFragment>();
            if (BasePlayer.HasItem(player, type, 1))
            {
                for (int m = 0; m < 50; m++)
                {
                    Item item = player.inventory[m];
                    if (item != null && item.type == type && item.stack >= 1)
                    {
                        item.stack--;
                        NPC.NewNPC(i * 16, (j * 16) - 72, mod.NPCType("MagmaHeart"));
                    }
                }
                CWorld.Altar3 = true;
                return true;
            }
            else
            {
                return false;
            }
        }

        public override void MouseOver(int i, int j)
        {
            Player player = Main.LocalPlayer;

            player.showItemIcon2 = ModContent.ItemType<Items.Boss.Void.VoidFragment>();

            player.showItemIconText = "";
            player.noThrow = 2;
            player.showItemIcon = true;
        }

        public override void MouseOverFar(int i, int j)
        {
            MouseOver(i, j);
            Player player = Main.LocalPlayer;
            if (player.showItemIconText == "")
            {
                player.showItemIcon = false;
                player.showItemIcon2 = 0;
            }
        }

        public override bool CanKillTile(int i, int j, ref bool blockDamaged)
        {
            return false;
        }

        public override bool CanExplode(int i, int j)
        {
            return false;
        }

        public Color GetColor(Color color)
        {
            Color glowColor = Colors.COLOR_GLOWPULSE;
            return glowColor;
        }

        public Color HeartColor(Color color)
        {
            Color glowColor = Color.White;
            return glowColor;
        }

        public override void PostDraw(int x, int y, SpriteBatch sb)
        {
            Tile tile = Main.tile[x, y];
            Texture2D glowTex = mod.GetTexture("Glowmasks/HeartAltar_Glow");
            Texture2D heart = mod.GetTexture("Glowmasks/HeartAltar_Heart");
            int frameX = tile != null && tile.active() ? tile.frameX : 0;
            int frameY = tile != null && tile.active() ? tile.frameY + (Main.tileFrame[Type] * 54) : 0;
            BaseDrawing.DrawTileTexture(sb, glowTex, x, y, 16, 16, frameX, frameY, false, false, false, null, GetColor);
            if (!CWorld.Altar3)
            {
                BaseDrawing.DrawTileTexture(sb, heart, x, y, 16, 16, frameX, frameY, false, false, false, null, HeartColor);
            }
        }
    }
}