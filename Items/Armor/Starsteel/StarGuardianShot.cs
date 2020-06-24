using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace CSkies.Items.Armor.Starsteel
{
    public class StarGuardianShot : ModProjectile
	{
		public override void SetDefaults()
		{
			projectile.width = 14;
			projectile.height = 14;
			projectile.aiStyle = 1;
			projectile.friendly = true;
			projectile.penetrate = 1;
			projectile.aiStyle = 27;
            projectile.minion = true;
            projectile.minionSlots = 0;
            projectile.timeLeft = 300;
			ProjectileID.Sets.TrailCacheLength[projectile.type] = 5;
			ProjectileID.Sets.TrailingMode[projectile.type] = 2;
		}
		
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Star");
		}
		
		public override Color? GetAlpha(Color lightColor)
		{
			return Colors.COLOR_GLOWPULSE;
		}
		
		public override void AI()
        {
            if (Main.rand.Next(2) == 0)
			{
				int dustnumber = Dust.NewDust(projectile.position, projectile.width, projectile.height, ModContent.DustType<Dusts.StarDust>(), 0f, 0f, 200, Color.Cyan, 0.8f);
				Main.dust[dustnumber].velocity *= 0.3f;
			}

            projectile.rotation -= projectile.velocity.X * .04f;
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            if (projectile.velocity.X != oldVelocity.X)
            {
                projectile.position.X = projectile.position.X + projectile.velocity.X;
                projectile.velocity.X = -oldVelocity.X;
                projectile.damage = (int)(projectile.damage * 1.2);
            }
            if (projectile.velocity.Y != oldVelocity.Y)
            {
                projectile.position.Y = projectile.position.Y + projectile.velocity.Y;
                projectile.velocity.Y = -oldVelocity.Y;
                projectile.damage = (int)(projectile.damage * 1.2);
            }

            return false;
        }

        public override void Kill(int a)
        {
            for (int num468 = 0; num468 < 5; num468++)
            {
                int num469 = Dust.NewDust(new Vector2(projectile.Center.X, projectile.Center.Y), projectile.width, projectile.height, ModContent.DustType<Dusts.StarDust>(), -projectile.velocity.X * 0.2f,
                    -projectile.velocity.Y * 0.2f, 100, Color.Cyan, 2f);
                Main.dust[num469].noGravity = true;
            }
        }
    }
}