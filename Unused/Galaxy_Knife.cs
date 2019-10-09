using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace CSkies.Projectiles
{
    public class Galaxy_Knife : ModProjectile
	{
		public override void SetDefaults()
        {
            projectile.ranged = true;
            projectile.width = 14;
			projectile.height = 14;
			projectile.aiStyle = 1;
			projectile.friendly = true;
			projectile.penetrate = 1;
			projectile.timeLeft = 60;
		}
		
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Galaxy Knife");
		}
		
		public override Color? GetAlpha(Color lightColor)
		{
			return Color.White;
		}
		
		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
			projectile.ai[0] += 0.1f;
			projectile.velocity *= 0.75f;
		}
		
		int HomeOnTarget()
		{
			const bool homingCanAimAtWetEnemies = true;
			const float homingMaximumRangeInPixels = 1000;

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
						projectile.Distance(Main.npc[selectedTarget].Center) > distance) //or we are closer to this target than the already selected target
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

			}
			
			const int aislotHomingCooldown = 0;
			const int homingDelay = 10;
			const float desiredFlySpeedInPixelsPerFrame = 20; //How fast can it go once in homing mode?
			const float amountOfFramesToLerpBy = 10; // minimum of 1, How quickly can it turn?

			projectile.ai[aislotHomingCooldown]++;
			if(projectile.ai[aislotHomingCooldown] > homingDelay)
			{
				projectile.ai[aislotHomingCooldown] = homingDelay; //cap this value 

				int foundTarget = HomeOnTarget();
				if(foundTarget != -1)
				{
					NPC n = Main.npc[foundTarget];
					Vector2 desiredVelocity = projectile.DirectionTo(n.Center) * desiredFlySpeedInPixelsPerFrame;
					projectile.velocity = Vector2.Lerp(projectile.velocity, desiredVelocity, 1f / amountOfFramesToLerpBy);
				}
			}
		}
	}
}