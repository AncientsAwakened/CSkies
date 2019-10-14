using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CSkies.NPCs.Bosses.Void
{
    public class VoidDeath : ModNPC
    {
        public override string Texture => "CSkies/BlankTex";
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("VOID's Defeat");
            NPCID.Sets.TechnicallyABoss[npc.type] = true;
        }

        public override void SetDefaults()
        {
            npc.width = 230;
            npc.height = 142;
            npc.friendly = false;
            npc.lifeMax = 1;
            npc.dontTakeDamage = true;
            npc.noGravity = true;
            npc.aiStyle = -1;
            npc.alpha = 255;
            for (int k = 0; k < npc.buffImmune.Length; k++)
            {
                npc.buffImmune[k] = true;
            }
        }

        public override void AI()
        {
            if (npc.ai[1] > 180)
            {
                int p = Projectile.NewProjectile(npc.Center.X, npc.Center.Y, 0f, 0f, mod.ProjectileType("ShockwaveBoom"), 0, 1, Main.myPlayer, 0, 12);
                Main.projectile[p].Center = npc.Center;
                npc.life = 0;
                npc.netUpdate = true;
            }
            else
            {
                npc.ai[1]++;
                npc.ai[0]++;
                if (npc.ai[0] > 4)
                {
                    npc.ai[0] = 0;
                    Main.PlaySound(SoundID.Item14, npc.position);
                    for (int i = 0; i < 2; i++)
                    {
                        Vector2 Pos = new Vector2(npc.position.X + Main.rand.Next(0, 230), npc.position.Y - Main.rand.Next(0, 142));
                        Projectile.NewProjectile(Pos, Vector2.Zero, ModContent.ProjectileType<VoidDeathBoom>(), 0, 0, Main.myPlayer);
                    }
                }
            }
        }
    }

    public class VoidDeathBoom : ModProjectile
    {
        public override string Texture => "CSkies/NPCs/Bosses/Void/VoidDeathBoom";
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Blast");     
            Main.projFrames[projectile.type] = 7;
        }

        public override void SetDefaults()
        {
            projectile.width = 98;
            projectile.height = 98;
            projectile.penetrate = -1;
            projectile.hostile = true;
            projectile.damage = 50;
            projectile.tileCollide = false;
            projectile.ignoreWater = true;
            projectile.timeLeft = 600;
            projectile.alpha = 80;
        }

        bool draw = true;
        public override void AI()
        {
            if (!draw)
            {
                draw = true;
            }
            else
            {
                draw = false;
            }
            
            if (++projectile.frameCounter >= 3)
            {
                projectile.frameCounter = 0;
                if (++projectile.frame >= 7)
                {
                    projectile.Kill();

                }
            }
            projectile.velocity.X *= 0.00f;
            projectile.velocity.Y *= 0.00f;

        }

        public override void Kill(int timeLeft)
        {
            projectile.timeLeft = 0;
        }

        public float auraPercent = 0f;
        public bool auraDirection = true;

        public override bool PreDraw(SpriteBatch sb, Color lightColor)
        {
            Rectangle frame = BaseDrawing.GetFrame(projectile.frame, Main.projectileTexture[projectile.type].Width, Main.projectileTexture[projectile.type].Height / 7, 0, 2);

            if (!draw)
            {
                return false;
            }
            Texture2D Tex = Main.projectileTexture[projectile.type];
            BaseDrawing.DrawTexture(sb, Tex, 0, projectile.position, projectile.width, projectile.height, projectile.scale, projectile.rotation, 0, 7, frame, projectile.GetAlpha(Color.White), true);
            return false;
        }
    }
}
