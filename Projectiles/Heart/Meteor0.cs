using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CSkies.Projectiles.Heart
{
    public class Meteor0 : ModProjectile
	{
        public override string Texture => "CSkies/NPCs/Bosses/Heartcore/Meteor0";
        public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Meteor");
			ProjectileID.Sets.TrailCacheLength[projectile.type] = 8;
			ProjectileID.Sets.TrailingMode[projectile.type] = 0;
		}

		public override void SetDefaults()
		{
			projectile.width = 24;
			projectile.height = 24;
			projectile.friendly = true;
			projectile.tileCollide = false;
			projectile.extraUpdates = 2;
            projectile.magic = true;
            projectile.alpha = 0;
		}

		public override void AI()
		{
			if (projectile.position.Y > Main.player[projectile.owner].position.Y - 300f)
			{
				projectile.tileCollide = true;
			}
			if (projectile.position.Y < Main.worldSurface * 16.0)
			{
				projectile.tileCollide = true;
			}
			projectile.scale = projectile.ai[1];
			projectile.rotation += projectile.velocity.X * 2f;
			Vector2 position = projectile.Center + Vector2.Normalize(projectile.velocity) * 10f;
			Dust dust20 = Main.dust[Dust.NewDust(projectile.position, projectile.width, projectile.height, DustID.Fire, 0f, 0f, 0, default, 1f)];
			dust20.position = position;
			dust20.velocity = projectile.velocity.RotatedBy(1.5707963705062866, default) * 0.33f + projectile.velocity / 4f;
			dust20.position += projectile.velocity.RotatedBy(1.5707963705062866, default);
			dust20.fadeIn = 0.5f;
			dust20.noGravity = true;
			dust20 = Main.dust[Dust.NewDust(projectile.position, projectile.width, projectile.height, DustID.Fire, 0f, 0f, 0, default, 1f)];
			dust20.position = position;
			dust20.velocity = projectile.velocity.RotatedBy(-1.5707963705062866, default) * 0.33f + projectile.velocity / 4f;
			dust20.position += projectile.velocity.RotatedBy(-1.5707963705062866, default);
			dust20.fadeIn = 0.5f;
			dust20.noGravity = true;
			for (int num189 = 0; num189 < 1; num189++)
			{
				int num190 = Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), projectile.width, projectile.height, DustID.Fire, 0f, 0f, 0, default, 1f);
				Main.dust[num190].velocity *= 0.5f;
				Main.dust[num190].scale *= 1.3f;
				Main.dust[num190].fadeIn = 1f;
				Main.dust[num190].noGravity = true;
			}
		}

		public override void Kill(int timeLeft)
		{
			Main.PlaySound(SoundID.Item89, projectile.position);
			projectile.position.X = projectile.position.X + projectile.width / 2;
			projectile.position.Y = projectile.position.Y + projectile.height / 2;
			projectile.width = (int)(128f * projectile.scale);
			projectile.height = (int)(128f * projectile.scale);
			projectile.position.X = projectile.position.X - projectile.width / 2;
			projectile.position.Y = projectile.position.Y - projectile.height / 2;
			for (int num336 = 0; num336 < 4; num336++)
			{
				Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), projectile.width, projectile.height, DustID.Fire, 0f, 0f, 100, default, 1.5f);
			}
			for (int num337 = 0; num337 < 16; num337++)
			{
				int num338 = Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), projectile.width, projectile.height, DustID.Fire, 0f, 0f, 100, default, 2.5f);
				Main.dust[num338].noGravity = true;
				Main.dust[num338].velocity *= 3f;
				num338 = Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), projectile.width, projectile.height, DustID.Fire, 0f, 0f, 100, default, 1.5f);
				Main.dust[num338].velocity *= 2f;
				Main.dust[num338].noGravity = true;
			}
			for (int num339 = 0; num339 < 2; num339++)
			{
				int num340 = Gore.NewGore(projectile.position + new Vector2(projectile.width * Main.rand.Next(100) / 100f, projectile.height * Main.rand.Next(100) / 100f) - Vector2.One * 10f, default, Main.rand.Next(61, 64), 1f);
				Main.gore[num340].velocity *= 0.3f;
				Gore expr_B4D2_cp_0 = Main.gore[num340];
				expr_B4D2_cp_0.velocity.X += Main.rand.Next(-10, 11) * 0.05f;
				Gore expr_B502_cp_0 = Main.gore[num340];
				expr_B502_cp_0.velocity.Y += Main.rand.Next(-10, 11) * 0.05f;
			}
			if (projectile.owner == Main.myPlayer)
			{
				projectile.localAI[1] = -1f;
				projectile.maxPenetrate = 0;
				projectile.Damage();
			}
			for (int num341 = 0; num341 < 5; num341++)
			{
				int num343 = Dust.NewDust(projectile.position, projectile.width, projectile.height, DustID.Fire, 2.5f * projectile.direction, -2.5f, 0, default, 1f);
				Main.dust[num343].alpha = 200;
				Main.dust[num343].velocity *= 2.4f;
				Main.dust[num343].scale += Main.rand.NextFloat();
			}
		}

        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            int RockType = Utils.SelectRandom(Main.rand, new int[]
            {
                0, 1, 2
            });

            Texture2D Rock = mod.GetTexture("NPCs/Bosses/Heartcore/Meteor" + RockType);
            Texture2D RockGlow = mod.GetTexture("Glowmasks/Meteor" + RockType + "_Glow");
            BaseDrawing.DrawTexture(spriteBatch, Rock, 0, projectile, lightColor);
            BaseDrawing.DrawTexture(spriteBatch, RockGlow, 0, projectile, projectile.GetAlpha(Colors.COLOR_GLOWPULSE));
            return false;
        }
    }
}
