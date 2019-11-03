using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.IO;
using Terraria;
using Terraria.Audio;
using Terraria.Graphics.Shaders;
using Terraria.ID;
using Terraria.ModLoader;
using CSkies.NPCs.Bosses.FurySoul;

namespace CSkies.NPCs.Bosses.Enigma
{
    [AutoloadBossHead]
    public class Enigma : ModNPC
    {
        public override void SetDefaults()
        {
            npc.npcSlots = 100;
            npc.width = 62;
            npc.height = 50;
            npc.aiStyle = -1;
            npc.damage = 40;
            npc.defense = 30;
            npc.lifeMax = 40000;
            npc.value = Item.sellPrice(0, 12, 0, 0);
            npc.HitSound = SoundID.NPCHit1;
            npc.DeathSound = SoundID.NPCDeath1;
            npc.knockBackResist = 0f;
            npc.noGravity = true;
            npc.netAlways = true;
            npc.boss = true;
            npc.noTileCollide = true;
            music = mod.GetSoundSlot(Terraria.ModLoader.SoundType.Music, "Sounds/Music/Enigma");
            bossBag = mod.ItemType("EnigmaBag");
        }

        public float[] Preamble = new float[2];

        public override void SendExtraAI(BinaryWriter writer)
        {
            base.SendExtraAI(writer);
            if (Main.netMode == NetmodeID.Server || Main.dedServ)
            {
                writer.Write(Preamble[0]);
                writer.Write(Preamble[1]);
            }
        }

        public override void ReceiveExtraAI(BinaryReader reader)
        {
            base.ReceiveExtraAI(reader);
            if (Main.netMode == NetmodeID.MultiplayerClient)
            {
                Preamble[0] = reader.ReadFloat();
                Preamble[1] = reader.ReadFloat();
            }
        }

        public override void ScaleExpertStats(int numPlayers, float bossLifeScale)
        {
            npc.lifeMax = (int)(npc.lifeMax * 0.5f * bossLifeScale);
            npc.damage = (int)(npc.damage * 0.5f);
        }

        public override void AI()
        {
            if (Preamble[0] != 1)
            {
                npc.dontTakeDamage = true;
                Prefight();
                return;
            }
            else
            {
                npc.dontTakeDamage = false;
            }

            npc.ai[1]++;

            switch ((int)npc.ai[0])
            {
                case 0:

                    break;

                default:
                    npc.ai[0] = 0;
                    goto case 0;
            }

        }

        public override void NPCLoot()
        {
            for (int num468 = 0; num468 < 12; num468++)
            {
                int num469 = Dust.NewDust(new Vector2(npc.position.X, npc.position.Y), npc.width, npc.height, DustID.Electric, -npc.velocity.X * 0.2f,
                    -npc.velocity.Y * 0.2f, 100, default, 2f);
                Main.dust[num469].noGravity = true;
            }

            if (Main.rand.Next(10) == 0)
            {
                npc.DropLoot(mod.ItemType("EnigmaTrophy"));
            }
            if (Main.rand.Next(7) == 0)
            {
                npc.DropLoot(mod.ItemType("EnigmaMask"));
            }

            if (Main.expertMode)
            {
                npc.DropBossBags();
            }
            else
            {
                string[] lootTableA = { "" };
                int lootA = Main.rand.Next(lootTableA.Length);

                npc.DropLoot(mod.ItemType(lootTableA[lootA]));

                CWorld.downedHeartcore = true;
            }
        }

        #region Visuals

        int handCounter = 0;
        int HandFrame = 0;

        Texture2D body;
        Texture2D hand;
        Texture2D glow;

        public override bool PreDraw(SpriteBatch spriteBatch, Color drawColor)
        {
            Textures(); 
            Texture2D RingTex = mod.GetTexture("NPCs/Bosses/Heartcore/Ring1");
            Rectangle handsframe = BaseDrawing.GetFrame(HandFrame, hand.Width, hand.Height, 0, 0);
            if (scale > 0)
            {
                RingEffects();
                BaseDrawing.DrawTexture(spriteBatch, RingTex, 0, npc.position, npc.width, npc.height, scale, rotation, 0, 1, new Rectangle(0, 0, RingTex.Width, RingTex.Height), drawColor, true);
            }
            BaseDrawing.DrawTexture(spriteBatch, body, 0, npc.position, npc.width, npc.height, npc.scale, npc.rotation, 0, 6, npc.frame, drawColor, true);
            BaseDrawing.DrawTexture(spriteBatch, glow, 0, npc.position, npc.width, npc.height, npc.scale, npc.rotation, 0, 6, npc.frame, drawColor, true);
            BaseDrawing.DrawTexture(spriteBatch, hand, 0, npc.position, npc.width, npc.height, npc.scale, npc.rotation, 0, 4, handsframe, drawColor, true);
            return false;
        }

        public void Textures()
        {
            body = Main.npcTexture[npc.type];
            glow = mod.GetTexture("Glowmasks/Enigma_Glow");
            if (npc.life < npc.life / 2)
            {
                body = mod.GetTexture("NPCs/Bosses/Enigma/EnigmaUnhooded");
                glow = mod.GetTexture("Glowmasks/EnigmaUnhooded_Glow");
            }

            switch ((int)npc.ai[0])
            {
                case 0:
                    hand = mod.GetTexture("NPCs/Bosses/Enigma/EnigmaHands");
                    break;
                case 1:
                    hand = mod.GetTexture("NPCs/Bosses/Enigma/EnigmaHandsBlast");
                    break;
                case 2:
                    hand = mod.GetTexture("NPCs/Bosses/Enigma/EnigmaHandsCast");
                    break;
                case 3:
                    hand = mod.GetTexture("NPCs/Bosses/Enigma/EnigmaHandsPrep");
                    break;
                case 4:
                    hand = mod.GetTexture("NPCs/Bosses/Enigma/EnigmaHandsLaser");
                    break;
                case 5:
                    hand = mod.GetTexture("NPCs/Bosses/Enigma/EnigmaHandsCarge");
                    break;
                case 6:
                    hand = mod.GetTexture("NPCs/Bosses/Enigma/EnigmaHandsAim");
                    break;
            }
            if (Preamble[0] == 0)
            {
                hand = mod.GetTexture("NPCs/Bosses/Enigma/EnigmaHandsPrelude");
            }
        }

        public override void FindFrame(int frameHeight)
        {
            if (npc.frameCounter++ > 10)
            {
                npc.frameCounter = 0;
                npc.frame.Y += frameHeight;
                if (npc.frame.Y > frameHeight * 5)
                {
                    npc.frame.Y = 0;
                }
            }
            var fpt = ((int)npc.ai[0]) switch
            {
                0 => 10,
                _ => 3,
            };
            if (handCounter >= fpt)
            {
                handCounter = 0;
                HandFrame++;
                if (HandFrame > 3)
                {
                    HandFrame = 0;
                }
            }
        }

        float scale = 0;
        float rotation = 0;

        private void RingEffects()
        {
            rotation += .2f;
            if (npc.ai[0] == 5 || npc.ai[0] == 6)
            {
                if (scale >= 1f)
                {
                    scale = 1f;
                }
                else
                {
                    scale += .02f;
                }
            }
            else
            {
                if (scale > .1f)
                {
                    scale -= .02f;
                }
                else
                {
                    scale = 0;
                }
            }
        }

        public void Prefight()
        {
            if (Main.netMode != 1)
            {
                npc.velocity *= 0;
                if (Preamble[1]++ < 600)
                {
                    music = mod.GetSoundSlot(Terraria.ModLoader.SoundType.Music, "Sounds/Music/silence");
                    if (!CWorld.downedEnigma)
                    {
                        if (Preamble[1] == 90)
                        {
                            if (Main.netMode != 1) BaseUtility.Chat("Well...I've finally found you.", Color.Cyan);
                        }

                        if (Preamble[1] == 180)
                        {
                            if (Main.netMode != 1) BaseUtility.Chat("You have been quite the thorn in my side.", Color.Cyan);
                        }

                        if (Preamble[1] == 270)
                        {
                            if (Main.netMode != 1) BaseUtility.Chat("Especially after you trashed my beatuiful starcore...", Color.Cyan);
                        }

                        if (Preamble[1] == 360)
                        {
                            if (Main.netMode != 1) BaseUtility.Chat("You're too much of a liability for my plans, so...", Color.Cyan);
                        }

                        if (Preamble[1] >= 450)
                        {
                            if (Main.netMode != 1) BaseUtility.Chat("I'll squash you like the insect you are.", Color.Cyan);
                            Preamble[0] = 1;

                            npc.netUpdate = true;
                        }
                    }
                    else
                    {
                        if (Main.netMode != 1) BaseUtility.Chat("You again? Haven't you humiliated me enough as is?", Color.Cyan);
                        Preamble[0] = 1;

                        npc.netUpdate = true;
                    }
                }
            }
        }

        #endregion
    }
}