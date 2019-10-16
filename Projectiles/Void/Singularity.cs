using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;

namespace CSkies.Projectiles.Void
{
    public class Singularity : ModProjectile
    {
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
        }
		
		public override void AI()
		{
            if (projectile.timeLeft == 120)
            {
                projectile.ai[0] = 1f;
            }

            if (Main.player[projectile.owner].dead)
            {
                projectile.Kill();
                return;
            }

            Main.player[projectile.owner].itemAnimation = 5;
            Main.player[projectile.owner].itemTime = 5;

            projectile.rotation += .2f;

            if (projectile.alpha == 0)
            {
                if (projectile.position.X + (projectile.width / 2) > Main.player[projectile.owner].position.X + (Main.player[projectile.owner].width / 2))
                {
                    Main.player[projectile.owner].ChangeDir(1);
                }
                else
                {
                    Main.player[projectile.owner].ChangeDir(-1);
                }
            }
            Vector2 vector14 = new Vector2(projectile.position.X + (projectile.width * 0.5f), projectile.position.Y + (projectile.height * 0.5f));
            float num166 = Main.player[projectile.owner].position.X + (Main.player[projectile.owner].width / 2) - vector14.X;
            float num167 = Main.player[projectile.owner].position.Y + (Main.player[projectile.owner].height / 2) - vector14.Y;
            float num168 = (float)Math.Sqrt((num166 * num166) + (num167 * num167));
            if (projectile.ai[0] == 0f)
            {
                if (num168 > 700f)
                {
                    projectile.ai[0] = 1f;
                }
                else if (num168 > 500f)
                {
                    projectile.ai[0] = 1f;
                }
                projectile.ai[1] += 1f;
                if (projectile.ai[1] > 5f)
                {
                    projectile.alpha = 0;
                }
                if (projectile.ai[1] > 8f)
                {
                    projectile.ai[1] = 8f;
                }
                if (projectile.ai[1] >= 10f)
                {
                    projectile.ai[1] = 15f;
                    projectile.velocity.Y = projectile.velocity.Y + 0.3f;
                }
                if (projectile.velocity.X < 0f)
                {
                    projectile.spriteDirection = -1;
                }
                else
                {
                    projectile.spriteDirection = 1;
                }
            }
            else if (projectile.ai[0] == 1f)
            {
                projectile.tileCollide = false;
                float num169 = 30f;

                if (num168 < 50f)
                {
                    projectile.Kill();
                }
                num168 = num169 / num168;
                num166 *= num168;
                num167 *= num168;
                projectile.velocity.X = num166;
                projectile.velocity.Y = num167;
                if (projectile.velocity.X < 0f)
                {
                    projectile.spriteDirection = 1;
                }
                else
                {
                    projectile.spriteDirection = -1;
                }
            }
            for (int u = 0; u < Main.maxNPCs; u++)
            {
                NPC target = Main.npc[u];

                if (target.active && !target.boss && Vector2.Distance(projectile.Center, target.Center) < 200)
                {
                    float num3 = 12f;
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
		
		public override void OnHitNPC (NPC target, int damage, float knockback, bool crit)
		{
            projectile.ai[0] = 1f;
        }
		
		public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough)
        {
            width = 16;
            height = 16;
            return true;
        }
		
		public override bool OnTileCollide (Vector2 oldVelocity)
		{
			projectile.ai[0] = 1f;
			return false;
		}
		
 
        // chain voodoo
        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
        { 
            Texture2D texture = ModContent.GetTexture("CSkies/Projectiles/Void/Singularity_Chain");
 
            Vector2 position = projectile.Center;
            Vector2 mountedCenter = Main.player[projectile.owner].MountedCenter;
            Rectangle? sourceRectangle = new Rectangle?();
            Vector2 origin = new Vector2(texture.Width * 0.5f, texture.Height * 0.5f);
            float num1 = texture.Height;
            Vector2 vector24 = mountedCenter - position;
            float rotation = (float)Math.Atan2(vector24.Y, vector24.X) - 1.57f;
            bool flag = true;
            if (float.IsNaN(position.X) && float.IsNaN(position.Y))
                flag = false;
            if (float.IsNaN(vector24.X) && float.IsNaN(vector24.Y))
                flag = false;
            while (flag)
            {
                if (vector24.Length() < num1 + 1.0)
                {
                    flag = false;
                }
                else
                {
                    Vector2 vector21 = vector24;
                    vector21.Normalize();
                    position += vector21 * num1;
                    vector24 = mountedCenter - position;
                    Color color2 = Lighting.GetColor((int)position.X / 16, (int)(position.Y / 16.0));
                    color2 = projectile.GetAlpha(color2);
                    Main.spriteBatch.Draw(texture, position - Main.screenPosition, sourceRectangle, color2, rotation, origin, 1, SpriteEffects.None, 0.0f);
                }
            }
            Texture2D Tex = Main.projectileTexture[projectile.type];
            Texture2D Vortex = mod.GetTexture("NPCs/Bosses/ObserverVoid/Vortex1");
            Rectangle frame = new Rectangle(0, 0, Tex.Width, Tex.Height);
            BaseDrawing.DrawTexture(spriteBatch, Vortex, 0, projectile.position, projectile.width, projectile.height, projectile.scale, projectile.rotation, 0, 1, frame, projectile.GetAlpha(Colors.COLOR_GLOWPULSE), true);
            BaseDrawing.DrawTexture(spriteBatch, Tex, 0, projectile.position, projectile.width, projectile.height, projectile.scale, -projectile.rotation, 0, 1, frame, projectile.GetAlpha(Colors.COLOR_GLOWPULSE), true);
            return false;
        }
    }
}