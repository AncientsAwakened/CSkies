using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace CSkies.NPCs.Bosses.ObserverVoid
{
    public class VoidLaser : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Void Beam");
		}

        public override void SetDefaults()
        {
            projectile.width = 18;
            projectile.height = 18;
            projectile.hostile = true;
            projectile.friendly = false;
            projectile.tileCollide = false;
            projectile.usesLocalNPCImmunity = true;
            cooldownSlot = 1;
            projectile.penetrate = -1;
            projectile.alpha = 255;
        }

        public override void AI()
        {
            Lighting.AddLight(projectile.Center, 0, 0f, .15f);
            Vector2? vector69 = null;
            if (projectile.velocity.HasNaNs() || projectile.velocity == Vector2.Zero)
            {
                projectile.velocity = -Vector2.UnitY;
            }
            if (Main.npc[(int)projectile.ai[1]].active && Main.npc[(int)projectile.ai[1]].type == ModContent.NPCType<ObserverVoid>())
            {
                Vector2 offset = new Vector2(30f, 30f);
                Vector2 offsetElipse = Utils.Vector2FromElipse(Main.npc[(int)projectile.ai[1]].localAI[0].ToRotationVector2(), offset * Main.npc[(int)projectile.ai[1]].localAI[1]);
                projectile.position = Main.npc[(int)projectile.ai[1]].Center + offsetElipse - new Vector2(projectile.width, projectile.height) / 2f;
            }
            if (projectile.velocity.HasNaNs() || projectile.velocity == Vector2.Zero)
            {
                projectile.velocity = -Vector2.UnitY;
            }
            if (projectile.localAI[0] == 0f)
            {
                Main.PlaySound(SoundID.Zombie, (int)projectile.position.X, (int)projectile.position.Y, 104, 1f, 0f);
            }
            if (projectile.localAI[0]++ >= 180f)
            {
                projectile.Kill();
                return;
            }
            projectile.scale = (float)Math.Sin(projectile.localAI[0] * 3.14159274f / 180f) * 10f * 0.4f;
            if (projectile.scale > 0.4f)
            {
                projectile.scale = 0.4f;
            }
            float velocity = projectile.velocity.ToRotation();
            velocity += projectile.ai[0];
            projectile.rotation = velocity - 1.57079637f;
            projectile.velocity = velocity.ToRotationVector2();
            Vector2 samplingPoint = projectile.Center;
            if (vector69.HasValue)
            {
                samplingPoint = vector69.Value;
            }
            float num799 = 3f;
            float width = projectile.width;
            float[] array3 = new float[(int)num799];
            Collision.LaserScan(samplingPoint, projectile.velocity, width * projectile.scale, 2400f, array3);
            float num801 = 0f;
            for (int num802 = 0; num802 < array3.Length; num802++)
            {
                num801 += array3[num802];
            }
            num801 /= num799;
            float amount = 0.5f;
            projectile.localAI[1] = MathHelper.Lerp(projectile.localAI[1], num801, amount);
            Vector2 vector70 = projectile.Center + projectile.velocity * (projectile.localAI[1] - 14f);
            for (int num803 = 0; num803 < 2; num803++)
            {
                float laserVelocity = projectile.velocity.ToRotation() + ((Main.rand.Next(2) == 1) ? -1f : 1f) * 1.57079637f;
                float rand = (float)Main.rand.NextDouble() * 2f + 2f;
                Vector2 dustVelocity = new Vector2((float)Math.Cos(laserVelocity) * rand, (float)Math.Sin(laserVelocity) * rand);
                int dust = Dust.NewDust(vector70, 0, 0, 229, dustVelocity.X, dustVelocity.Y, 0, default, 1f);
                Main.dust[dust].noGravity = true;
                Main.dust[dust].scale = 1.7f;
            }
            if (Main.rand.Next(5) == 0)
            {
                Vector2 laserVelocity = projectile.velocity.RotatedBy(1.5707963705062866, default) * ((float)Main.rand.NextDouble() - 0.5f) * projectile.width;
                int dust = Dust.NewDust(vector70 + laserVelocity - Vector2.One * 4f, 8, 8, 31, 0f, 0f, 100, default, 1.5f);
                Main.dust[dust].velocity *= 0.5f;
                Main.dust[dust].velocity.Y = -Math.Abs(Main.dust[dust].velocity.Y);
            }
            DelegateMethods.v3_1 = new Vector3(0.3f, 0.65f, 0.7f);
            Utils.PlotTileLine(projectile.Center, projectile.Center + projectile.velocity * projectile.localAI[1], projectile.width * projectile.scale, new Utils.PerLinePoint(DelegateMethods.CastLight));
        }

        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
            float n = 0f;
            if (Collision.CheckAABBvLineCollision(targetHitbox.TopLeft(), targetHitbox.Size(), projectile.Center, projectile.Center + projectile.velocity * projectile.localAI[1], 30f * projectile.scale, ref n))
                return true;

            return false;
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            if (projectile.velocity == Vector2.Zero)
                return false;

            Texture2D tex2 = Main.projectileTexture[projectile.type];
            float num210 = projectile.localAI[1];
            Color c_ = new Color(255, 255, 255, 127);
            Vector2 value20 = projectile.Center.Floor();
            num210 -= projectile.scale * 10.5f;
            Vector2 vector41 = new Vector2(projectile.scale);
            DelegateMethods.f_1 = 1f;
            DelegateMethods.c_1 = c_;
            DelegateMethods.i_1 = 54000 - (int)Main.time / 2;
            Vector2 vector42 = projectile.oldPos[0] + new Vector2(projectile.width, projectile.height) / 2f + Vector2.UnitY * projectile.gfxOffY - Main.screenPosition;
            Utils.DrawLaser(Main.spriteBatch, tex2, value20 - Main.screenPosition, value20 + projectile.velocity * num210 - Main.screenPosition, vector41, new Utils.LaserLineFraming(DelegateMethods.TurretLaserDraw));
            DelegateMethods.c_1 = new Color(255, 255, 255, 127) * 0.75f * projectile.Opacity;
            Utils.DrawLaser(Main.spriteBatch, tex2, value20 - Main.screenPosition, value20 + projectile.velocity * num210 - Main.screenPosition, vector41 / 2f, new Utils.LaserLineFraming(DelegateMethods.TurretLaserDraw));
            return false;
        }

    }
}
