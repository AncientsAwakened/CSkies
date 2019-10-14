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
            npc.defense = 60;
            npc.lifeMax = 150000;
            npc.value = Item.sellPrice(0, 12, 0, 0);
            npc.HitSound = new LegacySoundStyle(21, 1);
            npc.DeathSound = new LegacySoundStyle(4, 14, Terraria.Audio.SoundType.Sound);
            npc.knockBackResist = 0f;
            npc.noGravity = true;
            npc.netAlways = true;
            npc.boss = true;
            npc.noTileCollide = true;
            music = mod.GetSoundSlot(Terraria.ModLoader.SoundType.Music, "Sounds/Music/FurySoul");
        }

        public override void ScaleExpertStats(int numPlayers, float bossLifeScale)
        {
            npc.lifeMax = (int)(npc.lifeMax * 0.5f * bossLifeScale);
            npc.damage = (int)(npc.damage * 0.5f);
        }

        public float[] Shoot = new float[1];
        public float[] Move = new float[1];
        public float[] Movement = new float[4];

        public override void SendExtraAI(BinaryWriter writer)
        {
            base.SendExtraAI(writer);
            if (Main.netMode == NetmodeID.Server || Main.dedServ)
            {
                writer.Write(Shoot[0]);
                writer.Write(Move[0]);
                writer.Write(Movement[0]);
                writer.Write(Movement[1]);
                writer.Write(Movement[2]);
                writer.Write(Movement[3]);
            }
        }

        public override void ReceiveExtraAI(BinaryReader reader)
        {
            base.ReceiveExtraAI(reader);
            if (Main.netMode == NetmodeID.MultiplayerClient)
            {
                Shoot[0] = reader.ReadFloat();
                Move[0] = reader.ReadFloat();
                Movement[0] = reader.ReadFloat();
                Movement[1] = reader.ReadFloat();
                Movement[2] = reader.ReadFloat();
                Movement[3] = reader.ReadFloat();
            }
        }

        public override void NPCLoot()
        {
            for (int num468 = 0; num468 < 12; num468++)
            {
                int num469 = Dust.NewDust(new Vector2(npc.position.X, npc.position.Y), npc.width, npc.height, DustID.SolarFlare, -npc.velocity.X * 0.2f,
                    -npc.velocity.Y * 0.2f, 100, default, 2f);
                Main.dust[num469].noGravity = true;
            }
        }

        public static Color Flame => BaseUtility.MultiLerpColor(Main.LocalPlayer.miscCounter % 100 / 100f, Color.Orange, Color.Red, Color.Orange);

        float scale = 0;
        float rot = 0;


        public override void AI()
        {

            Lighting.AddLight(npc.Center, Flame.R / 150, Flame.G / 150, Flame.B / 150);

            if (!npc.HasPlayerTarget)
            {
                npc.TargetClosest();
            }

            Player player = Main.player[npc.target];

            if (Vector2.Distance(player.Center, npc.Center) < 204 && !player.dead && player.active)
            {
                player.AddBuff(ModContent.BuffType<Buffs.Heartburn>(), 2);
            }

            switch (Move[0])
            {
                case 0:
                    BaseAI.AISpaceOctopus(npc, ref Movement, 0.3f, 12f, 350f, 70f, null);
                    if (npc.ai[0]++ > 600)
                    {
                        Move[0] = Main.rand.Next(2) == 0 ? 1 : 2;
                    }
                    break;
                case 1:
                    BaseAI.AIWeapon(npc, ref Movement, ref npc.rotation, player.position, false, 120, 120, 10f, 3f, 2f);
                    if (npc.ai[0]++ > 900)
                    {
                        npc.ai[0] = 0;
                        Move[0] = 0;
                    }
                    break;
                case 2:
                    BaseAI.AIShadowflameGhost(npc, ref Movement, false, 330f, 0.6f, 12f, 0.3f, 7f, 4f, 15f, 0.4f, 0.4f, 0.95f, 5f);
                    if (npc.ai[0]++ > 900)
                    {
                        npc.ai[0] = 0;
                        Move[0] = 0;
                    }
                    break;
            }

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
                                    if (npc.life < (int)(npc.lifeMax * .4f) && Main.rand.Next(2) == 0)
                                    {
                                        Projectile.NewProjectile(npc.Center.X, npc.Center.Y, baseSpeed * (float)Math.Sin(offsetAngle), baseSpeed * (float)Math.Cos(offsetAngle), ModContent.ProjectileType<BigHeartshot>(), npc.damage / 4, 5, Main.myPlayer);
                                    }
                                    else
                                    {
                                        Projectile.NewProjectile(npc.Center.X, npc.Center.Y, baseSpeed * (float)Math.Sin(offsetAngle), baseSpeed * (float)Math.Cos(offsetAngle), ModContent.ProjectileType<Heartshot>(), npc.damage / 4, 5, Main.myPlayer);
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
                                Projectile.NewProjectile(npc.Center.X, npc.Center.Y, baseSpeed * (float)Math.Sin(offsetAngle), baseSpeed * (float)Math.Cos(offsetAngle), ModContent.ProjectileType<Heartshot>(), npc.damage / 4, 12, Main.myPlayer);
                            }
                            npc.ai[2] = 0;
                            Shoot[0] = Main.rand.Next(4);
                        }
                        break;
                    case 1:
                        float spread1 = 12f * 0.0174f;
                        double startAngle1 = Math.Atan2(npc.velocity.X, npc.velocity.Y) - spread1 / 2;
                        double deltaAngle1 = spread1 / 10f;
                        for (int i = 0; i < 10; i++)
                        {
                            double offsetAngle1 = (startAngle1 + deltaAngle1 * (i + i * i) / 2f) + 32f * i;
                            Projectile.NewProjectile(npc.Center.X, npc.Center.Y, (float)(Math.Sin(offsetAngle1) * 8f), (float)(Math.Cos(offsetAngle1) * 8f), mod.ProjectileType("BrimstoneBarrage"), npc.damage / 4, 6);
                            Projectile.NewProjectile(npc.Center.X, npc.Center.Y, (float)(-Math.Sin(offsetAngle1) * 8f), (float)(-Math.Cos(offsetAngle1) * 8f), mod.ProjectileType("BrimstoneBarrage"), npc.damage / 4, 6);
                        }
                        npc.ai[2] = 0;
                        Shoot[0] = Main.rand.Next(4);
                        break;
                    case 2:
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

            rot += .3f;

            if (scale >= 1f)
            {
                scale = 1f;
            }
            else
            {
                scale += .02f;
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
                BaseDrawing.DrawTexture(sb, RitualTex, r, npc.position, npc.width, npc.height, scale, rot, 0, 1, new Rectangle(0, 0, RitualTex.Width, RitualTex.Height), lightColor, true);
                BaseDrawing.DrawTexture(sb, RingTex1, r, npc.position, npc.width, npc.height, scale, -rot, 0, 1, new Rectangle(0, 0, RingTex1.Width, RingTex1.Height), lightColor, true);
                BaseDrawing.DrawTexture(sb, RingTex2, r, npc.position, npc.width, npc.height, scale, rot, 0, 1, new Rectangle(0, 0, RingTex1.Width, RingTex1.Height), lightColor, true);
            }

            BaseDrawing.DrawTexture(sb, RingTex, r, npc.position, npc.width, npc.height, npc.scale, npc.rotation, 0, 1, new Rectangle(0, 0, RingTex.Width, RingTex.Height), Color.White, true);
            BaseDrawing.DrawTexture(sb, texture2D13, 0, npc.position, npc.width, npc.height, npc.scale, 0, 0, 1, new Rectangle(0, 0, texture2D13.Width, texture2D13.Height), lightColor, true);
            BaseDrawing.DrawAura(sb, texture2D13, 0, npc.position, npc.width, npc.height, auraPercent, 1.5f, 1f, 0, npc.direction, 4, npc.frame, 0f, 0f, npc.GetAlpha(Color.White));

            return false;
        }
    }
}