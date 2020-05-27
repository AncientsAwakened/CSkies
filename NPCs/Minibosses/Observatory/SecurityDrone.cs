using Terraria;
using Terraria.ModLoader;
using System;
using Microsoft.Xna.Framework;
using Terraria.ID;
using Microsoft.Xna.Framework.Graphics;
using System.IO;

namespace CSkies.NPCs.Minibosses
{
    [AutoloadBossHead]
    public class SecurityDrone : ModNPC
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Security Drone");
            Main.npcFrameCount[npc.type] = 4;
        }

        public override void SetDefaults()
        {
            npc.width = 70;
            npc.height = 136;
            npc.value = BaseUtility.CalcValue(0, 10, 0, 0);
            npc.npcSlots = 1000;
            npc.aiStyle = -1;
            npc.lifeMax = 12000;
            npc.defense = 10;
            npc.damage = 30;
            npc.knockBackResist = 0f;
            npc.noGravity = true;
            npc.HitSound = SoundID.NPCHit1;
            npc.DeathSound = SoundID.NPCDeath1;
            npc.boss = false;
            npc.alpha = 255;
            npc.noTileCollide = true;
            npc.value = Item.sellPrice(0, 1, 0, 0);
        }

        public override void ScaleExpertStats(int numPlayers, float bossLifeScale)
        {
            npc.lifeMax = (int)(npc.lifeMax * 0.6f * bossLifeScale);
            npc.damage = (int)(npc.damage * 0.6f);
        }

        public float[] internalAI = new float[4];
        public bool Hostile = false;

        public override void SendExtraAI(BinaryWriter writer)
        {
            base.SendExtraAI(writer);
            if (Main.netMode == NetmodeID.Server || Main.dedServ)
            {
                writer.Write(internalAI[0]);
                writer.Write(internalAI[1]);
                writer.Write(internalAI[2]);
                writer.Write(internalAI[3]);
                writer.Write(Hostile);
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
                Hostile = reader.ReadBool();
            }
        }

        public int StarCount = Main.expertMode ? 6 : 4;

        public override void AI()
        {
            Vector3 rgb = Color.LimeGreen.ToVector3();
            rgb *= 0.65f;
            Lighting.AddLight(npc.Center, rgb);

            if (!Hostile)
            {
                PassiveAI();
            }
            npc.boss = true;
            music = mod.GetSoundSlot(SoundType.Music, "Sounds/Music/CyberBoss");

            if (!npc.HasPlayerTarget)
            {
                npc.TargetClosest();
            }

            Player player = Main.player[npc.target];

            BaseAI.AISpaceOctopus(npc, ref npc.ai, .2f, 6, 270, 70, null);

            BaseAI.LookAt(player.Center, npc.Center, ref npc.rotation, ref npc.spriteDirection, 0, 0, .05f);

        }

        public void PassiveAI()
        {
            if (npc.direction == 0)
            {
                npc.TargetClosest();
                npc.netUpdate = true;
            }
            if (npc.collideX)
            {
                npc.direction = -npc.direction;
                npc.netUpdate = true;
            }
            npc.velocity.X = 3f * npc.direction;
            Vector2 center24 = npc.Center;
            Point point9 = center24.ToTileCoordinates();
            int num1250 = 30;
            if (WorldGen.InWorld(point9.X, point9.Y, 30))
            {
                for (int num1251 = 0; num1251 < 30; num1251++)
                {
                    if (WorldGen.SolidTile(point9.X, point9.Y + num1251))
                    {
                        num1250 = num1251;
                        break;
                    }
                }
            }
            if (num1250 < 15)
            {
                npc.velocity.Y = Math.Max(npc.velocity.Y - 0.05f, -3.5f);
            }
            else if (num1250 < 20)
            {
                npc.velocity.Y *= 0.95f;
            }
            else
            {
                npc.velocity.Y = Math.Min(npc.velocity.Y + 0.05f, 1.5f);
            }
            int num1252 = npc.FindClosestPlayer(out float distanceToPlayer);
            if (num1252 == -1 || Main.player[num1252].dead)
            {
                return;
            }
            if (distanceToPlayer < 352f && Main.player[num1252].Center.Y > npc.Center.Y)
            {
                Hostile = true;
                npc.netUpdate = true;
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
            potionType = ItemID.GreaterHealingPotion;
        }

        public override void NPCLoot()
        {

        }

        public override void HitEffect(int hitDirection, double damage)
        {
            if (Main.netMode == NetmodeID.Server) { return; }
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
            (npc.position - Main.screenPosition + Vector2.UnitY * npc.gfxOffY).Floor();
            float num74 = 5f;
            for (int num75 = 0; num75 < num74; num75++)
            {
                float num76 = 1f - (Main.GlobalTime + num75) % num74 / num74;
                Color color21 = Color.LimeGreen;
                if (npc.ai[0] == 1f)
                {
                    color21 = Color.Lerp(Color.LimeGreen, Color.Red, MathHelper.Clamp(npc.ai[1] / 20f, 0f, 1f));
                }
                if (npc.ai[0] == 2f)
                {
                    color21 = Color.Red;
                }
                color21 *= 1f - num76;
                color21.A = 0;
                for (int num77 = 0; num77 < 2; num77++)
                {
                    sb.Draw(Main.extraTexture[27], npc.Center - Main.screenPosition + Vector2.UnitY * (npc.gfxOffY - 4f + 6f), null, color21, (float)Math.PI / 2f, new Vector2(10f, 48f), num76 * 4f, SpriteEffects.None, 0f);
                }
            }
            BaseDrawing.DrawTexture(sb, tex, 0, npc, dColor);
            return false;
        }
    }
}