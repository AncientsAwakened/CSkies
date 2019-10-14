using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using Terraria.Audio;
using System;
using ReLogic.Utilities;
using Microsoft.Xna.Framework.Graphics;
using Terraria.Graphics.Shaders;
using Terraria.ID;

namespace CSkies.NPCs.Bosses.Heartcore
{
    public class Fire : ModProjectile
	{
		public override void SetDefaults()
        {
            projectile.width = 80;
            projectile.height = 80;
            projectile.aiStyle = -1;
            projectile.hostile = true;
            projectile.alpha = 255;
            projectile.scale = 1f;
            projectile.ignoreWater = true;
            projectile.tileCollide = false;
            projectile.penetrate = -1;
            Main.projFrames[projectile.type] = 4;
            projectile.timeLeft = 400;
        }
		
		public override Color? GetAlpha(Color lightColor)
		{
			return Color.White;
		}
		
		public override void AI()
        {
            Lighting.AddLight(projectile.Center, 0f, .7f, 0f);
            Vector2 vector62 = -Vector2.UnitY.RotatedBy(6.28318548f * projectile.ai[0] / 30f, default);
            float val = 0.75f + vector62.Y * 0.25f;
            float val2 = 0.8f - vector62.Y * 0.2f;
            float num728 = Math.Max(val, val2);
            projectile.frameCounter++;
            if (projectile.frameCounter++ >= 7)
            {
                projectile.frameCounter = 0;
                projectile.frame++;
                if (projectile.frame++ >= 3)
                {
                    projectile.frame = 0;
                }
            }
            for (int num729 = 0; num729 < 1; num729++)
            {
                float num730 = 55f * num728;
                float num731 = 11f * num728;
                float num732 = 0.5f;
                int num733 = Dust.NewDust(projectile.position, projectile.width, projectile.height, DustID.SolarFlare, 0f, 0f, 100, default, 0.5f);
                Main.dust[num733].noGravity = true;
                Main.dust[num733].velocity *= 2f;
                Main.dust[num733].position = ((float)Main.rand.NextDouble() * 6.28318548f).ToRotationVector2() * (num731 + num732 * (float)Main.rand.NextDouble() * num730) + projectile.Center;
                Main.dust[num733].velocity = Main.dust[num733].velocity / 2f + Vector2.Normalize(Main.dust[num733].position - projectile.Center);
                if (Main.rand.Next(2) == 0)
                {
                    num733 = Dust.NewDust(projectile.position, projectile.width, projectile.height, DustID.SolarFlare, 0f, 0f, 100, default, 0.9f);
                    Main.dust[num733].noGravity = true;
                    Main.dust[num733].velocity *= 1.2f;
                    Main.dust[num733].position = ((float)Main.rand.NextDouble() * 6.28318548f).ToRotationVector2() * (num731 + num732 * (float)Main.rand.NextDouble() * num730) + projectile.Center;
                    Main.dust[num733].velocity = Main.dust[num733].velocity / 2f + Vector2.Normalize(Main.dust[num733].position - projectile.Center);
                }
                if (Main.rand.Next(4) == 0)
                {
                    num733 = Dust.NewDust(projectile.position, projectile.width, projectile.height, DustID.SolarFlare, 0f, 0f, 100, default, 0.7f);
                    Main.dust[num733].noGravity = true;
                    Main.dust[num733].velocity *= 1.2f;
                    Main.dust[num733].position = ((float)Main.rand.NextDouble() * 6.28318548f).ToRotationVector2() * (num731 + num732 * (float)Main.rand.NextDouble() * num730) + projectile.Center;
                    Main.dust[num733].velocity = Main.dust[num733].velocity / 2f + Vector2.Normalize(Main.dust[num733].position - projectile.Center);
                }
            }

            int p = BaseAI.GetPlayer(projectile.Center, -1);
            Player player = Main.player[p];

            BaseAI.ShootPeriodic(projectile, player.position, player.width, player.height, ModContent.ProjectileType<Fireshot>(), ref projectile.ai[0], Main.rand.Next(60, 90), 30, 10, true);
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
            BaseDrawing.DrawAura(sb, mod.GetTexture("Glowmasks/Fire_Heart"), 0, projectile.position, projectile.width, projectile.height, auraPercent, 1.5f, 1f, projectile.rotation, projectile.direction, 4, frame, 0f, 0f, projectile.GetAlpha(Color.White));
            return false;
        }
    }
}