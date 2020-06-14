using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework.Graphics;
using Terraria.ID;

namespace CSkies.NPCs.Bosses.Novacore
{
    public class Novashock : ModProjectile
	{
        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[projectile.type] = 25;
        }
        public override void SetDefaults()
        {
            projectile.penetrate = -1;
            projectile.width = 14;
            projectile.height = 14;
            projectile.aiStyle = -1;
            projectile.hostile = true;
            projectile.alpha = 255;
            projectile.ignoreWater = true;
            projectile.tileCollide = false;
            projectile.extraUpdates = 4;
            projectile.timeLeft = 600;
        }
		
        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
            for (int i = 0; i < projectile.oldPos.Length; i++)
            {
                projHitbox.X = (int)projectile.oldPos[i].X;
                projHitbox.Y = (int)projectile.oldPos[i].Y;
                if (projHitbox.Intersects(targetHitbox))
                {
                    return true;
                }
            }
            return base.Colliding(projHitbox, targetHitbox);
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            if (projectile.localAI[1] < 1f)
            {
                projectile.localAI[1] += 2f;
                projectile.position += projectile.velocity;
                projectile.velocity = Vector2.Zero;
            }
            return false;
        }

        public override void AI()
		{
			Lighting.AddLight(projectile.Center, 0.45f, 0f, 0.5f);

            CAI.LightningAI(projectile);
		}

		public override bool PreDraw(SpriteBatch sb, Color lightColor)
        {
            Vector2 end3 = projectile.position + new Vector2(projectile.width, projectile.height) / 2f + Vector2.UnitY * projectile.gfxOffY - Main.screenPosition;
            Texture2D value139 = Main.extraTexture[33];
            projectile.GetAlpha(lightColor);
            Vector2 vector55 = new Vector2(projectile.scale) / 2f;
            for (int num343 = 0; num343 < 2; num343++)
            {
                float num344 = (projectile.localAI[1] == -1f || projectile.localAI[1] == 1f) ? (-0.2f) : 0f;
                if (num343 == 0)
                {
                    vector55 = new Vector2(projectile.scale) * (0.5f + num344);
                    DelegateMethods.c_1 = Color.Purple * 0.5f;
                }
                else
                {
                    vector55 = new Vector2(projectile.scale) * (0.3f + num344);
                    DelegateMethods.c_1 = Color.White * 0.5f;
                }
                DelegateMethods.f_1 = 1f;
                for (int num345 = projectile.oldPos.Length - 1; num345 > 0; num345--)
                {
                    if (!(projectile.oldPos[num345] == Vector2.Zero))
                    {
                        Vector2 start3 = projectile.oldPos[num345] + new Vector2(projectile.width, projectile.height) / 2f + Vector2.UnitY * projectile.gfxOffY - Main.screenPosition;
                        Vector2 end4 = projectile.oldPos[num345 - 1] + new Vector2(projectile.width, projectile.height) / 2f + Vector2.UnitY * projectile.gfxOffY - Main.screenPosition;
                        Utils.DrawLaser(sb, value139, start3, end4, vector55, DelegateMethods.LightningLaserDraw);
                    }
                }
                if (projectile.oldPos[0] != Vector2.Zero)
                {
                    DelegateMethods.f_1 = 1f;
                    Vector2 start4 = projectile.oldPos[0] + new Vector2(projectile.width, projectile.height) / 2f + Vector2.UnitY * projectile.gfxOffY - Main.screenPosition;
                    Utils.DrawLaser(sb, value139, start4, end3, vector55, DelegateMethods.LightningLaserDraw);
                }
            }
            return false;
        }
    }
}