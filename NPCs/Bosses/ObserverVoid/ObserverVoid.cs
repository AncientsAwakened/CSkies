using Terraria;
using Terraria.ModLoader;
using System;
using Microsoft.Xna.Framework;
using Terraria.ID;
using Microsoft.Xna.Framework.Graphics;
using System.IO;

namespace CSkies.NPCs.Bosses.ObserverVoid
{
    [AutoloadBossHead]
    public class ObserverVoid : ModNPC
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Observer Void");
            Main.npcFrameCount[npc.type] = 4;
        }

        public override void SetDefaults()
        {
            npc.width = 70;
            npc.height = 136;
            npc.value = BaseUtility.CalcValue(0, 10, 0, 0);
            npc.npcSlots = 1000;
            npc.aiStyle = -1;
            npc.lifeMax = 100000;
            npc.defense = 50;
            npc.damage = 120;
            npc.knockBackResist = 0f;
            npc.noGravity = true;
            npc.HitSound = SoundID.NPCHit1;
            npc.DeathSound = SoundID.NPCDeath1;
            npc.boss = true;
            music = mod.GetSoundSlot(SoundType.Music, "Sounds/Music/ObserverVoid");
            npc.alpha = 255;
            npc.noTileCollide = true;
        }

        public override void ScaleExpertStats(int numPlayers, float bossLifeScale)
        {
            npc.lifeMax = (int)(npc.lifeMax * 0.8f * bossLifeScale);
            npc.defense = (int)(npc.defense * 1.2f);
            npc.damage = (int)(npc.damage * 0.6f);
        }

        public float[] internalAI = new float[4];

        public override void SendExtraAI(BinaryWriter writer)
        {
            base.SendExtraAI(writer);
            if (Main.netMode == NetmodeID.Server || Main.dedServ)
            {
                writer.Write(internalAI[0]);
                writer.Write(internalAI[1]);
                writer.Write(internalAI[2]);
                writer.Write(internalAI[3]);
            }
        }

        public override void ReceiveExtraAI(BinaryReader reader)
        {
            base.ReceiveExtraAI(reader);
            if (Main.netMode == NetmodeID.MultiplayerClient)
            {
                internalAI[0] = reader.ReadFloat();
                internalAI[1] = reader.ReadFloat();
                internalAI[2] = reader.ReadFloat();
                internalAI[3] = reader.ReadFloat();
            }
        }

        public int StarCount = Main.expertMode ? 10 : 8;
        public Vector2 pos;

        public override void AI()
        {
            Lighting.AddLight(npc.Center, 0, 0f, .15f);
            npc.TargetClosest();
            if (!Main.dayTime)
            {
                if (npc.alpha <= 0)
                {
                    npc.alpha = 0;
                }
                else
                {
                    npc.alpha -= 5;
                }
            }
            else
            {
                if (npc.alpha >= 255)
                {
                    npc.active = false;
                }
                else
                {
                    npc.alpha += 5;
                }
            }

            if (internalAI[3] == 0 && npc.ai[3] < 1000)
            {
                if (Main.netMode != 1)
                {
                    for (int m = 0; m < StarCount; m++)
                    {
                        int projectileID = Projectile.NewProjectile(npc.Center, Vector2.Zero, mod.ProjectileType("BlackHole"), npc.damage / 4, 4, Main.myPlayer);
                        Main.projectile[projectileID].Center = npc.Center;
                        Main.projectile[projectileID].velocity = new Vector2(MathHelper.Lerp(-1f, 1f, (float)Main.rand.NextDouble()), MathHelper.Lerp(-1f, 1f, (float)Main.rand.NextDouble()));
                        Main.projectile[projectileID].velocity *= 8f;
                        Main.projectile[projectileID].ai[0] = m;
                    }
                    internalAI[3] = 1;
                    npc.netUpdate = true;
                }
            }
            if (internalAI[1] < 120)
            {
                internalAI[1] += 2;
            }
            else
            {
                internalAI[1] = 120;
                npc.netUpdate = true;
            }

            BaseAI.AISkull(npc, ref npc.ai, true, 11, 350, .05f, .07f);

            if (Main.netMode != 1)
            {
                if (internalAI[2]++ > 200)
                {
                    FireLaser(npc);
                    internalAI[2] = 0;
                    npc.netUpdate = true;
                }
                if (npc.ai[3] == 1200f)
                {
                    npc.netUpdate = true;
                }
                else if (npc.ai[3] < 1200)
                {
                    npc.ai[3]++;
                    npc.netUpdate = true;
                }
                else
                {
                    if (npc.ai[2]++ == (Main.expertMode ? 400 : 500))
                    {
                        internalAI[0] += 1;
                        npc.netUpdate = true;
                    }
                }
                if (!CUtils.AnyProjectiles(mod.ProjectileType<BlackHole>()))
                {
                    npc.ai[2] = 0;
                    internalAI[0] = 0;
                    internalAI[1] = 0;
                    internalAI[2] = 0;
                    internalAI[3] = 0;
                    npc.netUpdate = true;
                }
            }

            if (npc.ai[3] >= 1200f)
            {
                if (npc.ai[3] > (Main.expertMode ? 1500 : 1400))
                {
                    npc.ai[3] = 0;
                    npc.netUpdate = true;
                }
                else
                {
                    npc.ai[3]++;
                }

                npc.ai[2] = 0;
                npc.velocity *= .98f;
                if (VortexScale < 1f)
                {
                    VortexScale += .01f;
                }
                VortexRotation += .01f;

                for (int u = 0; u < Main.maxPlayers; u++)
                {
                    Player target = Main.player[u];

                    if (target.active && Vector2.Distance(npc.Center, target.Center) < 300 * VortexScale)
                    {
                        float num3 = 3f;
                        Vector2 vector = new Vector2(target.position.X + target.width / 4, target.position.Y + target.height / 4);
                        float num4 = npc.Center.X - vector.X;
                        float num5 = npc.Center.Y - vector.Y;
                        float num6 = (float)Math.Sqrt(num4 * num4 + num5 * num5);
                        num6 = num3 / num6;
                        num4 *= num6;
                        num5 *= num6;
                        int num7 = 6;
                        target.velocity.X = (target.velocity.X * (num7 - 1) + num4) / num7;
                        target.velocity.Y = (target.velocity.Y * (num7 - 1) + num5) / num7;
                    }
                }
            }
            else
            {
                if (VortexScale > 0f)
                {
                    VortexScale -= .05f;
                }
                VortexRotation += .05f;
            }

            npc.rotation = 0;

            for (int m = npc.oldPos.Length - 1; m > 0; m--)
            {
                npc.oldPos[m] = npc.oldPos[m - 1];
            }
            npc.oldPos[0] = npc.position;

        }

        public void FireLaser(NPC npc)
        {
            Player player = Main.player[npc.target];
            int projType = mod.ProjectileType<ShadowBlast>();
            if (internalAI[0] == 0)
            {
                if (Main.expertMode)
                {
                    float shots = Main.rand.Next(1, 5);
                    float spread = 45f * 0.0174f;
                    Vector2 dir = Vector2.Normalize(player.Center - npc.Center);
                    dir *= 14f;
                    float baseSpeed = (float)Math.Sqrt((dir.X * dir.X) + (dir.Y * dir.Y));
                    double startAngle = Math.Atan2(dir.X, dir.Y) - .1d;
                    double deltaAngle = spread / (shots * 2);
                    for (int i = 0; i < shots; i++)
                    {
                        double offsetAngle = startAngle + (deltaAngle * i);
                        Projectile.NewProjectile(npc.Center.X, npc.Center.Y, baseSpeed * (float)Math.Sin(offsetAngle), baseSpeed * (float)Math.Cos(offsetAngle), projType, npc.damage / 4, 5, Main.myPlayer);
                    }
                }
                else
                {
                    BaseAI.FireProjectile(player.position, npc.Center, projType, npc.damage/2, 4, 12, 0, Main.myPlayer);
                }
            }
        }

        public override void HitEffect(int hitDirection, double damage)
        {
            if (Main.expertMode)
            {
                if (npc.life < npc.lifeMax / 2)
                {
                    if (Main.rand.Next(7) == 0)
                    {
                        npc.DropLoot(mod.ItemType("ObserverVoidMask"));
                    }
                    if (Main.rand.Next(10) == 0)
                    {
                        npc.DropLoot(mod.ItemType("ObserverVoidTrophy"));
                    }
                    int n = NPC.NewNPC((int)npc.Center.X, (int)npc.Center.Y, mod.NPCType("VoidTransition1"));
                    Main.npc[n].Center = npc.Center;
                    Main.npc[n].velocity = npc.velocity;
                    npc.active = false;
                }
            }
        }

        public override void FindFrame(int frameHeight)
        {
            if (npc.frameCounter++ >= 6)
            {
                npc.frame.Y += frameHeight;
                npc.frameCounter = 0;
            }
            if (npc.frame.Y >= frameHeight * 3)
            {
                npc.frame.Y = 0;
            }
        }

        public override void BossLoot(ref string name, ref int potionType)
        {
            potionType = ItemID.SuperHealingPotion;
        }

        public override void NPCLoot()
        {
            if (Main.rand.Next(7) == 0)
            {
                npc.DropLoot(mod.ItemType("ObserverVoidMask"));
            }
            npc.DropLoot(mod.ItemType<Items.Void.VoidFragment>(), Main.rand.Next(8, 12));
            string[] lootTable = { };
            int loot = Main.rand.Next(lootTable.Length);
            npc.DropLoot(mod.ItemType(lootTable[loot]));
        }

        float VortexScale = 0f;
        float VortexRotation = 0f;
        public float auraPercent = 0f;
        public bool auraDirection = true;

        public override bool PreDraw(SpriteBatch sb, Color dColor)
        {
            Texture2D tex = Main.npcTexture[npc.type];
            Texture2D Cyclone = mod.GetTexture("NPCs/Bosses/ObserverVoid/DarkVortex");
            if (VortexScale > 0)
            {
                Rectangle frame = BaseDrawing.GetFrame(0, Cyclone.Width, Cyclone.Height, 0, 0);
                BaseDrawing.DrawTexture(sb, Cyclone, 0, npc.position, npc.width, npc.height, VortexScale, VortexRotation, npc.direction, 1, frame, Color.White, true);
                if (auraDirection) { auraPercent += 0.1f; auraDirection = auraPercent < 1f; }
                else { auraPercent -= 0.1f; auraDirection = auraPercent <= 0f; }
            }
            else
            {
                if (auraDirection) { auraPercent += 0.1f; auraDirection = auraPercent < 1f; }
                else { auraPercent -= 0.1f; auraDirection = auraPercent <= 0f; }
            }
            BaseDrawing.DrawAura(sb, tex, 0, npc, auraPercent, 2f, 0f, 0f, npc.GetAlpha(Color.White));
            BaseDrawing.DrawTexture(sb, tex, 0, npc, npc.GetAlpha(Color.White));
            return false;
        }
    }
}