using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Audio;
using Terraria.Graphics.Shaders;
using Terraria.ID;
using Terraria.ModLoader;
using CSkies.NPCs.Bosses.Heartcore;
using System;

namespace CSkies.NPCs.Minibosses
{
    public class MagmaHeart : ModNPC
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Magma Heart");
        }
        public override void SetDefaults()
        {
            npc.npcSlots = 100;
            npc.width = 26;
            npc.height = 24;
            npc.aiStyle = -1;
            npc.damage = 80;
            npc.defense = 0;
            npc.lifeMax = 30000;
            npc.value = Item.sellPrice(0, 1, 0, 0);
            npc.HitSound = new LegacySoundStyle(21, 1);
            npc.DeathSound = new LegacySoundStyle(4, 14, Terraria.Audio.SoundType.Sound);
            npc.knockBackResist = 0f;
            npc.noGravity = true;
            npc.netAlways = true;
            npc.boss = true;
            npc.noTileCollide = true;
            music = mod.GetSoundSlot(Terraria.ModLoader.SoundType.Music, "Sounds/Music/Heart");
        }

        public override void ScaleExpertStats(int numPlayers, float bossLifeScale)
        {
            npc.lifeMax = (int)(npc.lifeMax * 0.5f * bossLifeScale);
            npc.damage = (int)(npc.damage * 0.5f);
        }

        public override void NPCLoot()
        {
            CWorld.downedHeart = true;
            npc.DropLoot(ModContent.ItemType<Items.Materials.MoltenHeart>());
            for (int num468 = 0; num468 < 12; num468++)
            {
                int num469 = Dust.NewDust(new Vector2(npc.position.X, npc.position.Y), npc.width, npc.height, DustID.SolarFlare, -npc.velocity.X * 0.2f,
                    -npc.velocity.Y * 0.2f, 100, default, 2f);
                Main.dust[num469].noGravity = true;
            }
        }

        public static Color Flame => BaseUtility.MultiLerpColor(Main.LocalPlayer.miscCounter % 100 / 100f, Color.Orange, Color.Red, Color.Orange);

        float scale = 0;


        public override void AI()
        {
            npc.rotation += npc.velocity.X / 20;
            Lighting.AddLight(npc.Center, Flame.R / 150, Flame.G / 150, Flame.B / 150);

            if (!npc.HasPlayerTarget)
            {
                npc.TargetClosest();
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

            if (npc.ai[3]++ < 380)
            {
                BaseAI.AISkull(npc, ref npc.ai, true, 10, 250, .015f, .025f);
                float spread = 45f * 0.0174f;
                Vector2 dir = Vector2.Normalize(player.Center - npc.Center);
                dir *= 12f;
                float baseSpeed = (float)Math.Sqrt((dir.X * dir.X) + (dir.Y * dir.Y));
                double startAngle = Math.Atan2(dir.X, dir.Y) - .1d;
                double deltaAngle = spread / 6f;
                if (npc.ai[2]++ > 120)
                {
                    for (int i = 0; i < 3; i++)
                    {
                        if (npc.ai[2] % 30 == 0)
                        {
                            double offsetAngle = startAngle + (deltaAngle * i);
                            int p = Projectile.NewProjectile(npc.Center.X, npc.Center.Y, baseSpeed * (float)Math.Sin(offsetAngle), baseSpeed * (float)Math.Cos(offsetAngle), ModContent.ProjectileType<Fireshot>(), npc.damage / 4, 5, Main.myPlayer);
                            Main.projectile[p].tileCollide = false;
                        }
                    }
                    npc.ai[2] = 0;
                }
            }
            else
            {
                npc.ai[2] = 0;
                BaseAI.AIFlier(npc, ref npc.ai, true, 0.15f, 0.08f, 6f, 4f, false, 300);
            }

            if (npc.ai[3] > 500)
            {
                npc.ai[3] = 0;
            }
        }

        public override void OnHitPlayer(Player target, int damage, bool crit)
        {
            target.AddBuff(ModContent.BuffType<Buffs.Heartburn>(), 120);
        }

        public override bool PreDraw(SpriteBatch sb, Color lightColor)
        {
            int r = GameShaders.Armor.GetShaderIdFromItemId(ItemID.LivingFlameDye);

            Texture2D texture2D13 = Main.npcTexture[npc.type];
            Texture2D Glow = mod.GetTexture("Glowmasks/MHeart_Glow");
            Texture2D RingTex = mod.GetTexture("NPCs/Minibosses/HeartRing");

            BaseDrawing.DrawTexture(sb, texture2D13, r, npc.position, npc.width, npc.height, 1f, 0, 0, 1, new Rectangle(0, 0, texture2D13.Width, texture2D13.Height), npc.GetAlpha(lightColor), true);
            BaseDrawing.DrawTexture(sb, Glow, r, npc.position, npc.width, npc.height, 1f, 0, 0, 1, new Rectangle(0, 0, RingTex.Width, RingTex.Height), npc.GetAlpha(Colors.COLOR_GLOWPULSE), true);

            if (scale > 0)
            {
                BaseDrawing.DrawTexture(sb, RingTex, r, npc.position, npc.width, npc.height, scale, npc.rotation, 0, 1, new Rectangle(0, 0, RingTex.Width, RingTex.Height), Color.White, true);
            }

            return false;
        }
    }
}