using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;

namespace CSkies.NPCs.Bosses.FurySoul
{
    public class FurySoulTransition : ModNPC
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("???");
            Main.npcFrameCount[npc.type] = 15;
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
            music = mod.GetSoundSlot(SoundType.Music, "Sounds/Music/FurySoul");
        }

        public override void AI()
        {
            if (++npc.ai[0] >= 12 && Main.netMode != 1)
            {
                if (++npc.ai[1] >= 15)
                {
                    int n = NPC.NewNPC((int)npc.Center.X, (int)npc.Center.Y, ModContent.NPCType<FurySoul>(), 0, 10) ;
                    Main.npc[n].Center = npc.Center;
                    Main.npc[n].velocity = npc.velocity;
                    int p = Projectile.NewProjectile(npc.Center.X, npc.Center.Y, 0f, 0f, mod.ProjectileType("ShockwaveBoom"), 0, 1, Main.myPlayer, 0, 12);
                    Main.projectile[p].Center = npc.Center;
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

        public float auraPercent = 0f;
        public bool auraDirection = true;

        public override bool PreDraw(SpriteBatch sb, Color dColor)
        {
            Texture2D tex = Main.npcTexture[npc.type];

            if (auraDirection) { auraPercent += 0.1f; auraDirection = auraPercent < 1f; }
            else { auraPercent -= 0.1f; auraDirection = auraPercent <= 0f; }

            BaseDrawing.DrawAura(sb, tex, 0, npc, auraPercent, 2f, 0f, 0f, npc.GetAlpha(Color.White));
            return false;
        }
    }
}