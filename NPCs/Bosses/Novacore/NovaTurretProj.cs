using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Microsoft.Xna.Framework.Graphics;
using Terraria.Graphics.Shaders;
using System;

namespace CSkies.NPCs.Bosses.Novacore
{
    public class NovaTurretProj : ModProjectile
	{
		public override void SetDefaults()
		{
			projectile.width = 40;
			projectile.height = 40;
			projectile.aiStyle = -1;
			projectile.hostile = true;
			projectile.penetrate = 1;
			projectile.aiStyle = -1;
			projectile.timeLeft = 1200;
            Main.projFrames[projectile.type] = 4;
        }
		
		public override Color? GetAlpha(Color lightColor)
		{
			return Color.White;
        }

        public int body = -1;
        public float rotValue = -1f;
        public Vector2 pos;

        public override void AI()
        {
            Lighting.AddLight(projectile.Center, 0.5f, 0f, .5f);

            if (!NPC.AnyNPCs(ModContent.NPCType<Novacore>()))
            {
                projectile.active = false;
            }

            if (body == -1)
            {
                int npcID = BaseAI.GetNPC(projectile.Center, ModContent.NPCType<Novacore>(), 400f, null);
                if (npcID >= 0) body = npcID;
            }
            if (body == -1) return;
            NPC novacore = Main.npc[body];
            if (novacore == null || novacore.life <= 0 || !novacore.active || novacore.type != ModContent.NPCType<Novacore>()) { projectile.active = false; return; }

            pos = novacore.Center;

            int starNumber = ((Novacore)novacore.modNPC).TurretCount();

            if (projectile.localAI[0] == 0)
            {
                starNumber = ((Novacore)novacore.modNPC).TurretCount();
                projectile.localAI[0]++;
                projectile.netUpdate = true;
            }

            projectile.rotation += .06f;
            float dist = ((Novacore)novacore.modNPC).OrbitterDist;

            if (rotValue == -1f) rotValue = projectile.ai[0] % starNumber * ((float)Math.PI * 2f / starNumber);
            rotValue += 0.04f;
            while (rotValue > (float)Math.PI * 2f) rotValue -= (float)Math.PI * 2f;

            projectile.Center = BaseUtility.RotateVector(novacore.Center, novacore.Center + new Vector2(dist, 0f), rotValue);

            if (((Novacore)novacore.modNPC).OrbitterDist == 300)
            {
                projectile.Kill();
            }
        }

        public override void Kill(int timeleft)
        {
            Main.PlaySound(SoundID.Item94, projectile.position);
            int num290 = Main.rand.Next(3, 7);
            for (int num291 = 0; num291 < num290; num291++)
            {
                int num292 = Dust.NewDust(projectile.position, projectile.width, projectile.height, 135, 0f, 0f, 100, default, 2.1f);
                Main.dust[num292].velocity *= 2f;
                Main.dust[num292].noGravity = true;
            }
            Projectile.NewProjectile(projectile.Center, Vector2.Zero, ModContent.ProjectileType<NovaTurret>(), projectile.damage / 2, 0, Main.myPlayer, projectile.ai[0]);
        }

        public float auraPercent = 0f;
        public bool auraDirection = true;

        public override bool PreDraw(SpriteBatch sb, Color lightColor)
        {
            if (auraDirection) { auraPercent += 0.1f; auraDirection = auraPercent < 1f; }
            else { auraPercent -= 0.1f; auraDirection = auraPercent <= 0f; }

            Rectangle frame = BaseDrawing.GetFrame(projectile.frame, Main.projectileTexture[projectile.type].Width, Main.projectileTexture[projectile.type].Height / Main.projFrames[projectile.type], 0, 0);

            BaseDrawing.DrawAura(sb, Main.projectileTexture[projectile.type], 0, projectile.position, projectile.width, projectile.height, auraPercent, 1.5f, 1f, projectile.rotation, projectile.direction, 4, frame, 0f, 0f, null);
            BaseDrawing.DrawTexture(sb, Main.projectileTexture[projectile.type], 0, projectile.position, projectile.width, projectile.height, projectile.scale, projectile.rotation, 0, 4, frame, projectile.GetAlpha(Colors.COLOR_GLOWPULSE), true);
            return false;
        }
    }
}