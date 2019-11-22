using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
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
            npc.width = 64;
            npc.height = 64;
            npc.value = BaseUtility.CalcValue(0, 0, 0, 0);
            npc.npcSlots = 1;
			npc.aiStyle = -1;
            npc.lifeMax = 2300;
            npc.defense = 20;
            npc.damage = 45;
            npc.knockBackResist = 0.3f;
			npc.noGravity = true;
            npc.HitSound = new LegacySoundStyle(3, 4, Terraria.Audio.SoundType.Sound);
            npc.DeathSound = new LegacySoundStyle(4, 14, Terraria.Audio.SoundType.Sound);
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