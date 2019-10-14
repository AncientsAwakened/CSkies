using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

using System.IO;
using Terraria.Audio;

namespace CSkies.NPCs.Bosses.Observer
{
    public class Star : ModProjectile
	{
        public int damage = 0;

		public override void SetStaticDefaults()
		{
            DisplayName.SetDefault("Star");
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
            if (Main.expertMode)
            {
                damage = projectile.damage / 4;
            }
            else
            {
                damage = projectile.damage / 2;
            }

            if (!NPC.AnyNPCs(ModContent.NPCType<Observer>()))
            {
                projectile.active = false;
            }

            if (body == -1)
            {
                int npcID = BaseAI.GetNPC(projectile.Center, ModContent.NPCType<Observer>(), 400f, null);
                if (npcID >= 0) body = npcID;
            }
            if (body == -1) return;
            NPC observer = Main.npc[body];
            if (observer == null || observer.life <= 0 || !observer.active || observer.type != ModContent.NPCType<Observer>()) { projectile.active = false; return; }

            Player player = Main.player[observer.target];

            pos = observer.Center;

            int starNumber = ((Observer)observer.modNPC).StarCount;

            projectile.rotation += .1f;

            if (((Observer)observer.modNPC).internalAI[0] == 0)
            {
                projectile.timeLeft = 120;
                float dist = ((Observer)observer.modNPC).internalAI[1];

                if (rotValue == -1f) rotValue = projectile.ai[0] % starNumber * ((float)Math.PI * 2f / starNumber);
                rotValue += 0.04f;
                while (rotValue > (float)Math.PI * 2f) rotValue -= (float)Math.PI * 2f;

                projectile.Center = BaseUtility.RotateVector(observer.Center, observer.Center + new Vector2(dist, 0f), rotValue);

                if (projectile.ai[1]++ > 180)
                {
                    if (Collision.CanHit(projectile.position, projectile.width, projectile.height, player.Center, player.width, player.height))
                    {
                        Vector2 fireTarget = projectile.Center;
                        float rot = BaseUtility.RotationTo(projectile.Center, player.Center);
                        fireTarget = BaseUtility.RotateVector(projectile.Center, fireTarget, rot);
                        BaseAI.FireProjectile(player.Center, fireTarget, mod.ProjType("StarProj"), damage, 0f, 4f);
                    }
                    projectile.ai[1] = 0;
                }
            }
            else if (((Observer)observer.modNPC).internalAI[0] == 1)
            {
                projectile.ai[1] = 0;
                projectile.tileCollide = true;
                const int homingDelay = 0;
                const float desiredFlySpeedInPixelsPerFrame = 10;
                const float amountOfFramesToLerpBy = 20;

                internalAI[0]++;
                if (internalAI[0] > homingDelay)
                {
                    internalAI[0] = homingDelay;

                    int foundTarget = HomeOnTarget();
                    if (foundTarget != -1)
                    {
                        Player target = Main.player[foundTarget];
                        Vector2 desiredVelocity = projectile.DirectionTo(target.Center) * desiredFlySpeedInPixelsPerFrame;
                        projectile.velocity = Vector2.Lerp(projectile.velocity, desiredVelocity, 1f / amountOfFramesToLerpBy);
                    }
                }
            }
        }

        private int HomeOnTarget()
        {
            const float homingMaximumRangeInPixels = 10000000;

            int selectedTarget = -1;
            for (int i = 0; i < Main.maxPlayers; i++)
            {
                Player target = Main.player[i];
                if (target.active)
                {
                    float distance = projectile.Distance(target.Center);
                    if (distance <= homingMaximumRangeInPixels && (selectedTarget == -1 || projectile.Distance(Main.player[selectedTarget].Center) > distance))
                    {
                        selectedTarget = i;
                    }
                }
            }

            return selectedTarget;
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            Main.PlaySound(new LegacySoundStyle(2, 89, Terraria.Audio.SoundType.Sound));
            Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y - 20, projectile.velocity.X, projectile.velocity.Y, mod.ProjectileType("Starshock"), projectile.damage, projectile.knockBack, projectile.owner, 0f, 0f);
            return true;
        }

        public override void Kill(int timeLeft)
        {
            int dustID = Dust.NewDust(projectile.position, projectile.width, projectile.height, 17, projectile.velocity.X * 0.2f, projectile.velocity.Y * 0.2f, 10, Color.White, 1f);
            Main.dust[dustID].velocity = new Vector2(MathHelper.Lerp(-1f, 1f, (float)Main.rand.NextDouble()), MathHelper.Lerp(-1f, 1f, (float)Main.rand.NextDouble()));
        }

        public override bool PreDraw(SpriteBatch sb, Color dColor)
        {
            BaseDrawing.DrawAfterimage(sb, Main.projectileTexture[projectile.type], 0, projectile, 2.5f, 1, 3, true, 0f, 0f, projectile.GetAlpha(Colors.COLOR_GLOWPULSE));
            BaseDrawing.DrawTexture(sb, Main.projectileTexture[projectile.type], 0, projectile, projectile.GetAlpha(Colors.COLOR_GLOWPULSE), true);
            return false;
		}		
	}
}