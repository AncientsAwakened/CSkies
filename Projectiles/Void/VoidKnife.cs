using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ModLoader;

namespace CSkies.Projectiles.Void
{
    public class VoidKnife : ModProjectile
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
            projectile.extraUpdates = 1;
		}
		
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Void Knife");
		}
		
		public override Color? GetAlpha(Color lightColor)
		{
			return Color.White;
		}

        public override void Kill(int timeleft)
        {
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

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            for (int u = 0; u < Main.maxNPCs; u++)
            {
                NPC t = Main.npc[u];

                if (t.active && !t.boss && Vector2.Distance(projectile.Center, t.Center) < 160)
                {
                    float num3 = 6f;
                    Vector2 vector = new Vector2(target.position.X + target.width / 2, target.position.Y + target.height / 2);
                    float num4 = projectile.Center.X - vector.X;
                    float num5 = projectile.Center.Y - vector.Y;
                    float num6 = (float)Math.Sqrt(num4 * num4 + num5 * num5);
                    num6 = num3 / num6;
                    num4 *= num6;
                    num5 *= num6;
                    int num7 = 6;
                    target.velocity.X = (target.velocity.X * (num7 - 1) + num4) / num7;
                    target.velocity.Y = (target.velocity.Y * (num7 - 1) + num5) / num7;
                }
            }
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
            const int aislotHomingCooldown = 0;
            const int homingDelay = 10;
            const float desiredFlySpeedInPixelsPerFrame = 20; //How fast can it go once in homing mode?
            const float amountOfFramesToLerpBy = 20; // minimum of 1, How quickly can it turn?

            projectile.ai[aislotHomingCooldown]++;
            if (projectile.ai[aislotHomingCooldown] > homingDelay)
            {
                projectile.ai[aislotHomingCooldown] = homingDelay; //cap this value 

                int foundTarget = HomeOnTarget();
                if (foundTarget != -1)
                {
                    NPC n = Main.npc[foundTarget];
                    Vector2 desiredVelocity = projectile.DirectionTo(n.Center) * desiredFlySpeedInPixelsPerFrame;
                    projectile.velocity = Vector2.Lerp(projectile.velocity, desiredVelocity, 1f / amountOfFramesToLerpBy);
                }
            }
        }

        public override void PostAI()
        {
            projectile.alpha += 2;
            if (projectile.alpha >= 255)
            {
                projectile.active = false;
            }

            for (int m = projectile.oldPos.Length - 1; m > 0; m--)
            {
                projectile.oldPos[m] = projectile.oldPos[m - 1];
            }
            projectile.oldPos[0] = projectile.position;
        }

        public override bool PreDraw(SpriteBatch sb, Color dColor)
        {
            Texture2D tex = Main.projectileTexture[projectile.type];
            BaseDrawing.DrawAfterimage(sb, tex, 0, projectile, 2.5f, 1, 3, true, 0f, 0f, projectile.GetAlpha(Colors.COLOR_GLOWPULSE));
            BaseDrawing.DrawTexture(sb, tex, 0, projectile, projectile.GetAlpha(Colors.COLOR_GLOWPULSE));
            return false;
        }
    }
}