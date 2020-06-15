using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CSkies.NPCs.Bosses.Novacore
{
    public class NovaBlast : ModProjectile
    {
		public override void SetStaticDefaults()
		{
			ProjectileID.Sets.TrailingMode[projectile.type] = 0;
		}
		public override void SetDefaults()
        {
            projectile.width = 24;
            projectile.height = 24;
            projectile.aiStyle = 5;
            projectile.hostile = true;
            projectile.penetrate = 2;
            projectile.alpha = 255;
            projectile.tileCollide = false;
            projectile.extraUpdates = 1;
        }

        public override void AI()
		{
			if (projectile.soundDelay == 0)
			{
				projectile.soundDelay = 20 + Main.rand.Next(40);
				Main.PlaySound(SoundID.Item9, projectile.position);
			}
			projectile.alpha -= 15;
			int num75 = 150;
			if (projectile.Center.Y >= projectile.ai[1])
			{
				num75 = 0;
			}
			if (projectile.alpha < num75)
			{
				projectile.alpha = num75;
			}

			projectile.rotation = projectile.velocity.ToRotation() - (float)Math.PI / 2f;

			if (Main.rand.Next(16) == 0)
			{
				Vector2 value3 = Vector2.UnitX.RotatedByRandom(1.5707963705062866).RotatedBy(projectile.velocity.ToRotation());
				int num77 = Dust.NewDust(projectile.position, projectile.width, projectile.height, 58, projectile.velocity.X * 0.5f, projectile.velocity.Y * 0.5f, 150, default, 1.2f);
				Main.dust[num77].velocity = value3 * 0.66f;
				Main.dust[num77].position = projectile.Center + value3 * 12f;
			}
			if (Main.rand.Next(48) == 0)
			{
				int num78 = Gore.NewGore(projectile.Center, new Vector2(projectile.velocity.X * 0.2f, projectile.velocity.Y * 0.2f), 16);
				Gore gore = Main.gore[num78];
				gore.velocity *= 0.66f;
				gore = Main.gore[num78];
				gore.velocity += projectile.velocity * 0.3f;
			}
		}

		public override void Kill(int timeLeft)
		{
			Main.PlaySound(SoundID.Item10, projectile.position);
			projectile.velocity /= 2f;
			for (int num611 = 0; num611 < 40; num611++)
			{
				Dust.NewDust(projectile.position, projectile.width, projectile.height, 58, projectile.velocity.X * 0.1f, projectile.velocity.Y * 0.1f, 150, default, 1.2f);
			}
			for (int num612 = 0; num612 < 2; num612++)
			{
				Gore.NewGore(projectile.position, new Vector2(projectile.velocity.X * 0.05f, projectile.velocity.Y * 0.05f), 16);
			}
        }

		public override Color? GetAlpha(Color lightColor)
		{
			Color value2 = Color.Lerp(lightColor, Color.White, 0.5f) * (1f - projectile.alpha / 255f);
			Color value3 = Color.Lerp(Color.Purple, Color.White, 0.33f);
			float amount = 0.25f + (float)Math.Cos(projectile.localAI[0]) * 0.25f;
			return Color.Lerp(value2, value3, amount);
		}

		public override bool PreDraw(SpriteBatch sb, Color lightColor)
		{
			Texture2D value13 = Main.projectileTexture[projectile.type];
			Rectangle rectangle3 = new Rectangle(0, 0, value13.Width, value13.Height);
			Vector2 origin4 = rectangle3.Size() / 2f;
			origin4.Y = 70f;
			
			Color color38 = projectile.GetAlpha(lightColor);
			float num169 = projectile.scale;
			float rotation23 = projectile.rotation;

			sb.Draw(value13, projectile.Center + Vector2.Zero - Main.screenPosition + new Vector2(0f, projectile.gfxOffY), rectangle3, color38, rotation23, origin4, num169, SpriteEffects.None, 0);

			sb.Draw(mod.GetTexture("NPCs/Bosses/Novacore/NovaBlast_Nova"), projectile.Center - Main.screenPosition + new Vector2(0f, projectile.gfxOffY), rectangle3, Color.White, projectile.localAI[0], origin4, projectile.scale, SpriteEffects.None, 0);
			return false;
		}
	}
}