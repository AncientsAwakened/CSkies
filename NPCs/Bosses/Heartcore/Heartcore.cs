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

namespace CSkies.NPCs.Bosses.Heartcore
{
    public class Heartcore : ModNPC
    {
        public override void SetDefaults()
        {
            npc.npcSlots = 100;
            npc.width = 50;
            npc.height = 50;
            npc.aiStyle = -1;
            npc.damage = 90;
            npc.defense = 40;
            npc.lifeMax = 150000;
            npc.value = Item.sellPrice(0, 12, 0, 0);
            npc.HitSound = new LegacySoundStyle(21, 1);
            npc.DeathSound = new LegacySoundStyle(4, 14, Terraria.Audio.SoundType.Sound);
            npc.knockBackResist = 0f;
            npc.noGravity = true;
            npc.netAlways = true;
            npc.boss = true;
            npc.noTileCollide = true;
            music = mod.GetSoundSlot(Terraria.ModLoader.SoundType.Music, "Sounds/Music/Heartcore");
        }

        public override void ScaleExpertStats(int numPlayers, float bossLifeScale)
        {
            npc.lifeMax = (int)(npc.lifeMax * 0.5f * bossLifeScale);
            npc.damage = (int)(npc.damage * 0.5f);
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
            Gore.NewGore(npc.position, npc.velocity * 0.2f, mod.GetGoreSlot("Gores/HeartcoreGore1"), 1f);
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
                string[] lootTableA = { "Sol", "MeteorShower" };
                int lootA = Main.rand.Next(lootTableA.Length);

                npc.DropLoot(mod.ItemType(lootTableA[lootA]));

                npc.DropLoot(ModContent.ItemType<Items.Heart.HeartSoul>(), Main.rand.Next(8, 12));
                int n = NPC.NewNPC((int)npc.Center.X, (int)npc.Center.Y, ModContent.NPCType<HeartcoreDefeat>());
                Main.npc[n].Center = npc.Center;
            }
        }

        public static Color Flame => BaseUtility.MultiLerpColor(Main.LocalPlayer.miscCounter % 100 / 100f, Color.Orange, Color.Red, Color.Orange);

        bool Rage = false;

        public override void AI()
        {
            int speed = 9;
            float interval = .02f;

            if (npc.life < npc.lifeMax / 3)
            {
                if (!Rage)
                {
                    Rage = true;
                }
                speed = 11;
                interval = .03f;
            }

            RingEffects();

            Lighting.AddLight(npc.Center, Flame.R / 150, Flame.G / 150, Flame.B / 150);


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
                    npc.rotation += .06f;
                }
                else if (npc.velocity.X < 0)
                {
                    npc.rotation -= .06f;
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
                        if (npc.life < npc.lifeMax / 3)
                        {
                            for (int i = 0; i < 3; i++)
                            {
                                if (npc.ai[2] % 30 == 0)
                                {
                                    double offsetAngle = startAngle + (deltaAngle * i);
                                    Projectile.NewProjectile(npc.Center.X, npc.Center.Y, baseSpeed * (float)Math.Sin(offsetAngle), baseSpeed * (float)Math.Cos(offsetAngle), ModContent.ProjectileType<BigHeartshot>(), npc.damage / 4, 5, Main.myPlayer);
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
                                Projectile.NewProjectile(npc.Center.X, npc.Center.Y, baseSpeed * (float)Math.Sin(offsetAngle), baseSpeed * (float)Math.Cos(offsetAngle), ModContent.ProjectileType<BigHeartshot>(), npc.damage / 4, 5, Main.myPlayer);
                            }
                            npc.ai[2] = 0;
                            Shoot[0] = Main.rand.Next(4);
                        }
                        break;
                    case 1:
                        float spread1 = 12f * 0.0174f;
                        double startAngle1 = Math.Atan2(npc.velocity.X, npc.velocity.Y) - spread1 / 2;
                        double deltaAngle1 = spread1 / 10f;
                        Main.PlaySound(mod.GetLegacySoundSlot(Terraria.ModLoader.SoundType.Custom, "Sounds/Sounds/FireCast"), npc.position);
                        for (int i = 0; i < 10; i++)
                        {
                            double offsetAngle1 = (startAngle1 + deltaAngle1 * (i + i * i) / 2f) + 32f * i;
                            Projectile.NewProjectile(npc.Center.X, npc.Center.Y, (float)(Math.Sin(offsetAngle1) * 8f), (float)(Math.Cos(offsetAngle1) * 8f), ModContent.ProjectileType<Fireshot>(), npc.damage / 4, 6);
                            Projectile.NewProjectile(npc.Center.X, npc.Center.Y, (float)(-Math.Sin(offsetAngle1) * 8f), (float)(-Math.Cos(offsetAngle1) * 8f), ModContent.ProjectileType<Fireshot>(), npc.damage / 4, 6);
                        }
                        npc.ai[2] = 0;
                        Shoot[0] = Main.rand.Next(4);
                        break;
                    case 2:
                        Main.PlaySound(mod.GetLegacySoundSlot(Terraria.ModLoader.SoundType.Custom, "Sounds/Sounds/ArcaneCast"), npc.position);
                        Projectile.NewProjectile(npc.Center, new Vector2(7, 7), ModContent.ProjectileType<Fireball>(), npc.damage / 4, 0f, Main.myPlayer, 0, npc.whoAmI);
                        Projectile.NewProjectile(npc.Center, new Vector2(-7, 7), ModContent.ProjectileType<Fireball>(), npc.damage / 4, 0f, Main.myPlayer, 0, npc.whoAmI);
                        Projectile.NewProjectile(npc.Center, new Vector2(7, -7), ModContent.ProjectileType<Fireball>(), npc.damage / 4, 0f, Main.myPlayer, 0, npc.whoAmI);
                        Projectile.NewProjectile(npc.Center, new Vector2(-7, -7), ModContent.ProjectileType<Fireball>(), npc.damage / 4, 0f, Main.myPlayer, 0, npc.whoAmI);
                        Projectile.NewProjectile(npc.Center, new Vector2(9, 0), ModContent.ProjectileType<Fireball>(), npc.damage / 4, 0f, Main.myPlayer, 0, npc.whoAmI);
                        Projectile.NewProjectile(npc.Center, new Vector2(-9, 0), ModContent.ProjectileType<Fireball>(), npc.damage / 4, 0f, Main.myPlayer, 0, npc.whoAmI);
                        Projectile.NewProjectile(npc.Center, new Vector2(0, -9), ModContent.ProjectileType<Fireball>(), npc.damage / 4, 0f, Main.myPlayer, 0, npc.whoAmI);
                        Projectile.NewProjectile(npc.Center, new Vector2(0, 9), ModContent.ProjectileType<Fireball>(), npc.damage / 4, 0f, Main.myPlayer, 0, npc.whoAmI);

                        npc.ai[2] = 0;
                        Shoot[0] = Main.rand.Next(4);
                        break;
                    case 3:
                        if (npc.ai[2] % 20 == 0)
                        {
                            Main.PlaySound(mod.GetLegacySoundSlot(Terraria.ModLoader.SoundType.Custom, "Sounds/Sounds/FireCast"), npc.position);
                            Projectile.NewProjectile(npc.Center, new Vector2(9, 9), ModContent.ProjectileType<Flamewave>(), npc.damage / 4, 0f, Main.myPlayer, 0, npc.whoAmI);
                            Projectile.NewProjectile(npc.Center, new Vector2(-9, 9), ModContent.ProjectileType<Flamewave>(), npc.damage / 4, 0f, Main.myPlayer, 0, npc.whoAmI);
                            Projectile.NewProjectile(npc.Center, new Vector2(9, -9), ModContent.ProjectileType<Flamewave>(), npc.damage / 4, 0f, Main.myPlayer, 0, npc.whoAmI);
                            Projectile.NewProjectile(npc.Center, new Vector2(-9, -9), ModContent.ProjectileType<Flamewave>(), npc.damage / 4, 0f, Main.myPlayer, 0, npc.whoAmI);
                            Projectile.NewProjectile(npc.Center, new Vector2(9, 0), ModContent.ProjectileType<Flamewave>(), npc.damage / 4, 0f, Main.myPlayer, 0, npc.whoAmI);
                            Projectile.NewProjectile(npc.Center, new Vector2(-9, 0), ModContent.ProjectileType<Flamewave>(), npc.damage / 4, 0f, Main.myPlayer, 0, npc.whoAmI);
                            Projectile.NewProjectile(npc.Center, new Vector2(0, -9), ModContent.ProjectileType<Flamewave>(), npc.damage / 4, 0f, Main.myPlayer, 0, npc.whoAmI);
                            Projectile.NewProjectile(npc.Center, new Vector2(0, 9), ModContent.ProjectileType<Flamewave>(), npc.damage / 4, 0f, Main.myPlayer, 0, npc.whoAmI);
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
                    BaseAI.ShootPeriodic(npc, player.position, player.width, player.height, ModContent.ProjectileType<Meteor>(), ref npc.ai[3], Main.rand.Next(30, 50), npc.damage / 4, 10, true);
                }
            }
        }

        float scale = 0;

        private void RingEffects()
        {
            if (npc.life < npc.lifeMax / 3)
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

        public override bool PreDraw(SpriteBatch spriteBatch, Color drawColor)
        {
            Texture2D texture2D13 = Main.npcTexture[npc.type];
            Texture2D BladeTex = mod.GetTexture("NPCs/Bosses/Heartcore/HeartcoreBack");
            Texture2D BladeGlowTex = mod.GetTexture("Glowmasks/HeartcoreBack_Glow");
            Texture2D GlowTex = mod.GetTexture("Glowmasks/Heartcore_Glow");
            Texture2D Heart = mod.GetTexture("Glowmasks/Heart_Glow");

            Texture2D RingTex1 = mod.GetTexture("NPCs/Bosses/Heartcore/Ring1");
            Texture2D RingTex2 = mod.GetTexture("NPCs/Bosses/Heartcore/Ring2");
            Texture2D RitualTex = mod.GetTexture("NPCs/Bosses/Heartcore/Ritual");

            int r = GameShaders.Armor.GetShaderIdFromItemId(ItemID.LivingFlameDye);

            if (scale > 0)
            {
                BaseDrawing.DrawTexture(spriteBatch, RitualTex, r, npc.position, npc.width, npc.height, scale, -npc.rotation, 0, 1, new Rectangle(0, 0, RitualTex.Width, RitualTex.Height), drawColor, true);
                BaseDrawing.DrawTexture(spriteBatch, RingTex1, r, npc.position, npc.width, npc.height, scale, npc.rotation, 0, 1, new Rectangle(0, 0, RingTex1.Width, RingTex1.Height), drawColor, true);
                BaseDrawing.DrawTexture(spriteBatch, RingTex2, r, npc.position, npc.width, npc.height, scale, npc.rotation, 0, 1, new Rectangle(0, 0, RingTex1.Width, RingTex1.Height), drawColor, true);
            }

            BaseDrawing.DrawTexture(spriteBatch, BladeTex, 0, npc.position, npc.width, npc.height, npc.scale, npc.rotation, 0, 1, new Rectangle(0, 0, BladeTex.Width, BladeTex.Height), drawColor, true);
            BaseDrawing.DrawTexture(spriteBatch, BladeGlowTex, r, npc.position, npc.width, npc.height, npc.scale, npc.rotation, 0, 1, new Rectangle(0, 0, BladeTex.Width, BladeTex.Height), Color.White, true);

            BaseDrawing.DrawTexture(spriteBatch, texture2D13, 0, npc.position, npc.width, npc.height, npc.scale, 0, 0, 1, new Rectangle(0, 0, texture2D13.Width, texture2D13.Height), drawColor, true);
            BaseDrawing.DrawTexture(spriteBatch, GlowTex, r, npc.position, npc.width, npc.height, npc.scale, 0, 0, 1, new Rectangle(0, 0, texture2D13.Width, texture2D13.Height), Color.White, true);
            BaseDrawing.DrawTexture(spriteBatch, Heart, 0, npc.position, npc.width, npc.height, npc.scale, 0, 0, 1, new Rectangle(0, 0, texture2D13.Width, texture2D13.Height), Colors.COLOR_GLOWPULSE, true);

            return false;
        }
    }
}