using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.IO;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

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
            music = mod.GetSoundSlot(SoundType.Music, "Sounds/Music/Enigma");
            bossBag = mod.ItemType("EnigmaBag");
        }

        bool Unhooded = false;

        public override void ScaleExpertStats(int numPlayers, float bossLifeScale)
        {
            npc.lifeMax = (int)(npc.lifeMax * 0.5f * bossLifeScale);
            npc.damage = (int)(npc.damage * 0.5f);
        }

        public override void HitEffect(int hitDirection, double damage)
        {
            if (npc.life < npc.lifeMax / 2 && !Unhooded)
            {
                Unhooded = true;
                Gore.NewGore(npc.position, npc.velocity * 0.2f, mod.GetGoreSlot("Gores/EnigmaHood"), 1f);
                Preamble[0] = 2;
            }
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
                music = mod.GetSoundSlot(SoundType.Music, "Sounds/Music/Enigma");
                npc.dontTakeDamage = false;
            }

            if (!npc.HasPlayerTarget)
            {
                npc.TargetClosest();
            }

            Player player = Main.player[npc.target];

            float ChangeRate = Main.expertMode ? 180 : 240;

            float Movespeed = .2f;
            float VelMax = 6;

            if (npc.life < npc.lifeMax / 2)
            {
                music = mod.GetSoundSlot(SoundType.Music, "Sounds/Music/EnigmaU");
                Movespeed = .25f;
                VelMax = 8;
                ChangeRate = Main.expertMode ? 120 : 180;
                npc.damage = 48;
            }

            npc.ai[1]++;

            BaseAI.AISpaceOctopus(npc, ref EAI, Movespeed, VelMax, 400); 

            switch ((int)npc.ai[0])
            {
                case 0:
                    if (npc.ai[1] > ChangeRate * 1.5f)
                    {
                        AIChange();
                    }
                    break;
                case 1:

                    if (npc.ai[1] % (ChangeRate / 6) == 0)
                    {
                        int a = Projectile.NewProjectile(npc.position, new Vector2(-10, Main.rand.Next(-20, 10)), ModContent.ProjectileType<EngimaBurst>(), npc.damage / 4, 4, Main.myPlayer);
                        Main.projectile[a].Center = npc.Center + new Vector2(30, 0);
                        int b = Projectile.NewProjectile(npc.position, new Vector2(10, Main.rand.Next(-20, 10)), ModContent.ProjectileType<EngimaBurst>(), npc.damage / 4, 4, Main.myPlayer);
                        Main.projectile[b].Center = npc.Center - new Vector2(30, 0);
                    }

                    if (npc.ai[1] > ChangeRate + 60)
                    {
                        AIReset();
                    }
                    break;
                case 2:

                    if (npc.ai[1] % (ChangeRate / 4) == 0)
                    {
                        int a = Projectile.NewProjectile(npc.position, new Vector2(-10, Main.rand.Next(-20, 10)), ModContent.ProjectileType<EngimaBurst>(), npc.damage / 4, 4, Main.myPlayer);
                        Main.projectile[a].Center = npc.Center + new Vector2(30, 0);
                        int b = Projectile.NewProjectile(npc.position, new Vector2(10, Main.rand.Next(-20, 10)), ModContent.ProjectileType<EngimaBurst>(), npc.damage / 4, 4, Main.myPlayer);
                        Main.projectile[b].Center = npc.Center - new Vector2(30, 0);
                    }

                    if (npc.ai[1] > ChangeRate)
                    {
                        AIReset();
                    }
                    break;
                case 3:

                    for (int num468 = 0; num468 < 3; num468++)
                    {
                        int num469 = Dust.NewDust(npc.Center, 0, 0, DustID.Electric, 0, 0, 100, default, 1f);
                        Main.dust[num469].noGravity = true;
                    }

                    if (npc.ai[1] > ChangeRate)
                    {
                        Projectile.NewProjectile(npc.Center, new Vector2(0, -10), mod.ProjectileType("EnigmaBeam"), npc.damage / 4, 0f, Main.myPlayer, 0, npc.whoAmI);
                        npc.ai[0] = 4;
                        npc.ai[1] = 0;
                        npc.ai[2] = 0;
                        npc.ai[3] = 0;
                    }
                    break;
                case 4:

                    if (npc.ai[1] > ChangeRate + 60)
                    {
                        AIReset();
                    }
                    break;

                case 5:

                    if (npc.ai[1] > ChangeRate)
                    {
                        npc.ai[0] = 6;
                        npc.ai[1] = 0;
                        npc.ai[2] = 0;
                        npc.ai[3] = 0;
                    }

                    break;
                case 6:

                    Vector2 vector2 = new Vector2(npc.position.X + (npc.width * 0.5f), npc.position.Y + (npc.height * 0.5f));
                    float num1 = Main.player[npc.target].position.X + (player.width / 2) - vector2.X;
                    float num2 = Main.player[npc.target].position.Y + (player.height / 2) - vector2.Y;
                    float NewRotation = (float)Math.Atan2(num2, num1);
                    handRot = MathHelper.Lerp(npc.rotation, NewRotation, 1f / 30f);

                    int[] array4 = new int[5];
                    Vector2[] array5 = new Vector2[5];
                    int num838 = 0;
                    float num839 = 2000f;
                    for (int num840 = 0; num840 < 255; num840++)
                    {
                        if (Main.player[num840].active && !Main.player[num840].dead)
                        {
                            Vector2 center9 = Main.player[num840].Center;
                            float num841 = Vector2.Distance(center9, npc.Center);
                            if (num841 < num839 && Collision.CanHit(npc.Center, 1, 1, center9, 1, 1))
                            {
                                array4[num838] = num840;
                                array5[num838] = center9;
                                if (++num838 >= array5.Length)
                                {
                                    break;
                                }
                            }
                        }
                    }
                    for (int num842 = 0; num842 < num838; num842++)
                    {
                        Vector2 vector82 = array5[num842] - npc.Center;
                        float ai = Main.rand.Next(100);
                        for (int i = 0; i < 3; i++)
                        {
                            Vector2 vector83 = Vector2.Normalize(vector82.RotatedByRandom(0.78539818525314331)) * 20f;
                            Projectile.NewProjectile(npc.Center.X, npc.Center.Y, vector83.X, vector83.Y, ModContent.ProjectileType<Enigmashock>(), npc.damage / 2, 0f, Main.myPlayer, vector82.ToRotation(), ai);
                        }
                    }

                    if (npc.ai[1] > ChangeRate + 60)
                    {
                        AIReset();
                    }

                    break;

                case 7:

                    if (npc.ai[1] == 45)
                    {
                        int a = Projectile.NewProjectile(npc.position, new Vector2(-10, Main.rand.Next(-20, 10)), ModContent.ProjectileType<EnigmaVortex>(), npc.damage / 4, 4, Main.myPlayer);
                        Main.projectile[a].Center = npc.Center + new Vector2(200, 0);
                        int b = Projectile.NewProjectile(npc.position, new Vector2(10, Main.rand.Next(-20, 10)), ModContent.ProjectileType<EnigmaVortex>(), npc.damage / 4, 4, Main.myPlayer);
                        Main.projectile[b].Center = npc.Center - new Vector2(200, 0);
                    }

                    if (npc.ai[1] > 90)
                    {
                        AIReset();
                    }

                    break;
                default:
                    npc.ai[0] = 0;
                    goto case 0;
            }
        }

        public void AIChange()
        {
            if (Main.netMode != 1)
            {
                npc.ai[0] = AIType();
                npc.ai[1] = 0;
                npc.ai[2] = 0;
                npc.ai[3] = 0;
                npc.netUpdate = true;
            }
        }

        public void AIReset()
        {
            if (Main.netMode != 1)
            {
                npc.ai[0] = 0;
                npc.ai[1] = 0;
                npc.ai[2] = 0;
                npc.ai[3] = 0;
                npc.netUpdate = true;
            }
        }

        public int AIType()
        {
            int aitype = Main.rand.Next(Unhooded ? 5 : 4);

            switch (aitype)
            {
                case 0:
                    return 1;
                case 1:
                    return 2;
                case 2:
                    return 3;
                case 3:
                    return 7;
                default:
                    return 5;
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

            CWorld.downedEnigma = true;

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
        int ChargeFrame = 0;
        float handRot = 0;

        Texture2D body;
        Texture2D hand;
        Texture2D glow;

        public override bool PreDraw(SpriteBatch spriteBatch, Color drawColor)
        {
            Textures();
            Texture2D RingTex = mod.GetTexture("NPCs/Bosses/Enigma/EnigmaCircle");
            Texture2D ChargeTex = mod.GetTexture("NPCs/Bosses/Enigma/EnigmaCharge");


            Rectangle handsframe = BaseDrawing.GetFrame(HandFrame, hand.Width, hand.Height, 0, 0);
            Rectangle charge = BaseDrawing.GetFrame(ChargeFrame, hand.Width, hand.Height, 0, 0);
            if (scale > 0)
            {
                RingEffects();
                BaseDrawing.DrawTexture(spriteBatch, RingTex, 0, npc.position, npc.width, npc.height, scale, rotation, 0, 1, new Rectangle(0, 0, RingTex.Width, RingTex.Height), drawColor, true);
            }
            if (npc.ai[0] == 5)
            {
                BaseDrawing.DrawTexture(spriteBatch, hand, 0, npc.position, npc.width, npc.height, npc.scale, npc.rotation, 0, 4, handsframe, drawColor, true);
            }
            BaseDrawing.DrawTexture(spriteBatch, body, 0, npc.position, npc.width, npc.height, npc.scale, 0, 0, 6, npc.frame, drawColor, true);
            BaseDrawing.DrawTexture(spriteBatch, glow, 0, npc.position, npc.width, npc.height, npc.scale, 0, 0, 6, npc.frame, drawColor, true);
            BaseDrawing.DrawTexture(spriteBatch, ChargeTex, 0, npc.position, npc.width, npc.height, npc.scale, 0, 0, 4, charge, Color.White, true);
            BaseDrawing.DrawTexture(spriteBatch, hand, 0, npc.position, npc.width, npc.height, npc.scale, handRot, 0, 4, handsframe, drawColor, true);

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
                case 7:
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
            if (Preamble[0] == 2)
            {
                hand = mod.GetTexture("NPCs/Bosses/Enigma/EnigmaHandsCover");
            }
        }

        public override void FindFrame(int frameHeight)
        {
            if (npc.frameCounter++ > 10)
            {
                npc.frameCounter = 0;
                npc.frame.Y += frameHeight;
                ChargeFrame += 1;
                if (npc.frame.Y > frameHeight * 5)
                {
                    npc.frame.Y = 0;
                }
                if (ChargeFrame > 3)
                {
                    ChargeFrame = 0;
                }
            }

            int fpt = (int)npc.ai[0] == 0 ? 10 : 3;

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

        public float[] Preamble = new float[2];
        public float[] EAI = new float[1];

        public override void SendExtraAI(BinaryWriter writer)
        {
            base.SendExtraAI(writer);
            if (Main.netMode == NetmodeID.Server || Main.dedServ)
            {
                writer.Write(Preamble[0]);
                writer.Write(Preamble[1]);
                writer.Write(EAI[0]);
            }
        }

        public override void ReceiveExtraAI(BinaryReader reader)
        {
            base.ReceiveExtraAI(reader);
            if (Main.netMode == NetmodeID.MultiplayerClient)
            {
                Preamble[0] = reader.ReadFloat();
                Preamble[1] = reader.ReadFloat();
                EAI[0] = reader.ReadFloat();
            }
        }

        public void Prefight()
        {
            npc.ai[0] = 0;
            if (Main.netMode != 1)
            {
                Preamble[1]++;
                music = mod.GetSoundSlot(SoundType.Music, "Sounds/Music/silence");
                if (Preamble[0] == 0)
                {
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
                else if (Preamble[0] == 2)
                {
                    npc.velocity *= .96f;
                    if (Preamble[1] == 90)
                    {
                        if (Main.netMode != 1) BaseUtility.Chat("..!!!", Color.Cyan);
                    }

                    if (Preamble[1] == 180)
                    {
                        if (Main.netMode != 1) BaseUtility.Chat("MY HOOD!!!", Color.Cyan);
                    }

                    if (Preamble[1] == 270)
                    {
                        if (Main.netMode != 1) BaseUtility.Chat("MY SUPREME BRAIN IS EXPOSED!", Color.Cyan);
                    }

                    if (Preamble[1] >= 360)
                    {
                        if (Main.netMode != 1) BaseUtility.Chat("HOW DARE YOU! TASTE ELECTRICITY YOU INSIGNIFICANT IMBECILE!", Color.Cyan);
                        Preamble[0] = 1;

                        npc.netUpdate = true;
                    }
                }
            }
        }

        #endregion
    }
}