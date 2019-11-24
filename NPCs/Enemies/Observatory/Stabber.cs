using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CSkies.NPCs.Enemies.Observatory
{
    public class Stabber : ModNPC
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Stabber");
            Main.npcFrameCount[npc.type] = 5;
        }
        public override void SetDefaults()
        {
            npc.width = 54;
            npc.height = 38;
            npc.defense = 20;
            npc.lifeMax = 50;
            npc.HitSound = SoundID.NPCHit1;
            npc.DeathSound = SoundID.NPCDeath1;
            npc.knockBackResist = 0.5f;
            npc.npcSlots = 0f;
            npc.aiStyle = -1;
            npc.dontTakeDamageFromHostiles = true;
            npc.damage = 40;
            npc.catchItem = (short)mod.ItemType("Sweeper");
        }

        int status = -1;
        
        public override void AI()
        {
            Lighting.AddLight(npc.Center, Color.LimeGreen.R / 150, Color.LimeGreen.G / 150, Color.LimeGreen.B / 150);
            BaseAI.AISnail(npc, ref npc.ai, ref status, 0.9f, 0.1f);
        }

        public override void HitEffect(int hitDirection, double damage)
        {
            if (npc.life <= 0)
            {
                Gore.NewGore(npc.position, npc.velocity, 77, 1f);
                Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/Sweeper1"), 1f);
                Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/Sweeper2"), 1f);
            }
        }

        public override void FindFrame(int frameHeight)
        {
            if (npc.frameCounter++ > 6)
            {
                npc.frameCounter = 0;
                npc.frame.Y += frameHeight;
                if (npc.frame.Y > frameHeight * 4)
                {
                    npc.frame.Y = 0;
                }
            }
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Color drawColor)
        {
            BaseDrawing.DrawTexture(spriteBatch, Main.npcTexture[npc.type], 0, npc, drawColor, false);
            BaseDrawing.DrawTexture(spriteBatch, mod.GetTexture("Glowmasks/Stabber_Glow"), 0, npc, Color.White, false);
            return false;
        }
    }
}