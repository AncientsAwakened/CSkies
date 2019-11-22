using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CSkies.NPCs.Bosses.Enigma
{
	public class SaucerMinion : ModNPC
	{
        public override void SetStaticDefaults()
		{
            Main.npcFrameCount[npc.type] = 4;		
		}			
		
        public override void SetDefaults()
        {
            npc.width = 60;
            npc.height = 40;
            npc.value = BaseUtility.CalcValue(0, 0, 10, 0);
            npc.npcSlots = 1;
			npc.aiStyle = -1;
            npc.lifeMax = 500;
            npc.defense = 20;
            npc.damage = 55;
            npc.knockBackResist = 0.3f;
			npc.noGravity = true;
            npc.HitSound = SoundID.NPCHit1;
            npc.DeathSound = SoundID.NPCDeath1;
            npc.noTileCollide = true;
        }

        public bool Friendly = true;

		public override void AI()
		{
            BaseAI.AIFlier(npc, ref npc.ai, true, 0.15f, 0.08f, 8f, 7f, false, 300);

            if (npc.alpha > 0)
            {
                npc.alpha -= 4;
            }
            else
            {
                npc.alpha = 0;
            }

            npc.spriteDirection = npc.direction;
            npc.rotation = npc.velocity.X * 0.05f;
        }

		public override void FindFrame(int frameHeight)
		{
            npc.frameCounter++;
            if (npc.frameCounter > 8)
            {
                npc.frameCounter = 0;
                npc.frame.Y += frameHeight;
                if (npc.frame.Y > frameHeight * 3)
                {
                    npc.frame.Y = 0;
                }
            }
        }
    }
}