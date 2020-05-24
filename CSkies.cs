using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.IO;
using Terraria;
using Terraria.Graphics.Effects;
using Terraria.ModLoader;
using CSkies.Backgrounds;
using Terraria.Graphics.Shaders;
using Terraria.ID;
using System.Collections.Generic;
using Terraria.UI;
using ReLogic.Graphics;

namespace CSkies
{
    public class CSkies : Mod
	{
        public CSkies instance;
        public static CSkies inst;

        public static ModHotKey AccessoryAbilityKey;

        public CSkies()
        {
            Properties = new ModProperties()
            {
                Autoload = true,
                AutoloadGores = true,
                AutoloadSounds = true,
                AutoloadBackgrounds = true
            };
            instance = this;
            inst = this;
        }

        #region setup
        public static void PremultiplyTexture(Texture2D texture)
        {
            Color[] buffer = new Color[texture.Width * texture.Height];
            texture.GetData(buffer);
            for (int i = 0; i < buffer.Length; i++)
            {
                buffer[i] = Color.FromNonPremultiplied(
                        buffer[i].R, buffer[i].G, buffer[i].B, buffer[i].A);
            }
            texture.SetData(buffer);
        }

        public override void Load()
        {
            instance = this;
            inst = this;
            AccessoryAbilityKey = RegisterHotKey("Celestial Accessory Ability", "V");
            WeakReferences.PerformModSupport();
            if (!Main.dedServ)
            {
                LoadClient();
            }
        }

        public void LoadClient()
        {
            PremultiplyTexture(GetTexture("NPCs/Bosses/Observer/Star"));
            PremultiplyTexture(GetTexture("NPCs/Bosses/Observer/StarProj"));
            PremultiplyTexture(GetTexture("NPCs/Bosses/ObserverVoid/Vortex"));
            PremultiplyTexture(GetTexture("NPCs/Bosses/ObserverVoid/Vortex1"));
            PremultiplyTexture(GetTexture("NPCs/Bosses/ObserverVoid/DarkVortex"));
            PremultiplyTexture(GetTexture("Backgrounds/VoidBolt"));
            PremultiplyTexture(GetTexture("Backgrounds/VoidFlash"));

            Ref<Effect> screenRef = new Ref<Effect>(GetEffect("Effects/Shockwave"));
            Filters.Scene["Shockwave"] = new Filter(new ScreenShaderData(screenRef, "Shockwave"), EffectPriority.VeryHigh);
            Filters.Scene["Shockwave"].Load();

            Filters.Scene["CSkies:AbyssSky"] = new Filter(new AbyssSkyData("FilterMiniTower").UseColor(.2f, .2f, .2f).UseOpacity(0.5f), EffectPriority.VeryHigh);
            SkyManager.Instance["CSkies:AbyssSky"] = new AbyssSky();
            AbyssSky.boltTexture = GetTexture("Backgrounds/VoidBolt");
            AbyssSky.flashTexture = GetTexture("Backgrounds/VoidFlash");

            SetupMusicBoxes();
        }

        public override void Unload()
        {
            instance = null;
            inst = null;
            AccessoryAbilityKey = null;
        }

        #endregion

        public override void UpdateMusic(ref int music, ref MusicPriority priority)
        {
            if (Main.gameMenu)
                return;

            if (priority > MusicPriority.Environment)
                return;

            Player player = Main.LocalPlayer;

            if (!player.active)
                return;

            CPlayer cPlayer = player.GetModPlayer<CPlayer>();

            if (cPlayer.ZoneVoid)
            {
                priority = MusicPriority.Event;
                music = GetSoundSlot(SoundType.Music, "Sounds/Music/Abyss");
            }

            if (cPlayer.ZoneComet)
            {
                priority = MusicPriority.Event;
                music = GetSoundSlot(SoundType.Music, "Sounds/Music/Comet");
            }

            if (cPlayer.ZoneObservatory)
            {
                priority = MusicPriority.Event;
                music = GetSoundSlot(SoundType.Music, "Sounds/Music/Observatory");
            }
        }

        public override void HandlePacket(BinaryReader bb, int whoAmI)
        {
            MsgType msg = (MsgType)bb.ReadByte();
            if (msg == MsgType.ProjectileHostility) //projectile hostility and ownership
            {
                int owner = bb.ReadInt32();
                int projID = bb.ReadInt32();
                bool friendly = bb.ReadBoolean();
                bool hostile = bb.ReadBoolean();
                if (Main.projectile[projID] != null)
                {
                    Main.projectile[projID].owner = owner;
                    Main.projectile[projID].friendly = friendly;
                    Main.projectile[projID].hostile = hostile;
                }
                if (Main.netMode == NetmodeID.Server) MNet.SendBaseNetMessage(0, owner, projID, friendly, hostile);
            }
            else
            if (msg == MsgType.SyncAI) //sync AI array
            {
                int classID = bb.ReadByte();
                int id = bb.ReadInt16();
                int aitype = bb.ReadByte();
                int arrayLength = bb.ReadByte();
                float[] newAI = new float[arrayLength];
                for (int m = 0; m < arrayLength; m++)
                {
                    newAI[m] = bb.ReadSingle();
                }
                if (classID == 0 && Main.npc[id] != null && Main.npc[id].active && Main.npc[id].modNPC != null && Main.npc[id].modNPC is ParentNPC)
                {
                    ((ParentNPC)Main.npc[id].modNPC).SetAI(newAI, aitype);
                }
                else
                if (classID == 1 && Main.projectile[id] != null && Main.projectile[id].active && Main.projectile[id].modProjectile != null && Main.projectile[id].modProjectile is ParentProjectile)
                {
                    ((ParentProjectile)Main.projectile[id].modProjectile).SetAI(newAI, aitype);
                }
                if (Main.netMode == NetmodeID.Server) BaseNet.SyncAI(classID, id, newAI, aitype);
            }
        }

        public void SetupMusicBoxes()
        {
            AddMusicBox(GetSoundSlot(SoundType.Music, "Sounds/Music/Comet"), ItemType("CometBox"), TileType("CometBox"));
            AddMusicBox(GetSoundSlot(SoundType.Music, "Sounds/Music/Observer"), ItemType("ObserverBox"), TileType("ObserverBox"));
            AddMusicBox(GetSoundSlot(SoundType.Music, "Sounds/Music/Starcore"), ItemType("StarcoreBox"), TileType("StarcoreBox"));
            AddMusicBox(GetSoundSlot(SoundType.Music, "Sounds/Music/Abyss"), ItemType("AbyssBox"), TileType("AbyssBox"));
            AddMusicBox(GetSoundSlot(SoundType.Music, "Sounds/Music/ObserverVoid"), ItemType("OVBox"), TileType("OVBox"));
            AddMusicBox(GetSoundSlot(SoundType.Music, "Sounds/Music/Void"), ItemType("VOIDBox"), TileType("VOIDBox"));
            AddMusicBox(GetSoundSlot(SoundType.Music, "Sounds/Music/Heart"), ItemType("MagmaHeartBox"), TileType("MagmaHeartBox"));
            AddMusicBox(GetSoundSlot(SoundType.Music, "Sounds/Music/Heartcore"), ItemType("HCBox"), TileType("HCBox"));
            AddMusicBox(GetSoundSlot(SoundType.Music, "Sounds/Music/FurySoul"), ItemType("FSBox"), TileType("FSBox"));
        }

        public override void ModifyInterfaceLayers(List<GameInterfaceLayer> layers)
        {
            Titles modPlayer = Main.player[Main.myPlayer].GetModPlayer<Titles>();
            if (modPlayer.text)
            {
                var textLayer = layers.FindIndex(layer => layer.Name.Equals("Vanilla: Inventory"));
                var computerState = new LegacyGameInterfaceLayer("AAMod: UI",
                    delegate
                    {
                        BossTitle(modPlayer.BossID);
                        return true;
                    },
                    InterfaceScaleType.UI);
                layers.Insert(textLayer, computerState);
            }
        }

        private void BossTitle(int BossID)
        {
            string BossName = "";
            string BossTitle = "";
            Color titleColor = Color.White;

            switch (BossID)
            {
                case 1:
                    BossName = "The Observer";
                    BossTitle = "All-Seeing Eye";
                    titleColor = Color.SkyBlue;
                    break;
                case 2:
                    BossName = "Starcore";
                    BossTitle = "Cosmic Construct";
                    titleColor = Color.LimeGreen;
                    break;
                case 3:
                    BossName = "Observer Void";
                    BossTitle = "Abyssal Gazer";
                    titleColor = new Color(75, 68, 124);
                    break;
                case 4:
                    BossName = "V O I D";
                    BossTitle = "All-Seeing Evil";
                    titleColor = new Color(143, 204, 204);
                    break;
                case 5:
                    BossName = "Heartcore";
                    BossTitle = "Sealed Fury";
                    titleColor = Color.HotPink;
                    break;
                case 6:
                    BossName = "Fury Soul";
                    BossTitle = "Hellish Wrath Incarnate";
                    titleColor = new Color(254, 121, 2);
                    break;
                case 7:
                    BossName = "Enigma";
                    BossTitle = "Mechanical Madman";
                    titleColor = Color.DarkBlue;
                    break;
                case 8:
                    BossName = "Enigma Prime";
                    BossTitle = "Supreme Galactic Genius";
                    titleColor = Color.LimeGreen;
                    break;
                case 9:
                    BossName = "Artemis Luminoth";
                    BossTitle = "Mechanical Masterpiece";
                    titleColor = Color.LimeGreen;
                    break;
            }

            Titles modPlayer2 = Main.player[Main.myPlayer].GetModPlayer<Titles>();
            float alpha = modPlayer2.alphaText;
            float alpha2 = modPlayer2.alphaText2;

            Vector2 textSize = Main.fontDeathText.MeasureString("~ " + BossName + " ~");
            Vector2 textSize2 = Main.fontDeathText.MeasureString(BossTitle) * .6f; ;
            float textPositionLeft = Main.screenWidth / 2 - textSize.X / 2;
            float text2PositionLeft = Main.screenWidth / 2 - textSize2.X / 2;

            Main.spriteBatch.DrawString(Main.fontDeathText, BossTitle, new Vector2(text2PositionLeft, (Main.screenHeight / 2) - 350), titleColor * ((255 - alpha2) / 255f), 0f, Vector2.Zero, .6f, SpriteEffects.None, 0f);
            Main.spriteBatch.DrawString(Main.fontDeathText, "~ " + BossName + " ~", new Vector2(textPositionLeft, Main.screenHeight / 2 - 300), titleColor * ((255 - alpha) / 255f), 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);

        }

        public static void ShowTitle(NPC npc, int ID)
        {
            if (CConfigClient.Instance.BossIntroText)
            {
                Projectile.NewProjectile(npc.Center, Vector2.Zero, ModContent.ProjectileType<Title>(), 0, 0, Main.myPlayer, ID, 0);
            }
        }

        public static void ShowTitle(Player player, int ID)
        {
            if (CConfigClient.Instance.BossIntroText)
            {
                Projectile.NewProjectile(player.Center, Vector2.Zero, ModContent.ProjectileType<Title>(), 0, 0, Main.myPlayer, ID, 0);
            }
        }
    }
    enum MsgType : byte
    {
        ProjectileHostility,
        SyncAI
    }
}