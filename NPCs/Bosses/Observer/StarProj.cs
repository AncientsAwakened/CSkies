
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Audio;
using Terraria.ModLoader;

namespace CSkies.NPCs.Bosses.Observer
{
    public class StarProj : ModProjectile
    {
        public override void SetDefaults()
        {
            projectile.width = 30;
            projectile.height = 30;
            projectile.aiStyle = 0;
            projectile.friendly = false;
            projectile.hostile = true;
            projectile.timeLeft = 180;
            projectile.penetrate = 1;
            projectile.tileCollide = true;
        }

		public override void SetStaticDefaults()
		{
		    DisplayName.SetDefault("Cosmic Star");
		}

        public override void AI()
        {
            projectile.rotation += .2f;
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            Main.PlaySound(new LegacySoundStyle(2, 89, Terraria.Audio.SoundType.Sound));
            Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y - 15, projectile.velocity.X, projectile.velocity.Y, mod.ProjectileType("Shock"), projectile.damage, projectile.knockBack, projectile.owner, 0f, 0f);
            return true;
        }

        public override void Kill(int timeLeft)
        {
            int dustID = Dust.NewDust(projectile.position, projectile.width, projectile.height, 17, projectile.velocity.X * 0.2f, projectile.velocity.Y * 0.2f, 0, Color.White, 1f);
            Main.dust[dustID].velocity = new Vector2(MathHelper.Lerp(-1f, 1f, (float)Main.rand.NextDouble()), MathHelper.Lerp(-1f, 1f, (float)Main.rand.NextDouble()));
        }

        public override bool PreDraw(SpriteBatch sb, Color dColor)
        {
            BaseDrawing.DrawTexture(sb, Main.projectileTexture[projectile.type], 0, projectile, projectile.GetAlpha(Colors.COLOR_GLOWPULSE), true);
            return false;
        }
    }
}
