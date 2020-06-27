using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Terraria.Graphics.Shaders;

namespace CSkies.NPCs.Bosses.FurySoul
{
    public class FurySoulDeath : ModNPC
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Fury's End");
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
            music = mod.GetSoundSlot(SoundType.Music, "Sounds/Music/silence");
        }

        public override void AI()
        {
            if (scale > 0)
            {
                scale *= .96f;
            }
            if (++npc.ai[0] >= 240)
            {
                npc.active = false;
            }
            if (npc.frame.Y == 18)
            {
                int p = Projectile.NewProjectile(npc.Center.X, npc.Center.Y, 0f, 0f, mod.ProjectileType("ShockwaveBoom"), 0, 1, Main.myPlayer, 0, 12);
                Main.projectile[p].Center = npc.Center;
            }
            if (npc.frame.Y > 18)
            {
                npc.alpha = 255;
            }
            npc.velocity *= 0;
        }

        public override void FindFrame(int frameHeight)
        {
            if (npc.frameCounter++ >= 5)
            {
                npc.frameCounter = 0;

                npc.frame.Y += frameHeight;

                if (npc.ai[0] < 120 && npc.frame.Y > frameHeight * 6)
                {
                    npc.frame.Y = frameHeight * 6;
                }
            }
        }

        public float auraPercent = 0f;
        public bool auraDirection = true;
        public float scale = 1;

        public override bool PreDraw(SpriteBatch sb, Color lightColor)
        {
            int r = GameShaders.Armor.GetShaderIdFromItemId(ItemID.LivingFlameDye);

            if (auraDirection) { auraPercent += 0.1f; auraDirection = auraPercent < 1f; }
            else { auraPercent -= 0.1f; auraDirection = auraPercent <= 0f; }

            Texture2D texture2D13 = Main.npcTexture[npc.type];
            Texture2D RingTex = mod.GetTexture("NPCs/Bosses/FurySoul/FuryRing");

            Texture2D RingTex1 = mod.GetTexture("NPCs/Bosses/Heartcore/Ring1");
            Texture2D RingTex2 = mod.GetTexture("NPCs/Bosses/Heartcore/Ring2");
            Texture2D RitualTex = mod.GetTexture("NPCs/Bosses/Heartcore/Ritual");

            if (scale > 0)
            {
                BaseDrawing.DrawTexture(sb, RitualTex, r, npc.position, npc.width, npc.height, scale, -npc.rotation, 0, 1, new Rectangle(0, 0, RitualTex.Width, RitualTex.Height), lightColor, true);
                BaseDrawing.DrawTexture(sb, RingTex1, r, npc.position, npc.width, npc.height, scale, npc.rotation, 0, 1, new Rectangle(0, 0, RingTex1.Width, RingTex1.Height), lightColor, true);
                BaseDrawing.DrawTexture(sb, RingTex2, r, npc.position, npc.width, npc.height, scale, npc.rotation, 0, 1, new Rectangle(0, 0, RingTex1.Width, RingTex1.Height), lightColor, true);
                BaseDrawing.DrawTexture(sb, RingTex, r, npc.position, npc.width, npc.height, scale, npc.rotation, 0, 1, new Rectangle(0, 0, RingTex.Width, RingTex.Height), Color.White, true);
            }

            BaseDrawing.DrawAura(sb, texture2D13, 0, npc.position, npc.width, npc.height, auraPercent, 1.5f, 1f, 0, npc.direction, 1, npc.frame, 0f, 0f, npc.GetAlpha(Color.White));

            return false;
        }
    }
}