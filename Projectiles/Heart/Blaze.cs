using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CSkies.Projectiles.Heart
{
    public class Blaze : ModProjectile
    {
        public override void SetDefaults()
        {
            projectile.width = 4;
            projectile.height = 4;
            projectile.aiStyle = -1;
            projectile.friendly = true;
            projectile.hostile = false;
            projectile.ranged = true;
            projectile.extraUpdates = 100;
            projectile.timeLeft = 400;
            projectile.penetrate = -1;
            projectile.tileCollide = false;
        }

        public override void AI()
        {
            projectile.localAI[0] += 1f;
            if (projectile.localAI[0] > 9f)
            {
                for (int num447 = 0; num447 < 4; num447++)
                {
                    Vector2 vector33 = projectile.position;
                    vector33 -= projectile.velocity * (num447 * 0.25f);
                    projectile.alpha = 255;
                    int num448 = Dust.NewDust(vector33, projectile.width, projectile.height, DustID.SolarFlare, 0f, 0f, 200, default, .8f);
                    Main.dust[num448].position = vector33;
                    Main.dust[num448].scale = Main.rand.Next(70, 110) * 0.013f;
                    Main.dust[num448].velocity *= 0.2f;
                    Main.dust[num448].noGravity = true;
                }
            }
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            target.AddBuff(BuffID.Daybreak, 200);
            int b = Projectile.NewProjectile(target.position, Vector2.Zero, ModContent.ProjectileType<BlazeBoom>(), projectile.damage, projectile.knockBack, Main.myPlayer);
            Main.projectile[b].Center = target.Center;
            for (int num468 = 0; num468 < 8; num468++)
            {
                int num469 = Dust.NewDust(new Vector2(target.position.X, target.position.Y), target.width, target.height, DustID.Fire, -target.velocity.X * 0.2f,
                    -target.velocity.Y * 0.2f, 100, default, 2f);
                Main.dust[num469].noGravity = true;
                Main.dust[num469].velocity *= 2;
            }
        }
    }
}
