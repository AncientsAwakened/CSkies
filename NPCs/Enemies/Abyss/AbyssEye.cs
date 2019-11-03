using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CSkies.NPCs.Enemies.Abyss
{
    public class AbyssEye : ModNPC
    {
		public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Eldritch Eye");
            Main.npcFrameCount[npc.type] = 2;
		}		
		
        public override void SetDefaults()
        {
            npc.width = 42;
            npc.height = 38;
            npc.value = BaseUtility.CalcValue(0, 0, 80, 0);
            npc.npcSlots = 1;
            npc.aiStyle = -1;
            npc.lifeMax = 500;
            npc.defense = 30;
            npc.damage = 50;
            npc.HitSound = SoundID.NPCHit1;
            npc.DeathSound = SoundID.NPCDeath1;
            npc.knockBackResist = 0.7f;
        }

		public override void NPCLoot()
		{
			BaseAI.DropItem(npc, mod.ItemType("VoidLens"), 1, 1, 30, true);
		}

        public override void HitEffect(int hitDirection, double damage)
		{
			if (Main.netMode == 2) { return; }
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
                if (npc.frame.Y > frameheight)
                {
                    npc.frame.Y = 0;
                }
            }
		}

		public override void AI()
		{
			npc.noGravity = true;
			npc.TargetClosest(true);

			for (int m = npc.oldPos.Length - 1; m > 0; m--)
			{
				npc.oldPos[m] = npc.oldPos[m - 1];
			}
			npc.oldPos[0] = npc.position;

            BaseAI.AIEye(npc, ref npc.ai, false, true, .2f, .1f, 7, 3);
            BaseAI.Look(npc, 0);
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