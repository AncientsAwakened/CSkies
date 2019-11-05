using System;
using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CSkies.NPCs.Bosses.Enigma
{
    public class EnigmaBeam : ModProjectile
    {
        private const float MoveDistance = 70f;

        public float Distance;


        public NPC shooter;
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Nova Ray");
        }
        public override void SetDefaults()
        {
            projectile.width = 10;
            projectile.height = 10;
            projectile.penetrate = -1;
            projectile.tileCollide = false;
            projectile.hide = false;
            projectile.friendly = false;
            projectile.hostile = true;
        }
        // The AI of the projectile
        public bool runOnce = true;
        public override void AI()
        {
            Vector2? vector78 = null;
            shooter = Main.npc[(int)projectile.ai[0]];
            Player player = Main.player[projectile.owner];
            if (!shooter.active || shooter.life <= 0)
            {
                projectile.Kill();
            }
            if (shooter.ai[0] == 3 || shooter.ai[0] == 4)
            {
            }
            else
            {
                projectile.Kill();
            }

            if (projectile.velocity.HasNaNs() || projectile.velocity == Vector2.Zero)
            {
                projectile.velocity = -Vector2.UnitY;
            }
            if (projectile.localAI[0] == 0f)
            {
                Main.PlaySound(29, (int)Main.player[Main.myPlayer].Center.X, (int)Main.player[Main.myPlayer].Center.Y, 104, 1f, 0f);
            }
            float num801 = 10f;
            projectile.localAI[0] += 1f;
            if (projectile.localAI[0] >= ((Enigma)shooter.modNPC).ChangeRate)
            {
                projectile.Kill();
                return;
            }
            projectile.scale = (float)Math.Sin(projectile.localAI[0] * 3.14159274f / ChangeRate) * 5f * num801;
            if (projectile.scale > num801)
            {
                projectile.scale = num801;
            }
            //float num804 = projectile.velocity.ToRotation();
            //num804 += projectile.ai[0];
            //projectile.rotation = num804 - 1.57079637f;
            float num804 = Main.npc[(int)projectile.ai[1]].ai[3] - 1.57079637f;
            //if (projectile.ai[0] != 0f) num804 -= (float)Math.PI;
            projectile.rotation = num804;
            num804 += 1.57079637f;
            projectile.velocity = num804.ToRotationVector2();
            float num805 = 3f;
            float num806 = (float)projectile.width;
            Vector2 samplingPoint = projectile.Center;
            if (vector78.HasValue)
            {
                samplingPoint = vector78.Value;
            }
            float[] array3 = new float[(int)num805];
            //Collision.LaserScan(samplingPoint, projectile.velocity, num806 * projectile.scale, 3000f, array3);
            for (int i = 0; i < array3.Length; i++)
                array3[i] = 3000f;
            float num807 = 0f;
            int num3;
            for (int num808 = 0; num808 < array3.Length; num808 = num3 + 1)
            {
                num807 += array3[num808];
                num3 = num808;
            }
            num807 /= num805;
            float amount = 0.5f;
            projectile.localAI[1] = MathHelper.Lerp(projectile.localAI[1], num807, amount);
            Vector2 vector79 = projectile.Center + projectile.velocity * (projectile.localAI[1] - 14f);
            for (int num809 = 0; num809 < 2; num809 = num3 + 1)
            {
                float num810 = projectile.velocity.ToRotation() + ((Main.rand.Next(2) == 1) ? -1f : 1f) * 1.57079637f;
                float num811 = (float)Main.rand.NextDouble() * 2f + 2f;
                Vector2 vector80 = new Vector2((float)Math.Cos((double)num810) * num811, (float)Math.Sin((double)num810) * num811);
                int num812 = Dust.NewDust(vector79, 0, 0, 244, vector80.X, vector80.Y, 0, default(Color), 1f);
                Main.dust[num812].noGravity = true;
                Main.dust[num812].scale = 1.7f;
                num3 = num809;
            }
            if (Main.rand.Next(5) == 0)
            {
                Vector2 value29 = projectile.velocity.RotatedBy(1.5707963705062866, default(Vector2)) * ((float)Main.rand.NextDouble() - 0.5f) * (float)projectile.width;
                int num813 = Dust.NewDust(vector79 + value29 - Vector2.One * 4f, 8, 8, 244, 0f, 0f, 100, default(Color), 1.5f);
                Dust dust = Main.dust[num813];
                dust.velocity *= 0.5f;
                Main.dust[num813].velocity.Y = -Math.Abs(Main.dust[num813].velocity.Y);
            }
        }
        public int colorCounter;
        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            DrawLaser(spriteBatch, Main.projectileTexture[projectile.type], new Vector2(shooter.Center.X, shooter.Center.Y),
                projectile.velocity, 10, -1.57f, 1f, (int)MoveDistance);

            return false;
        }

        // The core function of drawing a laser
        public void DrawLaser(SpriteBatch spriteBatch, Texture2D texture, Vector2 start, Vector2 unit, float step, float rotation = 0f, float scale = 1f, int transDist = 50)
        {
            float r = unit.ToRotation() + rotation;

            #region Draw laser body
            for (float i = transDist; i <= Distance; i += step)
            {
                Color c = Color.White;
                Vector2 origin = start + i * unit;
                spriteBatch.Draw(texture, origin - Main.screenPosition,
                    new Rectangle(0, 26, 28, 26), i < transDist ? Color.Transparent : c, r,
                    new Vector2(28 * .5f, 26 * .5f), scale, 0, 0);
            }
            #endregion

            #region Draw laser tail
            spriteBatch.Draw(texture, start + unit * (transDist - step) - Main.screenPosition,
                new Rectangle(0, 0, 28, 26), Color.White, r, new Vector2(28 * .5f, 26 * .5f), scale, 0, 0);
            #endregion

            #region Draw laser head
            spriteBatch.Draw(texture, start + (Distance + step) * unit - Main.screenPosition,
                new Rectangle(0, 52, 28, 26), Color.White, r, new Vector2(28 * .5f, 26 * .5f), scale, 0, 0);
            #endregion
        }

        private void SpawnDusts(NPC N)
        {
            Vector2 unit = projectile.velocity * -1;
            Vector2 dustPos = N.Center + projectile.velocity * Distance;

            for (int i = 0; i < 2; ++i)
            {
                float num1 = projectile.velocity.ToRotation() + (Main.rand.Next(2) == 1 ? -1.0f : 1.0f) * 1.57f;
                float num2 = (float)(Main.rand.NextDouble() * 0.8f + 1.0f);
                Vector2 dustVel = new Vector2((float)Math.Cos(num1) * num2, (float)Math.Sin(num1) * num2);
                Dust dust = Main.dust[Dust.NewDust(dustPos, 0, 0, 226, dustVel.X, dustVel.Y)];
                dust.noGravity = true;
                dust.scale = 1.2f;
                dust = Dust.NewDustDirect(Main.player[projectile.owner].Center, 0, 0, 31,
                    -unit.X * Distance, -unit.Y * Distance);
                dust.fadeIn = 0f;
                dust.noGravity = true;
                dust.scale = 0.88f;
                dust.color = Color.Cyan;
            }

            if (Main.rand.NextBool(5))
            {
                Vector2 offset = projectile.velocity.RotatedBy(1.57f) * ((float)Main.rand.NextDouble() - 0.5f) * projectile.width;
                Dust dust = Main.dust[Dust.NewDust(dustPos + offset - Vector2.One * 4f, 8, 8, 31, 0.0f, 0.0f, 100, new Color(), 1.5f)];
                dust.velocity *= 0.5f;
                dust.velocity.Y = -Math.Abs(dust.velocity.Y);
                unit = dustPos - Main.player[projectile.owner].Center;
                unit.Normalize();
                dust = Main.dust[Dust.NewDust(Main.player[projectile.owner].Center + 55 * unit, 8, 8, 31, 0.0f, 0.0f, 100, new Color(), 1.5f)];
                dust.velocity *= 0.5f;
                dust.velocity.Y = -Math.Abs(dust.velocity.Y);
            }
        }


        // Change the way of collision check of the projectile
        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
            Vector2 unit = projectile.velocity;
            float point = 0f;
            // Run an AABB versus Line check to look for collisions, look up AABB collision first to see how it works
            // It will look for collisions on the given line using AABB
            return Collision.CheckAABBvLineCollision(targetHitbox.TopLeft(), targetHitbox.Size(), new Vector2(shooter.Center.X, shooter.Center.Y),
                new Vector2(shooter.Center.X, shooter.Center.Y) + unit * Distance, 22, ref point);
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            target.immune[projectile.owner] = 5;
        }

        public override bool ShouldUpdatePosition()
        {
            return false;
        }
    }
}
