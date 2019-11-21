using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Localization;

namespace CSkies.NPCs.Critters
{
    public class Sweeper : ModNPC
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Sweeper");
            Main.npcFrameCount[npc.type] = 5;
        }
        public override void SetDefaults()
        {
            npc.width = 44;
            npc.height = 38;
            npc.defense = 20;
            npc.lifeMax = 50;
            npc.HitSound = SoundID.NPCHit1;
            npc.DeathSound = SoundID.NPCDeath1;
            npc.knockBackResist = 0.5f;
            npc.npcSlots = 0f;
            npc.aiStyle = -1;
            npc.dontTakeDamageFromHostiles = true;
            npc.catchItem = (short)mod.ItemType("Sweeper");
        }

        int status = -1;
        
        public override void AI()
        {
            BaseAI.AISnail(npc, ref npc.ai, ref status, 0.3f, 0.1f);
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
        
        public override bool CanHitPlayer(Player target, ref int cooldownSlot)
        {
            return false;
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
    }
}