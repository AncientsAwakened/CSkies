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
			projectile.width = 14;
			projectile.height = 14;
			projectile.aiStyle = 1;
			projectile.friendly = true;
			projectile.penetrate = 1;
			projectile.aiStyle = 27;
			projectile.melee = true;
			projectile.timeLeft = 1200;
			ProjectileID.Sets.TrailCacheLength[projectile.type] = 5;
			ProjectileID.Sets.TrailingMode[projectile.type] = 2;
            projectile.usesLocalNPCImmunity = true;
            projectile.localNPCHitCooldown = 2;
		}
		
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Fireball");
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
                int dustnumber = Dust.NewDust(projectile.position, projectile.width, projectile.height, DustID.SolarFlare, 0f, 0f, 200, default, 0.8f);
                Main.dust[dustnumber].velocity *= 0.3f;
			}
			
			const int aislotHomingCooldown = 0;
			const int homingDelay = 30;
			const float desiredFlySpeedInPixelsPerFrame = 9; 
			const float amountOfFramesToLerpBy = 5; 

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

        public override void Kill(int timeleft)
        {
            Main.PlaySound(SoundID.Item14, projectile.position);
            int num290 = Main.rand.Next(3, 7);
            for (int num291 = 0; num291 < num290; num291++)
            {
                int num292 = Dust.NewDust(projectile.position, projectile.width, projectile.height, DustID.SolarFlare, 0f, 0f, 100, default, 2.1f);
                Main.dust[num292].velocity *= 2f;
                Main.dust[num292].noGravity = true;
            };
        }
    }
}