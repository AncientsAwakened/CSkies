using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.IO;
using Terraria;
using Terraria.Graphics.Effects;
using Terraria.ModLoader;
using CSkies.Backgrounds;
using Terraria.Graphics.Shaders;

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
                if (Main.netMode == 2) MNet.SendBaseNetMessage(0, owner, projID, friendly, hostile);
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
                if (Main.netMode == 2) BaseNet.SyncAI(classID, id, newAI, aitype);
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
    }
    enum MsgType : byte
    {
        ProjectileHostility,
        SyncAI
    }
}