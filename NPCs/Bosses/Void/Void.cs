using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.IO;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using CSkies.Items.Boss.Void;

namespace CSkies.NPCs.Bosses.Void
{
    [AutoloadBossHead]
    public class Void : ModNPC
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("VOID");
            Main.npcFrameCount[npc.type] = 6;
        }

        public override void SetDefaults()
        {
            npc.width = 70;
            npc.height = 136;
            npc.value = BaseUtility.CalcValue(0, 10, 0, 0);
            npc.npcSlots = 1000;
            npc.aiStyle = -1;
            npc.lifeMax = 150000;
            npc.defense = 70;
            npc.damage = 130;
            npc.knockBackResist = 0f;
            npc.noGravity = true;
            npc.HitSound = SoundID.NPCHit49;
            npc.DeathSound = SoundID.NPCDeath51;
            npc.boss = true;
            npc.noTileCollide = true;
            music = mod.GetSoundSlot(SoundType.Music, "Sounds/Music/Void");
            bossBag = ModContent.ItemType<ObserverVoidBag>();
            npc.dontTakeDamage = true;
            npc.value = Item.sellPrice(0, 30, 0, 0);
        }

        public override void ScaleExpertStats(int numPlayers, float bossLifeScale)
        {
            npc.lifeMax = (int)(npc.lifeMax * 0.5f * bossLifeScale);
            npc.defense = (int)(npc.defense * 1.2f);
            npc.damage = (int)(npc.damage * .8f);
        }

        float VortexScale = 0f;
        float VortexRotation = 0f;

        public float[] Vortex = new float[1];

        public override void SendExtraAI(BinaryWriter writer)
        {
            base.SendExtraAI(writer);
            if (Main.netMode == NetmodeID.Server || Main.dedServ)
            {
                writer.Write(Vortex[0]);
            }
        }

        public override void ReceiveExtraAI(BinaryReader reader)
        {
            base.ReceiveExtraAI(reader);
            if (Main.netMode == NetmodeID.MultiplayerClient)
            {
                Vortex[0] = reader.ReadFloat();
            }
        }

        bool rage = false;

        readonly float Pi2 = (float)Math.PI * 2;

        public override void AI()
        {
            if (npc.life < npc.lifeMax / 4)
            {
                if (!rage)
                {
                    rage = true;
                    Main.NewText("VOID's form begins to destabilize", Color.LightCyan);
                }
                music = mod.GetSoundSlot(SoundType.Music, "Sounds/Music/Pinch");
            }

            Lighting.AddLight(npc.Center, 0, .1f, .3f);
            isCharging = false;
            Player player = Main.player[npc.target];
            Vector2 targetPos;

            npc.TargetClosest();
            SuckPlayer();

            switch ((int)npc.ai[0])
            {
                case 0: //fly to corner for dash

                    if (!AliveCheck(player))
                        break;
                    targetPos = player.Center;
                    targetPos.X += 430 * (npc.Center.X < targetPos.X ? -1 : 1);
                    targetPos.Y -= 430;
                    Movement(targetPos, .7f);
                    if (++npc.ai[1] > 180 || Math.Abs(npc.Center.Y - targetPos.Y) < 100) //initiate dash
                    {
                        npc.ai[0]++;
                        npc.ai[1] = 0;
                        npc.netUpdate = true;
                        npc.velocity = npc.DirectionTo(player.Center) * 45;
                    }
                    break;

                case 1: //dashing
                    isCharging = true;
                    if (npc.Center.Y > player.Center.Y + 500 || Math.Abs(npc.Center.X - player.Center.X) > 1000)
                    {
                        npc.velocity.Y *= 0.5f;
                        npc.ai[1] = 0;
                        if (++npc.ai[2] >= Repeats() - 1) //repeat three times
                        {
                            npc.ai[0]++;
                            npc.ai[2] = 0;
                        }
                        else
                            npc.ai[0]--;
                        npc.netUpdate = true;
                    }
                    break;
                case 2:
                    npc.velocity *= 0;
                    if (npc.ai[1] < 90)
                    {
                        if (npc.ai[3] < Repeats() - 1)
                        {
                            if (Main.netMode != NetmodeID.MultiplayerClient) { npc.ai[2]++; }
                            int teleportRate = npc.life < npc.lifeMax / 4 ? 15 : 30;
                            if (npc.ai[2] >= teleportRate) // + lasers
                            {
                                Teleport();
                                Starblast();
                                npc.ai[3] += 1;
                                npc.ai[2] = 0;
                                npc.netUpdate = true;
                            }
                            break;
                        }
                        else
                        {
                            npc.ai[0]++;
                            npc.ai[1] = 0;
                            npc.ai[2] = 0;
                            npc.ai[3] = 0;
                        }
                    }
                    break;

                case 3: //prepare for queen bee dashes
                    if (!AliveCheck(player))
                        break;

                    if (++npc.ai[1] > 60)
                    {
                        targetPos = player.Center;
                        targetPos.X += 400 * (npc.Center.X < targetPos.X ? -1 : 1);
                        Movement(targetPos, 1f);
                        if (npc.ai[1] > 180 || Math.Abs(npc.Center.Y - targetPos.Y) < 40) //initiate dash
                        {
                            npc.ai[0]++;
                            npc.ai[1] = 0;
                            npc.netUpdate = true;
                            npc.velocity.X = -40 * (npc.Center.X < player.Center.X ? -1 : 1);
                            npc.velocity.Y *= 0.1f;
                        }
                    }
                    else
                    {
                        npc.velocity *= 0.9f; //decelerate briefly
                    }
                    break;

                case 4:
                    isCharging = true;

                    if (++npc.ai[1] > 240 || (Math.Sign(npc.velocity.X) > 0 ? npc.Center.X > player.Center.X + 900 : npc.Center.X < player.Center.X - 900))
                    {
                        npc.ai[1] = 0;
                        npc.ai[2] = 0;
                        if (++npc.ai[3] >= 3) //repeat dash three times
                        {
                            Teleport();
                            npc.ai[0]++;
                            npc.ai[3] = 0;
                        }
                        else
                            npc.ai[0]--;
                        npc.netUpdate = true;
                    }
                    
                    if (npc.ai[1] % 20 == 0)
                    {
                        int a = Projectile.NewProjectile(npc.Center, Vector2.Zero, mod.ProjectileType("VoidVortex"), npc.damage / 2, 3);
                        Main.projectile[a].Center = npc.Center;
                    }
                    if (npc.ai[1] % 30 == 0)
                    {
                        if (npc.life < npc.lifeMax / 3)
                        {
                            Starblast();
                        }
                    }
                    break;

                case 5: //Prep Deathray
                    if (!AliveCheck(Main.player[npc.target]))
                        break;

                    npc.velocity *= 0;

                    if (!CUtils.AnyProjectiles(ModContent.ProjectileType<VoidraySmall>()))
                    {
                        float dir = Pi2 / 8;

                        if (Main.rand.NextBool() || npc.life < npc.lifeMax / 2)
                        {
                            dir = 0;
                        }

                        int loops = (npc.life < npc.lifeMax / 2 ? 8 : 4);

                        for (int i = 0; i < loops; i++)
                        {
                            Vector2 shotDir = dir.ToRotationVector2();

                            Projectile.NewProjectile(npc.Center, shotDir, mod.ProjectileType("VoidraySmall"), npc.damage / 2, 0f, Main.myPlayer, 0, npc.whoAmI);

                            dir += (float)Math.PI * 2 / loops;
                        }
                    }

                    if (++npc.ai[2] > 60)
                    {
                        npc.ai[0]++;
                        npc.ai[1] = 0;
                        npc.ai[2] = 0;

                        if (Main.netMode != NetmodeID.MultiplayerClient)
                            npc.TargetClosest(false);

                        if (npc.life < npc.lifeMax / 2 && Main.netMode != NetmodeID.MultiplayerClient)
                        {
                            int choice = Main.rand.Next(2);
                            if (choice == 0)
                            {
                                Main.PlaySound(SoundID.Item73, (int)npc.position.X, (int)npc.position.Y);
                                int a = Projectile.NewProjectile(npc.Center, new Vector2(0f, -12f), mod.ProjectileType("VoidShot"), npc.damage / 2, 3);
                                Main.projectile[a].Center = npc.Center;
                                int b = Projectile.NewProjectile(npc.Center, new Vector2(0f, 12f), mod.ProjectileType("VoidShot"), npc.damage / 2, 3);
                                Main.projectile[b].Center = npc.Center;
                                int c = Projectile.NewProjectile(npc.Center, new Vector2(-12f, 0f), mod.ProjectileType("VoidShot"), npc.damage / 2, 3);
                                Main.projectile[c].Center = npc.Center;
                                int d = Projectile.NewProjectile(npc.Center, new Vector2(12f, 0f), mod.ProjectileType("VoidShot"), npc.damage / 2, 3);
                                Main.projectile[d].Center = npc.Center;
                            }
                            else
                            {
                                int a = NPC.NewNPC((int)npc.Center.X, (int)npc.Center.Y, ModContent.NPCType<Enemies.AbyssGazer>());
                                Main.npc[a].Center = npc.Center + new Vector2(0, 60);
                                int b = NPC.NewNPC((int)npc.Center.X, (int)npc.Center.Y, ModContent.NPCType<Enemies.AbyssGazer>());
                                Main.npc[b].Center = npc.Center + new Vector2(0, -60);
                                int c = NPC.NewNPC((int)npc.Center.X, (int)npc.Center.Y, ModContent.NPCType<Enemies.AbyssGazer>());
                                Main.npc[c].Center = npc.Center + new Vector2(-60, 0);
                                int d = NPC.NewNPC((int)npc.Center.X, (int)npc.Center.Y, ModContent.NPCType<Enemies.AbyssGazer>());
                                Main.npc[d].Center = npc.Center + new Vector2(-60, 0);
                            }
                            npc.netUpdate = true;
                        }
                    }
                    break;

                case 6: //firing mega ray

                    npc.velocity *= 0;

                    if (++npc.ai[1] > 180)
                    {
                        npc.velocity *= .98f;
                        npc.ai[0]++;
                        npc.ai[1] = 0;
                        npc.ai[3] = 0;
                        npc.netUpdate = true;
                    }
                    break;
                

                case 7: //prepare for fishron dash
                    if (!AliveCheck(player))
                        break;
                    targetPos = player.Center + player.DirectionTo(npc.Center) * 600;
                    Movement(targetPos, 0.6f);
                    if (++npc.ai[1] > 20)
                    {
                        npc.ai[0]++;
                        npc.ai[1] = 0;
                        npc.netUpdate = true;
                        npc.velocity = npc.DirectionTo(player.Center) * 40;
                    }
                    break;

                case 8: //dashing
                    isCharging = true;
                    if (++npc.ai[1] > 40)
                    {
                        npc.ai[1] = 0;
                        npc.ai[2] = 0;
                        if (++npc.ai[3] >= Repeats())
                        {
                            npc.ai[0]++;
                            npc.ai[3] = 0;
                        }
                        else
                            npc.ai[0]--;
                        npc.netUpdate = true;
                    }
                    break;

                case 9: //hover nearby, shoot lightning
                    if (!AliveCheck(player))
                        break;

                    npc.velocity *= 0;

                    if (++npc.ai[2] > 20)
                    {
                        Teleport();
                        npc.ai[2] = 0;
                        if (Main.netMode != NetmodeID.MultiplayerClient) //spawn lightning
                        {
                            for (int l = 0; l < Repeats(); l++)
                            {
                                int speed = 16;
                                int DirectionX = Main.rand.Next(3);
                                switch (DirectionX)
                                {
                                    case 0:
                                        DirectionX = speed;
                                        break;
                                    case 1:
                                        DirectionX = -speed;
                                        break;
                                    case 2:
                                        DirectionX = 0;
                                        break;
                                }
                                int DirectionY = DirectionX == 0 ? Main.rand.Next(2) : Main.rand.Next(3);
                                switch (DirectionY)
                                {
                                    case 0:
                                        DirectionY = speed;
                                        break;
                                    case 1:
                                        DirectionY = -speed;
                                        break;
                                    case 2:
                                        DirectionY = 0;
                                        break;
                                }


                                Vector2 vel = new Vector2(DirectionX, DirectionY);

                                DirectionX += DirectionX == 0 ?  0 : Main.rand.Next(-2, 2);
                                DirectionY += DirectionY == 0 ? 0 : Main.rand.Next(-2, 2);

                                Projectile.NewProjectile((int)npc.Center.X + DirectionX, (int)npc.Center.Y + DirectionY, vel.X * 2, vel.Y * 2, ModContent.ProjectileType<VoidShock>(), npc.damage / 2, 0f, Main.myPlayer, vel.ToRotation(), 0f);
                            }
                        }
                    }
                    if (++npc.ai[1] > 360)
                    {
                        npc.ai[0]++;
                        npc.ai[1] = 0;
                        npc.ai[2] = 0;
                        npc.ai[3] = npc.Distance(player.Center);
                        npc.netUpdate = true;
                    }
                    break;

                case 30:
                    if (npc.ai[1]++ > 80)
                    {
                        npc.ai[0] = 0;
                        npc.dontTakeDamage = false;
                        npc.ai[1] = 0;
                    }
                    break;

                default:
                    npc.ai[0] = 0;
                    goto case 0;
            }

            for (int m = npc.oldPos.Length - 1; m > 0; m--)
            {
                npc.oldPos[m] = npc.oldPos[m - 1];
            }
            npc.oldPos[0] = npc.position;

            npc.rotation = 0;

            if (npc.dontTakeDamage)
            {
                npc.ai[0] = 30;
            }
        }

        public void Starblast()
        {
            Main.PlaySound(SoundID.Item73, (int)npc.position.X, (int)npc.position.Y);

            float dir = 0;

            if (Main.rand.NextBool() || npc.life < npc.lifeMax / 2)
            {
                dir = (float)Math.PI / 8; ;
            }

            int loops = (npc.life < npc.lifeMax / 2 ? 8 : 4);

            for (int i = 0; i < loops; i++)
            {
                Vector2 shotDir = dir.ToRotationVector2();

                int a = Projectile.NewProjectile(npc.Center, shotDir * 10, mod.ProjectileType("VoidBlast"), npc.damage / 2, 0f, Main.myPlayer, 0, npc.whoAmI);
                Main.projectile[a].Center = npc.Center;

                dir += (float)Math.PI * 2 / loops;
            }
        }

        public void Teleport()
        {
            Player player = Main.player[npc.target];
            Vector2 targetPos = player.Center;
            int posX = Main.rand.Next(3);
            switch (posX)
            {
                case 0:
                    posX = -400;
                    break;
                case 1:
                    posX = 0;
                    break;
                case 2:
                    posX = 400;
                    break;
            }
            int posY = Main.rand.Next(posX == 0 ? 1 : 2);
            switch (posY)
            {
                case 0:
                    posY = -400;
                    break;
                case 1:
                    posY = 0;
                    break;
            }

            npc.position = new Vector2(targetPos.X + posX, targetPos.Y + posY);

            Vector2 position = npc.Center + (Vector2.One * -20f);
            int num84 = 40;
            int height3 = num84;
            for (int num85 = 0; num85 < 3; num85++)
            {
                int num86 = Dust.NewDust(position, num84, height3, 240, 0f, 0f, 100, default, 1.5f);
                Main.dust[num86].position = npc.Center + (Vector2.UnitY.RotatedByRandom(3.1415927410125732) * (float)Main.rand.NextDouble() * num84 / 2f);
            }
            for (int num87 = 0; num87 < 15; num87++)
            {
                int num88 = Dust.NewDust(position, num84, height3, ModContent.DustType<Dusts.VoidDust>(), 0f, 0f, 50, default, 3.7f);
                Main.dust[num88].position = npc.Center + (Vector2.UnitY.RotatedByRandom(3.1415927410125732) * (float)Main.rand.NextDouble() * num84 / 2f);
                Main.dust[num88].noGravity = true;
                Main.dust[num88].noLight = true;
                Main.dust[num88].velocity *= 3f;
                Main.dust[num88].velocity += npc.DirectionTo(Main.dust[num88].position) * (2f + (Main.rand.NextFloat() * 4f));
                num88 = Dust.NewDust(position, num84, height3, ModContent.DustType<Dusts.VoidDust>(), 0f, 0f, 25, default, 1.5f);
                Main.dust[num88].position = npc.Center + (Vector2.UnitY.RotatedByRandom(3.1415927410125732) * (float)Main.rand.NextDouble() * num84 / 2f);
                Main.dust[num88].velocity *= 2f;
                Main.dust[num88].noGravity = true;
                Main.dust[num88].fadeIn = 1f;
                Main.dust[num88].color = Color.Black * 0.5f;
                Main.dust[num88].noLight = true;
                Main.dust[num88].velocity += npc.DirectionTo(Main.dust[num88].position) * 8f;
            }
            for (int num89 = 0; num89 < 10; num89++)
            {
                int num90 = Dust.NewDust(position, num84, height3, ModContent.DustType<Dusts.VoidDust>(), 0f, 0f, 0, default, 2.7f);
                Main.dust[num90].position = npc.Center + (Vector2.UnitX.RotatedByRandom(3.1415927410125732).RotatedBy(npc.velocity.ToRotation(), default) * num84 / 2f);
                Main.dust[num90].noGravity = true;
                Main.dust[num90].noLight = true;
                Main.dust[num90].velocity *= 3f;
                Main.dust[num90].velocity += npc.DirectionTo(Main.dust[num90].position) * 2f;
            }
            for (int num91 = 0; num91 < 30; num91++)
            {
                int num92 = Dust.NewDust(position, num84, height3, ModContent.DustType<Dusts.VoidDust>(), 0f, 0f, 0, default, 1.5f);
                Main.dust[num92].position = npc.Center + (Vector2.UnitX.RotatedByRandom(3.1415927410125732).RotatedBy(npc.velocity.ToRotation(), default) * num84 / 2f);
                Main.dust[num92].noGravity = true;
                Main.dust[num92].velocity *= 3f;
                Main.dust[num92].velocity += npc.DirectionTo(Main.dust[num92].position) * 3f;
            }

        }

        public void SuckPlayer()
        {
            bool V = npc.ai[0] == 4;
            Player target = Main.player[Main.myPlayer];

            if (Vector2.Distance(target.Center, npc.Center) > 4000 && !target.dead && target.active && V)
            {
                target.AddBuff(ModContent.BuffType<Buffs.Sucked>(), 2);
            }

            if (V)
            {
                if (VortexScale < 1f)
                {
                    VortexScale += .01f;
                }
                VortexRotation += .01f;
            }
            else
            {
                if (VortexScale > 0f)
                {
                    VortexScale -= .05f;
                }
                VortexRotation += .05f;
            }
        }

        private int Repeats()
        {
            if (npc.life < (int)(npc.life * .66f))
            {
                return 5;
            }
            else if (npc.life < (int)(npc.life * .33f))
            {
                return 6;
            }
            else
            {
                return 4;
            }
        }

        private bool AliveCheck(Player player)
        {
            if (player.dead || !player.active || Vector2.Distance(npc.Center, player.Center) > 10000)
            {
                npc.TargetClosest();

                if (player.dead || !player.active || Vector2.Distance(npc.Center, player.Center) > 10000)
                {
                    npc.alpha += 3;
                    if (npc.alpha > 255)
                    {
                        npc.active = false;
                        npc.netUpdate = true;
                    }
                }
            }
            else
            {
                if (npc.alpha > 0)
                {
                    for (int spawnDust = 0; spawnDust < 2; spawnDust++)
                    {
                        int dust = ModContent.DustType<Dusts.VoidDust>();
                        int num935 = Dust.NewDust(new Vector2(npc.position.X, npc.position.Y), npc.width, npc.height, dust, 0f, 0f, 100, default, 2f);
                        Main.dust[num935].noGravity = true;
                        Main.dust[num935].noLight = true;
                    }
                    npc.alpha -= 4;
                }
                else
                {
                    npc.alpha = 0;
                }
            }
            return true;
        }

        private void Movement(Vector2 targetPos, float speedModifier)
        {
            if (npc.Center.X < targetPos.X)
            {
                npc.velocity.X += speedModifier;
                if (npc.velocity.X < 0)
                    npc.velocity.X += speedModifier * 2;
            }
            else
            {
                npc.velocity.X -= speedModifier;
                if (npc.velocity.X > 0)
                    npc.velocity.X -= speedModifier * 2;
            }
            if (npc.Center.Y < targetPos.Y)
            {
                npc.velocity.Y += speedModifier;
                if (npc.velocity.Y < 0)
                    npc.velocity.Y += speedModifier * 2;
            }
            else
            {
                npc.velocity.Y -= speedModifier;
                if (npc.velocity.Y > 0)
                    npc.velocity.Y -= speedModifier * 2;
            }
            if (Math.Abs(npc.velocity.X) > 30)
                npc.velocity.X = 30 * Math.Sign(npc.velocity.X);
            if (Math.Abs(npc.velocity.Y) > 30)
                npc.velocity.Y = 30 * Math.Sign(npc.velocity.Y);
        }

        public override void NPCLoot()
        {
            CWorld.downedObserverV = true;
            CWorld.downedVoid = true;
            int n = NPC.NewNPC((int)npc.Center.X, (int)npc.Center.Y, mod.NPCType("VoidDeath"), 0, 0);
            Main.npc[n].Center = npc.Center;
            Main.npc[n].velocity = npc.velocity;
            if (Main.rand.Next(10) == 0)
            {
                npc.DropLoot(mod.ItemType("VOIDTrophy"));
            }
            if (Main.expertMode)
            {
                npc.DropBossBags();
            }
        }

        public override void FindFrame(int frameHeight)
        {
            npc.frameCounter++;
            if (npc.frameCounter >= 7)
            {
                npc.frameCounter = 0;
                npc.frame.Y += frameHeight;
                if (npc.frame.Y > (frameHeight * 5))
                {
                    npc.frame.Y = 0;
                }
            }
        }

        public override void HitEffect(int hitDirection, double damage)
        {
            base.HitEffect(hitDirection, npc.damage / 2);
            for (int Loop = 0; Loop < 3; Loop++)
            {
                int dust = Dust.NewDust(new Vector2(npc.position.X, npc.position.Y), npc.width, npc.height, ModContent.DustType<Dusts.VoidDust>(), 0f, 0f, 0);
                Main.dust[dust].velocity.Y = hitDirection * 0.1f;
                Main.dust[dust].noGravity = false;
            }
            if (npc.life <= 0)
            {
                for (int Loop = 0; Loop < 5; Loop++)
                {
                    int dust = Dust.NewDust(new Vector2(npc.position.X, npc.position.Y), npc.width, npc.height, ModContent.DustType<Dusts.VoidDust>(), 0f, 0f, 0);
                    Main.dust[dust].noGravity = false;
                }
            }
        }

        bool isCharging = false;
        public float auraPercent = 0f;
        public bool auraDirection = true;

        public override bool PreDraw(SpriteBatch sb, Color dColor)
        {
            Texture2D tex = Main.npcTexture[npc.type];
            Texture2D Cyclone = mod.GetTexture("NPCs/Bosses/Void/VoidCyclone");
            if (auraDirection) { auraPercent += 0.1f; auraDirection = auraPercent < 1f; }
            else { auraPercent -= 0.1f; auraDirection = auraPercent <= 0f; }
            if (VortexScale > 0)
            {
                Rectangle frame = BaseDrawing.GetFrame(0, Cyclone.Width, Cyclone.Height, 0, 0);
                BaseDrawing.DrawTexture(sb, Cyclone, 0, npc.position, npc.width, npc.height, VortexScale, VortexRotation, npc.direction, 1, frame, Color.White, true);
            }
            if (isCharging)
            {
                BaseDrawing.DrawAfterimage(sb, tex, 0, npc, .6f, 1, 6, true, 0, 0, Color.White, npc.frame, 4);
            }
            BaseDrawing.DrawAura(sb, tex, 0, npc, auraPercent, 2f, 0f, 0f, npc.GetAlpha(Color.White));
            return false;
        }
    }

}
