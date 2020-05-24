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
        public override void SetStaticDefaults()
        {
            Main.npcFrameCount[npc.type] = 6;
        }

        public override void SetDefaults()
        {
            npc.npcSlots = 100;
            npc.width = 62;
            npc.height = 50;
            npc.aiStyle = -1;
            npc.damage = 55;
            npc.defense = 40;
            npc.lifeMax = 48000;
            npc.value = Item.sellPrice(0, 5, 0, 0);
            npc.HitSound = SoundID.NPCHit1;
            npc.DeathSound = SoundID.NPCDeath1;
            npc.knockBackResist = 0f;
            npc.noGravity = true;
            npc.netAlways = true;
            npc.boss = true;
            npc.noTileCollide = true;
            music = mod.GetSoundSlot(SoundType.Music, "Sounds/Music/Enigma");
            bossBag = mod.ItemType("EnigmaBag");
            npc.alpha = 255;
        }

        bool Unhooded = false;

        public override void ScaleExpertStats(int numPlayers, float bossLifeScale)
        {
            npc.lifeMax = (int)(npc.lifeMax * 0.7f * bossLifeScale);
            npc.damage = (int)(npc.damage * 0.7f);
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

        public float ChangeRate = Main.expertMode ? 180 : 240;

        public const int Idle = 0, HomingMagic = 1, LightningStorm = 2, BeamPrep = 3, Beam = 4, Construct = 5, Vortexes = 6, Grenades = 7, ShockPrep = 8, Shock = 9, StaticPrep = 10, Static = 11, despawn = 12;
        
        bool title = false;

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

            if (!title)
            {
                CSkies.ShowTitle(npc, 7);
                title = true;
            }

            Player player = Main.player[npc.target];

            if (player.dead || !player.active || Vector2.Distance(player.Center, npc.Center) > 5000 || player.whoAmI == -1)
            {
                npc.TargetClosest();
                if (player.dead || !player.active || Vector2.Distance(player.Center, npc.Center) > 5000)
                {
                    npc.ai[0] = 12;
                    Despawn();
                    return;
                }
            }

            float Movespeed = .25f;
            float VelMax = 8;

            if (npc.life < npc.lifeMax / 2)
            {
                music = mod.GetSoundSlot(SoundType.Music, "Sounds/Music/EnigmaU");
                Movespeed = .3f;
                VelMax = 11;
                ChangeRate = Main.expertMode ? 120 : 180;
                npc.damage = 48;
            }

            npc.ai[1]++;

            BaseAI.AISpaceOctopus(npc, ref EAI, Movespeed, VelMax, 300); 

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
                        Main.PlaySound(mod.GetLegacySoundSlot(SoundType.Custom, "Sounds/Sounds/Zap"), npc.position);
                        int a = Projectile.NewProjectile(npc.position, new Vector2(-10, Main.rand.Next(0, 20)), ModContent.ProjectileType<EngimaBurst>(), npc.damage / 4, 4, Main.myPlayer);
                        Main.projectile[a].Center = npc.Center + new Vector2(30, 0);
                        int b = Projectile.NewProjectile(npc.position, new Vector2(10, Main.rand.Next(0, 20)), ModContent.ProjectileType<EngimaBurst>(), npc.damage / 4, 4, Main.myPlayer);
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
                        Main.PlaySound(mod.GetLegacySoundSlot(SoundType.Custom, "Sounds/Sounds/Zap2"), npc.position);
                        int a = Projectile.NewProjectile(npc.position, new Vector2(-10, Main.rand.Next(-5, 0)), ModContent.ProjectileType<EngimaSpell>(), npc.damage / 4, 4, Main.myPlayer);
                        Main.projectile[a].Center = npc.Center + new Vector2(30, 0);
                        int b = Projectile.NewProjectile(npc.position, new Vector2(10, Main.rand.Next(-5, 0)), ModContent.ProjectileType<EngimaSpell>(), npc.damage / 4, 4, Main.myPlayer);
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
                        Main.PlaySound(mod.GetLegacySoundSlot(SoundType.Custom, "Sounds/Sounds/Static"), npc.position); 
                        handRot = npc.DirectionFrom(player.Center).ToRotation() - 0.001f;
                        Projectile laser = Main.projectile[Projectile.NewProjectile(npc.Center.X, npc.Center.Y, 0f, 0f, ModContent.ProjectileType<EnigmaBeam>(), npc.damage / 4, 3f, Main.myPlayer, 0, npc.whoAmI)];
                        laser.velocity = BaseUtility.RotateVector(default, new Vector2(14f, 0f), laser.rotation);
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

                    if (Main.rand.Next(10) == 0)
                    {
                        int x = Main.rand.Next(-6, 6);
                        int y = -Main.rand.Next(3, 5);
                        int p = Projectile.NewProjectile(npc.Center, new Vector2(x, y), ModContent.ProjectileType<Nut>(), npc.damage / 2, 5, Main.myPlayer, 0, Main.rand.Next(3));
                        Main.projectile[p].Center = npc.Center;
                    }

                    if (npc.ai[1] >= 120)
                    {
                        int minion = Main.rand.Next(2);

                        switch (minion)
                        {
                            case 0: minion = mod.NPCType("BabyStarcore"); break;
                            default: minion = mod.NPCType("SaucerMinion"); break;
                        }

                        int m = NPC.NewNPC((int)npc.Center.X, (int)npc.Center.Y, minion);
                        Main.npc[m].Center = npc.Center;

                        AIReset();
                    }

                    break;
                case 6:
                    if (npc.ai[1] == 45)
                    {
                        Main.PlaySound(mod.GetLegacySoundSlot(SoundType.Custom, "Sounds/Sounds/Zap"), npc.position);
                        int a = Projectile.NewProjectile(npc.position, new Vector2(-10, Main.rand.Next(-20, 10)), ModContent.ProjectileType<EnigmaVortex>(), npc.damage / 4, 4, Main.myPlayer);
                        Main.projectile[a].Center = npc.Center + new Vector2(200, 0);
                        int b = Projectile.NewProjectile(npc.position, new Vector2(10, Main.rand.Next(-20, 10)), ModContent.ProjectileType<EnigmaVortex>(), npc.damage / 4, 4, Main.myPlayer);
                        Main.projectile[b].Center = npc.Center - new Vector2(200, 0);
                    }

                    if (npc.ai[1] > 180)
                    {
                        AIReset();
                    }
                    break;
                case 7:
                    npc.ai[2]++;

                    if (npc.ai[2] < 10)
                    {
                        HandFrame = 0;
                    }
                    if (npc.ai[2] == 10)
                    {
                        HandFrame = 1;
                    }
                    if (npc.ai[2] == 20)
                    {
                        HandFrame = 2;
                        if (Main.netMode != NetmodeID.MultiplayerClient)
                        {
                            int a = Projectile.NewProjectile(npc.Center, new Vector2(Main.rand.Next(-5, 5), Main.rand.Next(-7, -5)), ModContent.ProjectileType<EnigmaGrenade>(), npc.damage / 4, 4, Main.myPlayer);
                            Main.projectile[a].Center = npc.Center + new Vector2(20, 20);
                        }
                    }
                    if (npc.ai[2] == 30)
                    {
                        HandFrame = 3;
                    }
                    if (npc.ai[2] >= 40)
                    {
                        npc.ai[2] = 0;
                    }

                    if (npc.ai[1] > 241)
                    {
                        AIReset();
                    }
                    break;
                case 8:

                    for (int num468 = 0; num468 < 3; num468++)
                    {
                        int num469 = Dust.NewDust(npc.Center, 0, 0, DustID.Electric, 0, 0, 100, default, 1f);
                        Main.dust[num469].noGravity = true;
                    }

                    if (npc.ai[1] > 90)
                    {
                        Projectile.NewProjectile(npc.Center.X + 40, npc.Center.Y, 0, 10, mod.ProjectileType("Thundershock"), npc.damage / 4, 0f, Main.myPlayer);
                        Projectile.NewProjectile(npc.Center.X - 40, npc.Center.Y, 0, 10, mod.ProjectileType("Thundershock"), npc.damage / 4, 0f, Main.myPlayer);
                        npc.ai[0] = 9;
                        npc.ai[1] = 0;
                        npc.ai[2] = 0;
                        npc.ai[3] = 0;
                    }

                    break;
                case 9:

                    if (npc.ai[1] > 60)
                    {
                        AIReset();
                    }

                        break;
                case 10:

                    if (npc.ai[1] > ChangeRate)
                    {
                        npc.ai[0] = 11;
                        npc.ai[1] = 0;
                        npc.ai[2] = 0;
                        npc.ai[3] = 0;
                    }
                    break;
                case 11:
                    handRot = npc.DirectionFrom(player.Center).ToRotation() - 0.001f;
                    if (npc.ai[1] % 10 == 0)
                    {
                        Main.PlaySound(mod.GetLegacySoundSlot(SoundType.Custom, "Sounds/Sounds/Static"), npc.position);
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
                    }

                    if (npc.ai[1] > ChangeRate + 60)
                    {
                        AIReset();
                    }

                    break;
                default:
                    npc.ai[0] = 0;
                    goto case 0;
            }

            if (npc.ai[0] == 4 || npc.ai[0] == 11)
            {
                handRot -= GetSpinOffset();
            }
            else
            {
                handRot = 0;
            }
        }

        private float GetSpinOffset()
        {
            const float PI = (float)Math.PI;
            float newRotation = (Main.player[npc.target].Center - npc.Center).ToRotation();
            float difference = newRotation;
            float rotationDirection = 2f * (float)Math.PI * 1f / 6f / 60f;
            while (difference < -PI)
                difference += 2f * PI;
            while (difference > PI)
                difference -= 2f * PI;
            if (difference > 0f)
                rotationDirection *= -1f;
            return rotationDirection;
        }

        public void AIChange()
        {
            if (Main.netMode != NetmodeID.MultiplayerClient)
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
            if (Main.netMode != NetmodeID.MultiplayerClient)
            {
                npc.ai[0] = 0;
                npc.ai[1] = 0;
                npc.ai[2] = 0;
                npc.ai[3] = 0;
                npc.netUpdate = true;
            }
        }

        //Idle = 0, HomingMagic = 1, LightningStorm = 2, BeamPrep = 3, Beam = 4, Construct = 5, Vortexes = 6, Grenades = 7, StaticPrep = 10, Static = 11;

        public int AIType()
        {
            int aitype = Main.rand.Next(Unhooded ? 8 : 7);

            switch (aitype)
            {
                case 0:
                    return HomingMagic;
                case 1:
                    return LightningStorm;
                case 2:
                    return BeamPrep;
                case 3:
                    return Construct;
                case 4:
                    return Vortexes;
                case 5:
                    return Grenades;
                case 6:
                    return ShockPrep;
                default:
                    return StaticPrep;
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
        public float handRot = 0;

        Texture2D body;
        Texture2D hand;
        Texture2D glow;

        public override bool PreDraw(SpriteBatch spriteBatch, Color drawColor)
        {
            Textures();

            Texture2D RingTex = mod.GetTexture("NPCs/Bosses/Enigma/EnigmaCircle");
            Texture2D ChargeTex = mod.GetTexture("NPCs/Bosses/Enigma/EnigmaCharge");

            Rectangle handsframe = BaseDrawing.GetFrame(HandFrame, hand.Width, hand.Height / 4, 0, 0);
            Rectangle charge = BaseDrawing.GetFrame(ChargeFrame, ChargeTex.Width, ChargeTex.Height / 4, 0, 0);
            Rectangle RingFrame = BaseDrawing.GetFrame(0, RingTex.Width, RingTex.Height, 0, 0);

            RingEffects();
            if (scale > 0)
            {
                BaseDrawing.DrawTexture(spriteBatch, RingTex, 0, npc.position, npc.width, npc.height, scale, rotation, 0, 1, RingFrame, Color.White, true);
            }

            BaseDrawing.DrawTexture(spriteBatch, body, 0, npc.position, npc.width, npc.height, npc.scale, 0, 0, 6, npc.frame, drawColor, true);
            BaseDrawing.DrawTexture(spriteBatch, glow, 0, npc.position, npc.width, npc.height, npc.scale, 0, 0, 6, npc.frame, Color.White, true);

            if (npc.ai[0] == 10)
            {
                BaseDrawing.DrawTexture(spriteBatch, ChargeTex, 0, npc.position, npc.width, npc.height, npc.scale, 0, 0, 4, charge, Color.White, true);
            }

            BaseDrawing.DrawTexture(spriteBatch, hand, 0, npc.position, npc.width, npc.height, npc.scale, handRot, 0, 4, handsframe, drawColor, true);
            return false;
        }


        public void Textures()
        {
            body = Main.npcTexture[npc.type];
            glow = mod.GetTexture("Glowmasks/Enigma_Glow");
            if (npc.life < npc.lifeMax / 2)
            {
                body = mod.GetTexture("NPCs/Bosses/Enigma/EnigmaUnhooded");
                glow = mod.GetTexture("Glowmasks/EnigmaUnhooded_Glow");
            }

            switch ((int)npc.ai[0])
            {
                case Idle:
                    hand = mod.GetTexture("NPCs/Bosses/Enigma/EnigmaHands");
                    break;

                case HomingMagic:
                case Vortexes:
                case Shock:
                    hand = mod.GetTexture("NPCs/Bosses/Enigma/EnigmaHandsBlast");
                    break;

                case LightningStorm:
                    hand = mod.GetTexture("NPCs/Bosses/Enigma/EnigmaHandsCast");
                    break;

                case BeamPrep:
                case ShockPrep:
                    hand = mod.GetTexture("NPCs/Bosses/Enigma/EnigmaHandsPrep");
                    break;

                case Beam:
                    hand = mod.GetTexture("NPCs/Bosses/Enigma/EnigmaHandsLaser");
                    break;

                case Construct:
                    hand = mod.GetTexture("NPCs/Bosses/Enigma/EnigmaHandsAssemble");
                    break;

                case Grenades:
                    hand = mod.GetTexture("NPCs/Bosses/Enigma/EnigmaHandsThrow");
                    break;

                case StaticPrep:
                    hand = mod.GetTexture("NPCs/Bosses/Enigma/EnigmaHandsCharge");
                    break;

                case Static:
                    hand = mod.GetTexture("NPCs/Bosses/Enigma/EnigmaHandsAim");
                    break;

                case despawn:
                    hand = mod.GetTexture("NPCs/Bosses/Enigma/EnigmaHandsPrelude");
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
                if (npc.frame.Y > frameHeight * 5)
                {
                    npc.frame.Y = 0;
                }

                if (npc.ai[0] == 10)
                {
                    ChargeFrame += 1;
                    if (ChargeFrame > 3)
                    {
                        ChargeFrame = 0;
                    }
                }
            }

            if (npc.ai[0] != 7)
            {
                int fpt = (int)npc.ai[0] == 0 ? 10 : 1;

                if (handCounter++ >= fpt)
                {
                    handCounter = 0;
                    HandFrame++;
                    if (HandFrame > 3)
                    {
                        HandFrame = 0;
                    }
                }
            }
        }

        float scale = 0;
        float rotation = 0;

        private void RingEffects()
        {
            rotation += .2f;
            if (npc.ai[0] == 10 || npc.ai[0] == 11)
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
            if (npc.alpha > 0)
            {
                npc.alpha -= 5;
            }
            else
            {
                npc.alpha = 0;
            }
            if (Main.netMode != NetmodeID.MultiplayerClient)
            {
                Preamble[1]++;
                music = mod.GetSoundSlot(SoundType.Music, "Sounds/Music/silence");
                if (Preamble[0] == 0)
                {
                    if (!CWorld.downedEnigma)
                    {
                        if (Preamble[1] == 1)
                        {
                            for (int num572 = 0; num572 < 10; num572++)
                            {
                                float num573 = npc.velocity.X * 0.2f * num572;
                                float num574 = -(npc.velocity.Y * 0.2f) * num572;
                                int num575 = Dust.NewDust(new Vector2(npc.position.X, npc.position.Y), npc.width, npc.height, DustID.Electric, 0f, 0f, 100, default, 1f);
                                Main.dust[num575].velocity *= 0f;
                                Dust expr_178B4_cp_0 = Main.dust[num575];
                                expr_178B4_cp_0.position.X -= num573;
                                Dust expr_178D3_cp_0 = Main.dust[num575];
                                expr_178D3_cp_0.position.Y -= num574;
                            }
                        }
                        if (Preamble[1] == 120)
                        {
                            BaseUtility.Chat("Well...I've finally found you.", Color.Cyan);
                        }

                        if (Preamble[1] == 240)
                        {
                            BaseUtility.Chat("You have been quite the thorn in my side.", Color.Cyan);
                        }

                        if (Preamble[1] == 360)
                        {
                            BaseUtility.Chat("Especially after you trashed my beatuiful starcore...", Color.Cyan);
                        }

                        if (Preamble[1] == 480)
                        {
                            BaseUtility.Chat("You're too much of a liability for my plans, so...", Color.Cyan);
                        }

                        if (Preamble[1] >= 600)
                        {
                            BaseUtility.Chat("I'll squash you like the insect you are.", Color.Cyan);
                            Preamble[0] = 1;
                            Preamble[1] = 0;

                            npc.netUpdate = true;
                        }
                    }
                    else
                    {
                        BaseUtility.Chat("You again? Haven't you humiliated me enough as is?", Color.Cyan);
                        Preamble[1] = 0;
                        Preamble[0] = 1;

                        npc.netUpdate = true;
                    }
                }
                else if (Preamble[0] == 2)
                {
                    npc.velocity *= .96f;
                    if (Preamble[1] == 90)
                    {
                        BaseUtility.Chat("..!!!", Color.Cyan);
                    }

                    if (Preamble[1] == 180)
                    {
                        BaseUtility.Chat("MY HOOD!!!", Color.Cyan);
                    }

                    if (Preamble[1] == 270)
                    {
                        BaseUtility.Chat("MY SUPREME BRAIN IS EXPOSED!", Color.Cyan);
                    }

                    if (Preamble[1] >= 360)
                    {
                        BaseUtility.Chat("HOW DARE YOU! TASTE ELECTRICITY YOU INSIGNIFICANT IMBECILE!", Color.Cyan);
                        Preamble[0] = 1;
                        Preamble[1] = 0;

                        npc.ai[0] = 10;
                        npc.ai[1] = 0;
                        npc.ai[2] = 0;
                        npc.ai[3] = 0;

                        npc.netUpdate = true;
                    }
                }
            }
        }

        public void Despawn()
        {
            Preamble[1]++;

            if (Preamble[1] > 120 && Main.netMode != NetmodeID.MultiplayerClient)
            {
                BaseUtility.Chat("Hmpf. Pathetic.", Color.Cyan);
                for (int num572 = 0; num572 < 10; num572++)
                {
                    float num573 = npc.velocity.X * 0.2f * num572;
                    float num574 = -(npc.velocity.Y * 0.2f) * num572;
                    int num575 = Dust.NewDust(new Vector2(npc.position.X, npc.position.Y), npc.width, npc.height, DustID.Electric, 0f, 0f, 100, default, 1f);
                    Main.dust[num575].velocity *= 0f;
                    Dust expr_178B4_cp_0 = Main.dust[num575];
                    expr_178B4_cp_0.position.X -= num573;
                    Dust expr_178D3_cp_0 = Main.dust[num575];
                    expr_178D3_cp_0.position.Y -= num574;
                }
                npc.active = false;
                npc.netUpdate = true;
            }
        }

        #endregion
    }
}