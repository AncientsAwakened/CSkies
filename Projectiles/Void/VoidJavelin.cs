using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Audio;
using Terraria.ModLoader;

namespace CSkies.Projectiles.Void
{
    public class VoidJavelin : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Void Javelin");
        }

        public override void SetDefaults()
        {
            projectile.width = 16;
            projectile.height = 16;
            projectile.friendly = true;
            projectile.aiStyle = -1;
            projectile.melee = true;
            projectile.penetrate = -1;
            projectile.extraUpdates = 3;
        }

        public bool StuckInEnemy = false;
        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            Rectangle myRect = new Rectangle((int)projectile.position.X, (int)projectile.position.Y, projectile.width, projectile.height);
            bool flag3 = projectile.Colliding(myRect, target.getRect());
            if (!target.boss)
            {
                if (flag3 && !StuckInEnemy)
                {
                    StuckInEnemy = true;
                    projectile.ai[0] = 1f;
                    projectile.ai[1] = target.whoAmI;
                    projectile.velocity = (target.Center - projectile.Center) * 0.75f;
                    projectile.netUpdate = true;
                    int pieCut = 20;
                    for (int m = 0; m < pieCut; m++)
                    {
                        int dustID = Dust.NewDust(new Vector2(projectile.Center.X - 1, projectile.Center.Y - 1), 2, 2, ModContent.DustType<Dusts.VoidDust>(), 0f, 0f, 100, Color.White, 1.6f);
                        Main.dust[dustID].velocity = BaseUtility.RotateVector(default, new Vector2(6f, 0f), m / (float)pieCut * 6.28f);
                        Main.dust[dustID].noLight = false;
                        Main.dust[dustID].noGravity = true;
                    }
                    for (int m = 0; m < pieCut; m++)
                    {
                        int dustID = Dust.NewDust(new Vector2(projectile.Center.X - 1, projectile.Center.Y - 1), 2, 2, ModContent.DustType<Dusts.VoidDust>(), 0f, 0f, 100, Color.White, 2f);
                        Main.dust[dustID].velocity = BaseUtility.RotateVector(default, new Vector2(9f, 0f), m / (float)pieCut * 6.28f);
                        Main.dust[dustID].noLight = false;
                        Main.dust[dustID].noGravity = true;
                    }
                }
            }
        }

        public override void AI()
        {
            if (projectile.alpha > 0)
            {
                projectile.alpha -= 25;
            }
            if (projectile.alpha < 0)
            {
                projectile.alpha = 0;
            }
            if (projectile.ai[0] == 0f)
            {
                projectile.rotation = projectile.velocity.ToRotation() + 1.57079637f;
            }
            if (projectile.ai[0] == 1f)
            {
                projectile.ignoreWater = true;
                projectile.tileCollide = false;
                int num977 = 15;
                bool flag53 = false;
                projectile.localAI[0] += 1f;
                int num978 = (int)projectile.ai[1];
                if (projectile.localAI[0] >= 60 * num977)
                {
                    flag53 = true;
                }
                else if (num978 < 0 || num978 >= 200)
                {
                    flag53 = true;
                }
                else if (Main.npc[num978].active)
                {
                    projectile.Center = Main.npc[num978].Center - projectile.velocity * 2f;
                    projectile.gfxOffY = Main.npc[num978].gfxOffY;
                }
                else
                {
                    flag53 = true;
                }
                if (flag53)
                {
                    projectile.Kill();
                }
            }
        }

        public override void Kill(int timeLeft)
        {
            if (projectile.ai[0] == 1f)
            {
                Main.PlaySound(new LegacySoundStyle(2, 89, Terraria.Audio.SoundType.Sound));
                int p = Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, projectile.velocity.X, projectile.velocity.Y, mod.ProjectileType("VoidBoom"), projectile.damage, projectile.knockBack, projectile.owner, 0f, 0f);
                Main.projectile[p].Center = projectile.Center;
            }
        }

        public override bool PreDraw(SpriteBatch sb, Color dColor)
        {
            Texture2D tex = Main.projectileTexture[projectile.type];
            if (!StuckInEnemy)
            {
                BaseDrawing.DrawAfterimage(sb, tex, 0, projectile, 2.5f, 1, 3, true, 0f, 0f, projectile.GetAlpha(Colors.COLOR_GLOWPULSE));
            }
            BaseDrawing.DrawTexture(sb, tex, 0, projectile, projectile.GetAlpha(Colors.COLOR_GLOWPULSE));
            return false;
        }
    }
}
