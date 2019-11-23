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

            if (projectile.ai[1]++ > 60)
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
            Projectile.NewProjectile(projectile.position, new Vector2(0, 12), ModContent.ProjectileType<ShockSummon>(), projectile.damage / 4, 5, Main.myPlayer);
            for (int num468 = 0; num468 < 5; num468++)
            {
                int num469 = Dust.NewDust(projectile.Center, projectile.width, projectile.height, DustID.Electric, -projectile.velocity.X * 0.2f, -projectile.velocity.Y * 0.2f, 100, default, 2f);
                Main.dust[num469].noGravity = true;
            }
        }
    }
}