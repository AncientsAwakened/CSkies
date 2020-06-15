using Microsoft.Xna.Framework;
using Terraria.ID;
using Terraria.ModLoader;

namespace CSkies.NPCs.Enemies.Observatory
{
    public class ObservatoryLaser : ModProjectile
    {
    	public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Laser");
		}
    	
        public override void SetDefaults()
        {
            projectile.width = 10;
            projectile.height = 10;
            projectile.hostile = true;
            projectile.ignoreWater = true;
            projectile.tileCollide = true;
            projectile.penetrate = 1;
            projectile.timeLeft = 600;
            projectile.alpha = 30;
            projectile.extraUpdates = 1;
            aiType = ProjectileID.Bullet;
        }

        public override Color? GetAlpha(Color lightColor)
        {
            return Color.White;
        }
    }
}