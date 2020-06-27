using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using CSkies.NPCs.Bosses.Heartcore;
using Terraria.Graphics.Shaders;

namespace CSkies.NPCs.Bosses.FurySoul
{
	public class TeleportLocation : ModNPC
	{
		public override void SetStaticDefaults()
		{
            DisplayName.SetDefault("Ring of Fire");
		}		

        public override void SetDefaults()
        {
            npc.width = 90;
            npc.height = 90;
            npc.npcSlots = 0;
            npc.aiStyle = -1;
            npc.lifeMax = 100;
            npc.noTileCollide = true;
            npc.dontTakeDamage = true;
            npc.scale = 0;
            npc.noGravity = true;
        }

        public override void AI()
		{
            npc.rotation = Main.npc[(int)npc.ai[0]].rotation;

            if (npc.scale < 1)
            {
                if (Main.npc[(int)npc.ai[0]].ai[0] == 3)
                {
                    npc.scale += .1f;
                }
                else
                {
                    npc.scale += .05f;
                }
            }
            else
            {
                npc.scale = 1;
            }
        }

        public override bool PreDraw(SpriteBatch sb, Color dColor)
		{
            Texture2D Tex = Main.npcTexture[npc.type];
            int r = GameShaders.Armor.GetShaderIdFromItemId(ItemID.LivingFlameDye);

            Rectangle f = BaseDrawing.GetFrame(0, Main.npcTexture[npc.type].Width, Main.npcTexture[npc.type].Height / Main.npcFrameCount[npc.type], 0, 0);

            BaseDrawing.DrawTexture(sb, Tex, r, npc.position, npc.width, npc.height, npc.scale, npc.rotation, npc.direction, 1, f, npc.GetAlpha(Colors.COLOR_GLOWPULSE), true);
            return false;
		}
	}
}