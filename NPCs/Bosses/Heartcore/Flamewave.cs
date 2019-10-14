using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Graphics.Shaders;
using Terraria.ID;
using Terraria.ModLoader;

namespace CSkies.NPCs.Bosses.Heartcore
{
    public class Flamewave : ModProjectile
	{
		public override void SetDefaults()
		{
			projectile.width = 34;
			projectile.height = 34;
			projectile.friendly = false;
            projectile.hostile = true;
            projectile.penetrate = 2;
			projectile.aiStyle = -1;
			projectile.timeLeft = 300;
			projectile.penetrate = 5;
            projectile.tileCollide = false;
            projectile.extraUpdates = 1;
		}
        
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Flamewave");
		}
		
		public override Color? GetAlpha(Color lightColor)
		{
			return Color.White;
		}

        public override void OnHitPlayer(Player target, int damage, bool crit)
		{
            for (int num468 = 0; num468 < 5; num468++)
            {
                int num469 = Dust.NewDust(new Vector2(projectile.Center.X, projectile.Center.Y), projectile.width, projectile.height, DustID.SolarFlare, -projectile.velocity.X * 0.2f,
                    -projectile.velocity.Y * 0.2f, 100, default, 2f);
                Main.dust[num469].noGravity = true;
            }
		}
		
		public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
		{
			// Inflate some target hitboxes if they are beyond 8,8 size
			if (targetHitbox.Width > 8 && targetHitbox.Height > 8)
			{
				targetHitbox.Inflate(-targetHitbox.Width / 8, -targetHitbox.Height / 8);
			}
			// Return if the hitboxes intersects, which means the javelin collides or not
			return projHitbox.Intersects(targetHitbox);
		}
		
		public override void AI()
		{
			projectile.rotation =
			projectile.velocity.ToRotation() +
			MathHelper.ToRadians(90f);
		}

        public override void Kill(int timeLeft)
        {
            for (int num468 = 0; num468 < 5; num468++)
            {
                int num469 = Dust.NewDust(new Vector2(projectile.Center.X, projectile.Center.Y), projectile.width, projectile.height, DustID.SolarFlare, -projectile.velocity.X * 0.2f,
                    -projectile.velocity.Y * 0.2f, 100, default, 2f);
                Main.dust[num469].noGravity = true;
            }
        }

        public override bool PreDraw(SpriteBatch sb, Color lightColor)
        {
            int r = GameShaders.Armor.GetShaderIdFromItemId(ItemID.LivingFlameDye);
            BaseDrawing.DrawAfterimage(sb, Main.projectileTexture[projectile.type], r, projectile, .7f, 1, 5, false, 0, 0);
            BaseDrawing.DrawTexture(sb, Main.projectileTexture[projectile.type], r, projectile, Color.White, true);
            return false;
        }
	}
}