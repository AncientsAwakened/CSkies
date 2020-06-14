using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ID;
using Terraria.Utilities;

namespace CSkies
{
    public class Colors
    {
        public static Color COLOR_GLOWPULSE => Color.White * (Main.mouseTextColor / 255f);

        public static Color Flash => BaseUtility.MultiLerpColor(Main.LocalPlayer.miscCounter % 100 / 100f, Color.Transparent, Color.White, Color.Transparent);
        public static Color FlashInverse => BaseUtility.MultiLerpColor(Main.LocalPlayer.miscCounter % 100 / 100f, Color.White, Color.Transparent, Color.White);

        public static Color Rarity12 => new Color(239, 0, 243);

        public static Color Rarity13 => new Color(0, 125, 243);

        public static Color Rarity14 => new Color(255, 22, 0);

        public static Color Rarity15 => new Color(0, 178, 107);
    }

    public static class CUtils
    {
        public static void DropLoot(this Entity ent, int type, int stack = 1)
        {
            Item.NewItem(ent.Hitbox, type, stack);
        }

        public static void DropLoot(this Entity ent, int type, float chance)
        {
            if (Main.rand.NextDouble() < chance)
            {
                Item.NewItem(ent.Hitbox, type);
            }
        }

        public static void DropLoot(this Entity ent, int type, int min, int max)
        {
            Item.NewItem(ent.Hitbox, type, Main.rand.Next(min, max));
        }

        public static bool AnyProjectiles(int type)
        {
            for (int i = 0; i < Main.maxProjectiles; i++)
            {
                if (Main.projectile[i].active && Main.projectile[i].type == type)
                {
                    return true;
                }
            }
            return false;
        }

        public static int CountProjectiles(int type)
        {
            int p = 0;
            for (int i = 0; i < Main.maxProjectiles; i++)
            {
                if (Main.projectile[i].active && Main.projectile[i].type == type)
                {
                    p++;
                }
            }
            return p;
        }

        public static void ObectPlace(int x, int y, int type)
        {
            WorldGen.PlaceObject(x, y, type);
            NetMessage.SendObjectPlacment(-1, x, y, type, 0, 0, -1, -1);
        }
    }

    public class CAI
    {
        public static void LightningAI(Projectile projectile)
        {
			if (projectile.localAI[1] == 0f && projectile.ai[0] >= 900f)
			{
				projectile.ai[0] -= 1000f;
				projectile.localAI[1] = -1f;
			}
			projectile.frameCounter++;
			if (projectile.velocity == Vector2.Zero)
			{
				if (projectile.frameCounter >= projectile.extraUpdates * 2)
				{
					projectile.frameCounter = 0;
					bool flag32 = true;
					for (int num751 = 1; num751 < projectile.oldPos.Length; num751++)
					{
						if (projectile.oldPos[num751] != projectile.oldPos[0])
						{
							flag32 = false;
						}
					}
					if (flag32)
					{
						projectile.Kill();
						return;
					}
				}
				if (Main.rand.Next(projectile.extraUpdates) == 0 && (projectile.velocity != Vector2.Zero || Main.rand.Next((projectile.localAI[1] == 2f) ? 2 : 6) == 0))
				{
					for (int num752 = 0; num752 < 2; num752++)
					{
						float num753 = projectile.rotation + ((Main.rand.Next(2) == 1) ? (-1f) : 1f) * ((float)Math.PI / 2f);
						float num754 = (float)Main.rand.NextDouble() * 0.8f + 1f;
						Vector2 vector56 = new Vector2((float)Math.Cos(num753) * num754, (float)Math.Sin(num753) * num754);
						int num755 = Dust.NewDust(projectile.Center, 0, 0, 226, vector56.X, vector56.Y);
						Main.dust[num755].noGravity = true;
						Main.dust[num755].scale = 1.2f;
					}
					if (Main.rand.Next(5) == 0)
					{
						Vector2 value43 = projectile.velocity.RotatedBy(1.5707963705062866) * ((float)Main.rand.NextDouble() - 0.5f) * projectile.width;
						int num756 = Dust.NewDust(projectile.Center + value43 - Vector2.One * 4f, 8, 8, 31, 0f, 0f, 100, default, 1.5f);
						Dust dust = Main.dust[num756];
						dust.velocity *= 0.5f;
						Main.dust[num756].velocity.Y = 0f - Math.Abs(Main.dust[num756].velocity.Y);
					}
				}
			}
			else
			{
				if (projectile.frameCounter < projectile.extraUpdates * 2)
				{
					return;
				}
				projectile.frameCounter = 0;
				float num757 = projectile.velocity.Length();
				UnifiedRandom unifiedRandom2 = new UnifiedRandom((int)projectile.ai[1]);
				int num758 = 0;
				Vector2 spinningpoint15 = -Vector2.UnitY;
				while (true)
				{
					int num759 = unifiedRandom2.Next();
					projectile.ai[1] = num759;
					num759 %= 100;
					float f2 = num759 / 100f * ((float)Math.PI * 2f);
					Vector2 vector57 = f2.ToRotationVector2();
					if (vector57.Y > 0f)
					{
						vector57.Y *= -1f;
					}
					bool flag33 = false;
					if (vector57.Y > -0.02f)
					{
						flag33 = true;
					}
					if (vector57.X * (projectile.extraUpdates + 1) * 2f * num757 + projectile.localAI[0] > 40f)
					{
						flag33 = true;
					}
					if (vector57.X * (projectile.extraUpdates + 1) * 2f * num757 + projectile.localAI[0] < -40f)
					{
						flag33 = true;
					}
					if (flag33)
					{
						if (num758++ >= 100)
						{
							projectile.velocity = Vector2.Zero;
							if (projectile.localAI[1] < 1f)
							{
								projectile.localAI[1] += 2f;
							}
							break;
						}
						continue;
					}
					spinningpoint15 = vector57;
					break;
				}
				if (!(projectile.velocity != Vector2.Zero))
				{
					return;
				}
				projectile.localAI[0] += spinningpoint15.X * (projectile.extraUpdates + 1) * 2f * num757;
				projectile.velocity = spinningpoint15.RotatedBy(projectile.ai[0] + (float)Math.PI / 2f) * num757;
				projectile.rotation = projectile.velocity.ToRotation() + (float)Math.PI / 2f;
				if (Main.rand.Next(4) == 0 && Main.netMode != NetmodeID.MultiplayerClient && projectile.localAI[1] == 0f)
				{
					float num760 = Main.rand.Next(-3, 4) * ((float)Math.PI / 3f) / 3f;
					Vector2 vector58 = projectile.ai[0].ToRotationVector2().RotatedBy(num760) * projectile.velocity.Length();
					if (!Collision.CanHitLine(projectile.Center, 0, 0, projectile.Center + vector58 * 50f, 0, 0))
					{
						Projectile.NewProjectile(projectile.Center.X - vector58.X, projectile.Center.Y - vector58.Y, vector58.X, vector58.Y, projectile.type, projectile.damage, projectile.knockBack, projectile.owner, vector58.ToRotation() + 1000f, projectile.ai[1]);
					}
				}
			}
		}
	}

	public class CDrawing
	{ 
		public static void LightningDraw(Projectile projectile, SpriteBatch sb, Color lightColor, Color OuterColor, Color InnerColor)
		{
			Vector2 end3 = projectile.position + new Vector2(projectile.width, projectile.height) / 2f + Vector2.UnitY * projectile.gfxOffY - Main.screenPosition;
			Texture2D value139 = Main.extraTexture[33];
			projectile.GetAlpha(lightColor);
			Vector2 vector55 = new Vector2(projectile.scale) / 2f;
			for (int num343 = 0; num343 < 2; num343++)
			{
				float num344 = (projectile.localAI[1] == -1f || projectile.localAI[1] == 1f) ? (-0.2f) : 0f;
				if (num343 == 0)
				{
					vector55 = new Vector2(projectile.scale) * (0.5f + num344);
					DelegateMethods.c_1 = OuterColor * 0.5f;
				}
				else
				{
					vector55 = new Vector2(projectile.scale) * (0.3f + num344);
					DelegateMethods.c_1 = InnerColor * 0.5f;
				}
				DelegateMethods.f_1 = 1f;
				for (int num345 = projectile.oldPos.Length - 1; num345 > 0; num345--)
				{
					if (!(projectile.oldPos[num345] == Vector2.Zero))
					{
						Vector2 start3 = projectile.oldPos[num345] + new Vector2(projectile.width, projectile.height) / 2f + Vector2.UnitY * projectile.gfxOffY - Main.screenPosition;
						Vector2 end4 = projectile.oldPos[num345 - 1] + new Vector2(projectile.width, projectile.height) / 2f + Vector2.UnitY * projectile.gfxOffY - Main.screenPosition;
						Utils.DrawLaser(sb, value139, start3, end4, vector55, DelegateMethods.LightningLaserDraw);
					}
				}
				if (projectile.oldPos[0] != Vector2.Zero)
				{
					DelegateMethods.f_1 = 1f;
					Vector2 start4 = projectile.oldPos[0] + new Vector2(projectile.width, projectile.height) / 2f + Vector2.UnitY * projectile.gfxOffY - Main.screenPosition;
					Utils.DrawLaser(sb, value139, start4, end3, vector55, DelegateMethods.LightningLaserDraw);
				}
			}
		}
	}
}
