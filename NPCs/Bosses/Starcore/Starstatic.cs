using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace CSkies.NPCs.Bosses.Starcore
{
    public class Starstatic : ModProjectile
	{
        public int damage = 0;

		public override void SetStaticDefaults()
		{
            DisplayName.SetDefault("Star Static");
            Main.projFrames[projectile.type] = 4;
        }

        public override void SetDefaults()
        {
            projectile.tileCollide = false;
            projectile.width = 24;
            projectile.height = 24;
            projectile.aiStyle = -1;
            projectile.penetrate = -1;
            projectile.hostile = true;
            projectile.ignoreWater = true;
            projectile.timeLeft = 600;
        }

        public override void AI()
        {
            Lighting.AddLight(projectile.Center, 0f, .4f, 0f);
            projectile.rotation = projectile.velocity.ToRotation() + MathHelper.ToRadians(90f);
            if (projectile.frameCounter++ > 5)
            {
                projectile.frameCounter = 0;
                if (projectile.frame++ > 2)
                {
                    projectile.frame = 0;
                }
            }
        }

        public override Color? GetAlpha(Color lightColor)
        {
            return Color.White;
        }

        public override void Kill(int timeLeft)
        {
            int dustID = Dust.NewDust(projectile.position, projectile.width, projectile.height, ModContent.DustType<Dusts.StarDust>(), projectile.velocity.X * 0.2f, projectile.velocity.Y * 0.2f, 10, Color.White, 1f);
            Main.dust[dustID].velocity = new Vector2(MathHelper.Lerp(-1f, 1f, (float)Main.rand.NextDouble()), MathHelper.Lerp(-1f, 1f, (float)Main.rand.NextDouble()));
        }
	}
}