using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Terraria;
using Terraria.ModLoader;

namespace CSkies.NPCs.Bosses.Starcore
{
    public class Starblast : ModProjectile
	{
        public int damage = 0;

		public override void SetStaticDefaults()
		{
            DisplayName.SetDefault("Starbeam");
		}

        public override void SetDefaults()
        {
            projectile.tileCollide = false;
            projectile.width = 30;
            projectile.height = 30;
            projectile.aiStyle = -1;
            projectile.penetrate = -1;
            projectile.hostile = true;
            projectile.ignoreWater = true;
            projectile.timeLeft = 600;
        }

        public override void AI()
        {
            Lighting.AddLight(projectile.Center, 0f, .3f, 0f);
            projectile.rotation = projectile.velocity.ToRotation() + MathHelper.ToRadians(90f);
            for (int m = projectile.oldPos.Length - 1; m > 0; m--)
            {
                projectile.oldPos[m] = projectile.oldPos[m - 1];
            }
            projectile.oldPos[0] = projectile.position;
        }

        public override void Kill(int timeLeft)
        {
            int dustID = Dust.NewDust(projectile.position, projectile.width, projectile.height, ModContent.DustType<Dusts.StarDust>(), projectile.velocity.X * 0.2f, projectile.velocity.Y * 0.2f, 10, Color.White, 1f);
            Main.dust[dustID].velocity = new Vector2(MathHelper.Lerp(-1f, 1f, (float)Main.rand.NextDouble()), MathHelper.Lerp(-1f, 1f, (float)Main.rand.NextDouble()));
        }

        public override bool PreDraw(SpriteBatch sb, Color dColor)
        {
            BaseDrawing.DrawAfterimage(sb, Main.projectileTexture[projectile.type], 0, projectile, 2.5f, 1, 3, true, 0f, 0f, projectile.GetAlpha(Colors.COLOR_GLOWPULSE));
            BaseDrawing.DrawTexture(sb, Main.projectileTexture[projectile.type], 0, projectile, projectile.GetAlpha(Colors.COLOR_GLOWPULSE), true);
            return false;
		}		
	}
}