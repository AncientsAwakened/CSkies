using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

using System.IO;
using Terraria.Audio;

namespace CSkies.NPCs.Bosses.ObserverVoid
{
    public class BlackHole : ModProjectile
	{
        public int damage = 0;

		public override void SetStaticDefaults()
		{
            DisplayName.SetDefault("Black Hole");
            Main.projFrames[projectile.type] = 4;
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

        public float[] internalAI = new float[1];

        public override void SendExtraAI(BinaryWriter writer)
        {
            base.SendExtraAI(writer);
            if (Main.netMode == NetmodeID.Server || Main.dedServ)
            {
                writer.Write(internalAI[0]);
            }
        }

        public override void ReceiveExtraAI(BinaryReader reader)
        {
            base.ReceiveExtraAI(reader);
            if (Main.netMode == NetmodeID.MultiplayerClient)
            {
                internalAI[0] = reader.ReadFloat();
            }
        }

        public override void AI()
        {
            Lighting.AddLight(projectile.Center, 0, 0f, .15f);
            if (projectile.frameCounter > 5)
            {
                projectile.frame++;
                projectile.frameCounter = 0;
                if (projectile.frame > 3)
                {
                    projectile.frame = 0;
                }
            }

            if (!NPC.AnyNPCs(mod.NPCType<ObserverVoid>()))
            {
                projectile.active = false;
            }

            if (body == -1)
            {
                int npcID = BaseAI.GetNPC(projectile.Center, mod.NPCType<ObserverVoid>(), 400f, null);
                if (npcID >= 0) body = npcID;
            }
            if (body == -1) return;
            NPC observer = Main.npc[body];
            if (observer == null || observer.life <= 0 || !observer.active || observer.type != mod.NPCType<ObserverVoid>()) { projectile.active = false; return; }

            Player player = Main.player[observer.target];

            pos = observer.Center;

            for (int m = projectile.oldPos.Length - 1; m > 0; m--)
            {
                projectile.oldPos[m] = projectile.oldPos[m - 1];
            }
            projectile.oldPos[0] = projectile.position;

            int starNumber = ((ObserverVoid)observer.modNPC).StarCount;

            if (((ObserverVoid)observer.modNPC).internalAI[0] == 0)
            {
                projectile.timeLeft = 180;
                float dist = ((ObserverVoid)observer.modNPC).internalAI[1];

                if (rotValue == -1f) rotValue = projectile.ai[0] % starNumber * ((float)Math.PI * 2f / starNumber);
                rotValue += 0.08f;
                while (rotValue > (float)Math.PI * 2f) rotValue -= (float)Math.PI * 2f;

                for (int m = projectile.oldPos.Length - 1; m > 0; m--)
                {
                    projectile.oldPos[m] = projectile.oldPos[m - 1];
                }
                projectile.oldPos[0] = projectile.position;

                projectile.Center = BaseUtility.RotateVector(observer.Center, observer.Center + new Vector2(dist, 0f), rotValue);

                if (projectile.ai[1]++ > 180)
                {
                    if (Collision.CanHit(projectile.position, projectile.width, projectile.height, player.Center, player.width, player.height))
                    {
                        Vector2 fireTarget = projectile.Center;
                        float rot = BaseUtility.RotationTo(projectile.Center, player.Center);
                        fireTarget = BaseUtility.RotateVector(projectile.Center, fireTarget, rot);
                        BaseAI.FireProjectile(player.Center, fireTarget, mod.ProjType("BlackHoleProj"), damage, 0f, 4f);
                    }
                    projectile.ai[1] = 0;
                }
            }
            else if (((ObserverVoid)observer.modNPC).internalAI[0] == 1)
            {
                projectile.Kill();
            }
        }

        public override void Kill(int timeLeft)
        {
            Main.PlaySound(new LegacySoundStyle(2, 89, Terraria.Audio.SoundType.Sound));
            int p = Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, projectile.velocity.X, projectile.velocity.Y, mod.ProjectileType("Vortex"), projectile.damage, projectile.knockBack, projectile.owner, 0f, 0f);
            Main.projectile[p].Center = projectile.Center;
        }

        public override bool PreDraw(SpriteBatch sb, Color dColor)
        {
            Rectangle frame = BaseDrawing.GetFrame(projectile.frame, Main.projectileTexture[projectile.type].Width, Main.projectileTexture[projectile.type].Height / 4, 0, 0);
            BaseDrawing.DrawTexture(sb, Main.projectileTexture[projectile.type], 0, projectile.position, projectile.width, projectile.height, projectile.scale, projectile.rotation, projectile.direction, 4, frame, Color.White, true);
            return false;
		}		
	}
}