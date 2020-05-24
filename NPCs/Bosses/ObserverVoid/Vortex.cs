using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace CSkies.NPCs.Bosses.ObserverVoid
{
    class Vortex : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Vortex");
        }

        public override void SetDefaults()
        {
            projectile.width = 60;
            projectile.height = 60;
            projectile.timeLeft = 180;
            projectile.aiStyle = -1;
            projectile.alpha = 255;
            projectile.tileCollide = false;
            projectile.penetrate = -1;
        }

        public override void AI()
        {
            Lighting.AddLight(projectile.Center, 0, 0f, .15f);
            projectile.rotation += 0.1f;
            projectile.velocity *= 0;
            if (projectile.alpha > 0)
            {
                projectile.alpha -= 3;
            }
            else
            {
                projectile.alpha = 0;
            }
            if (Main.netMode != NetmodeID.MultiplayerClient)
            {
                projectile.ai[0]++;
            }
            if (projectile.ai[0] > 120)
            {
                projectile.scale = projectile.ai[1];

                if (projectile.ai[0] == 121)
                {
                    projectile.netUpdate = true;
                }
                if (projectile.ai[1] > 0)
                {
                    projectile.ai[1] -= .01f;
                }

                if (projectile.ai[0] <= 0 && Main.netMode != NetmodeID.MultiplayerClient)
                {
                    projectile.active = false;
                    projectile.netUpdate = true;
                }
            }

            for (int u = 0; u < Main.maxPlayers; u++)
            {
                Player target = Main.player[u];

                if (target.active && Vector2.Distance(projectile.Center, target.Center) < 160 * projectile.ai[1] && !target.immune)
                {
                    float num3 = 3f;
                    Vector2 vector = target.Center;
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


        public override bool PreDraw(SpriteBatch spritebatch, Color lightColor)
        {
            Texture2D Tex = Main.projectileTexture[projectile.type];
            Texture2D Vortex = mod.GetTexture("NPCs/Bosses/ObserverVoid/Vortex1");
            Rectangle frame = new Rectangle(0, 0, Tex.Width, Tex.Height);
            BaseDrawing.DrawTexture(spritebatch, Vortex, 0, projectile.position, projectile.width, projectile.height, projectile.scale, projectile.rotation, 0, 1, frame, projectile.GetAlpha(Colors.COLOR_GLOWPULSE), true);
            BaseDrawing.DrawTexture(spritebatch, Tex, 0, projectile.position, projectile.width, projectile.height, projectile.scale, -projectile.rotation, 0, 1, frame, projectile.GetAlpha(Colors.COLOR_GLOWPULSE), true);
            return false;
        }
    }
}
