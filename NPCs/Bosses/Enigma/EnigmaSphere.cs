using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CSkies.NPCs.Bosses.Enigma
{
    class EnigmaSphere : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            Main.projFrames[projectile.type] = 4;
        }

        public override void SetDefaults()
        {
            projectile.width = 48;
            projectile.height = 30;
            projectile.ignoreWater = true;
            projectile.tileCollide = true;
            projectile.alpha = 255;
            projectile.penetrate = 1;
            projectile.timeLeft = 300;
            projectile.hostile = true;
        }

        public override void AI()
        {
            projectile.frameCounter++;
            if (projectile.frameCounter > 5)
            {
                projectile.frame++;
                projectile.frameCounter = 0;
                if (projectile.frame > 3)
                {
                    projectile.frame = 0;
                }
            }

            int dustId = Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y + 2f), projectile.width, projectile.height + 5, DustID.Electric, projectile.velocity.X * 0.2f,
                projectile.velocity.Y * 0.2f, 100, default, 2f);
            Main.dust[dustId].noGravity = true;

            const int aislotHomingCooldown = 0;
            const int homingDelay = 20;
            const float desiredFlySpeedInPixelsPerFrame = 10;
            const float amountOfFramesToLerpBy = 20;

            projectile.ai[aislotHomingCooldown]++;
            if (projectile.ai[aislotHomingCooldown] > homingDelay)
            {
                projectile.ai[aislotHomingCooldown] = homingDelay; 

                int foundTarget = HomeOnTarget();
                if (foundTarget != -1)
                {
                    Player n = Main.player[foundTarget];
                    Vector2 desiredVelocity = projectile.DirectionTo(n.Center) * desiredFlySpeedInPixelsPerFrame;
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
                if (distance <= homingMaximumRangeInPixels && ( selectedTarget == -1 || projectile.Distance(Main.player[selectedTarget].Center) > distance))
                {
                    selectedTarget = i;
                }
            }
            return selectedTarget;
        }

        public override void Kill(int timeleft)
        {
            Main.PlaySound(SoundID.Item14, projectile.position);
            float spread1 = 12f * 0.0174f;
            double startAngle1 = Math.Atan2(projectile.velocity.X, projectile.velocity.Y) - spread1 / 2;
            double deltaAngle1 = spread1 / 20f;
            for (int i = 0; i < 10; i++)
            {
                double offsetAngle = startAngle1 + deltaAngle1 * (i + i * i) / 2f + 32f * i;
                Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, (float)(Math.Sin(offsetAngle) * 10f), (float)(Math.Cos(offsetAngle) * 10f), ModContent.ProjectileType<EnigmaStatic>(), projectile.damage / 4, 5);
                Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, (float)(-Math.Sin(offsetAngle) * 10f), (float)(-Math.Cos(offsetAngle) * 10f), ModContent.ProjectileType<EnigmaStatic>(), projectile.damage / 4, 5);
            }
        }
    }
}