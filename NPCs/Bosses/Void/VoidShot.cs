using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CSkies.NPCs.Bosses.Void
{
    public class VoidShot : ModProjectile
    {
        public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Shadow Sphere");
		}
    	
        public override void SetDefaults()
        {
            projectile.width = 38;
            projectile.height = 38;
            projectile.aiStyle = -1;
            projectile.hostile = true;
            projectile.timeLeft = 660;
            projectile.light = 0.5f;
            projectile.alpha = 255;
        }

        public override void AI()
        {
            if (projectile.alpha > 0)
            {
                projectile.alpha -= 5;
            }
            else
            {
                projectile.alpha = 0;
            }

            for (int m = projectile.oldPos.Length - 1; m > 0; m--)
            {
                projectile.oldPos[m] = projectile.oldPos[m - 1];
            }
            projectile.oldPos[0] = projectile.position;

            projectile.frameCounter++;
            if (projectile.frameCounter > 6)
            {
                projectile.frameCounter = 0;
                projectile.frame++;
                if (projectile.frame > 4)
                {
                    projectile.frame = 0;
                }
            }

            if (projectile.ai[1]++ > 60)
            {
                projectile.velocity *= 0.98f;
            }
            if (projectile.ai[1]++ > 120)
            {
                projectile.Kill();
            }
        }

        public override void Kill(int timeLeft)
        {
            int damage = 120;
            if (Main.rand.Next(2) == 0) // + lasers
            {
                Main.PlaySound(SoundID.Item73, (int)projectile.position.X, (int)projectile.position.Y);
                int a = Projectile.NewProjectile(new Vector2(projectile.Center.X, projectile.Center.Y), new Vector2(0f, -12f), mod.ProjectileType("VoidBlast"), damage, 3);
                Main.projectile[a].Center = projectile.Center;
                int b = Projectile.NewProjectile(new Vector2(projectile.Center.X, projectile.Center.Y), new Vector2(0f, 12f), mod.ProjectileType("VoidBlast"), damage, 3);
                Main.projectile[b].Center = projectile.Center;
                int c = Projectile.NewProjectile(new Vector2(projectile.Center.X, projectile.Center.Y), new Vector2(-12f, 0f), mod.ProjectileType("VoidBlast"), damage, 3);
                Main.projectile[c].Center = projectile.Center;
                int d = Projectile.NewProjectile(new Vector2(projectile.Center.X, projectile.Center.Y), new Vector2(12f, 0f), mod.ProjectileType("VoidBlast"), damage, 3);
                Main.projectile[d].Center = projectile.Center;
            }
            else // x lasers
            {
                Main.PlaySound(SoundID.Item73, (int)projectile.position.X, (int)projectile.position.Y);
                int a = Projectile.NewProjectile(new Vector2(projectile.Center.X, projectile.Center.Y), new Vector2(8f, 8f), mod.ProjectileType("VoidBlast"), damage, 3);
                Main.projectile[a].Center = projectile.Center;
                int b = Projectile.NewProjectile(new Vector2(projectile.Center.X, projectile.Center.Y), new Vector2(8f, -8f), mod.ProjectileType("VoidBlast"), damage, 3);
                Main.projectile[b].Center = projectile.Center;
                int c = Projectile.NewProjectile(new Vector2(projectile.Center.X, projectile.Center.Y), new Vector2(-8f, 8f), mod.ProjectileType("VoidBlast"), damage, 3);
                Main.projectile[c].Center = projectile.Center;
                int d = Projectile.NewProjectile(new Vector2(projectile.Center.X, projectile.Center.Y), new Vector2(-8f, -8f), mod.ProjectileType("VoidBlast"), damage, 3);
                Main.projectile[d].Center = projectile.Center;
            }
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            Texture2D t = Main.projectileTexture[projectile.type];
            Rectangle frame = BaseDrawing.GetFrame(projectile.frame, Main.projectileTexture[projectile.type].Width, Main.projectileTexture[projectile.type].Height / 5, 0, 0);
            BaseDrawing.DrawAfterimage(spriteBatch, t, 0, projectile, 1, 1, 3, true, 0, 0, projectile.GetAlpha(Colors.COLOR_GLOWPULSE), frame, 5);
            BaseDrawing.DrawTexture(spriteBatch, t, 0, projectile, projectile.GetAlpha(Colors.COLOR_GLOWPULSE), true);
            return false;
        }
    }
}
 