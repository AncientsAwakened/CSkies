using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;

namespace CSkies.Projectiles
{
    public class Comet_Knife : ModProjectile
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