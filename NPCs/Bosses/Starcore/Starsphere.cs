using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace CSkies.NPCs.Bosses.Starcore
{
    public class Starsphere : ModProjectile
	{
		public override void SetDefaults()
		{
			projectile.width = 14;
			projectile.height = 14;
			projectile.aiStyle = 1;
			projectile.hostile = true;
			projectile.penetrate = 1;
			projectile.aiStyle = -1;
			projectile.timeLeft = 1200;
            Main.projFrames[projectile.type] = 5;
        }
		
		public override Color? GetAlpha(Color lightColor)
		{
			return Color.White;
		}
		
		public override void AI()
        {
            Lighting.AddLight(projectile.Center, 0f, .5f, 0f);
            if (projectile.ai[0]++ > 20)
            {
                projectile.velocity *= .93f;
            }

            if (projectile.ai[0] > 120)
            {
                projectile.Kill();
            }
            if (projectile.frameCounter++ > 5)
            {
                projectile.frameCounter = 0;
                if (projectile.frame++ > 3)
                {
                    projectile.frame = 0;
                }
            }
        }

        public override void Kill(int timeleft)
        {
            Main.PlaySound(SoundID.Item94, projectile.position);
            int num290 = Main.rand.Next(3, 7);
            for (int num291 = 0; num291 < num290; num291++)
            {
                int num292 = Dust.NewDust(projectile.position, projectile.width, projectile.height, 135, 0f, 0f, 100, default, 2.1f);
                Main.dust[num292].velocity *= 2f;
                Main.dust[num292].noGravity = true;
            }
            for (int num293 = 0; num293 < 1000; num293++)
            {
                Rectangle value19 = new Rectangle((int)projectile.Center.X - 40, (int)projectile.Center.Y - 40, 80, 80);
                if (num293 != projectile.whoAmI && Main.projectile[num293].active && Main.projectile[num293].owner == projectile.owner && Main.projectile[num293].type == ModContent.ProjectileType<StormRing>() && Main.projectile[num293].getRect().Intersects(value19))
                {
                    Main.projectile[num293].ai[1] = 1f;
                    Main.projectile[num293].velocity = (projectile.Center - Main.projectile[num293].Center) / 5f;
                    Main.projectile[num293].netUpdate = true;
                }
            }
            Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, 0f, 0f, ModContent.ProjectileType<StormRing>(), projectile.damage / 4, 0f, projectile.owner, 0f, 0f);
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            projectile.velocity *= .90f;
            return false;
        }
    }
}