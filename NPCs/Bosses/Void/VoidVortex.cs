using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Terraria;
using Terraria.ModLoader;

namespace CSkies.NPCs.Bosses.Void
{
    public class VoidVortex : ModProjectile
	{
        public override void SetStaticDefaults()
		{
            DisplayName.SetDefault("Void Vortex");
        }

        public override void SetDefaults()
        {
            projectile.tileCollide = false;
            projectile.width = 30;
            projectile.height = 30;
            projectile.aiStyle = -1;
            projectile.penetrate = -1;
            projectile.friendly = false;
            projectile.hostile = true;
            projectile.ignoreWater = true;
            projectile.timeLeft = 180;
        }

        public override void AI()
        {
            Lighting.AddLight(projectile.Center, 0, .1f, .3f);

            for (int u = 0; u < Main.maxPlayers; u++)
            {
                Player target = Main.player[u];

                if (target.active && Vector2.Distance(projectile.Center, target.Center) < 100 && !target.immune)
                {
                    float num3 = 6f;
                    Vector2 vector = new Vector2(target.position.X + target.width / 2, target.position.Y + target.height / 2);
                    float num4 = projectile.Center.X - vector.X;
                    float num5 = projectile.Center.Y - vector.Y;
                    float num6 = (float)Math.Sqrt(num4 * num4 + num5 * num5);
                    num6 = num3 / num6;
                    num4 *= num6;
                    num5 *= num6;
                    int num7 = 3;
                    target.velocity.X = (target.velocity.X * (num7 - 1) + num4) / num7;
                    target.velocity.Y = (target.velocity.Y * (num7 - 1) + num5) / num7;
                }
            }
        }

        public float auraPercent = 0f;
        public bool auraDirection = true;

        public override bool PreDraw(SpriteBatch spritebatch, Color lightColor)
        {
            Texture2D Tex = Main.projectileTexture[projectile.type];
            Rectangle frame = new Rectangle(0, 0, Tex.Width, Tex.Height / 4);

            if (auraDirection) { auraPercent += 0.1f; auraDirection = auraPercent < 1f; }
            else { auraPercent -= 0.1f; auraDirection = auraPercent <= 0f; }

            BaseDrawing.DrawTexture(spritebatch, Tex, 0, projectile.position, projectile.width, projectile.height, projectile.scale, -projectile.rotation, 0, 4, frame, projectile.GetAlpha(Color.White), true);

            BaseDrawing.DrawAura(spritebatch, Tex, 0, projectile, auraPercent, 2f, 0f, 0f, projectile.GetAlpha(Color.White));
            return false;
        }
    }
}