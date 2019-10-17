using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.IO;
using Terraria;
using Terraria.Audio;
using Terraria.Graphics.Shaders;
using Terraria.ID;
using Terraria.ModLoader;
using CSkies.NPCs.Bosses.Heartcore;

namespace CSkies.NPCs.Bosses.FurySoul
{
    public class FurySoul : ModNPC
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Fury Soul");
        }
        public override void SetDefaults()
        {
            npc.npcSlots = 100;
            npc.width = 82;
            npc.height = 82;
            npc.aiStyle = -1;
            npc.damage = 120;
            npc.defense = 50;
            npc.lifeMax = 150000;
            npc.value = Item.sellPrice(0, 12, 0, 0);
            npc.HitSound = SoundID.Item20;
            npc.DeathSound = new LegacySoundStyle(1, 124, Terraria.Audio.SoundType.Sound);
            npc.knockBackResist = 0f;
            npc.noGravity = true;
            npc.netAlways = true;
            npc.boss = true;
            npc.noTileCollide = true;
            music = mod.GetSoundSlot(Terraria.ModLoader.SoundType.Music, "Sounds/Music/FurySoul");
            bossBag = mod.ItemType("HeartcoreBag");
        }

        public override void ScaleExpertStats(int numPlayers, float bossLifeScale)
        {
            npc.lifeMax = (int)(npc.lifeMax * 0.5f * bossLifeScale);
            npc.damage = (int)(npc.damage * 0.5f);
        }

        public float[] Movement = new float[4];
        public float[] start = new float[1];
        public float[] Minion = new float[1];

        public override void SendExtraAI(BinaryWriter writer)
        {
            base.SendExtraAI(writer);
            if (Main.netMode == NetmodeID.Server || Main.dedServ)
            {
                writer.Write(Movement[0]);
                writer.Write(Movement[1]);
                writer.Write(Movement[2]);
                writer.Write(Movement[3]);
                writer.Write(start[0]);
                writer.Write(Minion[0]);
            }
        }

        public override void ReceiveExtraAI(BinaryReader reader)
        {
            base.ReceiveExtraAI(reader);
            if (Main.netMode == NetmodeID.MultiplayerClient)
            {
                Movement[0] = reader.ReadFloat();
                Movement[1] = reader.ReadFloat();
                Movement[2] = reader.ReadFloat();
                Movement[3] = reader.ReadFloat();
                start[0] = reader.ReadFloat();
                Minion[0] = reader.ReadFloat();
            }
        }

        public override void NPCLoot()
        {
            if (Main.rand.Next(10) == 0)
            {
                npc.DropLoot(mod.ItemType("FurySoulTrophy"));
            }
            if (Main.expertMode)
            {
                npc.DropBossBags();
            }
            int n = NPC.NewNPC((int)npc.Center.X, (int)npc.Center.Y, ModContent.NPCType<FurySoulDeath>());
            Main.npc[n].Center = npc.Center;
            for (int num468 = 0; num468 < 12; num468++)
            {
                int num469 = Dust.NewDust(new Vector2(npc.position.X, npc.position.Y), npc.width, npc.height, DustID.SolarFlare, -npc.velocity.X * 0.2f,
                    -npc.velocity.Y * 0.2f, 100, default, 2f);
                Main.dust[num469].noGravity = true;
            }
        }

        public static Color Flame => BaseUtility.MultiLerpColor(Main.LocalPlayer.miscCounter % 100 / 100f, Color.Orange, Color.Red, Color.Orange);

        float scale = 0;

        float Changerate;

        bool rage = false;

        public override void AI()
        {
            if (npc.life < npc.lifeMax / 4)
            {
                if (!rage)
                {
                    rage = true;
                    Main.NewText("The soul begins to lash out in a fit of fiery rage.", new Color(253, 62, 3));
                }
                music = mod.GetSoundSlot(Terraria.ModLoader.SoundType.Music, "Sounds/Music/Pinch");
            }

            if (npc.life < npc.lifeMax / 2 && NPC.CountNPCS(ModContent.NPCType<FuryMinion>()) < 2)
            {
                Minion[0]++;
                if (Minion[0] > 360)
                {
                    Minion[0] = 0;
                    int a = NPC.NewNPC((int)npc.position.X, (int)npc.position.Y, ModContent.NPCType<FuryMinion>());
                    Main.npc[a].Center = npc.Center + new Vector2(200, 0);
                    Main.PlaySound(SoundID.Item14, Main.npc[a].Center);
                    int num290 = Main.rand.Next(3, 7);
                    for (int num291 = 0; num291 < num290; num291++)
                    {
                        int num292 = Dust.NewDust(Main.npc[a].position, Main.npc[a].width, Main.npc[a].height, DustID.Fire, 0f, 0f, 100, default, 2.1f);
                        Main.dust[num292].velocity *= 2f;
                        Main.dust[num292].noGravity = true;
                    }
                    int b = NPC.NewNPC((int)npc.position.X, (int)npc.position.Y, ModContent.NPCType<FuryMinion>());
                    Main.npc[b].Center = npc.Center + new Vector2(-200, 0);
                    Main.PlaySound(SoundID.Item14, Main.npc[b].Center);
                    num290 = Main.rand.Next(3, 7);
                    for (int num291 = 0; num291 < num290; num291++)
                    {
                        int num292 = Dust.NewDust(Main.npc[b].position, Main.npc[b].width, Main.npc[b].height, DustID.Fire, 0f, 0f, 100, default, 2.1f);
                        Main.dust[num292].velocity *= 2f;
                        Main.dust[num292].noGravity = true;
                    }
                    int c = NPC.NewNPC((int)npc.position.X, (int)npc.position.Y, ModContent.NPCType<FuryMinion>());
                    Main.npc[c].Center = npc.Center + new Vector2(0, 200);
                    Main.PlaySound(SoundID.Item14, Main.npc[c].Center);
                    num290 = Main.rand.Next(3, 7);
                    for (int num291 = 0; num291 < num290; num291++)
                    {
                        int num292 = Dust.NewDust(Main.npc[c].position, Main.npc[c].width, Main.npc[c].height, DustID.Fire, 0f, 0f, 100, default, 2.1f);
                        Main.dust[num292].velocity *= 2f;
                        Main.dust[num292].noGravity = true;
                    }
                    int d = NPC.NewNPC((int)npc.position.X, (int)npc.position.Y, ModContent.NPCType<FuryMinion>());
                    Main.npc[d].Center = npc.Center + new Vector2(0, -200);
                    Main.PlaySound(SoundID.Item14, Main.npc[d].Center);
                    num290 = Main.rand.Next(3, 7);
                    for (int num291 = 0; num291 < num290; num291++)
                    {
                        int num292 = Dust.NewDust(Main.npc[d].position, Main.npc[d].width, Main.npc[d].height, DustID.Fire, 0f, 0f, 100, default, 2.1f);
                        Main.dust[num292].velocity *= 2f;
                        Main.dust[num292].noGravity = true;
                    }
                }
            }

            Changerate = npc.life < npc.lifeMax / 2 ? 150 : 120;
            Lighting.AddLight(npc.Center, Flame.R / 150, Flame.G / 150, Flame.B / 150);

            if (!npc.HasPlayerTarget)
            {
                npc.TargetClosest();
            }

            if (npc.ai[0] == 10)
            {
                npc.velocity *= 0;
                if (start[0]++ > 60)
                {
                    AIChange();
                }
            }

            if (scale >= 1f)
            {
                scale = 1f;
            }
            else
            {
                scale += .02f;
            }

            Player player = Main.player[npc.target];

            if (Vector2.Distance(player.Center, npc.Center) < 204 && !player.dead && player.active)
            {
                player.AddBuff(ModContent.BuffType<Buffs.Heartburn>(), 10);
            }

            if (npc.ai[0] == 2 || npc.ai[0] == 4)
            {
                npc.velocity *= .0f;
            }
            else
            {
                BaseAI.AISkull(npc, ref Movement, true, 14, 350, .04f, .05f);
            }

            if (npc.ai[2]++ > Changerate)
            {
                npc.rotation += .06f;

                switch (npc.ai[0])
                {
                    case 0:
                        float spread = 45f * 0.0174f;
                        Vector2 dir = Vector2.Normalize(player.Center - npc.Center);
                        dir *= 12f;
                        float baseSpeed = (float)Math.Sqrt((dir.X * dir.X) + (dir.Y * dir.Y));
                        double startAngle = Math.Atan2(dir.X, dir.Y) - .1d;
                        double deltaAngle = spread / 6f;
                        for (int i = 0; i < 3; i++)
                        {
                            if (npc.ai[2] % Main.rand.Next(10) == 0 && Main.rand.Next(2) == 0)
                            {
                                double offsetAngle = startAngle + (deltaAngle * i);
                                Projectile.NewProjectile(npc.Center.X, npc.Center.Y, baseSpeed * (float)Math.Sin(offsetAngle), baseSpeed * (float)Math.Cos(offsetAngle), ModContent.ProjectileType<Fireshot>(), npc.damage / 4, 5, Main.myPlayer);
                            }
                        }
                        if (npc.ai[2] > 271)
                        {
                            AIChange();
                        }
                        break;
                    case 1:
                        float spread1 = 12f * 0.0174f;
                        double startAngle1 = Math.Atan2(npc.velocity.X, npc.velocity.Y) - spread1 / 2;
                        double deltaAngle1 = spread1 / 10f;
                        if (npc.ai[2] % 30 == 0)
                        {
                            Main.PlaySound(mod.GetLegacySoundSlot(Terraria.ModLoader.SoundType.Custom, "Sounds/Sounds/FireCast"), npc.position);
                            for (int i = 0; i < 10; i++)
                            {
                                double offsetAngle1 = (startAngle1 + deltaAngle1 * (i + i * i) / 2f) + 32f * i;
                                Projectile.NewProjectile(npc.Center.X, npc.Center.Y, (float)(Math.Sin(offsetAngle1) * 8f), (float)(Math.Cos(offsetAngle1) * 8f), ModContent.ProjectileType<Fireshot>(), npc.damage / 3, 6);
                                Projectile.NewProjectile(npc.Center.X, npc.Center.Y, (float)(-Math.Sin(offsetAngle1) * 8f), (float)(-Math.Cos(offsetAngle1) * 8f), ModContent.ProjectileType<Fireshot>(), npc.damage / 3, 6);
                            }
                        }
                        if (npc.ai[2] > 240)
                        {
                            AIChange();
                        }
                        break;
                    case 2:
                        if (npc.life < npc.lifeMax / 4)
                        {
                            LaserAttack3();
                        }
                        else if (npc.life < npc.lifeMax / 2)
                        {
                            LaserAttack2();
                        }
                        else
                        {
                            LaserAttack();
                        }
                        break;
                    case 3:
                        if (npc.life < npc.lifeMax / 4 && npc.ai[2] % 30 == 0)
                        {
                            Main.PlaySound(mod.GetLegacySoundSlot(Terraria.ModLoader.SoundType.Custom, "Sounds/Sounds/FireCast"), npc.position);
                            Projectile.NewProjectile(npc.Center, new Vector2(10, 10), ModContent.ProjectileType<Flamewave>(), npc.damage / 3, 0f, Main.myPlayer, 0, npc.whoAmI);
                            Projectile.NewProjectile(npc.Center, new Vector2(-10, 10), ModContent.ProjectileType<Flamewave>(), npc.damage / 3, 0f, Main.myPlayer, 0, npc.whoAmI);
                            Projectile.NewProjectile(npc.Center, new Vector2(10, -10), ModContent.ProjectileType<Flamewave>(), npc.damage / 3, 0f, Main.myPlayer, 0, npc.whoAmI);
                            Projectile.NewProjectile(npc.Center, new Vector2(-10, -10), ModContent.ProjectileType<Flamewave>(), npc.damage / 3, 0f, Main.myPlayer, 0, npc.whoAmI);
                            Projectile.NewProjectile(npc.Center, new Vector2(10, 0), ModContent.ProjectileType<Flamewave>(), npc.damage / 3, 0f, Main.myPlayer, 0, npc.whoAmI);
                            Projectile.NewProjectile(npc.Center, new Vector2(-10, 0), ModContent.ProjectileType<Flamewave>(), npc.damage / 3, 0f, Main.myPlayer, 0, npc.whoAmI);
                            Projectile.NewProjectile(npc.Center, new Vector2(0, -10), ModContent.ProjectileType<Flamewave>(), npc.damage / 3, 0f, Main.myPlayer, 0, npc.whoAmI);
                            Projectile.NewProjectile(npc.Center, new Vector2(0, 10), ModContent.ProjectileType<Flamewave>(), npc.damage / 3, 0f, Main.myPlayer, 0, npc.whoAmI);
                            Projectile.NewProjectile(npc.Center, new Vector2(5, 10), ModContent.ProjectileType<Flamewave>(), npc.damage / 3, 0f, Main.myPlayer, 0, npc.whoAmI);
                            Projectile.NewProjectile(npc.Center, new Vector2(-5, 10), ModContent.ProjectileType<Flamewave>(), npc.damage / 3, 0f, Main.myPlayer, 0, npc.whoAmI);
                            Projectile.NewProjectile(npc.Center, new Vector2(5, -10), ModContent.ProjectileType<Flamewave>(), npc.damage / 3, 0f, Main.myPlayer, 0, npc.whoAmI);
                            Projectile.NewProjectile(npc.Center, new Vector2(-5, -10), ModContent.ProjectileType<Flamewave>(), npc.damage / 3, 0f, Main.myPlayer, 0, npc.whoAmI);
                            Projectile.NewProjectile(npc.Center, new Vector2(10, 5), ModContent.ProjectileType<Flamewave>(), npc.damage / 3, 0f, Main.myPlayer, 0, npc.whoAmI);
                            Projectile.NewProjectile(npc.Center, new Vector2(-10, 5), ModContent.ProjectileType<Flamewave>(), npc.damage / 3, 0f, Main.myPlayer, 0, npc.whoAmI);
                            Projectile.NewProjectile(npc.Center, new Vector2(10, -5), ModContent.ProjectileType<Flamewave>(), npc.damage / 3, 0f, Main.myPlayer, 0, npc.whoAmI);
                            Projectile.NewProjectile(npc.Center, new Vector2(-10, -5), ModContent.ProjectileType<Flamewave>(), npc.damage / 3, 0f, Main.myPlayer, 0, npc.whoAmI);
                        }
                        else if (npc.ai[2] % 30 == 0)
                        {
                            Main.PlaySound(mod.GetLegacySoundSlot(Terraria.ModLoader.SoundType.Custom, "Sounds/Sounds/FireCast"), npc.position);
                            Projectile.NewProjectile(npc.Center, new Vector2(12, 12), ModContent.ProjectileType<Flamewave>(), npc.damage / 3, 0f, Main.myPlayer, 0, npc.whoAmI);
                            Projectile.NewProjectile(npc.Center, new Vector2(-12, 12), ModContent.ProjectileType<Flamewave>(), npc.damage / 3, 0f, Main.myPlayer, 0, npc.whoAmI);
                            Projectile.NewProjectile(npc.Center, new Vector2(12, -12), ModContent.ProjectileType<Flamewave>(), npc.damage / 3, 0f, Main.myPlayer, 0, npc.whoAmI);
                            Projectile.NewProjectile(npc.Center, new Vector2(-12, -12), ModContent.ProjectileType<Flamewave>(), npc.damage / 3, 0f, Main.myPlayer, 0, npc.whoAmI);
                            Projectile.NewProjectile(npc.Center, new Vector2(12, 0), ModContent.ProjectileType<Flamewave>(), npc.damage / 3, 0f, Main.myPlayer, 0, npc.whoAmI);
                            Projectile.NewProjectile(npc.Center, new Vector2(-12, 0), ModContent.ProjectileType<Flamewave>(), npc.damage / 3, 0f, Main.myPlayer, 0, npc.whoAmI);
                            Projectile.NewProjectile(npc.Center, new Vector2(0, -12), ModContent.ProjectileType<Flamewave>(), npc.damage / 3, 0f, Main.myPlayer, 0, npc.whoAmI);
                            Projectile.NewProjectile(npc.Center, new Vector2(0, 12), ModContent.ProjectileType<Flamewave>(), npc.damage / 3, 0f, Main.myPlayer, 0, npc.whoAmI);
                        }
                        else if (npc.ai[2] % 16 == 0)
                        {
                            Main.PlaySound(mod.GetLegacySoundSlot(Terraria.ModLoader.SoundType.Custom, "Sounds/Sounds/FireCast"), npc.position);
                            Projectile.NewProjectile(npc.Center, new Vector2(6, 12), ModContent.ProjectileType<Flamewave>(), npc.damage / 3, 0f, Main.myPlayer, 0, npc.whoAmI);
                            Projectile.NewProjectile(npc.Center, new Vector2(-6, 12), ModContent.ProjectileType<Flamewave>(), npc.damage / 3, 0f, Main.myPlayer, 0, npc.whoAmI);
                            Projectile.NewProjectile(npc.Center, new Vector2(6, -12), ModContent.ProjectileType<Flamewave>(), npc.damage / 3, 0f, Main.myPlayer, 0, npc.whoAmI);
                            Projectile.NewProjectile(npc.Center, new Vector2(-6, -12), ModContent.ProjectileType<Flamewave>(), npc.damage / 3, 0f, Main.myPlayer, 0, npc.whoAmI);
                            Projectile.NewProjectile(npc.Center, new Vector2(12, 6), ModContent.ProjectileType<Flamewave>(), npc.damage / 3, 0f, Main.myPlayer, 0, npc.whoAmI);
                            Projectile.NewProjectile(npc.Center, new Vector2(-12, 6), ModContent.ProjectileType<Flamewave>(), npc.damage / 3, 0f, Main.myPlayer, 0, npc.whoAmI);
                            Projectile.NewProjectile(npc.Center, new Vector2(12, -6), ModContent.ProjectileType<Flamewave>(), npc.damage / 3, 0f, Main.myPlayer, 0, npc.whoAmI);
                            Projectile.NewProjectile(npc.Center, new Vector2(-12, -6), ModContent.ProjectileType<Flamewave>(), npc.damage / 3, 0f, Main.myPlayer, 0, npc.whoAmI);
                        }
                        if (npc.ai[2] > 240)
                        {
                            AIChange();
                        }
                        break;
                    case 4:
                        if (npc.ai[2] == 180)
                        {
                            Teleport();
                            Main.PlaySound(mod.GetLegacySoundSlot(Terraria.ModLoader.SoundType.Custom, "Sounds/Sounds/FireCast"), npc.position);
                            Projectile.NewProjectile(npc.Center, new Vector2(12, 12), ModContent.ProjectileType<Furyrang>(), npc.damage / 3, 0f, Main.myPlayer, 0, npc.whoAmI);
                            Projectile.NewProjectile(npc.Center, new Vector2(-12, 12), ModContent.ProjectileType<Furyrang>(), npc.damage / 3, 0f, Main.myPlayer, 0, npc.whoAmI);
                            Projectile.NewProjectile(npc.Center, new Vector2(12, -12), ModContent.ProjectileType<Furyrang>(), npc.damage / 3, 0f, Main.myPlayer, 0, npc.whoAmI);
                            Projectile.NewProjectile(npc.Center, new Vector2(-12, -12), ModContent.ProjectileType<Furyrang>(), npc.damage / 3, 0f, Main.myPlayer, 0, npc.whoAmI);
                        }
                        if (npc.ai[2] == 210)
                        {
                            Teleport();
                            Main.PlaySound(mod.GetLegacySoundSlot(Terraria.ModLoader.SoundType.Custom, "Sounds/Sounds/FireCast"), npc.position);
                            Projectile.NewProjectile(npc.Center, new Vector2(12, 0), ModContent.ProjectileType<Furyrang>(), npc.damage / 3, 0f, Main.myPlayer, 0, npc.whoAmI);
                            Projectile.NewProjectile(npc.Center, new Vector2(-12, 0), ModContent.ProjectileType<Furyrang>(), npc.damage / 3, 0f, Main.myPlayer, 0, npc.whoAmI);
                            Projectile.NewProjectile(npc.Center, new Vector2(0, 12), ModContent.ProjectileType<Furyrang>(), npc.damage / 3, 0f, Main.myPlayer, 0, npc.whoAmI);
                            Projectile.NewProjectile(npc.Center, new Vector2(0, -12), ModContent.ProjectileType<Furyrang>(), npc.damage / 3, 0f, Main.myPlayer, 0, npc.whoAmI);
                        }
                        if (npc.ai[2] == 240)
                        {
                            Teleport();
                            Main.PlaySound(mod.GetLegacySoundSlot(Terraria.ModLoader.SoundType.Custom, "Sounds/Sounds/FireCast"), npc.position);
                            Projectile.NewProjectile(npc.Center, new Vector2(12, 12), ModContent.ProjectileType<Furyrang>(), npc.damage / 3, 0f, Main.myPlayer, 0, npc.whoAmI);
                            Projectile.NewProjectile(npc.Center, new Vector2(-12, 12), ModContent.ProjectileType<Furyrang>(), npc.damage / 3, 0f, Main.myPlayer, 0, npc.whoAmI);
                            Projectile.NewProjectile(npc.Center, new Vector2(12, -12), ModContent.ProjectileType<Furyrang>(), npc.damage / 3, 0f, Main.myPlayer, 0, npc.whoAmI);
                            Projectile.NewProjectile(npc.Center, new Vector2(-12, -12), ModContent.ProjectileType<Furyrang>(), npc.damage / 3, 0f, Main.myPlayer, 0, npc.whoAmI);
                        }
                        if (npc.ai[2] == 270)
                        {
                            Teleport();
                            Main.PlaySound(mod.GetLegacySoundSlot(Terraria.ModLoader.SoundType.Custom, "Sounds/Sounds/FireCast"), npc.position);
                            Projectile.NewProjectile(npc.Center, new Vector2(12, 0), ModContent.ProjectileType<Furyrang>(), npc.damage / 3, 0f, Main.myPlayer, 0, npc.whoAmI);
                            Projectile.NewProjectile(npc.Center, new Vector2(-12, 0), ModContent.ProjectileType<Furyrang>(), npc.damage / 3, 0f, Main.myPlayer, 0, npc.whoAmI);
                            Projectile.NewProjectile(npc.Center, new Vector2(0, 12), ModContent.ProjectileType<Furyrang>(), npc.damage / 3, 0f, Main.myPlayer, 0, npc.whoAmI);
                            Projectile.NewProjectile(npc.Center, new Vector2(0, -12), ModContent.ProjectileType<Furyrang>(), npc.damage / 3, 0f, Main.myPlayer, 0, npc.whoAmI);
                        }
                        if (npc.ai[2] == 300)
                        {
                            Teleport();
                            Main.PlaySound(mod.GetLegacySoundSlot(Terraria.ModLoader.SoundType.Custom, "Sounds/Sounds/FireCast"), npc.position);
                            Projectile.NewProjectile(npc.Center, new Vector2(12, 0), ModContent.ProjectileType<Furyrang>(), npc.damage / 3, 0f, Main.myPlayer, 0, npc.whoAmI);
                            Projectile.NewProjectile(npc.Center, new Vector2(-12, 0), ModContent.ProjectileType<Furyrang>(), npc.damage / 3, 0f, Main.myPlayer, 0, npc.whoAmI);
                            Projectile.NewProjectile(npc.Center, new Vector2(0, 12), ModContent.ProjectileType<Furyrang>(), npc.damage / 3, 0f, Main.myPlayer, 0, npc.whoAmI);
                            Projectile.NewProjectile(npc.Center, new Vector2(0, -12), ModContent.ProjectileType<Furyrang>(), npc.damage / 3, 0f, Main.myPlayer, 0, npc.whoAmI);
                            Projectile.NewProjectile(npc.Center, new Vector2(12, 12), ModContent.ProjectileType<Furyrang>(), npc.damage / 3, 0f, Main.myPlayer, 0, npc.whoAmI);
                            Projectile.NewProjectile(npc.Center, new Vector2(-12, 12), ModContent.ProjectileType<Furyrang>(), npc.damage / 3, 0f, Main.myPlayer, 0, npc.whoAmI);
                            Projectile.NewProjectile(npc.Center, new Vector2(12, -12), ModContent.ProjectileType<Furyrang>(), npc.damage / 3, 0f, Main.myPlayer, 0, npc.whoAmI);
                            Projectile.NewProjectile(npc.Center, new Vector2(-12, -12), ModContent.ProjectileType<Furyrang>(), npc.damage / 3, 0f, Main.myPlayer, 0, npc.whoAmI);
                        }

                        if (npc.ai[2] <= 300 && !CUtils.AnyProjectiles(ModContent.ProjectileType<Furyrang>()))
                        {
                            AIChange();
                        }
                        break;
                    default:
                        AIChange();
                        break;
                }
            }
            else
            {
                npc.rotation -= .03f;
                int Frequency = Main.rand.Next(30, 50);
                if (npc.life < npc.lifeMax / 2)
                {
                    Frequency = Main.rand.Next(20, 50);
                }
                if (npc.life < npc.lifeMax / 4)
                {
                    Frequency = Main.rand.Next(10, 40);
                }
                if (Main.rand.Next(2) == 0)
                {
                    BaseAI.ShootPeriodic(npc, player.position, player.width, player.height, ModContent.ProjectileType<BigHeartshot>(), ref npc.ai[3], Frequency, npc.damage / 3, 10, true);
                }
                else
                {
                    BaseAI.ShootPeriodic(npc, player.position, player.width, player.height, ModContent.ProjectileType<Meteor>(), ref npc.ai[3], Frequency, npc.damage / 3, 10, true);
                }
            }
        }

        private void AIChange()
        {
            if (Main.netMode != 1)
            {
                npc.ai[0] = Main.rand.Next(5);
                npc.ai[1] = 0;
                npc.ai[2] = 0;
                if (npc.ai[0] == 2 || npc.ai[0] == 4)
                {
                    Teleport();
                }
                else if ((npc.life < npc.lifeMax * (3 / 4)) && Main.rand.Next(3) == 0)
                {
                    Teleport();
                }
                else if ((npc.life < npc.lifeMax / 2) && Main.rand.Next(2) == 0)
                {
                    Teleport();
                }
                if (npc.life < npc.lifeMax / 4)
                {
                    Teleport();
                }
                npc.netUpdate = true;
            }
        }


        public void Teleport()
        {
            scale = 0;
            Player player = Main.player[npc.target];
            Vector2 targetPos = player.Center;
            int posX = Main.rand.Next(3);
            switch (posX)
            {
                case 0:
                    posX = -300;
                    break;
                case 1:
                    posX = 0;
                    break;
                case 2:
                    posX = 300;
                    break;
            }
            int posY = Main.rand.Next(posX == 0 ? 2 : 1);
            switch (posY)
            {
                case 0:
                    posY = -300;
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
                int num88 = Dust.NewDust(position, num84, height3, DustID.Fire, 0f, 0f, 50, default, 3.7f);
                Main.dust[num88].position = npc.Center + (Vector2.UnitY.RotatedByRandom(3.1415927410125732) * (float)Main.rand.NextDouble() * num84 / 2f);
                Main.dust[num88].noGravity = true;
                Main.dust[num88].noLight = true;
                Main.dust[num88].velocity *= 3f;
                Main.dust[num88].velocity += npc.DirectionTo(Main.dust[num88].position) * (2f + (Main.rand.NextFloat() * 4f));
                num88 = Dust.NewDust(position, num84, height3, DustID.Fire, 0f, 0f, 25, default, 1.5f);
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
                int num90 = Dust.NewDust(position, num84, height3, DustID.Fire, 0f, 0f, 0, default, 2.7f);
                Main.dust[num90].position = npc.Center + (Vector2.UnitX.RotatedByRandom(3.1415927410125732).RotatedBy(npc.velocity.ToRotation(), default) * num84 / 2f);
                Main.dust[num90].noGravity = true;
                Main.dust[num90].noLight = true;
                Main.dust[num90].velocity *= 3f;
                Main.dust[num90].velocity += npc.DirectionTo(Main.dust[num90].position) * 2f;
            }
            for (int num91 = 0; num91 < 30; num91++)
            {
                int num92 = Dust.NewDust(position, num84, height3, DustID.Fire, 0f, 0f, 0, default, 1.5f);
                Main.dust[num92].position = npc.Center + (Vector2.UnitX.RotatedByRandom(3.1415927410125732).RotatedBy(npc.velocity.ToRotation(), default) * num84 / 2f);
                Main.dust[num92].noGravity = true;
                Main.dust[num92].velocity *= 3f;
                Main.dust[num92].velocity += npc.DirectionTo(Main.dust[num92].position) * 3f;
            }

        }

        private void LaserAttack()
        {
            if (npc.ai[2] == Changerate + 10)
            {
                Main.PlaySound(mod.GetLegacySoundSlot(Terraria.ModLoader.SoundType.Custom, "Sounds/Sounds/ArcaneCast"), npc.position);
                Projectile.NewProjectile(npc.Center, new Vector2(10, 10), ModContent.ProjectileType<Flameray>(), npc.damage / 3, 0f, Main.myPlayer, 0, npc.whoAmI);
            }
            if (npc.ai[2] == Changerate + 20)
            {
                Main.PlaySound(mod.GetLegacySoundSlot(Terraria.ModLoader.SoundType.Custom, "Sounds/Sounds/ArcaneCast"), npc.position);
                Projectile.NewProjectile(npc.Center, new Vector2(10, 5), ModContent.ProjectileType<Flameray>(), npc.damage / 3, 0f, Main.myPlayer, 0, npc.whoAmI);
            }
            if (npc.ai[2] == Changerate + 30)
            {
                Main.PlaySound(mod.GetLegacySoundSlot(Terraria.ModLoader.SoundType.Custom, "Sounds/Sounds/ArcaneCast"), npc.position);
                Projectile.NewProjectile(npc.Center, new Vector2(10, 0), ModContent.ProjectileType<Flameray>(), npc.damage / 3, 0f, Main.myPlayer, 0, npc.whoAmI);
            }
            if (npc.ai[2] == Changerate + 40)
            {
                Main.PlaySound(mod.GetLegacySoundSlot(Terraria.ModLoader.SoundType.Custom, "Sounds/Sounds/ArcaneCast"), npc.position);
                Projectile.NewProjectile(npc.Center, new Vector2(10, -5), ModContent.ProjectileType<Flameray>(), npc.damage / 3, 0f, Main.myPlayer, 0, npc.whoAmI);
            }
            if (npc.ai[2] == Changerate + 50)
            {
                Main.PlaySound(mod.GetLegacySoundSlot(Terraria.ModLoader.SoundType.Custom, "Sounds/Sounds/ArcaneCast"), npc.position);
                Projectile.NewProjectile(npc.Center, new Vector2(10, -10), ModContent.ProjectileType<Flameray>(), npc.damage / 3, 0f, Main.myPlayer, 0, npc.whoAmI);
            }
            if (npc.ai[2] == Changerate + 60)
            {
                Main.PlaySound(mod.GetLegacySoundSlot(Terraria.ModLoader.SoundType.Custom, "Sounds/Sounds/ArcaneCast"), npc.position);
                Projectile.NewProjectile(npc.Center, new Vector2(5, -10), ModContent.ProjectileType<Flameray>(), npc.damage / 3, 0f, Main.myPlayer, 0, npc.whoAmI);
            }
            if (npc.ai[2] == Changerate + 70)
            {
                Main.PlaySound(mod.GetLegacySoundSlot(Terraria.ModLoader.SoundType.Custom, "Sounds/Sounds/ArcaneCast"), npc.position);
                Projectile.NewProjectile(npc.Center, new Vector2(0, -10), ModContent.ProjectileType<Flameray>(), npc.damage / 3, 0f, Main.myPlayer, 0, npc.whoAmI);
            }
            if (npc.ai[2] == Changerate + 80)
            {
                Main.PlaySound(mod.GetLegacySoundSlot(Terraria.ModLoader.SoundType.Custom, "Sounds/Sounds/ArcaneCast"), npc.position);
                Projectile.NewProjectile(npc.Center, new Vector2(-5, -10), ModContent.ProjectileType<Flameray>(), npc.damage / 3, 0f, Main.myPlayer, 0, npc.whoAmI);
            }
            if (npc.ai[2] == Changerate + 90)
            {
                Main.PlaySound(mod.GetLegacySoundSlot(Terraria.ModLoader.SoundType.Custom, "Sounds/Sounds/ArcaneCast"), npc.position);
                Projectile.NewProjectile(npc.Center, new Vector2(-10, -10), ModContent.ProjectileType<Flameray>(), npc.damage / 3, 0f, Main.myPlayer, 0, npc.whoAmI);
            }
            if (npc.ai[2] == Changerate + 100)
            {
                Main.PlaySound(mod.GetLegacySoundSlot(Terraria.ModLoader.SoundType.Custom, "Sounds/Sounds/ArcaneCast"), npc.position);
                Projectile.NewProjectile(npc.Center, new Vector2(-10, -5), ModContent.ProjectileType<Flameray>(), npc.damage / 3, 0f, Main.myPlayer, 0, npc.whoAmI);
            }
            if (npc.ai[2] == Changerate + 110)
            {
                Main.PlaySound(mod.GetLegacySoundSlot(Terraria.ModLoader.SoundType.Custom, "Sounds/Sounds/ArcaneCast"), npc.position);
                Projectile.NewProjectile(npc.Center, new Vector2(-10, 0), ModContent.ProjectileType<Flameray>(), npc.damage / 3, 0f, Main.myPlayer, 0, npc.whoAmI);
            }
            if (npc.ai[2] == Changerate + 120)
            {
                Main.PlaySound(mod.GetLegacySoundSlot(Terraria.ModLoader.SoundType.Custom, "Sounds/Sounds/ArcaneCast"), npc.position);
                Projectile.NewProjectile(npc.Center, new Vector2(-10, 5), ModContent.ProjectileType<Flameray>(), npc.damage / 3, 0f, Main.myPlayer, 0, npc.whoAmI);
            }
            if (npc.ai[2] == Changerate + 130)
            {
                Main.PlaySound(mod.GetLegacySoundSlot(Terraria.ModLoader.SoundType.Custom, "Sounds/Sounds/ArcaneCast"), npc.position);
                Projectile.NewProjectile(npc.Center, new Vector2(-10, 10), ModContent.ProjectileType<Flameray>(), npc.damage / 3, 0f, Main.myPlayer, 0, npc.whoAmI);
            }
            if (npc.ai[2] == Changerate + 140)
            {
                Main.PlaySound(mod.GetLegacySoundSlot(Terraria.ModLoader.SoundType.Custom, "Sounds/Sounds/ArcaneCast"), npc.position);
                Projectile.NewProjectile(npc.Center, new Vector2(-5, 10), ModContent.ProjectileType<Flameray>(), npc.damage / 3, 0f, Main.myPlayer, 0, npc.whoAmI);
            }
            if (npc.ai[2] == Changerate + 150)
            {
                Main.PlaySound(mod.GetLegacySoundSlot(Terraria.ModLoader.SoundType.Custom, "Sounds/Sounds/ArcaneCast"), npc.position);
                Projectile.NewProjectile(npc.Center, new Vector2(0, 10), ModContent.ProjectileType<Flameray>(), npc.damage / 3, 0f, Main.myPlayer, 0, npc.whoAmI);
            }
            if (npc.ai[2] == Changerate + 160)
            {
                Main.PlaySound(mod.GetLegacySoundSlot(Terraria.ModLoader.SoundType.Custom, "Sounds/Sounds/ArcaneCast"), npc.position);
                Projectile.NewProjectile(npc.Center, new Vector2(5, 10), ModContent.ProjectileType<Flameray>(), npc.damage / 3, 0f, Main.myPlayer, 0, npc.whoAmI);
            }
            if (npc.ai[2] > Changerate + 160 && !CUtils.AnyProjectiles(ModContent.ProjectileType<Flameray>()))
            {
                AIChange();
            }
        }

        private void LaserAttack2()
        {
            if (npc.ai[2] == Changerate + 15)
            {
                Main.PlaySound(mod.GetLegacySoundSlot(Terraria.ModLoader.SoundType.Custom, "Sounds/Sounds/ArcaneCast"), npc.position);
                Projectile.NewProjectile(npc.Center, new Vector2(10, 10), ModContent.ProjectileType<Flameray>(), npc.damage / 3, 0f, Main.myPlayer, 0, npc.whoAmI);
                Projectile.NewProjectile(npc.Center, new Vector2(-10, -10), ModContent.ProjectileType<Flameray>(), npc.damage / 3, 0f, Main.myPlayer, 0, npc.whoAmI);
            }
            if (npc.ai[2] == Changerate + 30)
            {
                Main.PlaySound(mod.GetLegacySoundSlot(Terraria.ModLoader.SoundType.Custom, "Sounds/Sounds/ArcaneCast"), npc.position);
                Projectile.NewProjectile(npc.Center, new Vector2(10, 5), ModContent.ProjectileType<Flameray>(), npc.damage / 3, 0f, Main.myPlayer, 0, npc.whoAmI);
                Projectile.NewProjectile(npc.Center, new Vector2(-10, -5), ModContent.ProjectileType<Flameray>(), npc.damage / 3, 0f, Main.myPlayer, 0, npc.whoAmI);
            }
            if (npc.ai[2] == Changerate + 45)
            {
                Main.PlaySound(mod.GetLegacySoundSlot(Terraria.ModLoader.SoundType.Custom, "Sounds/Sounds/ArcaneCast"), npc.position);
                Projectile.NewProjectile(npc.Center, new Vector2(10, 0), ModContent.ProjectileType<Flameray>(), npc.damage / 3, 0f, Main.myPlayer, 0, npc.whoAmI);
                Projectile.NewProjectile(npc.Center, new Vector2(-10, 0), ModContent.ProjectileType<Flameray>(), npc.damage / 3, 0f, Main.myPlayer, 0, npc.whoAmI);
            }
            if (npc.ai[2] == Changerate + 60)
            {
                Main.PlaySound(mod.GetLegacySoundSlot(Terraria.ModLoader.SoundType.Custom, "Sounds/Sounds/ArcaneCast"), npc.position);
                Projectile.NewProjectile(npc.Center, new Vector2(10, -5), ModContent.ProjectileType<Flameray>(), npc.damage / 3, 0f, Main.myPlayer, 0, npc.whoAmI);
                Projectile.NewProjectile(npc.Center, new Vector2(-10, 5), ModContent.ProjectileType<Flameray>(), npc.damage / 3, 0f, Main.myPlayer, 0, npc.whoAmI);
            }
            if (npc.ai[2] == Changerate + 75)
            {
                Main.PlaySound(mod.GetLegacySoundSlot(Terraria.ModLoader.SoundType.Custom, "Sounds/Sounds/ArcaneCast"), npc.position);
                Projectile.NewProjectile(npc.Center, new Vector2(10, -10), ModContent.ProjectileType<Flameray>(), npc.damage / 3, 0f, Main.myPlayer, 0, npc.whoAmI);
                Projectile.NewProjectile(npc.Center, new Vector2(-10, 10), ModContent.ProjectileType<Flameray>(), npc.damage / 3, 0f, Main.myPlayer, 0, npc.whoAmI);
            }
            if (npc.ai[2] == Changerate + 90)
            {
                Main.PlaySound(mod.GetLegacySoundSlot(Terraria.ModLoader.SoundType.Custom, "Sounds/Sounds/ArcaneCast"), npc.position);
                Projectile.NewProjectile(npc.Center, new Vector2(5, -10), ModContent.ProjectileType<Flameray>(), npc.damage / 3, 0f, Main.myPlayer, 0, npc.whoAmI);
                Projectile.NewProjectile(npc.Center, new Vector2(-5, 10), ModContent.ProjectileType<Flameray>(), npc.damage / 3, 0f, Main.myPlayer, 0, npc.whoAmI);
            }
            if (npc.ai[2] == Changerate + 105)
            {
                Main.PlaySound(mod.GetLegacySoundSlot(Terraria.ModLoader.SoundType.Custom, "Sounds/Sounds/ArcaneCast"), npc.position);
                Projectile.NewProjectile(npc.Center, new Vector2(0, -10), ModContent.ProjectileType<Flameray>(), npc.damage / 3, 0f, Main.myPlayer, 0, npc.whoAmI);
                Projectile.NewProjectile(npc.Center, new Vector2(0, 10), ModContent.ProjectileType<Flameray>(), npc.damage / 3, 0f, Main.myPlayer, 0, npc.whoAmI);
            }
            if (npc.ai[2] == Changerate + 120)
            {
                Main.PlaySound(mod.GetLegacySoundSlot(Terraria.ModLoader.SoundType.Custom, "Sounds/Sounds/ArcaneCast"), npc.position);
                Projectile.NewProjectile(npc.Center, new Vector2(-5, -10), ModContent.ProjectileType<Flameray>(), npc.damage / 3, 0f, Main.myPlayer, 0, npc.whoAmI);
                Projectile.NewProjectile(npc.Center, new Vector2(5, 10), ModContent.ProjectileType<Flameray>(), npc.damage / 3, 0f, Main.myPlayer, 0, npc.whoAmI);
            }
            if (npc.ai[2] == Changerate + 135)
            {
                Main.PlaySound(mod.GetLegacySoundSlot(Terraria.ModLoader.SoundType.Custom, "Sounds/Sounds/ArcaneCast"), npc.position);
                Projectile.NewProjectile(npc.Center, new Vector2(-10, -10), ModContent.ProjectileType<Flameray>(), npc.damage / 3, 0f, Main.myPlayer, 0, npc.whoAmI);
                Projectile.NewProjectile(npc.Center, new Vector2(10, 10), ModContent.ProjectileType<Flameray>(), npc.damage / 3, 0f, Main.myPlayer, 0, npc.whoAmI);
            }
            if (npc.ai[2] == Changerate + 150)
            {
                Main.PlaySound(mod.GetLegacySoundSlot(Terraria.ModLoader.SoundType.Custom, "Sounds/Sounds/ArcaneCast"), npc.position);
                Projectile.NewProjectile(npc.Center, new Vector2(-10, -5), ModContent.ProjectileType<Flameray>(), npc.damage / 3, 0f, Main.myPlayer, 0, npc.whoAmI);
                Projectile.NewProjectile(npc.Center, new Vector2(10, 5), ModContent.ProjectileType<Flameray>(), npc.damage / 3, 0f, Main.myPlayer, 0, npc.whoAmI);
            }
            if (npc.ai[2] == Changerate + 165)
            {
                Main.PlaySound(mod.GetLegacySoundSlot(Terraria.ModLoader.SoundType.Custom, "Sounds/Sounds/ArcaneCast"), npc.position);
                Projectile.NewProjectile(npc.Center, new Vector2(-10, 0), ModContent.ProjectileType<Flameray>(), npc.damage / 3, 0f, Main.myPlayer, 0, npc.whoAmI);
                Projectile.NewProjectile(npc.Center, new Vector2(10, 0), ModContent.ProjectileType<Flameray>(), npc.damage / 3, 0f, Main.myPlayer, 0, npc.whoAmI);
            }
            if (npc.ai[2] == Changerate + 180)
            {
                Main.PlaySound(mod.GetLegacySoundSlot(Terraria.ModLoader.SoundType.Custom, "Sounds/Sounds/ArcaneCast"), npc.position);
                Projectile.NewProjectile(npc.Center, new Vector2(-10, 5), ModContent.ProjectileType<Flameray>(), npc.damage / 3, 0f, Main.myPlayer, 0, npc.whoAmI);
                Projectile.NewProjectile(npc.Center, new Vector2(10, -5), ModContent.ProjectileType<Flameray>(), npc.damage / 3, 0f, Main.myPlayer, 0, npc.whoAmI);
            }
            if (npc.ai[2] == Changerate + 195)
            {
                Main.PlaySound(mod.GetLegacySoundSlot(Terraria.ModLoader.SoundType.Custom, "Sounds/Sounds/ArcaneCast"), npc.position);
                Projectile.NewProjectile(npc.Center, new Vector2(-10, 10), ModContent.ProjectileType<Flameray>(), npc.damage / 3, 0f, Main.myPlayer, 0, npc.whoAmI);
                Projectile.NewProjectile(npc.Center, new Vector2(10, -10), ModContent.ProjectileType<Flameray>(), npc.damage / 3, 0f, Main.myPlayer, 0, npc.whoAmI);
            }
            if (npc.ai[2] == Changerate + 210)
            {
                Main.PlaySound(mod.GetLegacySoundSlot(Terraria.ModLoader.SoundType.Custom, "Sounds/Sounds/ArcaneCast"), npc.position);
                Projectile.NewProjectile(npc.Center, new Vector2(-5, 10), ModContent.ProjectileType<Flameray>(), npc.damage / 3, 0f, Main.myPlayer, 0, npc.whoAmI);
                Projectile.NewProjectile(npc.Center, new Vector2(5, -10), ModContent.ProjectileType<Flameray>(), npc.damage / 3, 0f, Main.myPlayer, 0, npc.whoAmI);
            }
            if (npc.ai[2] == Changerate + 225)
            {
                Main.PlaySound(mod.GetLegacySoundSlot(Terraria.ModLoader.SoundType.Custom, "Sounds/Sounds/ArcaneCast"), npc.position);
                Projectile.NewProjectile(npc.Center, new Vector2(0, 10), ModContent.ProjectileType<Flameray>(), npc.damage / 3, 0f, Main.myPlayer, 0, npc.whoAmI);
                Projectile.NewProjectile(npc.Center, new Vector2(0, -10), ModContent.ProjectileType<Flameray>(), npc.damage / 3, 0f, Main.myPlayer, 0, npc.whoAmI);
            }
            if (npc.ai[2] == Changerate + 240)
            {
                Main.PlaySound(mod.GetLegacySoundSlot(Terraria.ModLoader.SoundType.Custom, "Sounds/Sounds/ArcaneCast"), npc.position);
                Projectile.NewProjectile(npc.Center, new Vector2(5, 10), ModContent.ProjectileType<Flameray>(), npc.damage / 3, 0f, Main.myPlayer, 0, npc.whoAmI);
                Projectile.NewProjectile(npc.Center, new Vector2(-5, -10), ModContent.ProjectileType<Flameray>(), npc.damage / 3, 0f, Main.myPlayer, 0, npc.whoAmI);
            }
            if (npc.ai[2] > Changerate + 240 && !CUtils.AnyProjectiles(ModContent.ProjectileType<Flameray>()))
            {
                AIChange();
            }
        }

        private void LaserAttack3()
        {
            if (npc.ai[2] == Changerate + 20)
            {
                Main.PlaySound(mod.GetLegacySoundSlot(Terraria.ModLoader.SoundType.Custom, "Sounds/Sounds/ArcaneCast"), npc.position);
                Projectile.NewProjectile(npc.Center, new Vector2(10, 10), ModContent.ProjectileType<Flameray>(), npc.damage / 3, 0f, Main.myPlayer, 0, npc.whoAmI);
                Projectile.NewProjectile(npc.Center, new Vector2(-10, -10), ModContent.ProjectileType<Flameray>(), npc.damage / 3, 0f, Main.myPlayer, 0, npc.whoAmI);
                Projectile.NewProjectile(npc.Center, new Vector2(-10, -10), ModContent.ProjectileType<Flameray>(), npc.damage / 3, 0f, Main.myPlayer, 0, npc.whoAmI);
                Projectile.NewProjectile(npc.Center, new Vector2(10, 10), ModContent.ProjectileType<Flameray>(), npc.damage / 3, 0f, Main.myPlayer, 0, npc.whoAmI);
            }
            if (npc.ai[2] == Changerate + 40)
            {
                Main.PlaySound(mod.GetLegacySoundSlot(Terraria.ModLoader.SoundType.Custom, "Sounds/Sounds/ArcaneCast"), npc.position);
                Projectile.NewProjectile(npc.Center, new Vector2(10, 5), ModContent.ProjectileType<Flameray>(), npc.damage / 3, 0f, Main.myPlayer, 0, npc.whoAmI);
                Projectile.NewProjectile(npc.Center, new Vector2(-10, -5), ModContent.ProjectileType<Flameray>(), npc.damage / 3, 0f, Main.myPlayer, 0, npc.whoAmI);
                Projectile.NewProjectile(npc.Center, new Vector2(-10, 5), ModContent.ProjectileType<Flameray>(), npc.damage / 3, 0f, Main.myPlayer, 0, npc.whoAmI);
                Projectile.NewProjectile(npc.Center, new Vector2(10, -5), ModContent.ProjectileType<Flameray>(), npc.damage / 3, 0f, Main.myPlayer, 0, npc.whoAmI);
            }
            if (npc.ai[2] == Changerate + 60)
            {
                Main.PlaySound(mod.GetLegacySoundSlot(Terraria.ModLoader.SoundType.Custom, "Sounds/Sounds/ArcaneCast"), npc.position);
                Projectile.NewProjectile(npc.Center, new Vector2(10, 0), ModContent.ProjectileType<Flameray>(), npc.damage / 3, 0f, Main.myPlayer, 0, npc.whoAmI);
                Projectile.NewProjectile(npc.Center, new Vector2(-10, 0), ModContent.ProjectileType<Flameray>(), npc.damage / 3, 0f, Main.myPlayer, 0, npc.whoAmI);
                Projectile.NewProjectile(npc.Center, new Vector2(0, -10), ModContent.ProjectileType<Flameray>(), npc.damage / 3, 0f, Main.myPlayer, 0, npc.whoAmI);
                Projectile.NewProjectile(npc.Center, new Vector2(0, 10), ModContent.ProjectileType<Flameray>(), npc.damage / 3, 0f, Main.myPlayer, 0, npc.whoAmI);
            }
            if (npc.ai[2] == Changerate + 80)
            {
                Main.PlaySound(mod.GetLegacySoundSlot(Terraria.ModLoader.SoundType.Custom, "Sounds/Sounds/ArcaneCast"), npc.position);
                Projectile.NewProjectile(npc.Center, new Vector2(10, -5), ModContent.ProjectileType<Flameray>(), npc.damage / 3, 0f, Main.myPlayer, 0, npc.whoAmI);
                Projectile.NewProjectile(npc.Center, new Vector2(-10, 5), ModContent.ProjectileType<Flameray>(), npc.damage / 3, 0f, Main.myPlayer, 0, npc.whoAmI);
                Projectile.NewProjectile(npc.Center, new Vector2(-10, -5), ModContent.ProjectileType<Flameray>(), npc.damage / 3, 0f, Main.myPlayer, 0, npc.whoAmI);
                Projectile.NewProjectile(npc.Center, new Vector2(-10, -5), ModContent.ProjectileType<Flameray>(), npc.damage / 3, 0f, Main.myPlayer, 0, npc.whoAmI);
            }
            if (npc.ai[2] == Changerate + 100)
            {
                Main.PlaySound(mod.GetLegacySoundSlot(Terraria.ModLoader.SoundType.Custom, "Sounds/Sounds/ArcaneCast"), npc.position);
                Projectile.NewProjectile(npc.Center, new Vector2(10, -10), ModContent.ProjectileType<Flameray>(), npc.damage / 3, 0f, Main.myPlayer, 0, npc.whoAmI);
                Projectile.NewProjectile(npc.Center, new Vector2(-10, 10), ModContent.ProjectileType<Flameray>(), npc.damage / 3, 0f, Main.myPlayer, 0, npc.whoAmI);
                Projectile.NewProjectile(npc.Center, new Vector2(-10, -10), ModContent.ProjectileType<Flameray>(), npc.damage / 3, 0f, Main.myPlayer, 0, npc.whoAmI);
                Projectile.NewProjectile(npc.Center, new Vector2(10, 10), ModContent.ProjectileType<Flameray>(), npc.damage / 3, 0f, Main.myPlayer, 0, npc.whoAmI);
            }
            if (npc.ai[2] == Changerate + 120)
            {
                Main.PlaySound(mod.GetLegacySoundSlot(Terraria.ModLoader.SoundType.Custom, "Sounds/Sounds/ArcaneCast"), npc.position);
                Projectile.NewProjectile(npc.Center, new Vector2(5, -10), ModContent.ProjectileType<Flameray>(), npc.damage / 3, 0f, Main.myPlayer, 0, npc.whoAmI);
                Projectile.NewProjectile(npc.Center, new Vector2(-5, 10), ModContent.ProjectileType<Flameray>(), npc.damage / 3, 0f, Main.myPlayer, 0, npc.whoAmI);
                Projectile.NewProjectile(npc.Center, new Vector2(-5, -10), ModContent.ProjectileType<Flameray>(), npc.damage / 3, 0f, Main.myPlayer, 0, npc.whoAmI);
                Projectile.NewProjectile(npc.Center, new Vector2(5, 10), ModContent.ProjectileType<Flameray>(), npc.damage / 3, 0f, Main.myPlayer, 0, npc.whoAmI);
            }
            if (npc.ai[2] == Changerate + 140)
            {
                Main.PlaySound(mod.GetLegacySoundSlot(Terraria.ModLoader.SoundType.Custom, "Sounds/Sounds/ArcaneCast"), npc.position);
                Projectile.NewProjectile(npc.Center, new Vector2(0, -10), ModContent.ProjectileType<Flameray>(), npc.damage / 3, 0f, Main.myPlayer, 0, npc.whoAmI);
                Projectile.NewProjectile(npc.Center, new Vector2(0, 10), ModContent.ProjectileType<Flameray>(), npc.damage / 3, 0f, Main.myPlayer, 0, npc.whoAmI);
                Projectile.NewProjectile(npc.Center, new Vector2(10, 0), ModContent.ProjectileType<Flameray>(), npc.damage / 3, 0f, Main.myPlayer, 0, npc.whoAmI);
                Projectile.NewProjectile(npc.Center, new Vector2(-10, 0), ModContent.ProjectileType<Flameray>(), npc.damage / 3, 0f, Main.myPlayer, 0, npc.whoAmI);
            }
            if (npc.ai[2] == Changerate + 160)
            {
                Main.PlaySound(mod.GetLegacySoundSlot(Terraria.ModLoader.SoundType.Custom, "Sounds/Sounds/ArcaneCast"), npc.position);
                Projectile.NewProjectile(npc.Center, new Vector2(-5, -10), ModContent.ProjectileType<Flameray>(), npc.damage / 3, 0f, Main.myPlayer, 0, npc.whoAmI);
                Projectile.NewProjectile(npc.Center, new Vector2(5, 10), ModContent.ProjectileType<Flameray>(), npc.damage / 3, 0f, Main.myPlayer, 0, npc.whoAmI);
                Projectile.NewProjectile(npc.Center, new Vector2(5, -10), ModContent.ProjectileType<Flameray>(), npc.damage / 3, 0f, Main.myPlayer, 0, npc.whoAmI);
                Projectile.NewProjectile(npc.Center, new Vector2(-5, 10), ModContent.ProjectileType<Flameray>(), npc.damage / 3, 0f, Main.myPlayer, 0, npc.whoAmI);
            }
            if (npc.ai[2] == Changerate + 180)
            {
                Main.PlaySound(mod.GetLegacySoundSlot(Terraria.ModLoader.SoundType.Custom, "Sounds/Sounds/ArcaneCast"), npc.position);
                Projectile.NewProjectile(npc.Center, new Vector2(-10, -10), ModContent.ProjectileType<Flameray>(), npc.damage / 3, 0f, Main.myPlayer, 0, npc.whoAmI);
                Projectile.NewProjectile(npc.Center, new Vector2(10, 10), ModContent.ProjectileType<Flameray>(), npc.damage / 3, 0f, Main.myPlayer, 0, npc.whoAmI);
                Projectile.NewProjectile(npc.Center, new Vector2(-10, 10), ModContent.ProjectileType<Flameray>(), npc.damage / 3, 0f, Main.myPlayer, 0, npc.whoAmI);
                Projectile.NewProjectile(npc.Center, new Vector2(10, -10), ModContent.ProjectileType<Flameray>(), npc.damage / 3, 0f, Main.myPlayer, 0, npc.whoAmI);
            }
            if (npc.ai[2] == Changerate + 200)
            {
                Main.PlaySound(mod.GetLegacySoundSlot(Terraria.ModLoader.SoundType.Custom, "Sounds/Sounds/ArcaneCast"), npc.position);
                Projectile.NewProjectile(npc.Center, new Vector2(-10, -5), ModContent.ProjectileType<Flameray>(), npc.damage / 3, 0f, Main.myPlayer, 0, npc.whoAmI);
                Projectile.NewProjectile(npc.Center, new Vector2(10, 5), ModContent.ProjectileType<Flameray>(), npc.damage / 3, 0f, Main.myPlayer, 0, npc.whoAmI);
                Projectile.NewProjectile(npc.Center, new Vector2(10, -5), ModContent.ProjectileType<Flameray>(), npc.damage / 3, 0f, Main.myPlayer, 0, npc.whoAmI);
                Projectile.NewProjectile(npc.Center, new Vector2(-10, 5), ModContent.ProjectileType<Flameray>(), npc.damage / 3, 0f, Main.myPlayer, 0, npc.whoAmI);
            }
            if (npc.ai[2] == Changerate + 220)
            {
                Main.PlaySound(mod.GetLegacySoundSlot(Terraria.ModLoader.SoundType.Custom, "Sounds/Sounds/ArcaneCast"), npc.position);
                Projectile.NewProjectile(npc.Center, new Vector2(-10, 0), ModContent.ProjectileType<Flameray>(), npc.damage / 3, 0f, Main.myPlayer, 0, npc.whoAmI);
                Projectile.NewProjectile(npc.Center, new Vector2(10, 0), ModContent.ProjectileType<Flameray>(), npc.damage / 3, 0f, Main.myPlayer, 0, npc.whoAmI);
                Projectile.NewProjectile(npc.Center, new Vector2(0, -10), ModContent.ProjectileType<Flameray>(), npc.damage / 3, 0f, Main.myPlayer, 0, npc.whoAmI);
                Projectile.NewProjectile(npc.Center, new Vector2(0, 10), ModContent.ProjectileType<Flameray>(), npc.damage / 3, 0f, Main.myPlayer, 0, npc.whoAmI);
            }
            if (npc.ai[2] == Changerate + 240)
            {
                Main.PlaySound(mod.GetLegacySoundSlot(Terraria.ModLoader.SoundType.Custom, "Sounds/Sounds/ArcaneCast"), npc.position);
                Projectile.NewProjectile(npc.Center, new Vector2(-10, 5), ModContent.ProjectileType<Flameray>(), npc.damage / 3, 0f, Main.myPlayer, 0, npc.whoAmI);
                Projectile.NewProjectile(npc.Center, new Vector2(10, -5), ModContent.ProjectileType<Flameray>(), npc.damage / 3, 0f, Main.myPlayer, 0, npc.whoAmI);
                Projectile.NewProjectile(npc.Center, new Vector2(10, 5), ModContent.ProjectileType<Flameray>(), npc.damage / 3, 0f, Main.myPlayer, 0, npc.whoAmI);
                Projectile.NewProjectile(npc.Center, new Vector2(-10, -5), ModContent.ProjectileType<Flameray>(), npc.damage / 3, 0f, Main.myPlayer, 0, npc.whoAmI);
            }
            if (npc.ai[2] == Changerate + 260)
            {
                Main.PlaySound(mod.GetLegacySoundSlot(Terraria.ModLoader.SoundType.Custom, "Sounds/Sounds/ArcaneCast"), npc.position);
                Projectile.NewProjectile(npc.Center, new Vector2(-10, 10), ModContent.ProjectileType<Flameray>(), npc.damage / 3, 0f, Main.myPlayer, 0, npc.whoAmI);
                Projectile.NewProjectile(npc.Center, new Vector2(10, -10), ModContent.ProjectileType<Flameray>(), npc.damage / 3, 0f, Main.myPlayer, 0, npc.whoAmI);
                Projectile.NewProjectile(npc.Center, new Vector2(-10, -10), ModContent.ProjectileType<Flameray>(), npc.damage / 3, 0f, Main.myPlayer, 0, npc.whoAmI);
                Projectile.NewProjectile(npc.Center, new Vector2(10, 10), ModContent.ProjectileType<Flameray>(), npc.damage / 3, 0f, Main.myPlayer, 0, npc.whoAmI);
            }
            if (npc.ai[2] == Changerate + 280)
            {
                Main.PlaySound(mod.GetLegacySoundSlot(Terraria.ModLoader.SoundType.Custom, "Sounds/Sounds/ArcaneCast"), npc.position);
                Projectile.NewProjectile(npc.Center, new Vector2(-5, 10), ModContent.ProjectileType<Flameray>(), npc.damage / 3, 0f, Main.myPlayer, 0, npc.whoAmI);
                Projectile.NewProjectile(npc.Center, new Vector2(5, -10), ModContent.ProjectileType<Flameray>(), npc.damage / 3, 0f, Main.myPlayer, 0, npc.whoAmI);
                Projectile.NewProjectile(npc.Center, new Vector2(-5, -10), ModContent.ProjectileType<Flameray>(), npc.damage / 3, 0f, Main.myPlayer, 0, npc.whoAmI);
                Projectile.NewProjectile(npc.Center, new Vector2(5, 10), ModContent.ProjectileType<Flameray>(), npc.damage / 3, 0f, Main.myPlayer, 0, npc.whoAmI);
            }
            if (npc.ai[2] == Changerate + 300)
            {
                Main.PlaySound(mod.GetLegacySoundSlot(Terraria.ModLoader.SoundType.Custom, "Sounds/Sounds/ArcaneCast"), npc.position);
                Projectile.NewProjectile(npc.Center, new Vector2(0, 10), ModContent.ProjectileType<Flameray>(), npc.damage / 3, 0f, Main.myPlayer, 0, npc.whoAmI);
                Projectile.NewProjectile(npc.Center, new Vector2(0, -10), ModContent.ProjectileType<Flameray>(), npc.damage / 3, 0f, Main.myPlayer, 0, npc.whoAmI);
                Projectile.NewProjectile(npc.Center, new Vector2(10, 0), ModContent.ProjectileType<Flameray>(), npc.damage / 3, 0f, Main.myPlayer, 0, npc.whoAmI);
                Projectile.NewProjectile(npc.Center, new Vector2(-10, 0), ModContent.ProjectileType<Flameray>(), npc.damage / 3, 0f, Main.myPlayer, 0, npc.whoAmI);
            }
            if (npc.ai[2] == Changerate + 320)
            {
                Main.PlaySound(mod.GetLegacySoundSlot(Terraria.ModLoader.SoundType.Custom, "Sounds/Sounds/ArcaneCast"), npc.position);
                Projectile.NewProjectile(npc.Center, new Vector2(5, 10), ModContent.ProjectileType<Flameray>(), npc.damage / 3, 0f, Main.myPlayer, 0, npc.whoAmI);
                Projectile.NewProjectile(npc.Center, new Vector2(-5, -10), ModContent.ProjectileType<Flameray>(), npc.damage / 3, 0f, Main.myPlayer, 0, npc.whoAmI);
                Projectile.NewProjectile(npc.Center, new Vector2(5, -10), ModContent.ProjectileType<Flameray>(), npc.damage / 3, 0f, Main.myPlayer, 0, npc.whoAmI);
                Projectile.NewProjectile(npc.Center, new Vector2(-5, 10), ModContent.ProjectileType<Flameray>(), npc.damage / 3, 0f, Main.myPlayer, 0, npc.whoAmI);
            }
            if (npc.ai[2] > Changerate + 320 && !CUtils.AnyProjectiles(ModContent.ProjectileType<Flameray>()))
            {
                AIChange();
            }
        }

        public float auraPercent = 0f;
        public bool auraDirection = true;

        public override bool PreDraw(SpriteBatch sb, Color lightColor)
        {
            int r = GameShaders.Armor.GetShaderIdFromItemId(ItemID.LivingFlameDye);

            if (auraDirection) { auraPercent += 0.1f; auraDirection = auraPercent < 1f; }
            else { auraPercent -= 0.1f; auraDirection = auraPercent <= 0f; }

            Texture2D texture2D13 = Main.npcTexture[npc.type];
            Texture2D RingTex = mod.GetTexture("NPCs/Bosses/FurySoul/FuryRing");

            Texture2D RingTex1 = mod.GetTexture("NPCs/Bosses/Heartcore/Ring1");
            Texture2D RingTex2 = mod.GetTexture("NPCs/Bosses/Heartcore/Ring2");
            Texture2D RitualTex = mod.GetTexture("NPCs/Bosses/Heartcore/Ritual");


            if (scale > 0)
            {
                BaseDrawing.DrawTexture(sb, RitualTex, r, npc.position, npc.width, npc.height, scale, -npc.rotation, 0, 1, new Rectangle(0, 0, RitualTex.Width, RitualTex.Height), lightColor, true);
                BaseDrawing.DrawTexture(sb, RingTex1, r, npc.position, npc.width, npc.height, scale, npc.rotation, 0, 1, new Rectangle(0, 0, RingTex1.Width, RingTex1.Height), lightColor, true);
                BaseDrawing.DrawTexture(sb, RingTex2, r, npc.position, npc.width, npc.height, scale, npc.rotation, 0, 1, new Rectangle(0, 0, RingTex1.Width, RingTex1.Height), lightColor, true);
                BaseDrawing.DrawTexture(sb, RingTex, r, npc.position, npc.width, npc.height, scale, npc.rotation, 0, 1, new Rectangle(0, 0, RingTex.Width, RingTex.Height), Color.White, true);
            }

            BaseDrawing.DrawAura(sb, texture2D13, 0, npc.position, npc.width, npc.height, auraPercent, 1.5f, 1f, 0, npc.direction, 1, npc.frame, 0f, 0f, npc.GetAlpha(Color.White));

            return false;
        }
    }
}