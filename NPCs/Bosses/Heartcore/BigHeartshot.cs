using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Microsoft.Xna.Framework.Graphics;

namespace CSkies.NPCs.Bosses.Heartcore
{
    public class BigHeartshot : ModProjectile
	{
		public override void SetDefaults()
		{
			projectile.width = 32;
            projectile.height = 32;
			projectile.aiStyle = 1;
			projectile.hostile = true;
			projectile.penetrate = 1;
			projectile.aiStyle = 27;
			projectile.magic = true;
			projectile.timeLeft = 300;
			ProjectileID.Sets.TrailCacheLength[projectile.type] = 5;
			ProjectileID.Sets.TrailingMode[projectile.type] = 2;
		}
		
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Heart");
		}
		
		public override Color? GetAlpha(Color lightColor)
		{
			return Colors.COLOR_GLOWPULSE;
		}
		
		public override void AI()
        {
            projectile.rotation =
            projectile.velocity.ToRotation() +
            MathHelper.ToRadians(90f);

            if (Main.rand.Next(2) == 0)
			{
				int dustnumber = Dust.NewDust(projectile.position, projectile.width, projectile.height, ModContent.DustType<Dusts.HeartDust>(), 0f, 0f, 200, default, 0.5f);
				Main.dust[dustnumber].velocity *= 0.3f;
			}
            const int aislotHomingCooldown = 20;
            const int homingDelay = 0;
            const float desiredFlySpeedInPixelsPerFrame = 15;
            const float amountOfFramesToLerpBy = 30; // minimum of 1, please keep in full numbers even though it's a float!

            projectile.ai[aislotHomingCooldown]++;
            if (projectile.ai[aislotHomingCooldown] > homingDelay)
            {
                projectile.ai[aislotHomingCooldown] = homingDelay;

                int foundTarget = HomeOnTarget();
                if (foundTarget != -1)
                {
                    Player target = Main.player[foundTarget];
                    Vector2 desiredVelocity = projectile.DirectionTo(target.Center) * desiredFlySpeedInPixelsPerFrame;
                    projectile.velocity = Vector2.Lerp(projectile.velocity, desiredVelocity, 1f / amountOfFramesToLerpBy);
                }
            }
        }

        private int HomeOnTarget()
        {
            const bool homingCanAimAtWetEnemies = true;
            const float homingMaximumRangeInPixels = 500;

            int selectedTarget = -1;
            for (int i = 0; i < Main.maxNPCs; i++)
            {
                Player target = Main.player[i];
                if (target.active && (!target.wet || homingCanAimAtWetEnemies))
                {
                    float distance = projectile.Distance(target.Center);
                    if (distance <= homingMaximumRangeInPixels &&
                    (
                        selectedTarget == -1 ||
                        projectile.Distance(Main.npc[selectedTarget].Center) > distance)
                    )
                        selectedTarget = i;
                }
            }

            return selectedTarget;
        }

        public override void Kill(int a)
        {
            for (int num468 = 0; num468 < 5; num468++)
            {
                int num469 = Dust.NewDust(new Vector2(projectile.Center.X, projectile.Center.Y), projectile.width, projectile.height, ModContent.DustType<Dusts.HeartDust>(), -projectile.velocity.X * 0.2f,
                    -projectile.velocity.Y * 0.2f, 100, default, 2f);
                Main.dust[num469].noGravity = true;
            }
        }

        public float auraPercent = 0f;
        public bool auraDirection = true;

        public override bool PreDraw(SpriteBatch sb, Color lightColor)
        {
            if (auraDirection) { auraPercent += 0.1f; auraDirection = auraPercent < 1f; }
            else { auraPercent -= 0.1f; auraDirection = auraPercent <= 0f; }

            Rectangle frame = BaseDrawing.GetFrame(projectile.frame, Main.projectileTexture[projectile.type].Width, Main.projectileTexture[projectile.type].Height, 0, 0);

            BaseDrawing.DrawAura(sb, Main.projectileTexture[projectile.type], 0, projectile.position, projectile.width, projectile.height, auraPercent, 1.5f, 1f, projectile.rotation, projectile.direction, 1, frame, 0f, 0f, projectile.GetAlpha(Colors.COLOR_GLOWPULSE));
            return false;
        }
    }
}