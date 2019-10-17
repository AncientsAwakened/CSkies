using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace CSkies.Projectiles.Heart
{
    public class FirePro : ModProjectile
	{
		public override void SetDefaults()
		{
			projectile.width = 38;
			projectile.height = 38;
			projectile.aiStyle = 1;
			projectile.friendly = true;
			projectile.penetrate = 1;
			projectile.aiStyle = -1;
			projectile.melee = true;
			projectile.timeLeft = 300;
		}
		
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Fireball");
            Main.projFrames[projectile.type] = 4;
		}
		
		public override Color? GetAlpha(Color lightColor)
		{
			return Colors.COLOR_GLOWPULSE;
		}
		
		int HomeOnTarget()
		{
			const bool homingCanAimAtWetEnemies = true;
			const float homingMaximumRangeInPixels = 500;

			int selectedTarget = -1;
			for (int i = 0; i < Main.maxNPCs; i++)
			{
				NPC n = Main.npc[i];
				if(n.CanBeChasedBy(projectile) && (!n.wet || homingCanAimAtWetEnemies))
				{
					float distance = projectile.Distance(n.Center);
					if(distance <= homingMaximumRangeInPixels &&
						(
						selectedTarget == -1 ||  //there is no selected target
						projectile.Distance(Main.npc[selectedTarget].Center) > distance) 
						)
					{
						selectedTarget = i;
					}
				}
			}
			return selectedTarget;
		}
		
		public override void AI()
		{
			if (Main.rand.Next(2) == 0)
            {
                int dustnumber = Dust.NewDust(projectile.position, projectile.width, projectile.height, DustID.Fire, 0f, 0f, 200, default, 0.8f);
                Main.dust[dustnumber].velocity *= 0.3f;
			}

            if (projectile.frameCounter++ > 5)
            {
                projectile.frameCounter = 0;
                if (projectile.frame++ > 2)
                {
                    projectile.frame = 0;
                }
            }
			
			const int aislotHomingCooldown = 0;
			const int homingDelay = 30;
			const float desiredFlySpeedInPixelsPerFrame = 14; 
			const float amountOfFramesToLerpBy = 15; 

			projectile.ai[aislotHomingCooldown]++;
			if(projectile.ai[aislotHomingCooldown] > homingDelay)
			{
				projectile.ai[aislotHomingCooldown] = homingDelay; 

				int foundTarget = HomeOnTarget();
				if(foundTarget != -1)
				{
					NPC n = Main.npc[foundTarget];
					Vector2 desiredVelocity = projectile.DirectionTo(n.Center) * desiredFlySpeedInPixelsPerFrame;
					projectile.velocity = Vector2.Lerp(projectile.velocity, desiredVelocity, 1f / amountOfFramesToLerpBy);
				}
			}
		}

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            target.AddBuff(BuffID.Daybreak, 200);
        }

        public override void Kill(int timeleft)
        {
            Main.PlaySound(SoundID.Item14, projectile.position);
            int b = Projectile.NewProjectile(projectile.position, Vector2.Zero, ModContent.ProjectileType<ProBoom>(), projectile.damage, projectile.knockBack, Main.myPlayer);
            Main.projectile[b].Center = projectile.Center;
            int num290 = Main.rand.Next(3, 7);
            for (int num291 = 0; num291 < num290; num291++)
            {
                int num292 = Dust.NewDust(projectile.position, projectile.width, projectile.height, DustID.Fire, 0f, 0f, 100, default, 2.1f);
                Main.dust[num292].velocity *= 2f;
                Main.dust[num292].noGravity = true;
            };
        }
    }
}