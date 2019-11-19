using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CSkies.NPCs.Bosses.Enigma
{
    class EngimaBurst : ModProjectile
    {
        public override string Texture => "CSkies/BlankTex";
        public override void SetDefaults()
        {
            projectile.width = 16;
            projectile.height = 16;
            projectile.ignoreWater = true;
            projectile.tileCollide = true;
            projectile.penetrate = 1;
            projectile.timeLeft = 900;
            projectile.hostile = true;
        }

        public override void AI()
        {
            for (int num572 = 0; num572 < 5; num572++)
            {
                float num573 = projectile.velocity.X * 0.2f * num572;
                float num574 = -(projectile.velocity.Y * 0.2f) * num572;
                int num575 = Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), projectile.width, projectile.height, DustID.Electric, 0f, 0f, 100, default, 1f);
                Main.dust[num575].velocity *= 0f;
                Dust expr_178B4_cp_0 = Main.dust[num575];
                expr_178B4_cp_0.position.X -= num573;
                Dust expr_178D3_cp_0 = Main.dust[num575];
                expr_178D3_cp_0.position.Y -= num574;
            }

            const int aislotHomingCooldown = 0;
            const int homingDelay = 20;
            const float desiredFlySpeedInPixelsPerFrame = 20;
            const float amountOfFramesToLerpBy = 30; // minimum of 1, please keep in full numbers even though it's a float!

            projectile.ai[aislotHomingCooldown]++;
            if (projectile.ai[aislotHomingCooldown] > homingDelay)
            {
                projectile.ai[aislotHomingCooldown] = homingDelay; 

                int foundTarget = HomeOnTarget();
                if (foundTarget != -1)
                {
                    Player p = Main.player[foundTarget];
                    Vector2 desiredVelocity = projectile.DirectionTo(p.Center) * desiredFlySpeedInPixelsPerFrame;
                    projectile.velocity = Vector2.Lerp(projectile.velocity, desiredVelocity, 1f / amountOfFramesToLerpBy);
                }
            }
        }

        private int HomeOnTarget()
        {
            const float homingMaximumRangeInPixels = 400;

            int selectedTarget = -1;
            for (int i = 0; i < Main.maxPlayers; i++)
            {
                Player n = Main.player[i];
                float distance = projectile.Distance(n.Center);
                if (distance <= homingMaximumRangeInPixels &&
                    (
                        selectedTarget == -1 || //there is no selected target
                        projectile.Distance(Main.player[selectedTarget].Center) > distance)
                )
                    selectedTarget = i;
            }
            return selectedTarget;
        }

        public override void Kill(int timeleft)
        {
            Main.PlaySound(mod.GetLegacySoundSlot(SoundType.Custom, "Sounds/Sounds/Zap"), projectile.position);
            for (int num468 = 0; num468 < 10; num468++)
            {
                int num469 = Dust.NewDust(projectile.Center, projectile.width, projectile.height, DustID.Electric, -projectile.velocity.X * 0.2f, -projectile.velocity.Y * 0.2f, 100, default, 2f);
                Main.dust[num469].noGravity = true;
            }
        }
    }

    class EngimaSpell : ModProjectile
    {
        public override string Texture => "CSkies/BlankTex";
        public override void SetDefaults()
        {
            projectile.width = 16;
            projectile.height = 16;
            projectile.ignoreWater = true;
            projectile.tileCollide = true;
            projectile.penetrate = 1;
            projectile.hostile = true;
        }

        public override void AI()
        {
            for (int num572 = 0; num572 < 5; num572++)
            {
                float num573 = projectile.velocity.X * 0.2f * num572;
                float num574 = -(projectile.velocity.Y * 0.2f) * num572;
                int num575 = Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), projectile.width, projectile.height, DustID.Electric, 0f, 0f, 100, default, 1f);
                Main.dust[num575].velocity *= 0f;
                Dust expr_178B4_cp_0 = Main.dust[num575];
                expr_178B4_cp_0.position.X -= num573;
                Dust expr_178D3_cp_0 = Main.dust[num575];
                expr_178D3_cp_0.position.Y -= num574;
            }

            if (projectile.ai[1]++ > 120)
            {
                projectile.Kill();
            }

            const int aislotHomingCooldown = 0;
            const int homingDelay = 0;
            const float desiredFlySpeedInPixelsPerFrame = 20;
            const float amountOfFramesToLerpBy = 30;

            projectile.ai[aislotHomingCooldown]++;
            if (projectile.ai[aislotHomingCooldown] > homingDelay)
            {
                projectile.ai[aislotHomingCooldown] = homingDelay;

                int foundTarget = HomeOnTarget();
                if (foundTarget != -1)
                {
                    Player p = Main.player[foundTarget];
                    Vector2 desiredVelocity = projectile.DirectionTo(p.Center) * desiredFlySpeedInPixelsPerFrame;
                    projectile.velocity.X = Vector2.Lerp(projectile.velocity, desiredVelocity, 1f / amountOfFramesToLerpBy).X;
                }
            }
        }

        private int HomeOnTarget()
        {
            const float homingMaximumRangeInPixels = 400;

            int selectedTarget = -1;
            for (int i = 0; i < Main.maxPlayers; i++)
            {
                Player n = Main.player[i];
                float distance = projectile.Distance(n.Center);
                if (distance <= homingMaximumRangeInPixels &&
                    (
                        selectedTarget == -1 || //there is no selected target
                        projectile.Distance(Main.player[selectedTarget].Center) > distance)
                )
                    selectedTarget = i;
            }
            return selectedTarget;
        }


        public override void Kill(int timeleft)
        {
            Main.PlaySound(mod.GetLegacySoundSlot(SoundType.Custom, "Sounds/Sounds/Shock"), projectile.position);
            Projectile.NewProjectile(projectile.position, new Vector2(0, 12), ModContent.ProjectileType<EnigmaRain>(), projectile.damage / 4, 5, Main.myPlayer);
            for (int num468 = 0; num468 < 5; num468++)
            {
                int num469 = Dust.NewDust(projectile.Center, projectile.width, projectile.height, DustID.Electric, -projectile.velocity.X * 0.2f, -projectile.velocity.Y * 0.2f, 100, default, 2f);
                Main.dust[num469].noGravity = true;
            }
        }
    }

    public class EnigmaRain : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            Main.projFrames[projectile.type] = 7;
        }
        public override void SetDefaults()
        {
            projectile.width = 64;
            projectile.height = 64;
            projectile.aiStyle = -1;
            projectile.hostile = true;
            projectile.tileCollide = false;
            projectile.extraUpdates = 2;
            projectile.penetrate = -1;
            projectile.alpha = 255;
        }

        public override void AI()
        {
            if (projectile.ai[1] != -1f && projectile.position.Y > projectile.ai[1])
            {
                projectile.tileCollide = true;
            }
            if (projectile.position.HasNaNs())
            {
                projectile.Kill();
                return;
            }
            bool flag5 = WorldGen.SolidTile(Framing.GetTileSafely((int)projectile.position.X / 16, (int)projectile.position.Y / 16));
            Dust dust19 = Main.dust[Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), projectile.width, projectile.height, DustID.Electric, 0f, 0f, 0, default, 1f)];
            dust19.position = projectile.Center;
            dust19.velocity = Vector2.Zero;
            dust19.noGravity = true;
            Dust dust18 = Main.dust[Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), projectile.width, projectile.height, DustID.Electric, 0f, 0f, 0, default, 1f)];
            dust18.position = projectile.Center;
            dust18.velocity = Vector2.Zero;
            dust18.noGravity = true;
            if (flag5)
            {
                dust19.noLight = true;
                dust18.noLight = true;
            }
            if (projectile.ai[1] == -1f)
            {
                projectile.ai[0] += 1f;
                projectile.velocity = Vector2.Zero;
                projectile.tileCollide = false;
                projectile.penetrate = -1;
                projectile.position = projectile.Center;
                projectile.width = projectile.height = 140;
                projectile.Center = projectile.position;
                projectile.alpha -= 10;
                if (projectile.alpha < 0)
                {
                    projectile.alpha = 0;
                }
                if (++projectile.frameCounter >= projectile.MaxUpdates * 3)
                {
                    projectile.frameCounter = 0;
                    projectile.frame++;
                }
                if (projectile.frame == 6)
                {
                    projectile.Kill();
                }
                return;
            }
            projectile.alpha = 255;
            if (projectile.numUpdates == 0)
            {
                int num185 = -1;
                float num186 = 60f;
                for (int num187 = 0; num187 < 200; num187++)
                {
                    Player p = Main.player[num187];
                    float num188 = projectile.Distance(p.Center);
                    if (num188 < num186 && Collision.CanHitLine(projectile.Center, 0, 0, p.Center, 0, 0))
                    {
                        num186 = num188;
                        num185 = num187;
                    }
                }
                if (num185 != -1)
                {
                    projectile.ai[0] = 0f;
                    projectile.ai[1] = -1f;
                    projectile.netUpdate = true;
                    return;
                }
            }
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            projectile.ai[0] = 0f;
            projectile.ai[1] = -1f;
            projectile.netUpdate = true;
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            if (projectile.ai[1] != -1)
            {
                projectile.ai[0] = 0f;
                projectile.ai[1] = -1f;
                projectile.netUpdate = true;
            }
            return false;
        }

        public override void Kill(int timeLeft)
        {
            Main.PlaySound(mod.GetLegacySoundSlot(SoundType.Custom, "Sounds/Sounds/Zap2"), projectile.position);
            bool flag = WorldGen.SolidTile(Framing.GetTileSafely((int)projectile.position.X / 16, (int)projectile.position.Y / 16));

            for (int num58 = 0; num58 < 4; num58++)
            {
                Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), projectile.width, projectile.height, DustID.Electric, 0f, 0f, 100, default, 1.5f);
            }
            for (int num59 = 0; num59 < 4; num59++)
            {
                int num60 = Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), projectile.width, projectile.height, DustID.Electric, 0f, 0f, 0, default, 2.5f);
                Main.dust[num60].noGravity = true;
                Main.dust[num60].velocity *= 3f;
                if (flag)
                {
                    Main.dust[num60].noLight = true;
                }
                num60 = Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), projectile.width, projectile.height, DustID.Electric, 0f, 0f, 100, default, 1.5f);
                Main.dust[num60].velocity *= 2f;
                Main.dust[num60].noGravity = true;
                if (flag)
                {
                    Main.dust[num60].noLight = true;
                }
            }
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            Texture2D t = Main.projectileTexture[projectile.type];
            Rectangle f = BaseDrawing.GetFrame(projectile.frame, t.Width, t.Height / 7, 0, 0);
            BaseDrawing.DrawTexture(spriteBatch, t, 0, projectile.position, projectile.width, projectile.height, projectile.scale, 0, 0, 7, f, Color.White, true);
            return false;
        }
    }
}