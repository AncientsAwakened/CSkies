using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CSkies.NPCs.Enemies
{
    public class Starprobe : ModNPC
    {
        public override void SetDefaults()
        {
            npc.width = 30;
            npc.height = 30;
            npc.aiStyle = 5;
            npc.damage = 50;
            npc.defense = 20;
            npc.lifeMax = 150;
            npc.HitSound = SoundID.NPCHit4;
            npc.DeathSound = SoundID.NPCDeath14;
            npc.noGravity = true;
            npc.knockBackResist = 0.8f;
            npc.noTileCollide = true;
        }

        public override void HitEffect(int hitDirection, double damage)
        {
            if (npc.life <= 0)
            {
                for (int num468 = 0; num468 < 4; num468++)
                {
                    int num469 = Dust.NewDust(new Vector2(npc.position.X, npc.position.Y), npc.width, npc.height, ModContent.DustType<Dusts.StarDust>(), -npc.velocity.X * 0.2f,
                        -npc.velocity.Y * 0.2f, 100, default, .8f);
                    Main.dust[num469].noGravity = true;
                }
            }
        }

        public override void AI()
        {
            if (!npc.HasPlayerTarget)
            {
                npc.TargetClosest();
            }
            Player player = Main.player[npc.target];

            Vector2 vector2 = new Vector2(npc.position.X + (npc.width * 0.5f), npc.position.Y + (npc.height * 0.5f));
            float num1 = Main.player[npc.target].position.X + (player.width / 2) - vector2.X;
            float num2 = Main.player[npc.target].position.Y + (player.height / 2) - vector2.Y;
            npc.rotation = (float)Math.Atan2(num2, num1);


            BaseAI.AISkull(npc, ref npc.ai, true, 6f, 350f, 0.1f, 0.15f);

            bool playerActive = player != null && player.active && !player.dead;
            if (Main.netMode != 1 && playerActive)
            {
                npc.ai[3]++;
                if (npc.ai[3] >= 90)
                {
                    npc.ai[3] = 0;
                    int projType = mod.ProjType("Starshot");
                    if (Collision.CanHit(npc.position, npc.width, npc.height, player.position, player.width, player.height))
                        BaseAI.FireProjectile(player.Center, npc, projType, (int)(npc.damage * 0.25f), 0f, 2f);
                }
            }
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Color drawColor)
        {
            Texture2D tex = Main.npcTexture[npc.type];
            Texture2D glowTex = mod.GetTexture("Glowmasks/Starprobe_Glow");
            BaseDrawing.DrawTexture(spriteBatch, tex, 0, npc, drawColor);
            BaseDrawing.DrawTexture(spriteBatch, glowTex, 0, npc, Colors.COLOR_GLOWPULSE);
            return false;
        }

        public override void NPCLoot()
        {
            Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("CosmicStar"));
        }
    }
}
