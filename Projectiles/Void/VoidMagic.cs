using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CSkies.Projectiles.Void
{
    public class VoidMagic : ModProjectile
    {
        public override string Texture => "CSkies/Projectiles/Void/Singularity";
        public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Vortex");
		}
        public override void SetDefaults()
        {
            projectile.width = 32;
            projectile.height = 32;
            projectile.aiStyle = -1;
            projectile.friendly = true;
            projectile.alpha = 255;
            projectile.ignoreWater = true;
            projectile.magic = true;
            projectile.penetrate = 3;
            projectile.usesLocalNPCImmunity = true;
            projectile.localNPCHitCooldown = 8;
        }
		
		public override void AI()
        {
            projectile.ai[0] += 1f;
            int num1002 = 0;
            if (projectile.velocity.Length() <= 4f)
            {
                num1002 = 1;
            }
            projectile.alpha -= 15;
            if (projectile.alpha < 0)
            {
                projectile.alpha = 0;
            }
            if (num1002 == 0)
            {
                projectile.rotation -= 0.104719758f;
                if (Main.rand.Next(3) == 0)
                {
                    if (Main.rand.Next(2) == 0)
                    {
                        Vector2 vector124 = Vector2.UnitY.RotatedByRandom(6.2831854820251465);
                        Dust dust27 = Main.dust[Dust.NewDust(projectile.Center - vector124 * 30f, 0, 0, ModContent.DustType<Dusts.VoidDust>(), 0f, 0f, 0, default, 1f)];
                        dust27.noGravity = true;
                        dust27.position = projectile.Center - vector124 * Main.rand.Next(10, 21);
                        dust27.velocity = vector124.RotatedBy(1.5707963705062866, default) * 6f;
                        dust27.scale = 0.5f + Main.rand.NextFloat();
                        dust27.fadeIn = 0.5f;
                        dust27.customData = this;
                    }
                    else
                    {
                        Vector2 vector125 = Vector2.UnitY.RotatedByRandom(6.2831854820251465);
                        Dust dust28 = Main.dust[Dust.NewDust(projectile.Center - vector125 * 30f, 0, 0, ModContent.DustType<Dusts.VoidDust>(), 0f, 0f, 0, default, 1f)];
                        dust28.noGravity = true;
                        dust28.position = projectile.Center - vector125 * 30f;
                        dust28.velocity = vector125.RotatedBy(-1.5707963705062866, default) * 3f;
                        dust28.scale = 0.5f + Main.rand.NextFloat();
                        dust28.fadeIn = 0.5f;
                        dust28.customData = this;
                    }
                }
                if (projectile.ai[0] >= 30f)
                {
                    projectile.velocity *= 0.98f;
                    projectile.scale += 0.00744680827f;
                    if (projectile.scale > 1.3f)
                    {
                        projectile.scale = 1.3f;
                    }
                    projectile.rotation -= 0.0174532924f;
                }
                if (projectile.velocity.Length() < 4.1f)
                {
                    projectile.velocity.Normalize();
                    projectile.velocity *= 4f;
                    projectile.ai[0] = 0f;
                }
            }
            else if (num1002 == 1)
            {
                projectile.rotation -= 0.104719758f;
                for (int num1003 = 0; num1003 < 1; num1003++)
                {
                    if (Main.rand.Next(2) == 0)
                    {
                        Vector2 vector126 = Vector2.UnitY.RotatedByRandom(6.2831854820251465);
                        Dust dust29 = Main.dust[Dust.NewDust(projectile.Center - vector126 * 30f, 0, 0, ModContent.DustType<Dusts.VoidDust>(), 0f, 0f, 0, default, 1f)];
                        dust29.noGravity = true;
                        dust29.position = projectile.Center - vector126 * Main.rand.Next(10, 21);
                        dust29.velocity = vector126.RotatedBy(1.5707963705062866, default) * 6f;
                        dust29.scale = 0.9f + Main.rand.NextFloat();
                        dust29.fadeIn = 0.5f;
                        dust29.customData = this;
                        vector126 = Vector2.UnitY.RotatedByRandom(6.2831854820251465);
                        dust29 = Main.dust[Dust.NewDust(projectile.Center - vector126 * 30f, 0, 0, ModContent.DustType<Dusts.VoidDust>(), 0f, 0f, 0, default, 1f)];
                        dust29.noGravity = true;
                        dust29.position = projectile.Center - vector126 * Main.rand.Next(10, 21);
                        dust29.velocity = vector126.RotatedBy(1.5707963705062866, default) * 6f;
                        dust29.scale = 0.9f + Main.rand.NextFloat();
                        dust29.fadeIn = 0.5f;
                        dust29.customData = this;
                    }
                    else
                    {
                        Vector2 vector127 = Vector2.UnitY.RotatedByRandom(6.2831854820251465);
                        Dust dust30 = Main.dust[Dust.NewDust(projectile.Center - vector127 * 30f, 0, 0, ModContent.DustType<Dusts.VoidDust>(), 0f, 0f, 0, default, 1f)];
                        dust30.noGravity = true;
                        dust30.position = projectile.Center - vector127 * Main.rand.Next(20, 31);
                        dust30.velocity = vector127.RotatedBy(-1.5707963705062866, default) * 5f;
                        dust30.scale = 0.9f + Main.rand.NextFloat();
                        dust30.fadeIn = 0.5f;
                        dust30.customData = this;
                    }
                }
                if (projectile.ai[0] % 30f == 0f && projectile.ai[0] < 241f && Main.myPlayer == projectile.owner)
                {
                    Vector2 vector128 = Vector2.UnitY.RotatedByRandom(6.2831854820251465) * 12f;
                    Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, vector128.X, vector128.Y, 618, projectile.damage / 2, 0f, projectile.owner, 0f, projectile.whoAmI);
                }
                Vector2 vector129 = projectile.Center;
                float num1004 = 800f;
                bool flag58 = false;
                int num1005 = 0;
                if (projectile.ai[1] == 0f)
                {
                    for (int num1006 = 0; num1006 < 200; num1006++)
                    {
                        if (Main.npc[num1006].CanBeChasedBy(this, false))
                        {
                            Vector2 center13 = Main.npc[num1006].Center;
                            if (projectile.Distance(center13) < num1004 && Collision.CanHit(new Vector2(projectile.position.X + projectile.width / 2, projectile.position.Y + projectile.height / 2), 1, 1, Main.npc[num1006].position, Main.npc[num1006].width, Main.npc[num1006].height))
                            {
                                num1004 = projectile.Distance(center13);
                                vector129 = center13;
                                flag58 = true;
                                num1005 = num1006;
                            }
                        }
                    }
                    if (flag58)
                    {
                        if (projectile.ai[1] != num1005 + 1)
                        {
                            projectile.netUpdate = true;
                        }
                        projectile.ai[1] = num1005 + 1;
                    }
                    flag58 = false;
                }
                if (projectile.ai[1] != 0f)
                {
                    int num1007 = (int)(projectile.ai[1] - 1f);
                    if (Main.npc[num1007].active && Main.npc[num1007].CanBeChasedBy(this, true) && projectile.Distance(Main.npc[num1007].Center) < 1000f)
                    {
                        flag58 = true;
                        vector129 = Main.npc[num1007].Center;
                    }
                }
                if (!projectile.friendly)
                {
                    flag58 = false;
                }
                if (flag58)
                {
                    float num1008 = 4f;
                    int num1009 = 8;
                    Vector2 vector130 = new Vector2(projectile.position.X + projectile.width * 0.5f, projectile.position.Y + projectile.height * 0.5f);
                    float num1010 = vector129.X - vector130.X;
                    float num1011 = vector129.Y - vector130.Y;
                    float num1012 = (float)Math.Sqrt(num1010 * num1010 + num1011 * num1011);
                    num1012 = num1008 / num1012;
                    num1010 *= num1012;
                    num1011 *= num1012;
                    projectile.velocity.X = (projectile.velocity.X * (num1009 - 1) + num1010) / num1009;
                    projectile.velocity.Y = (projectile.velocity.Y * (num1009 - 1) + num1011) / num1009;
                }
            }
            if (projectile.alpha < 150)
            {
                Lighting.AddLight(projectile.Center, 0.1f, 0.1f, 0.2f);
            }
            if (projectile.ai[0] >= 600f)
            {
                projectile.Kill();
                return;
            }

            for (int u = 0; u < Main.maxNPCs; u++)
            {
                NPC target = Main.npc[u];

                if (target.type != NPCID.TargetDummy && target.active && !target.boss && target.chaseable && Vector2.Distance(projectile.Center, target.Center) < 160)
                {
                    float num3 = 6f;
                    Vector2 vector = new Vector2(target.position.X + target.width / 2, target.position.Y + target.height / 2);
                    float num4 = projectile.Center.X - vector.X;
                    float num5 = projectile.Center.Y - vector.Y;
                    float num6 = (float)Math.Sqrt(num4 * num4 + num5 * num5);
                    num6 = num3 / num6;
                    num4 *= num6;
                    num5 *= num6;
                    int num7 = 6;
                    target.velocity.X = (target.velocity.X * (num7 - 1) + num4) / num7;
                    target.velocity.Y = (target.velocity.Y * (num7 - 1) + num5) / num7;
                }
            }
        }

        public override void OnHitNPC (NPC target, int damage, float knockback, bool crit)
		{
            projectile.ai[0] = 1f;
        }
		
		public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough)
        {
            width = 16;
            height = 16;
            return true;
        }

        public override void Kill(int timeLeft)
        {
            projectile.position = projectile.Center;
            projectile.width = (projectile.height = 176);
            projectile.Center = projectile.position;
            projectile.maxPenetrate = -1;
            projectile.penetrate = -1;
            projectile.Damage();
            Main.PlaySound(SoundID.Item14, projectile.position);
            for (int num93 = 0; num93 < 4; num93++)
            {
                int num94 = Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), projectile.width, projectile.height, ModContent.DustType<Dusts.VoidDust>(), 0f, 0f, 100, default, 1.5f);
                Main.dust[num94].position = projectile.Center + Vector2.UnitY.RotatedByRandom(3.1415927410125732) * (float)Main.rand.NextDouble() * projectile.width / 2f;
            }
            for (int num95 = 0; num95 < 30; num95++)
            {
                int num96 = Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), projectile.width, projectile.height, ModContent.DustType<Dusts.VoidDust>(), 0f, 0f, 200, default, 3.7f);
                Main.dust[num96].position = projectile.Center + Vector2.UnitY.RotatedByRandom(3.1415927410125732) * (float)Main.rand.NextDouble() * projectile.width / 2f;
                Main.dust[num96].noGravity = true;
                Main.dust[num96].velocity *= 3f;
                num96 = Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), projectile.width, projectile.height, ModContent.DustType<Dusts.VoidDust>(), 0f, 0f, 100, default, 1.5f);
                Main.dust[num96].position = projectile.Center + Vector2.UnitY.RotatedByRandom(3.1415927410125732) * (float)Main.rand.NextDouble() * projectile.width / 2f;
                Main.dust[num96].velocity *= 2f;
                Main.dust[num96].noGravity = true;
                Main.dust[num96].fadeIn = 1f;
            }
            for (int num97 = 0; num97 < 10; num97++)
            {
                int num98 = Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), projectile.width, projectile.height, ModContent.DustType<Dusts.VoidDust>(), 0f, 0f, 0, default, 2.7f);
                Main.dust[num98].position = projectile.Center + Vector2.UnitX.RotatedByRandom(3.1415927410125732).RotatedBy(projectile.velocity.ToRotation(), default) * projectile.width / 2f;
                Main.dust[num98].noGravity = true;
                Main.dust[num98].velocity *= 3f;
            }
            for (int num99 = 0; num99 < 10; num99++)
            {
                int num100 = Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), projectile.width, projectile.height, ModContent.DustType<Dusts.VoidDust>(), 0f, 0f, 0, default, 1.5f);
                Main.dust[num100].position = projectile.Center + Vector2.UnitX.RotatedByRandom(3.1415927410125732).RotatedBy(projectile.velocity.ToRotation(), default) * projectile.width / 2f;
                Main.dust[num100].noGravity = true;
                Main.dust[num100].velocity *= 3f;
            }
        }

        // chain voodoo
        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
        { 
            Texture2D Tex = Main.projectileTexture[projectile.type];
            Texture2D Vortex = mod.GetTexture("NPCs/Bosses/ObserverVoid/Vortex1");
            Rectangle frame = new Rectangle(0, 0, Tex.Width, Tex.Height);
            BaseDrawing.DrawTexture(spriteBatch, Vortex, 0, projectile.position, projectile.width, projectile.height, projectile.scale, projectile.rotation, 0, 1, frame, projectile.GetAlpha(Colors.COLOR_GLOWPULSE), true);
            BaseDrawing.DrawTexture(spriteBatch, Tex, 0, projectile.position, projectile.width, projectile.height, projectile.scale, -projectile.rotation, 0, 1, frame, projectile.GetAlpha(Colors.COLOR_GLOWPULSE), true);
            return false;
        }
    }
}