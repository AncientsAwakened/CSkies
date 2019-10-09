using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Terraria;
using Terraria.ModLoader;

namespace CSkies.NPCs.Bosses.Void
{
    public class VoidOrbitter : ModProjectile
	{
        public override string Texture => "CSkies/NPCs/Bosses/ObserverVoid/Vortex";
        public override void SetStaticDefaults()
		{
            DisplayName.SetDefault("Void Cyclone");
        }

        public override void SetDefaults()
        {
            projectile.tileCollide = false;
            projectile.width = 30;
            projectile.height = 30;
            projectile.aiStyle = -1;
            projectile.penetrate = -1;
            projectile.friendly = false;
            projectile.hostile = true;
            projectile.ignoreWater = true;
            projectile.timeLeft = 600;
        }

		public int body = -1;
		public float rotValue = -1f;
        public Vector2 pos;

        public override void AI()
        {
            Lighting.AddLight(projectile.Center, 0, .1f, .3f);
            if (!NPC.AnyNPCs(mod.NPCType<Void>()))
            {
                projectile.active = false;
            }

            projectile.rotation += 0.1f;

            if (body == -1)
            {
                int npcID = BaseAI.GetNPC(projectile.Center, mod.NPCType<Void>(), 400f, null);
                if (npcID >= 0) body = npcID;
            }
            if (body == -1) return;
            NPC observer = Main.npc[body];
            if (observer == null || observer.life <= 0 || !observer.active || observer.type != mod.NPCType<Void>()) { projectile.active = false; return; }

            pos = observer.Center;

            for (int m = projectile.oldPos.Length - 1; m > 0; m--)
            {
                projectile.oldPos[m] = projectile.oldPos[m - 1];
            }
            projectile.oldPos[0] = projectile.position;

            int starNumber = 4;

            projectile.timeLeft = 180;

            if (rotValue == -1f) rotValue = projectile.ai[0] % starNumber * ((float)Math.PI * 2f / starNumber);
            rotValue += 0.08f;
            while (rotValue > (float)Math.PI * 2f) rotValue -= (float)Math.PI * 2f;

            projectile.Center = BaseUtility.RotateVector(observer.Center, observer.Center + new Vector2(200, 0f), rotValue);

            for (int u = 0; u < Main.maxPlayers; u++)
            {
                Player target = Main.player[u];

                if (target.active && Vector2.Distance(projectile.Center, target.Center) < 100)
                {
                    float num3 = 6f;
                    Vector2 vector = new Vector2(target.position.X + target.width / 2, target.position.Y + target.height / 2);
                    float num4 = projectile.Center.X - vector.X;
                    float num5 = projectile.Center.Y - vector.Y;
                    float num6 = (float)Math.Sqrt(num4 * num4 + num5 * num5);
                    num6 = num3 / num6;
                    num4 *= num6;
                    num5 *= num6;
                    int num7 = 3;
                    target.velocity.X = (target.velocity.X * (num7 - 1) + num4) / num7;
                    target.velocity.Y = (target.velocity.Y * (num7 - 1) + num5) / num7;
                }
            }

            if (observer.ai[0] < 4 || observer.ai[0] > 5)
            {
                projectile.scale -= .05f;
                if (projectile.scale <= 0)
                {
                    projectile.active = false;
                    projectile.netUpdate = true;
                }
            }
        }

        public override bool PreDraw(SpriteBatch spritebatch, Color lightColor)
        {
            Texture2D Tex = mod.GetTexture("NPCs/Bosses/ObserverVoid/Vortex");
            Texture2D Vortex = mod.GetTexture("NPCs/Bosses/ObserverVoid/Vortex1");
            Rectangle frame = new Rectangle(0, 0, Tex.Width, Tex.Height);
            BaseDrawing.DrawTexture(spritebatch, Vortex, 0, projectile.position, projectile.width, projectile.height, projectile.scale, projectile.rotation, 0, 1, frame, projectile.GetAlpha(Color.White), true);
            BaseDrawing.DrawTexture(spritebatch, Tex, 0, projectile.position, projectile.width, projectile.height, projectile.scale, -projectile.rotation, 0, 1, frame, projectile.GetAlpha(Color.White), true);
            return false;
        }
    }
}