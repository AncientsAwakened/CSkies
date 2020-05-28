using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CSkies.Projectiles
{
    public class FallenShard : ModProjectile
	{
		public override void SetDefaults()
        {
            projectile.width = 16;
            projectile.height = 16;
            projectile.aiStyle = -1;
            projectile.friendly = true;
            projectile.penetrate = -1;
            projectile.alpha = 50;
            projectile.light = 1f;
        }
		
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Comet Knife");
		}
		
		public override Color? GetAlpha(Color lightColor)
		{
			return Color.White;
		}

        public override void AI()
        {
            if (projectile.ai[1] == 0f && !Collision.SolidCollision(projectile.position, projectile.width, projectile.height))
            {
                projectile.ai[1] = 1f;
                projectile.netUpdate = true;
            }
            if (projectile.ai[1] != 0f)
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
            if (projectile.ai[1] == 1f)
            {
                projectile.light = 0.9f;
                if (Main.rand.Next(10) == 0)
                {
                    Dust.NewDust(projectile.position, projectile.width, projectile.height, 58, projectile.velocity.X * 0.5f, projectile.velocity.Y * 0.5f, DustID.Electric, Color.Blue, 1.2f);
                }
                if (Main.rand.Next(20) == 0)
                {
                    Gore.NewGore(projectile.position, new Vector2(projectile.velocity.X * 0.2f, projectile.velocity.Y * 0.2f), Main.rand.Next(16, 18), 1f);
                    return;
                }
            }
        }

        public override void Kill(int timeLeft)
        {
            Main.PlaySound(SoundID.Item10, projectile.position);
            int num535 = 10;
            int num536 = 3;
            for (int num537 = 0; num537 < num535; num537++)
            {
                Dust.NewDust(projectile.position, projectile.width, projectile.height, 17, projectile.velocity.X * 0.1f, projectile.velocity.Y * 0.1f, DustID.Electric, Color.Blue, 1.2f);
            }
            for (int num538 = 0; num538 < num536; num538++)
            {
                int num539 = Main.rand.Next(16, 18);
                Gore.NewGore(projectile.position, new Vector2(projectile.velocity.X * 0.05f, projectile.velocity.Y * 0.05f), num539, 1f);
            }
            if (projectile.damage < 100)
            {
                for (int num540 = 0; num540 < 10; num540++)
                {
                    Dust.NewDust(projectile.position, projectile.width, projectile.height, 17, projectile.velocity.X * 0.1f, projectile.velocity.Y * 0.1f, DustID.Electric, Color.Blue, 1.2f);
                }
                for (int num541 = 0; num541 < 3; num541++)
                {
                    Gore.NewGore(projectile.position, new Vector2(projectile.velocity.X * 0.05f, projectile.velocity.Y * 0.05f), Main.rand.Next(16, 18), 1f);
                }
            }

            Item.NewItem((int)projectile.position.X, (int)projectile.position.Y, projectile.width, projectile.height, ModContent.ItemType<Items.Other.CometShard>(), 1, false, 0, false, false);
        }
    }
}