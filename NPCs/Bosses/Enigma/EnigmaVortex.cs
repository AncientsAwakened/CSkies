using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CSkies.NPCs.Bosses.Enigma
{
    public class EnigmaVortex : ModProjectile
	{
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Enigma Portal");
        }

		public override void SetDefaults()
		{
            projectile.width = 32;
            projectile.height = 32;
            projectile.aiStyle = -1;
            projectile.ignoreWater = true;
            projectile.tileCollide = false;
            projectile.timeLeft = 180;
        }

        public float Rotation = 0;
        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            Rectangle SunFrame = new Rectangle(0, 0, 60, 60);
            BaseDrawing.DrawTexture(spriteBatch, Main.extraTexture[50], 0, projectile.position + new Vector2(0, projectile.gfxOffY), projectile.width, projectile.height, projectile.scale, -projectile.rotation, projectile.spriteDirection, 1, SunFrame, Colors.COLOR_GLOWPULSE, true);
            BaseDrawing.DrawTexture(spriteBatch, mod.GetTexture("NPCs/Bosses/Enigma/EnigmaVortex"), 0, projectile.position + new Vector2(0, projectile.gfxOffY), projectile.width, projectile.height, projectile.scale, projectile.rotation, projectile.spriteDirection, 1, SunFrame, Colors.COLOR_GLOWPULSE, true);
            return false;
        }

        public override void AI()
        {
            Rotation += .0008f;
            projectile.rotation += .0008f;
            projectile.velocity = Vector2.Zero;
            if (projectile.direction == 0)
            {
                    projectile.direction = Main.player[projectile.owner].direction;
            }
            projectile.rotation -= projectile.direction * 6.28318548f / 120f;
            projectile.scale = projectile.Opacity;
            Lighting.AddLight(projectile.Center, new Vector3(0.3f, 0.9f, 0.7f) * projectile.Opacity);
            if (Main.rand.Next(2) == 0)
            {
                Vector2 vector135 = Vector2.UnitY.RotatedByRandom(6.2831854820251465);
                Dust dust31 = Main.dust[Dust.NewDust(projectile.Center - vector135 * 30f, 0, 0, DustID.Electric, 0f, 0f, 0, default, 1f)];
                dust31.noGravity = true;
                dust31.position = projectile.Center - vector135 * Main.rand.Next(10, 21);
                dust31.velocity = vector135.RotatedBy(1.5707963705062866, default) * 6f;
                dust31.scale = 0.5f + Main.rand.NextFloat();
                dust31.fadeIn = 0.5f;
                dust31.customData = projectile.Center;
            }
            if (Main.rand.Next(2) == 0)
            {
                Vector2 vector136 = Vector2.UnitY.RotatedByRandom(6.2831854820251465);
                Dust dust32 = Main.dust[Dust.NewDust(projectile.Center - vector136 * 30f, 0, 0, DustID.Electric, 0f, 0f, 0, default, 1f)];
                dust32.noGravity = true;
                dust32.position = projectile.Center - vector136 * 30f;
                dust32.velocity = vector136.RotatedBy(-1.5707963705062866, default) * 3f;
                dust32.scale = 0.5f + Main.rand.NextFloat();
                dust32.fadeIn = 0.5f;
                dust32.customData = projectile.Center;
            }
            if (projectile.ai[0] < 0f)
            {
                Vector2 center15 = projectile.Center;
                int num1059 = Dust.NewDust(center15 - Vector2.One * 8f, 16, 16, DustID.Electric, projectile.velocity.X / 2f, projectile.velocity.Y / 2f, 0);
                Main.dust[num1059].velocity *= 2f;
                Main.dust[num1059].noGravity = true;
                Main.dust[num1059].scale = Utils.SelectRandom(Main.rand, new float[]
                {
                    0.8f,
                    1.65f
                });
                Main.dust[num1059].customData = this;
            }
            if (projectile.ai[0] < 0f)
            {
                projectile.ai[0] += 1f;
                
                    projectile.ai[1] -= projectile.direction * 0.3926991f / 50f;
                
            }
            projectile.ai[0] += 1f;
            if (projectile.ai[0] % 20f == 0f && Main.netMode != NetmodeID.MultiplayerClient)
            {
                Main.PlaySound(mod.GetLegacySoundSlot(SoundType.Custom, "Sounds/Sounds/Zap2"), projectile.position);
                int[] array4 = new int[5];
                Vector2[] array5 = new Vector2[5];
                int num838 = 0;
                float num839 = 2000f;
                for (int num840 = 0; num840 < 255; num840++)
                {
                    if (Main.player[num840].active && !Main.player[num840].dead)
                    {
                        Vector2 center9 = Main.player[num840].Center;
                        float num841 = Vector2.Distance(center9, projectile.Center);
                        if (num841 < num839 && Collision.CanHit(projectile.Center, 1, 1, center9, 1, 1))
                        {
                            array4[num838] = num840;
                            array5[num838] = center9;
                            if (++num838 >= array5.Length)
                            {
                                break;
                            }
                        }
                    }
                }
                for (int num842 = 0; num842 < num838; num842++)
                {
                    Vector2 vector82 = array5[num842] - projectile.Center;
                    float ai = Main.rand.Next(100);
                    Vector2 vector83 = Vector2.Normalize(vector82.RotatedByRandom(0.78539818525314331)) * 16f;
                    Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, vector83.X, vector83.Y, ModContent.ProjectileType<Enigmashock>(), projectile.damage / 4, 0f, Main.myPlayer, vector82.ToRotation(), ai);
                }
            }
        }
    }
}
