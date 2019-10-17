using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.IO;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace CSkies.NPCs.Bosses.Starcore
{
    public class Starcore : ModNPC
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Starcore");
        }

        public override void SetDefaults()
        {
            npc.npcSlots = 100;
            npc.width = 50;
            npc.height = 50;
            npc.aiStyle = -1;
            npc.damage = 45;
            npc.defense = 25;
            npc.lifeMax = 12000;
            npc.value = Item.sellPrice(0, 0, 50, 0);
            npc.HitSound = new LegacySoundStyle(3, 4, Terraria.Audio.SoundType.Sound);
            npc.DeathSound = new LegacySoundStyle(4, 14, Terraria.Audio.SoundType.Sound);
            npc.knockBackResist = 0f;
            npc.noGravity = true;
            npc.netAlways = true;
            npc.boss = true;
            npc.noTileCollide = true;
            music = mod.GetSoundSlot(Terraria.ModLoader.SoundType.Music, "Sounds/Music/Starcore");
            bossBag = mod.ItemType("StarcoreBag");
        }

        public float[] Shoot = new float[1];

        public override void SendExtraAI(BinaryWriter writer)
        {
            base.SendExtraAI(writer);
            if (Main.netMode == NetmodeID.Server || Main.dedServ)
            {
                writer.Write(Shoot[0]);
            }
        }

        public override void ReceiveExtraAI(BinaryReader reader)
        {
            base.ReceiveExtraAI(reader);
            if (Main.netMode == NetmodeID.MultiplayerClient)
            {
                Shoot[0] = reader.ReadFloat();
            }
        }

        public override void NPCLoot()
        {
            if (Main.rand.Next(10) == 0)
            {
                npc.DropLoot(mod.ItemType("StarcoreTrophy"));
            }
            if (Main.expertMode)
            {
                npc.DropBossBags();
            }
            else
            {
                if (Main.rand.Next(7) == 0)
                {
                    npc.DropLoot(mod.ItemType("StarcoreMask"));
                }
                string[] lootTableA = { "Starsaber", "StormStaff", "StarDroneUnit", "Railscope" };
                int lootA = Main.rand.Next(lootTableA.Length);

                npc.DropLoot(mod.ItemType(lootTableA[lootA]));

                npc.DropLoot(ModContent.ItemType<Items.Star.Stelarite>(), Main.rand.Next(8, 12));
            }
            for (int num468 = 0; num468 < 12; num468++)
            {
                int num469 = Dust.NewDust(new Vector2(npc.position.X, npc.position.Y), npc.width, npc.height, ModContent.DustType<Dusts.StarDust>(), -npc.velocity.X * 0.2f,
                    -npc.velocity.Y * 0.2f, 100, default, 2f);
                Main.dust[num469].noGravity = true;
            }
        }

        bool warning = false;

        public static Color Warning => BaseUtility.MultiLerpColor(Main.LocalPlayer.miscCounter % 100 / 100f, Color.LimeGreen, Color.Red, Color.LimeGreen);

        public override void AI()
        {
            int speed = 10;
            float interval = .015f;
            if (npc.life < npc.lifeMax / 4)
            {
                speed = 14;
                interval = .02f;
                if (!warning)
                {
                    warning = true;
                    CombatText.NewText(npc.getRect(), Color.Lime, "BODY IN CRITICAL STATE. ENGAGE PANIC MODE", true);
                }
                Lighting.AddLight(npc.Center, Warning.R / 150, Warning.G / 150,  Warning.B / 150);
            }
            else
            {
                Lighting.AddLight(npc.Center, Color.LimeGreen.R / 150, Color.LimeGreen.G / 150, Color.LimeGreen.B / 150);
            }


            if (!npc.HasPlayerTarget)
            {
                npc.TargetClosest();
            }
            Player player = Main.player[npc.target];

            BaseAI.AISkull(npc, ref npc.ai, true, speed, 350, interval, .025f);

            if (npc.ai[2]++ > (Main.expertMode ? 150 : 220))
            {
                if (npc.velocity.X > 0)
                {
                    npc.rotation += .09f;
                }
                else if (npc.velocity.X < 0)
                {
                    npc.rotation -= .09f;
                }
                switch (Shoot[0])
                {
                    case 0:
                        float spread = 45f * 0.0174f;
                        Vector2 dir = Vector2.Normalize(player.Center - npc.Center);
                        dir *= 12f;
                        float baseSpeed = (float)Math.Sqrt((dir.X * dir.X) + (dir.Y * dir.Y));
                        double startAngle = Math.Atan2(dir.X, dir.Y) - .1d;
                        double deltaAngle = spread / 6f;
                        if (npc.life < (int)(npc.lifeMax * .5f))
                        {
                            for (int i = 0; i < 3; i++)
                            {
                                if (npc.ai[2] % 30 == 0)
                                {
                                    double offsetAngle = startAngle + (deltaAngle * i);
                                    if (npc.life < (int)(npc.lifeMax * .4f) && Main.rand.Next(2) == 0)
                                    {
                                        Projectile.NewProjectile(npc.Center.X, npc.Center.Y, baseSpeed * (float)Math.Sin(offsetAngle), baseSpeed * (float)Math.Cos(offsetAngle), ModContent.ProjectileType<BigStarProj>(), npc.damage / 4, 5, Main.myPlayer);
                                    }
                                    else
                                    {
                                        Projectile.NewProjectile(npc.Center.X, npc.Center.Y, baseSpeed * (float)Math.Sin(offsetAngle), baseSpeed * (float)Math.Cos(offsetAngle), ModContent.ProjectileType<MiniStar>(), npc.damage / 4, 5, Main.myPlayer);
                                    }
                                }
                            }
                            if (npc.ai[2] > (Main.expertMode ? 271 : 331))
                            {
                                npc.ai[2] = 0;
                                Shoot[0] = Main.rand.Next(4);
                            }
                        }
                        else
                        {
                            for (int i = 0; i < 3; i++)
                            {
                                double offsetAngle = startAngle + (deltaAngle * i);
                                Projectile.NewProjectile(npc.Center.X, npc.Center.Y, baseSpeed * (float)Math.Sin(offsetAngle), baseSpeed * (float)Math.Cos(offsetAngle), ModContent.ProjectileType<MiniStar>(), npc.damage / 4, 12, Main.myPlayer);
                            }
                            npc.ai[2] = 0;
                            Shoot[0] = Main.rand.Next(4);
                        }
                        break;
                    case 1:
                        float spread1 = 12f * 0.0174f;
                        double startAngle1 = Math.Atan2(npc.velocity.X, npc.velocity.Y) - spread1 / 2;
                        double deltaAngle1 = spread1 / 20f;
                        for (int i = 0; i < 10; i++)
                        {
                            double offsetAngle = startAngle1 + deltaAngle1 * (i + i * i) / 2f + 32f * i;
                            Projectile.NewProjectile(npc.Center.X, npc.Center.Y, (float)(Math.Sin(offsetAngle) * 10f), (float)(Math.Cos(offsetAngle) * 10f), ModContent.ProjectileType<Starstatic>(), npc.damage, 5);
                            Projectile.NewProjectile(npc.Center.X, npc.Center.Y, (float)(-Math.Sin(offsetAngle) * 10f), (float)(-Math.Cos(offsetAngle) * 10f), ModContent.ProjectileType<Starstatic>(), npc.damage, 5);
                        }
                        npc.ai[2] = 0;
                        Shoot[0] = Main.rand.Next(4);
                        break;
                    case 2:
                        Projectile.NewProjectile(npc.Center, new Vector2(7, 7), mod.ProjectileType("Starsphere"), npc.damage / 4, 0f, Main.myPlayer, 0, npc.whoAmI);
                        Projectile.NewProjectile(npc.Center, new Vector2(-7, 7), mod.ProjectileType("Starsphere"), npc.damage / 4, 0f, Main.myPlayer, 0, npc.whoAmI);
                        Projectile.NewProjectile(npc.Center, new Vector2(7, -7), mod.ProjectileType("Starsphere"), npc.damage / 4, 0f, Main.myPlayer, 0, npc.whoAmI);
                        Projectile.NewProjectile(npc.Center, new Vector2(-7, -7), mod.ProjectileType("Starsphere"), npc.damage / 4, 0f, Main.myPlayer, 0, npc.whoAmI);
                        Projectile.NewProjectile(npc.Center, new Vector2(9, 0), mod.ProjectileType("Starsphere"), npc.damage / 4, 0f, Main.myPlayer, 0, npc.whoAmI);
                        Projectile.NewProjectile(npc.Center, new Vector2(-9, 0), mod.ProjectileType("Starsphere"), npc.damage / 4, 0f, Main.myPlayer, 0, npc.whoAmI);
                        Projectile.NewProjectile(npc.Center, new Vector2(0, -9), mod.ProjectileType("Starsphere"), npc.damage / 4, 0f, Main.myPlayer, 0, npc.whoAmI);
                        Projectile.NewProjectile(npc.Center, new Vector2(0, 9), mod.ProjectileType("Starsphere"), npc.damage / 4, 0f, Main.myPlayer, 0, npc.whoAmI);

                        npc.ai[2] = 0;
                        Shoot[0] = Main.rand.Next(4);
                        break;
                    case 3:
                        if (npc.ai[2] % 20 == 0)
                        {
                            BaseAI.FireProjectile(player.position, npc.position, ModContent.ProjectileType<Starblast>(), npc.damage / 4, 5, 12, 0, Main.myPlayer);
                        }
                        if (npc.ai[2] > (Main.expertMode ? 271 : 331))
                        {
                            npc.ai[2] = 0;
                            Shoot[0] = Main.rand.Next(4);
                        }
                        break;
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
                    BaseAI.ShootPeriodic(npc, player.position, player.width, player.height, ModContent.ProjectileType<Starshot>(), ref npc.ai[3], Main.rand.Next(30, 50), npc.damage / 4, 10, true);
                }
            }
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Color drawColor)
        {
            Texture2D texture2D13 = Main.npcTexture[npc.type];
            Texture2D BladeTex = mod.GetTexture("NPCs/Bosses/Starcore/StarcoreBack");
            Texture2D GlowTex = mod.GetTexture("Glowmasks/Starcore_Glow");
            Texture2D BladeGlowTex = mod.GetTexture("Glowmasks/StarcoreBack_Glow");
            Texture2D Warning = mod.GetTexture("Glowmasks/StarcoreWarning");
            Texture2D WarningBack = mod.GetTexture("Glowmasks/StarcoreBackWarning");

            BaseDrawing.DrawTexture(spriteBatch, BladeTex, 0, npc.position, npc.width, npc.height, npc.scale, npc.rotation, 0, 1, new Rectangle(0, 0, BladeTex.Width, BladeTex.Height), drawColor, true);
            BaseDrawing.DrawTexture(spriteBatch, BladeGlowTex, 0, npc.position, npc.width, npc.height, npc.scale, npc.rotation, 0, 1, new Rectangle(0, 0, BladeTex.Width, BladeTex.Height), Colors.COLOR_GLOWPULSE, true);

            if (npc.life < npc.lifeMax / 4)
            {
                BaseDrawing.DrawTexture(spriteBatch, WarningBack, 0, npc.position, npc.width, npc.height, npc.scale, npc.rotation, 0, 1, new Rectangle(0, 0, WarningBack.Width, WarningBack.Height), Colors.Flash, true);
            }

            BaseDrawing.DrawTexture(spriteBatch, texture2D13, 0, npc.position, npc.width, npc.height, npc.scale, 0, 0, 1, new Rectangle(0, 0, texture2D13.Width, texture2D13.Height), drawColor, true);
            BaseDrawing.DrawTexture(spriteBatch, GlowTex, 0, npc.position, npc.width, npc.height, npc.scale, 0, 0, 1, new Rectangle(0, 0, texture2D13.Width, texture2D13.Height), Colors.COLOR_GLOWPULSE, true);

            if (npc.life < npc.lifeMax / 4)
            {
                BaseDrawing.DrawTexture(spriteBatch, Warning, 0, npc.position, npc.width, npc.height, npc.scale, 0, 0, 1, new Rectangle(0, 0, texture2D13.Width, texture2D13.Height), Colors.Flash, true);
            }
            return false;
        }
    }
}