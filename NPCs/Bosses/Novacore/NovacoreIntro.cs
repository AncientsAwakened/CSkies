using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace CSkies.NPCs.Bosses.Novacore
{
    public class NovacoreIntro : ModNPC
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("???");
            Main.npcFrameCount[npc.type] = 8;
            NPCID.Sets.TrailCacheLength[npc.type] = 20;
            NPCID.Sets.TrailingMode[npc.type] = 0;
        }

        public override void SetDefaults()
        {
            npc.width = 198;
            npc.height = 198;
            npc.friendly = false;
            npc.lifeMax = 1;
            npc.dontTakeDamage = true;
            npc.noTileCollide = true;
            npc.noGravity = true;
            npc.aiStyle = -1;
            npc.timeLeft = 10;
            npc.alpha = 255;
            for (int k = 0; k < npc.buffImmune.Length; k++)
            {
                npc.buffImmune[k] = true;
            }
            music = mod.GetSoundSlot(SoundType.Music, "Sounds/Music/Novacore1");
        }

        bool title = false;
        int ShineAlpha = 255;
        int Fadeout = 0;
        public override void AI()
        {
            npc.rotation += .06f;
            npc.velocity *= 0;
            npc.ai[0]++;
            if (npc.ai[0] == 120 && Main.netMode != NetmodeID.Server)
            {
                int p = Projectile.NewProjectile(npc.Center.X, npc.Center.Y, 0f, 0f, mod.ProjectileType("ShockwaveBoom"), 0, 1, Main.myPlayer, 0, 12);
                Main.projectile[p].Center = npc.Center;
            }
            if (npc.ai[0] > 60)
            {
                if (!title)
                {
                    CSkies.ShowTitle(npc, 7);
                    title = true;
                }

                if (npc.ai[0] > 120)
                {
                    if (npc.alpha > 0)
                    {
                        npc.alpha -= 3;
                    }
                    else
                    {
                        npc.alpha = 0;
                    }
                }

                if (npc.alpha > 0)
                {
                    Fadeout += 10;
                    ShineAlpha -= 5;
                    if (ShineAlpha <= 30)
                    {
                        ShineAlpha = 30;
                    }
                }
                else
                {
                    Fadeout -= 10;
                    ShineAlpha += 5;
                    if (ShineAlpha >= 255)
                    {
                        ShineAlpha = 255;
                    }
                }
            }

            if (++npc.ai[0] > 480 && Main.netMode != NetmodeID.MultiplayerClient)
            {
                int n = NPC.NewNPC((int)npc.position.X, (int)npc.position.Y, ModContent.NPCType<Novacore>(), 0, 10);
                Main.npc[n].Center = npc.Center;
                Main.npc[n].velocity = npc.velocity;
                Main.npc[n].frame.Y = npc.frame.Y;
                npc.active = false;
                npc.netUpdate = true;
            }
        }

        int Frame = 0;
        public override void FindFrame(int frameHeight)
        {
            if (npc.frameCounter++ > 5)
            {
                npc.frameCounter = 0;
                Frame++;
                if (Frame > 7)
                {
                    Frame = 0;
                }
            }
            npc.frame.Y = Frame * frameHeight;
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            Texture2D BladeTex = mod.GetTexture("NPCs/Bosses/Novacore/NovacoreBack");

            Texture2D BladeGlowTex = mod.GetTexture("Glowmasks/NovacoreBack_Glow");

            Texture2D Base = Main.npcTexture[npc.type];

            Texture2D BaseGlow = mod.GetTexture("Glowmasks/NovacoreIntro_Glow");

            Vector2 drawOrigin = new Vector2(npc.width * .5f, npc.height * .5f);

            spriteBatch.End();
            spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.Additive);

            if (ShineAlpha < 255) //Simple bool
            {
                for (int k = 0; k < npc.oldPos.Length; k++)
                {
                    Color Alpha = new Color(255 - Fadeout, 120 - Fadeout, 255 - Fadeout, ShineAlpha) * (npc.oldPos.Length - k / npc.oldPos.Length);

                    spriteBatch.Draw(BladeGlowTex, npc.position - Main.screenPosition + drawOrigin, npc.frame, Alpha, npc.rotation, drawOrigin, 1f + k, SpriteEffects.None, 0f);

                    spriteBatch.Draw(BaseGlow, npc.position - Main.screenPosition + drawOrigin, npc.frame, Alpha, 0, drawOrigin, 1f + k, SpriteEffects.None, 0f);
                }
            }

            spriteBatch.Draw(BladeTex, npc.position - Main.screenPosition + drawOrigin, npc.frame, npc.GetAlpha(lightColor), npc.rotation, drawOrigin, 1f, SpriteEffects.None, 0f);

            spriteBatch.Draw(BladeGlowTex, npc.position - Main.screenPosition + drawOrigin, npc.frame, npc.GetAlpha(Color.White), npc.rotation, drawOrigin, 1f, SpriteEffects.None, 0f);

            spriteBatch.Draw(Base, npc.position - Main.screenPosition + drawOrigin, npc.frame, npc.GetAlpha(lightColor), 0, drawOrigin, 1f, SpriteEffects.None, 0f);

            spriteBatch.Draw(BaseGlow, npc.position - Main.screenPosition + drawOrigin, npc.frame, npc.GetAlpha(Color.White), 0, drawOrigin, 1f, SpriteEffects.None, 0f);
            return false;
        }
    }
}