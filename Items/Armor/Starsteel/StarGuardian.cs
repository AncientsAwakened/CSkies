using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.IO;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
namespace CSkies.Items.Armor.Starsteel
{
    public class StarGuardian : ModProjectile
	{
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Terra Crystal");
            Main.projFrames[projectile.type] = 3;
		}

		public override void SetDefaults()
        {
            projectile.width = 22;
            projectile.height = 42;
            projectile.aiStyle = -1;
            projectile.friendly = true;
            projectile.tileCollide = false;
            projectile.penetrate = -1;
            projectile.timeLeft *= 5;
            projectile.light = 0.4f;
            projectile.ignoreWater = true;
            projectile.minion = true;
            projectile.minionSlots = 0;
        }

        public override void AI()
		{
			Player player = Main.player[projectile.owner];
			CPlayer modPlayer = player.GetModPlayer<CPlayer>();
			if (!modPlayer.Starsteel || modPlayer.StarsteelBonus != 4)
            {
				projectile.Kill();
				return;
			}

			Vector2 PlayerPoint;
			PlayerPoint.X = player.Center.X - projectile.width / 2;
			PlayerPoint.Y = player.Center.Y - projectile.height / 2 + player.gfxOffY - 60f;

			projectile.position = PlayerPoint;

			if (player.gravDir == -1f)
			{
				projectile.position.Y = projectile.position.Y + 120f;
				projectile.rotation = 3.14f;
			}
			else
			{
				projectile.rotation = 0f;
			}

			if (projectile.owner == Main.myPlayer)
			{
				if (projectile.ai[0] != 0f)
				{
					projectile.ai[0] -= 1f;
					return;
				}
				float num396 = projectile.position.X;
				float num397 = projectile.position.Y;
				float num398 = 700f;
				bool flag11 = false;
				for (int num399 = 0; num399 < 200; num399++)
				{
					if (Main.npc[num399].CanBeChasedBy(this, true))
					{
						float num400 = Main.npc[num399].position.X + Main.npc[num399].width / 2;
						float num401 = Main.npc[num399].position.Y + Main.npc[num399].height / 2;
						float num402 = Math.Abs(projectile.position.X + projectile.width / 2 - num400) + Math.Abs(projectile.position.Y + projectile.height / 2 - num401);
						if (num402 < num398 && Collision.CanHit(projectile.position, projectile.width, projectile.height, Main.npc[num399].position, Main.npc[num399].width, Main.npc[num399].height))
						{
							num398 = num402;
							num396 = num400;
							num397 = num401;
							flag11 = true;
						}
					}
				}
				if (flag11)
				{
					float num403 = 12f;
					Vector2 vector29 = new Vector2(projectile.position.X + projectile.width * 0.5f, projectile.position.Y + projectile.height * 0.5f);
					float num404 = num396 - vector29.X;
					float num405 = num397 - vector29.Y;
					float num406 = (float)Math.Sqrt(num404 * num404 + num405 * num405);
					num406 = num403 / num406;
					num404 *= num406;
					num405 *= num406;
					int p = Projectile.NewProjectile(projectile.Center.X - 4f, projectile.Center.Y, num404, num405, ModContent.ProjectileType<StarGuardianShot>(), Player.crystalLeafDamage, Player.crystalLeafKB, projectile.owner, 0f, 0f);
					Main.projectile[p].melee = false;
					Main.projectile[p].minion = true;
					Main.projectile[p].minionSlots = 0;
					projectile.ai[0] = 160;
					return;
				}
			}
		}

        public float auraPercent = 0f;
        public bool auraDirection = true;

        public override bool PreDraw(SpriteBatch sb, Color dColor)
        {
            if (auraDirection) { auraPercent += 0.1f; auraDirection = auraPercent < 1f; }
            else { auraPercent -= 0.1f; auraDirection = auraPercent <= 0f; }

            Rectangle frame = BaseDrawing.GetFrame(projectile.frame, Main.projectileTexture[projectile.type].Width, Main.projectileTexture[projectile.type].Height / 3, 0, 0);

            BaseDrawing.DrawAura(sb, Main.projectileTexture[projectile.type], 0, projectile.position, projectile.width, projectile.height, auraPercent, 1.4f, projectile.scale, projectile.rotation, projectile.direction, 3, frame, 0, 0, Color.White);
            BaseDrawing.DrawTexture(sb, Main.projectileTexture[projectile.type], 0, projectile.position, projectile.width, projectile.height, projectile.scale, projectile.rotation, projectile.direction, 3, frame, projectile.GetAlpha(Colors.COLOR_GLOWPULSE), true);

            return false;
        }

        public void MoveToPoint(Vector2 point)
		{
			float moveSpeed = 20f;
			float velMultiplier = 1f;
			Vector2 dist = point - projectile.Center;
			float length = dist == Vector2.Zero ? 0f : dist.Length();
			if (length < moveSpeed)
			{
				velMultiplier = MathHelper.Lerp(0f, 1f, length / moveSpeed);
			}
			if (length < 200f)
			{
				moveSpeed *= 0.5f;
			}
			if (length < 100f)
			{
				moveSpeed *= 0.5f;
			}
			if (length < 50f)
			{
				moveSpeed *= 0.5f;
			}
			if (length < 10f)
			{
				moveSpeed *= 0.01f;
			}
			projectile.velocity = length == 0f ? Vector2.Zero : Vector2.Normalize(dist);
			projectile.velocity *= moveSpeed;
			projectile.velocity *= velMultiplier;
		}
	}
}
