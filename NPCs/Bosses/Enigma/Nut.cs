using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework.Graphics;

namespace CSkies.NPCs.Bosses.Enigma
{
    public class Nut : ModProjectile
    {
        public override void SetDefaults()
        {
            projectile.width = 16;
            projectile.height = 16;
			projectile.aiStyle = -1;
            projectile.hostile = true;
            projectile.penetrate = 1;
            projectile.ignoreWater = true;
        }

		public override void SetStaticDefaults()
		{
            Main.projFrames[projectile.type] = 3;
		}

        public override void AI()
        {
            projectile.velocity.Y = projectile.velocity.Y + .98f;

            projectile.rotation += projectile.velocity.X / 10;

            for (int m = projectile.oldPos.Length - 1; m > 0; m--)
            {
                projectile.oldPos[m] = projectile.oldPos[m - 1];
            }
            projectile.oldPos[0] = projectile.position;
        }

        public override void PostAI()
        {
            projectile.frame = (int)projectile.ai[1];
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            Rectangle frame = BaseDrawing.GetFrame(projectile.frame, Main.projectileTexture[projectile.type].Width, Main.projectileTexture[projectile.type].Height / 3, 0, 0);
            BaseDrawing.DrawTexture(spriteBatch, Main.projectileTexture[projectile.type], 0, projectile.position, projectile.width, projectile.height, projectile.scale, projectile.rotation, 0, 3, frame, lightColor, true);
            return false;
        }

        public override void Kill(int timeLeft)
        {
            int DustType = DustID.Silver;
            for (int num468 = 0; num468 < 5; num468++)
            {
                float VelX = -projectile.velocity.X * 0.2f;
                float VelY = -projectile.velocity.Y * 0.2f;
                num468 = Dust.NewDust(projectile.Center, projectile.width, projectile.height, DustType, VelX, VelY);
            }
        }
    }
}
