using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CSkies.NPCs.Bosses.Novacore
{
    public class NovaRocket : ModProjectile
    {
		public override void SetDefaults()
		{
			projectile.width = 14;
			projectile.height = 14;
			projectile.aiStyle = 80;
			projectile.hostile = true;
			projectile.penetrate = -1;
			projectile.tileCollide = false;
		}

        public override void AI()
		{
			if (projectile.ai[0] == 0f && projectile.ai[1] > 0f)
			{
				ref float reference = ref projectile.ai[1];
				ref float reference32 = ref reference;
				float num15 = reference;
				reference32 = num15 - 1f;
			}
			else if (projectile.ai[0] == 0f && projectile.ai[1] == 0f)
			{
				projectile.ai[0] = 1f;
				projectile.ai[1] = Player.FindClosest(projectile.position, projectile.width, projectile.height);
				projectile.netUpdate = true;
				float num639 = projectile.velocity.Length();
				projectile.velocity = Vector2.Normalize(projectile.velocity) * (num639 + 4f);
				for (int num640 = 0; num640 < 8; num640++)
				{
					Vector2 spinningpoint9 = Vector2.UnitX * -8f;
					spinningpoint9 += -Vector2.UnitY.RotatedBy(num640 * (float)Math.PI / 4f) * new Vector2(2f, 8f);
					spinningpoint9 = spinningpoint9.RotatedBy(projectile.rotation - (float)Math.PI / 2f);
					int num641 = Dust.NewDust(projectile.Center, 0, 0, 228);
					Main.dust[num641].scale = 1.5f;
					Main.dust[num641].noGravity = true;
					Main.dust[num641].position = projectile.Center + spinningpoint9;
					Main.dust[num641].velocity = projectile.velocity * 0f;
				}
			}
			else if (projectile.ai[0] == 1f)
			{
				projectile.tileCollide = true;
				ref float reference = ref projectile.localAI[1];
				ref float reference33 = ref reference;
				float num15 = reference;
				reference33 = num15 + 1f;
				float num642 = 180f;
				float num643 = 0f;
				float num644 = 30f;
				if (projectile.localAI[1] == num642)
				{
					projectile.Kill();
					return;
				}
				if (projectile.localAI[1] >= num643 && projectile.localAI[1] < num643 + num644)
				{
					Vector2 v4 = Main.player[(int)projectile.ai[1]].Center - projectile.Center;
					float num645 = projectile.velocity.ToRotation();
					float num646 = v4.ToRotation();
					double num647 = num646 - num645;
					if (num647 > Math.PI)
					{
						num647 -= Math.PI * 2.0;
					}
					if (num647 < -Math.PI)
					{
						num647 += Math.PI * 2.0;
					}
					projectile.velocity = projectile.velocity.RotatedBy(num647 * 0.20000000298023224);
				}
				if (projectile.localAI[1] % 5f == 0f)
				{
					for (int num648 = 0; num648 < 4; num648++)
					{
						Vector2 spinningpoint10 = Vector2.UnitX * -8f;
						spinningpoint10 += -Vector2.UnitY.RotatedBy(num648 * (float)Math.PI / 4f) * new Vector2(2f, 4f);
						spinningpoint10 = spinningpoint10.RotatedBy(projectile.rotation - (float)Math.PI / 2f);
						int num649 = Dust.NewDust(projectile.Center, 0, 0, 228);
						Main.dust[num649].scale = 1.5f;
						Main.dust[num649].noGravity = true;
						Main.dust[num649].position = projectile.Center + spinningpoint10;
						Main.dust[num649].velocity = projectile.velocity * 0f;
					}
				}
			}
			projectile.rotation = projectile.velocity.ToRotation() + (float)Math.PI / 2f;
			if (++projectile.frameCounter >= 3)
			{
				projectile.frameCounter = 0;
				if (++projectile.frame >= 3)
				{
					projectile.frame = 0;
				}
			}
			for (int num650 = 0; num650 < 1f + projectile.ai[0]; num650++)
			{
				Vector2 value21 = Vector2.UnitY.RotatedBy(projectile.rotation) * 8f * (num650 + 1);
				int num651 = Dust.NewDust(projectile.Center, 0, 0, 228);
				Main.dust[num651].position = projectile.Center + value21;
				Main.dust[num651].scale = 1f;
				Main.dust[num651].noGravity = true;
			}
			int num652 = 0;
			while (true)
			{
				if (num652 < 255)
				{
					Player player7 = Main.player[num652];
					if (player7.active && !player7.dead && Vector2.Distance(player7.Center, projectile.Center) <= 42f)
					{
						break;
					}
					num652++;
					continue;
				}
				return;
			}
			projectile.Kill();
		}

		public override void Kill(int timeLeft)
		{
			Main.PlaySound(SoundID.Item14, projectile.position);
			projectile.position = projectile.Center;
			projectile.width = (projectile.height = 112);
			projectile.position.X -= projectile.width / 2;
			projectile.position.Y -= projectile.height / 2;
			for (int num359 = 0; num359 < 4; num359++)
			{
				Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), projectile.width, projectile.height, ModContent.DustType<Dusts.CDust>(), 0f, 0f, 100, default, 1.5f);
			}
			for (int num360 = 0; num360 < 40; num360++)
			{
				int num361 = Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), projectile.width, projectile.height, 228, 0f, 0f, 0, default, 2.5f);
				Main.dust[num361].noGravity = true;
				Dust dust2 = Main.dust[num361];
				dust2.velocity *= 3f;
				num361 = Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), projectile.width, projectile.height, 228, 0f, 0f, 100, default, 1.5f);
				dust2 = Main.dust[num361];
				dust2.velocity *= 2f;
				Main.dust[num361].noGravity = true;
			}
			for (int num362 = 0; num362 < 1; num362++)
			{
				int num363 = Gore.NewGore(projectile.position + new Vector2(projectile.width * Main.rand.Next(100) / 100f, projectile.height * Main.rand.Next(100) / 100f) - Vector2.One * 10f, default, Main.rand.Next(61, 64));
				Gore gore = Main.gore[num363];
				gore.velocity *= 0.3f;
				Main.gore[num363].velocity.X += Main.rand.Next(-10, 11) * 0.05f;
				Main.gore[num363].velocity.Y += Main.rand.Next(-10, 11) * 0.05f;
			}
			projectile.Damage();
		}
	}
}