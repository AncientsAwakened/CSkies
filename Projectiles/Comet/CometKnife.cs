using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;

namespace CSkies.Projectiles.Comet
{
    public class CometKnife : ModProjectile
	{
		public override void SetDefaults()
        {
            projectile.ranged = true;
            projectile.width = 14;
			projectile.height = 14;
			projectile.aiStyle = 1;
			projectile.friendly = true;
			projectile.penetrate = 1;
			projectile.timeLeft = 200;
		}
		
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Comet Knife");
		}
		
		public override Color? GetAlpha(Color lightColor)
		{
			return Color.White;
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

        public override void Kill(int timeleft)
        {
            int pieCut = 20;
            for (int m = 0; m < pieCut; m++)
            {
                int dustID = Dust.NewDust(new Vector2(projectile.Center.X - 1, projectile.Center.Y - 1), 2, 2, 17, 0f, 0f, 100, Color.White, 1.6f);
                Main.dust[dustID].velocity = BaseUtility.RotateVector(default, new Vector2(6f, 0f), m / (float)pieCut * 6.28f);
                Main.dust[dustID].noLight = false;
                Main.dust[dustID].noGravity = true;
            }
            for (int m = 0; m < pieCut; m++)
            {
                int dustID = Dust.NewDust(new Vector2(projectile.Center.X - 1, projectile.Center.Y - 1), 2, 2, 17, 0f, 0f, 100, Color.White, 2f);
                Main.dust[dustID].velocity = BaseUtility.RotateVector(default, new Vector2(9f, 0f), m / (float)pieCut * 6.28f);
                Main.dust[dustID].noLight = false;
                Main.dust[dustID].noGravity = true;
            }
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
			projectile.ai[0] += 0.1f;
			projectile.velocity *= 0.75f;
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