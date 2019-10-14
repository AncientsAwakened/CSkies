using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Microsoft.Xna.Framework.Graphics;
using Terraria.Graphics.Shaders;

namespace CSkies.NPCs.Bosses.Heartcore
{
    public class Fireball : ModProjectile
	{
		public override void SetDefaults()
		{
			projectile.width = 40;
			projectile.height = 40;
			projectile.aiStyle = -1;
			projectile.hostile = true;
			projectile.penetrate = 1;
			projectile.aiStyle = -1;
			projectile.timeLeft = 1200;
            Main.projFrames[projectile.type] = 4;
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
                if (projectile.frame++ > 2)
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
                if (num293 != projectile.whoAmI && Main.projectile[num293].active && Main.projectile[num293].owner == projectile.owner && Main.projectile[num293].type == ModContent.ProjectileType<Fire>() && Main.projectile[num293].getRect().Intersects(value19))
                {
                    Main.projectile[num293].ai[1] = 1f;
                    Main.projectile[num293].velocity = (projectile.Center - Main.projectile[num293].Center) / 5f;
                    Main.projectile[num293].netUpdate = true;
                }
            }
            Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, 0f, 0f, ModContent.ProjectileType<Fire>(), projectile.damage, 0f, projectile.owner, 0f, 0f);
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            projectile.velocity *= .90f;
            return false;
        }

        public float auraPercent = 0f;
        public bool auraDirection = true;

        public override bool PreDraw(SpriteBatch sb, Color lightColor)
        {
            int r = GameShaders.Armor.GetShaderIdFromItemId(ItemID.LivingFlameDye);
            if (auraDirection) { auraPercent += 0.1f; auraDirection = auraPercent < 1f; }
            else { auraPercent -= 0.1f; auraDirection = auraPercent <= 0f; }

            Rectangle frame = BaseDrawing.GetFrame(projectile.frame, Main.projectileTexture[projectile.type].Width, Main.projectileTexture[projectile.type].Height / Main.projFrames[projectile.type], 0, 0);

            BaseDrawing.DrawAura(sb, Main.projectileTexture[projectile.type], r, projectile.position, projectile.width, projectile.height, auraPercent, 1.5f, 1f, projectile.rotation, projectile.direction, 4, frame, 0f, 0f, null);
            BaseDrawing.DrawTexture(sb, Main.projectileTexture[projectile.type], r, projectile.position, projectile.width, projectile.height, projectile.scale, projectile.rotation, 0, 4, frame, projectile.GetAlpha(Colors.COLOR_GLOWPULSE), true);
            BaseDrawing.DrawAura(sb, mod.GetTexture("Glowmasks/Fireball_Heart"), 0, projectile.position, projectile.width, projectile.height, auraPercent, 1.5f, 1f, projectile.rotation, projectile.direction, 4, frame, 0f, 0f, projectile.GetAlpha(Color.White));
            return false;
        }
    }
}