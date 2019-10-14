using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ModLoader;

namespace CSkies.Projectiles
{
    public class VoidEyeVortex : ModProjectile
    {
        public override string Texture => "CSkies/Projectiles/Singularity";
        public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Singularity");
		}
        public override void SetDefaults()
        {
            projectile.width = 20;
            projectile.height = 20;
            projectile.friendly = true;
            projectile.penetrate = -1; 
            projectile.melee = true;
            projectile.knockBack = 0;
            projectile.alpha = 255;
            projectile.tileCollide = false;
        }
		
		public override void AI()
		{
            projectile.Center = Main.player[projectile.owner].Center;
            if (projectile.ai[0] == 0)
            {
                if (projectile.alpha > 0)
                {
                    projectile.alpha -= 5;
                    projectile.netUpdate = true;
                }
                else
                {
                    projectile.ai[0] = 1;
                }
            }
            else if (projectile.ai[0] == 1)
            {
                if (Main.netMode != 1 && projectile.ai[1]++ > 120)
                {
                    projectile.ai[0] = 2;
                    projectile.netUpdate = true;
                }
            }
            else
            {
                if (projectile.alpha < 255)
                {
                    projectile.alpha += 5;
                }
                else
                {
                    projectile.active = false;
                    projectile.netUpdate = true;
                }
            }

            for (int u = 0; u < Main.maxNPCs; u++)
            {
                NPC target = Main.npc[u];

                if (target.active && Vector2.Distance(projectile.Center, target.Center) < 300)
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
        }
 
        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            Texture2D Tex = Main.projectileTexture[projectile.type];
            Texture2D Vortex = mod.GetTexture("NPCs/Bosses/ObserverVoid/Vortex1");
            Rectangle frame = new Rectangle(0, 0, Tex.Width, Tex.Height);
            BaseDrawing.DrawTexture(spriteBatch, Vortex, 0, projectile.position, projectile.width, projectile.height, projectile.scale, projectile.rotation, 0, 1, frame, projectile.GetAlpha(Colors.COLOR_GLOWPULSE), true);
            BaseDrawing.DrawTexture(spriteBatch, Tex, 0, projectile.position, projectile.width, projectile.height, projectile.scale, -projectile.rotation, 0, 1, frame, projectile.GetAlpha(Colors.COLOR_GLOWPULSE), true);
            return false;
        }
    }
}