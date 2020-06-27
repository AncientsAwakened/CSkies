using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Utilities;

namespace CSkies.NPCs.Bosses.Void
{
    public class VoidShock : ModProjectile
    {

        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[projectile.type] = 25;
            ProjectileID.Sets.TrailingMode[projectile.type] = 1;
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
            projectile.tileCollide = true;
            projectile.extraUpdates = 4;
            projectile.timeLeft = 120;
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

        public override void AI()
        {
            Lighting.AddLight(projectile.Center, 0.45f, 0f, 0.5f);

            CAI.LightningAI(projectile, ref projectile.ai, ref projectile.localAI, 74);
        }

        public override bool PreDraw(SpriteBatch sb, Color lightColor)
        {
            CDrawing.LightningDraw(projectile, sb, Color.White, new Color(30, 30, 50), ref projectile.localAI);
            return false;
        }
    }
}
 