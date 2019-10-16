using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using System;

namespace CSkies.Projectiles.Star
{
    public class ShieldStar : ModProjectile
	{
		public override void SetDefaults()
        {
            projectile.width = 24;
            projectile.height = 24;
            projectile.aiStyle = 5;
            projectile.friendly = true;
            projectile.penetrate = 2;
            projectile.alpha = 50;
            projectile.scale = 0.8f;
            projectile.tileCollide = false;
        }
		
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Star");
		}
		
		public override Color? GetAlpha(Color lightColor)
		{
			return Color.White;
		}
		
		public override void AI()
        {
            if (projectile.position.Y > projectile.ai[1])
            {
                projectile.tileCollide = true;
            }
            if (projectile.soundDelay == 0)
            {
                projectile.soundDelay = 20 + Main.rand.Next(40);
                Main.PlaySound(SoundID.Item9, projectile.position);
            }
            if (projectile.localAI[0] == 0f)
            {
                projectile.localAI[0] = 1f;
            }
            projectile.alpha += (int)(25f * projectile.localAI[0]);
            if (projectile.alpha > 200)
            {
                projectile.alpha = 200;
                projectile.localAI[0] = -1f;
            }
            if (projectile.alpha < 0)
            {
                projectile.alpha = 0;
                projectile.localAI[0] = 1f;
            }
            projectile.rotation += (Math.Abs(projectile.velocity.X) + Math.Abs(projectile.velocity.Y)) * 0.01f * projectile.direction;
            projectile.light = 0.9f;
            if (Main.rand.Next(10) == 0)
            {
                Dust.NewDust(projectile.position, projectile.width, projectile.height, ModContent.DustType<Dusts.StarDust>(), projectile.velocity.X * 0.5f, projectile.velocity.Y * 0.5f, 150, default, 1.2f);
            }
            if (Main.rand.Next(20) == 0)
            {
                Gore.NewGore(projectile.position, new Vector2(projectile.velocity.X * 0.2f, projectile.velocity.Y * 0.2f), Main.rand.Next(16, 18), 1f);
                return;
            }
        }

        public override void Kill(int i)
        {
            Main.PlaySound(SoundID.Item10, projectile.position);
            for (int num479 = 0; num479 < 10; num479++)
            {
                Dust.NewDust(projectile.position, projectile.width, projectile.height, ModContent.DustType<Dusts.StarDust>(), projectile.velocity.X * 0.1f, projectile.velocity.Y * 0.1f, 150, default, 1.2f);
            }
            for (int num480 = 0; num480 < 3; num480++)
            {
                Gore.NewGore(projectile.position, new Vector2(projectile.velocity.X * 0.05f, projectile.velocity.Y * 0.05f), Main.rand.Next(16, 18), 1f);
            }
        }
    }
}