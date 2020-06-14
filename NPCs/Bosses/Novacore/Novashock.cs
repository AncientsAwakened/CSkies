using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework.Graphics;
using Terraria.ID;

namespace CSkies.NPCs.Bosses.Novacore
{
    public class Novashock : ModProjectile
	{
		public override void SetDefaults()
        {
            projectile.penetrate = -1;
            projectile.width = 14;
            projectile.height = 14;
            projectile.aiStyle = 88;
            aiType = ProjectileID.VortexLightning;
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
            CDrawing.LightningDraw(projectile, sb, lightColor, Color.Purple, Color.White);
			return false;
        }
    }
}