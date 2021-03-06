using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace CSkies.NPCs.Bosses.Void
{
    public class VoidTransition1 : ModNPC
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("???");
            Main.npcFrameCount[npc.type] = 18;
        }

        public override void SetDefaults()
        {
            npc.width = 230;
            npc.height = 142;
            npc.friendly = false;
            npc.lifeMax = 1;
            npc.dontTakeDamage = true;
            npc.noTileCollide = true;
            npc.noGravity = true;
            npc.aiStyle = -1;
            npc.timeLeft = 10;
            for (int k = 0; k < npc.buffImmune.Length; k++)
            {
                npc.buffImmune[k] = true;
            }
            music = mod.GetSoundSlot(SoundType.Music, "Sounds/Music/Void");
        }

        public override void AI()
        {
            if (++npc.ai[0] >= 5 && Main.netMode != NetmodeID.MultiplayerClient)
            {
                if (++npc.ai[1] >= 18)
                {
                    int n = NPC.NewNPC((int)npc.Center.X, (int)npc.Center.Y, ModContent.NPCType<VoidTransition2>());
                    Main.npc[n].Center = npc.Center;
                    Main.npc[n].velocity = npc.velocity;
                    npc.active = false;
                }
                npc.ai[0] = 0;
                npc.netUpdate = true;
            }
            npc.velocity *= .98f;
        }

        public override void FindFrame(int frameHeight)
        {
            npc.frame.Y = frameHeight *= (int)npc.ai[1];
        }

        public float auraPercent = 0f;
        public bool auraDirection = true;

        public override bool PreDraw(SpriteBatch sb, Color dColor)
        {
            Texture2D tex = Main.npcTexture[npc.type];
            BaseDrawing.DrawTexture(sb, tex, 0, npc, npc.GetAlpha(Color.White));
            BaseDrawing.DrawAura(sb, tex, 0, npc, auraPercent, 2f, 0f, 0f, npc.GetAlpha(Color.White));
            return false;
        }
    }
}