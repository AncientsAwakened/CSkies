using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CSkies.NPCs.Enemies.Abyss
{
    public class FlailingHate : ModNPC
    {
		public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Flailing Hate");
            Main.npcFrameCount[npc.type] = 5;
		}		
		
        public override void SetDefaults()
        {
            npc.width = 72;
            npc.height = 74;
            npc.value = BaseUtility.CalcValue(0, 1, 0, 0);
            npc.npcSlots = 1;
            npc.aiStyle = -1;
            npc.lifeMax = 1200;
            npc.defense = 40;
            npc.damage = 60;
            npc.HitSound = SoundID.NPCHit1;
            npc.DeathSound = SoundID.NPCDeath1;
            npc.knockBackResist = 0.4f;
        }

        public override void HitEffect(int hitDirection, double damage)
		{
			if (Main.netMode == NetmodeID.Server) { return; }
			if (npc.life <= 0)
			{
				for (int m = 0; m < 8; m++)
                {
                    Dust.NewDust(npc.position, npc.width, npc.height, ModContent.DustType<Dusts.VoidDust>());
                }
			}else
			{
				for (int m = 0; m < 2; m++)
                {
                    Dust.NewDust(npc.position, npc.width, npc.height, ModContent.DustType<Dusts.VoidDust>());
                }
			}
		}

		public override void FindFrame(int frameheight)
		{
            if (npc.frameCounter++ >= 4)
            {
                npc.frameCounter = 0;
                npc.frame.Y += frameheight;
                if (npc.frame.Y > 3)
                {
                    npc.frame.Y = 0;
                }
            }
		}

        public override void AI()
        {
            if (!npc.HasPlayerTarget)
            {
                npc.TargetClosest();
            }

            BaseAI.AIFlier(npc, ref npc.ai, true, 0.15f, 0.08f, 8f, 7f, true, 300);

            npc.spriteDirection = npc.direction;
            npc.rotation = npc.velocity.X * 0.05f;

			for (int m = npc.oldPos.Length - 1; m > 0; m--)
			{
				npc.oldPos[m] = npc.oldPos[m - 1];
			}
			npc.oldPos[0] = npc.position;
        }

		public override bool PreDraw(SpriteBatch sb, Color dColor)
        {
            Texture2D bodyTex = Main.npcTexture[npc.type];
            Color lightColor = BaseDrawing.GetNPCColor(npc, null);
			BaseDrawing.DrawAfterimage(sb, bodyTex, 0, npc, 2.5f, 0.9F, 3, true, 0f, 0f, lightColor);
			BaseDrawing.DrawTexture(sb, bodyTex, 0, npc, Colors.COLOR_GLOWPULSE);
			return false;
		}
	}
}