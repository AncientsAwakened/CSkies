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

namespace CSkies.NPCs.Bosses.Novacore
{
    [AutoloadBossHead]
    public class Novacore : ModNPC
    {
        public override void SetStaticDefaults()
        {
            Main.npcFrameCount[npc.type] = 8;
        }
        public override void SetDefaults()
        {
            npc.npcSlots = 200;
            npc.width = 46;
            npc.height = 46;
            npc.aiStyle = -1;
            npc.damage = 130;
            npc.defense = 50;
            npc.lifeMax = 1000000;
            npc.value = Item.sellPrice(0, 12, 0, 0);
            npc.HitSound = new LegacySoundStyle(3, 4, Terraria.Audio.SoundType.Sound);
            npc.DeathSound = new LegacySoundStyle(4, 14, Terraria.Audio.SoundType.Sound);
            npc.knockBackResist = 0f;
            npc.noGravity = true;
            npc.netAlways = true;
            npc.boss = true;
            npc.noTileCollide = true;
            music = mod.GetSoundSlot(Terraria.ModLoader.SoundType.Music, "Sounds/Music/Novacore1");
        }

        public override void ScaleExpertStats(int numPlayers, float bossLifeScale)
        {
            npc.lifeMax = (int)(npc.lifeMax * 0.5f * bossLifeScale);
            npc.damage = (int)(npc.damage * 0.8f);
            npc.defense = 100;
        }

        public float Shoot = 0;
        public float[] internalAI = new float[4];
        public bool LaserShot = false;
		public float OrbitterDist = 0;
		public float closestPlayer = 0;

		public override void SendExtraAI(BinaryWriter writer)
        {
            base.SendExtraAI(writer);
            if (Main.netMode == NetmodeID.Server || Main.dedServ)
            {
                writer.Write(Shoot);
                writer.Write(internalAI[0]);
                writer.Write(internalAI[1]);
                writer.Write(internalAI[2]);
                writer.Write(internalAI[3]);
                writer.Write(LaserShot);
                writer.Write(OrbitterDist);
				writer.Write(closestPlayer);
			}
        }

        public override void ReceiveExtraAI(BinaryReader reader)
        {
            base.ReceiveExtraAI(reader);
            if (Main.netMode == NetmodeID.MultiplayerClient)
            {
                Shoot = reader.ReadFloat();
                internalAI[0] = reader.ReadFloat();
                internalAI[1] = reader.ReadFloat();
                internalAI[2] = reader.ReadFloat();
                internalAI[3] = reader.ReadFloat();
                LaserShot = reader.ReadBool();
                OrbitterDist = reader.ReadFloat();
				closestPlayer = reader.ReadFloat();
			}
        }

        public int prismCount = 4;
        readonly int AIRate = (Main.expertMode ? 150 : 220);
        readonly float Pi2 = (float)Math.PI * 2;

        public override void AI()
        {
            int speed = 14;
            float interval = .02f;

            if (npc.life < npc.lifeMax / 2)
            {
                speed = 16;
                interval = .025f;
                music = mod.GetSoundSlot(Terraria.ModLoader.SoundType.Music, "Sounds/Music/Novacore2");
            }
            if (npc.life < npc.lifeMax / 4)
            {
                speed = 18;
                interval = .025f;
                music = mod.GetSoundSlot(Terraria.ModLoader.SoundType.Music, "Sounds/Music/Pinch");
            }

            Lighting.AddLight(npc.Center, Color.Purple.R / 150, Color.Purple.G / 150, Color.Purple.B / 150);

            if (CUtils.AnyProjectiles(ModContent.ProjectileType<NovaTurretProj>()) || CUtils.AnyProjectiles(ModContent.ProjectileType<NovaTurret>()))
            {
                OrbitterDist += .2f;
                if (OrbitterDist > 300)
                {
                    OrbitterDist = 300;
                }
            }
            else
            {
                OrbitterDist = 0;
            }

            if (!npc.HasPlayerTarget)
            {
                npc.TargetClosest();
            }
            Player player = Main.player[npc.target];

            if (player.dead || !player.active || Main.dayTime)
            {
                npc.TargetClosest();
                npc.noTileCollide = true;

                if (npc.timeLeft < 10)
                    npc.timeLeft = 10;
                npc.velocity.X *= 0.9f;

                if (npc.ai[1]++ > 300)
                {
                    npc.velocity.Y -= 0.2f;
                    if (npc.velocity.Y > 15f) npc.velocity.Y = 15f;
                    npc.rotation = 0f;
                    if (npc.position.Y + npc.velocity.Y <= 0f && Main.netMode != NetmodeID.MultiplayerClient) { BaseAI.KillNPC(npc); npc.netUpdate = true; }
                }
            }

            BaseAI.AISkull(npc, ref npc.ai, true, speed, 350, interval, .05f);

            if (npc.ai[2]++ > AIRate || internalAI[0] == 7)
            {
                if (internalAI[0] != 6 || internalAI[0] != 7)
                {
                    if (npc.velocity.X >= 0)
                    {
                        npc.rotation += .06f;
                        npc.spriteDirection = -1;
                    }
                    else if (npc.velocity.X < 0)
                    {
                        npc.rotation -= .06f;
                        npc.spriteDirection = 1;
                    }
                }
                else
                {
                    npc.rotation += .03f;
                }
                switch (internalAI[0])
                {
                    #region Lightning
                    case 0:
                    case 1:
                    case 2:
                    case 3:
                    case 4:

                        if (npc.localAI[0] == 0f)
                        {
                            npc.localAI[0] = 1f;
                            int num833 = Player.FindClosest(npc.Center, 0, 0);
                            Vector2 vector62 = Main.player[num833].Center - npc.Center;
                            if (vector62 == Vector2.Zero)
                            {
                                vector62 = Vector2.UnitY;
                            }
                            closestPlayer = vector62.ToRotation();
                            npc.netUpdate = true;
                        }

                        if (npc.ai[2] % 45 == 0)
                        {
                            Main.PlaySound(SoundID.Item8, npc.position);
                            Vector2 vector71 = closestPlayer.ToRotationVector2();
                            Vector2 value56 = vector71.RotatedBy(1.5707963705062866) * (Main.rand.Next(2) == 0).ToDirectionInt() * Main.rand.Next(10, 21);
                            vector71 *= Main.rand.Next(-80, 81);
                            Vector2 velocity3 = (vector71 - value56) / 10f;
                            Dust dust25 = Main.dust[Dust.NewDust(npc.Center, 0, 0, ModContent.DustType<Dusts.CDust>())];
                            dust25.noGravity = true;
                            dust25.position = npc.Center + value56;
                            dust25.velocity = velocity3;
                            dust25.scale = 0.5f + Main.rand.NextFloat();
                            dust25.fadeIn = 0.5f;
                            if (Main.netMode != NetmodeID.MultiplayerClient)
                            {
                                for (int a = 0; a < Repeats() + 1; a++)
                                {
                                    Vector2 vector72 = closestPlayer.ToRotationVector2() * 8f;
                                    float ai2 = Main.rand.Next(80);
                                    Projectile.NewProjectile(npc.Center.X - vector72.X, npc.Center.Y - vector72.Y, vector72.X, vector72.Y, ProjectileID.VortexLightning, 15, 1f, Main.myPlayer, closestPlayer, ai2);
                                }
                            }
                            npc.localAI[0] = 0;
                        }

                        if (npc.ai[2] > AIRate + 45 + (45 * Repeats()) && Main.netMode != NetmodeID.MultiplayerClient)
                        {
                            AIChange();
                        }
                        break;
                    #endregion

                    #region Turrets
                    case 5:
                        for (int m = 0; m < TurretCount(); m++)
                        {
                            int projectileID = Projectile.NewProjectile(npc.Center, Vector2.Zero, ModContent.ProjectileType<NovaTurretProj>(), npc.damage, 4, Main.myPlayer);
                            Main.projectile[projectileID].Center = npc.Center;
                            Main.projectile[projectileID].velocity = new Vector2(MathHelper.Lerp(-1f, 1f, (float)Main.rand.NextDouble()), MathHelper.Lerp(-1f, 1f, (float)Main.rand.NextDouble()));
                            Main.projectile[projectileID].velocity *= 8f;
                            Main.projectile[projectileID].ai[0] = m;
                        }
                        AIChange();
                        break;
                    #endregion

                    #region Lasers
                    case 6: //Nova Beams
                        if (!AliveCheck(Main.player[npc.target]))
                            break;

                        npc.velocity *= .94f;

                        LaserAttack();

                        if (!CUtils.AnyProjectiles(ModContent.ProjectileType<NovaBeam>()) || !CUtils.AnyProjectiles(ModContent.ProjectileType<NovaBeamSmall>()))
                        {
                            AIChange();
                        }
                        break;
                    #endregion
                }
            }
            else
            {
                if (npc.velocity.X > 0)
                {
                    npc.rotation += .03f;
                }
                else if (npc.velocity.X < 0)
                {
                    npc.rotation -= .03f;
                }
                if (npc.life < npc.lifeMax / 2)
                {
                    if (Shoot > 30)
                    {
                        if (Shoot % 10 == 0)
                        {
                            if (Main.netMode != NetmodeID.MultiplayerClient)
                            {
                                Vector2 spinningpoint2 = 0 * Vector2.UnitX;
                                spinningpoint2 = spinningpoint2.RotatedBy((Main.rand.NextDouble() - 0.5) * 0.78539818525314331);
                                spinningpoint2 *= 8f;
                                Projectile.NewProjectile(npc.Center.X, npc.Center.Y, spinningpoint2.X, spinningpoint2.Y, ModContent.ProjectileType<NovaRocket>(), npc.damage / 3, 0f, Main.myPlayer, 0f, 20f);
                                if (Shoot > 60)
                                {
                                    Shoot = 0;
                                    npc.netUpdate = true;
                                }
                            }
                            Main.PlaySound(SoundID.Item39, npc.Center);
                        }
                    }
                }
            }
        }

        private void AIChange()
        {
            if (Main.netMode != NetmodeID.MultiplayerClient)
            {
                npc.localAI[0] = 0;
                npc.ai[0] = Main.rand.Next(7); 
                if (CUtils.AnyProjectiles(ModContent.ProjectileType<NovaTurretProj>()) || CUtils.AnyProjectiles(ModContent.ProjectileType<NovaTurret>()))
                {
                    if (npc.ai[0] == 5)
                    {
                        npc.ai[0] = 4;
                    }
                }
                npc.ai[2] = 0;
                LaserShot = false;
                npc.netUpdate = true;
            }
        }

        private int Repeats()
        {
            if (npc.life < (int)(npc.lifeMax * .25f))
            {
                return 4;
            }
            else if (npc.life < (int)(npc.lifeMax * .5f))
            {
                return 3;
            }
            else if (npc.life < (int)(npc.lifeMax * .75f))
            {
                return 2;
            }
            else
            {
                return 1;
            }
        }

        public int TurretCount()
        {
            if (npc.life < (int)(npc.lifeMax * .25f))
            {
                return 8;
            }
            else if (npc.life < (int)(npc.lifeMax * .5f))
            {
                return 7;
            }
            else if (npc.life < (int)(npc.lifeMax * .75f))
            {
                return 6;
            }
            else
            {
                return 5;
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

        float rotAmt;

        private void LaserAttack()
        {
            rotAmt += .0005f;
            npc.rotation += rotAmt;
            int LaserCount;
            if (npc.life < npc.lifeMax / 4)
            {
                if (rotAmt > .028f)
                {
                    rotAmt = .028f;
                }
                LaserCount = 4;
            }
            else if (npc.life < npc.lifeMax / 2)
            {
                if (rotAmt > .024f)
                {
                    rotAmt = .024f;
                }
                LaserCount = 3;
            }
            else
            {
                if (rotAmt > .02f)
                {
                    rotAmt = .02f;
                }
                LaserCount = 2;
            }
            if ((!CUtils.AnyProjectiles(ModContent.ProjectileType<NovaBeam>()) || !CUtils.AnyProjectiles(ModContent.ProjectileType<NovaBeamSmall>())) && !LaserShot)
            {
                LaserShot = true;
                for (int l = 0; l < LaserCount; l++)
                {
                    float LaserPos = Pi2 / LaserCount;
                    float laserDir = LaserPos * l;
                    Projectile.NewProjectile(npc.Center, (npc.rotation + laserDir).ToRotationVector2(), ModContent.ProjectileType<FlameraySmall>(), npc.damage / 4, 0f, Main.myPlayer, laserDir, npc.whoAmI);
                }
            }
        }

        public override void NPCLoot()
        {
            /*Gore.NewGore(npc.position, npc.velocity * 0.2f, mod.GetGoreSlot("Gores/HeartcoreGore1"), 1f);
            Gore.NewGore(npc.position, npc.velocity * 0.2f, mod.GetGoreSlot("Gores/HeartcoreGore2"), 1f);
            Gore.NewGore(npc.position, npc.velocity * 0.2f, mod.GetGoreSlot("Gores/HeartcoreGore3"), 1f);
            Gore.NewGore(npc.position, npc.velocity * 0.2f, mod.GetGoreSlot("Gores/HeartcoreGore4"), 1f);
            Gore.NewGore(npc.position, npc.velocity * 0.2f, mod.GetGoreSlot("Gores/HeartcoreGoreHalf1"), 1f);
            Gore.NewGore(npc.position, npc.velocity * 0.2f, mod.GetGoreSlot("Gores/HeartcoreGoreHalf2"), 1f);

            for (int num468 = 0; num468 < 12; num468++)
            {
                int num469 = Dust.NewDust(new Vector2(npc.position.X, npc.position.Y), npc.width, npc.height, DustID.SolarFlare, -npc.velocity.X * 0.2f,
                    -npc.velocity.Y * 0.2f, 100, default, 2f);
                Main.dust[num469].noGravity = true;
            }

            if (Main.rand.Next(10) == 0)
            {
                npc.DropLoot(mod.ItemType("HeartcoreTrophy"));
            }
            if (Main.rand.Next(7) == 0)
            {
                npc.DropLoot(mod.ItemType("HeartcoreMask"));
            }

            if (Main.expertMode)
            {
                int n = NPC.NewNPC((int)npc.Center.X, (int)npc.Center.Y, ModContent.NPCType<FurySoulTransition>());
                Main.npc[n].Center = npc.Center;
                return;
            }
            else
            {
                string[] lootTableA = { "Sol", "MeteorShower", "BlazeBuster", "FlamingSoul" };
                int lootA = Main.rand.Next(lootTableA.Length);

                npc.DropLoot(mod.ItemType(lootTableA[lootA]));

                npc.DropLoot(ModContent.ItemType<Items.Boss.Heartcore.HeartSoul>(), Main.rand.Next(8, 12));
                //int n = NPC.NewNPC((int)npc.Center.X, (int)npc.Center.Y, ModContent.NPCType<HeartcoreDefeat>());
                //Main.npc[n].Center = npc.Center;
                CWorld.downedHeartcore = true;
            }*/
        }

        int Frame = 0;
        public override void FindFrame(int frameHeight)
        {
            if (npc.frameCounter++ > 5)
            {
                npc.frameCounter = 0;
                Frame++;
                if (Frame > 7)
                {
                    Frame = 0;
                }
            }
            npc.frame.Y = Frame * frameHeight;
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Color drawColor)
        {
            Texture2D BladeTex = mod.GetTexture("NPCs/Bosses/Novacore/NovacoreBack");
            Texture2D BladeGlowTex = mod.GetTexture("Glowmasks/NovacoreBack_Glow");

            Texture2D texture2D13 = Main.npcTexture[npc.type];
            Texture2D Glow = mod.GetTexture("Glowmasks/Novacore_Glow");

            Rectangle Bladeframe = BaseDrawing.GetFrame(Frame, BladeTex.Width, BladeTex.Height / 8, 0, 0);

            BaseDrawing.DrawTexture(spriteBatch, BladeTex, 0, npc.position, npc.width, npc.height, npc.scale, npc.rotation, npc.spriteDirection, 8, Bladeframe, drawColor, true);
            BaseDrawing.DrawTexture(spriteBatch, BladeGlowTex, 0, npc.position, npc.width, npc.height, npc.scale, npc.rotation, npc.spriteDirection, 8, Bladeframe, Color.White, true);

            BaseDrawing.DrawTexture(spriteBatch, texture2D13, 0, npc.position, npc.width, npc.height, npc.scale, 0, 0, 8, npc.frame, Colors.COLOR_GLOWPULSE, true);
            BaseDrawing.DrawTexture(spriteBatch, Glow, 0, npc.position, npc.width, npc.height, npc.scale, 0, 0, 8, npc.frame, Colors.COLOR_GLOWPULSE, true);

            return false;
        }
    }
}