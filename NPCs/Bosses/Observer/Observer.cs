using Terraria;
using Terraria.ModLoader;
using System;
using Microsoft.Xna.Framework;
using Terraria.ID;
using Microsoft.Xna.Framework.Graphics;
using System.IO;

namespace CSkies.NPCs.Bosses.Observer
{
    [AutoloadBossHead]
    public class Observer : ModNPC
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("The Observer");
            Main.npcFrameCount[npc.type] = 4;
        }

        public override void SetDefaults()
        {
            npc.width = 70;
            npc.height = 136;
            npc.value = BaseUtility.CalcValue(0, 10, 0, 0);
            npc.npcSlots = 1000;
            npc.aiStyle = -1;
            npc.lifeMax = 6000;
            npc.defense = 10;
            npc.damage = 30;
            npc.knockBackResist = 0f;
            npc.noGravity = true;
            npc.HitSound = SoundID.NPCHit1;
            npc.DeathSound = SoundID.NPCDeath1;
            npc.boss = true;
            music = mod.GetSoundSlot(SoundType.Music, "Sounds/Music/Observer");
            npc.alpha = 255;
            npc.noTileCollide = true;
            bossBag = ModContent.ItemType<Items.Boss.ObserverBag>();
            npc.value = Item.sellPrice(0, 1, 0, 0);
        }

        public override void ScaleExpertStats(int numPlayers, float bossLifeScale)
        {
            npc.lifeMax = (int)(npc.lifeMax * 0.6f * bossLifeScale);
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

        public int StarCount = Main.expertMode ? 6 : 4;

        public override void AI()
        {
            npc.TargetClosest();
            if (!Main.dayTime)
            {
                if (npc.alpha <= 0)
                {
                    npc.alpha = 0;
                }
                else
                {
                    Dust.NewDust(npc.position, npc.width, npc.height, 17, npc.velocity.X * 0.2f, npc.velocity.Y * 0.2f, DustID.Electric, Color.White, 1f);
                    npc.alpha -= 3;
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
                    Dust.NewDust(npc.position, npc.width, npc.height, 17, npc.velocity.X * 0.2f, npc.velocity.Y * 0.2f, DustID.Electric, Color.White, 1f);
                    npc.alpha += 3;
                }
            }

            if (internalAI[3] == 0)
            {
                if (Main.netMode != 1)
                {
                    for (int m = 0; m < StarCount; m++)
                    {
                        int projectileID = Projectile.NewProjectile(npc.Center, Vector2.Zero, mod.ProjectileType("Star"), npc.damage, 4, Main.myPlayer);
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

            BaseAI.AISpaceOctopus(npc, ref npc.ai, .2f, 6, 270, 70, null);

            if (Main.netMode != 1)
            {
                if (internalAI[2]++ > 100)
                {
                    FireLaser(npc);
                    internalAI[2] = 0;
                    npc.netUpdate = true;
                }
                if (npc.ai[2]++ == (Main.expertMode ? 501 : 701))
                {
                    internalAI[0] += 1;
                    npc.netUpdate = true;
                }
                if (!CUtils.AnyProjectiles(ModContent.ProjectileType<Star>()))
                {
                    npc.ai[2] = 0;
                    internalAI[0] = 0;
                    internalAI[1] = 0;
                    internalAI[2] = 0;
                    internalAI[3] = 0;
                    npc.netUpdate = true;
                }
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
            int projType = ModContent.ProjectileType<Starbeam>();
            if (internalAI[0] == 0)
            {
                if (Main.expertMode)
                {
                    float spread = 45f * 0.0174f;
                    Vector2 dir = Vector2.Normalize(player.Center - npc.Center);
                    dir *= 14f;
                    float baseSpeed = (float)Math.Sqrt((dir.X * dir.X) + (dir.Y * dir.Y));
                    double startAngle = Math.Atan2(dir.X, dir.Y) - .1d;
                    double deltaAngle = spread / 6f;
                    for (int i = 0; i < 3; i++)
                    {
                        double offsetAngle = startAngle + (deltaAngle * i);
                        Projectile.NewProjectile(npc.Center.X, npc.Center.Y, baseSpeed * (float)Math.Sin(offsetAngle), baseSpeed * (float)Math.Cos(offsetAngle), projType, npc.damage / 2, 5, Main.myPlayer);
                    }
                }
                else
                {
                    BaseAI.FireProjectile(player.position, npc.Center, projType, npc.damage/2, 4, 12, 0, Main.myPlayer);
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
            potionType = ItemID.HealingPotion;
        }

        public override void NPCLoot()
        {
            if (Main.rand.Next(10) == 0)
            {
                npc.DropLoot(mod.ItemType("ObserverTrophy"));
            }
            if (Main.expertMode)
            {
                npc.DropBossBags();
            }
            else
            {
                if (Main.rand.Next(7) == 0)
                {
                    npc.DropLoot(mod.ItemType("ObserverMask"));
                }
                npc.DropLoot(ModContent.ItemType<Items.Comet.CometFragment>(), Main.rand.Next(8, 12));
                string[] lootTable = { "Comet", "CometDagger", "CometFan", "CometJavelin", "CometPortal", "Comet Shot" };
                int loot = Main.rand.Next(lootTable.Length);
                int Drop = mod.ItemType(lootTable[loot]);

                if (Drop == mod.ItemType("CometDagger"))
                {
                    npc.DropLoot(mod.ItemType(lootTable[loot]), Main.rand.Next(25, 151));
                }
                else
                {
                    npc.DropLoot(mod.ItemType(lootTable[loot]));
                }
            }
        }

        public override void HitEffect(int hitDirection, double damage)
        {
            if (Main.netMode == 2) { return; }
            if (npc.life <= 0)
            {
                for (int m = 0; m < 20; m++)
                {
                    int dustID = Dust.NewDust(npc.position, npc.width, npc.height, 17, npc.velocity.X * 0.2f, npc.velocity.Y * 0.2f, 10, Color.White, 1f);
                    Main.dust[dustID].velocity = new Vector2(MathHelper.Lerp(-1f, 1f, (float)Main.rand.NextDouble()), MathHelper.Lerp(-1f, 1f, (float)Main.rand.NextDouble()));
                }
            }
            else
            {
                for (int m = 0; m < 5; m++)
                {
                    Dust.NewDust(npc.position, npc.width, npc.height, 17, npc.velocity.X * 0.2f, npc.velocity.Y * 0.2f, 0, Color.White, 1.1f);
                }
            }
        }

        public override bool PreDraw(SpriteBatch sb, Color dColor)
        {
            Texture2D tex = Main.npcTexture[npc.type];
            BaseDrawing.DrawAfterimage(sb, tex, 0, npc, 2.5f, 1, 3, true, 0f, 0f, npc.GetAlpha(Colors.COLOR_GLOWPULSE));
            BaseDrawing.DrawTexture(sb, tex, 0, npc, npc.GetAlpha(Colors.COLOR_GLOWPULSE));
            return false;
        }
    }
}