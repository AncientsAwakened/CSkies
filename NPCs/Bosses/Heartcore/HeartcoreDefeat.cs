using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace CSkies.NPCs.Bosses.Heartcore
{
    public class HeartcoreDefeat : ModNPC
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Broken Heart");
            Main.npcFrameCount[npc.type] = 23;
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
            music = mod.GetSoundSlot(SoundType.Music, "Sounds/Music/silence");
        }

        public override void AI()
        {
            if (++npc.ai[0] >= 12 && Main.netMode != 1)
            {
                if (++npc.ai[1] >= 23)
                {
                    npc.active = false;
                }
                npc.ai[0] = 0;
                npc.netUpdate = true;
            }
            npc.velocity *= 0;
        }

        public override void FindFrame(int frameHeight)
        {
            npc.frame.Y = frameHeight *= (int)npc.ai[1];
        }

        public override Color? GetAlpha(Color drawColor)
        {
            return Color.White;
        }
    }
}