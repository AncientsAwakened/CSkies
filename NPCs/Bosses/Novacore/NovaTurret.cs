using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using System;
using Microsoft.Xna.Framework.Graphics;

namespace CSkies.NPCs.Bosses.Novacore
{
    public class NovaTurret : ModProjectile
	{
		public override void SetDefaults()
        {
            projectile.width = 80;
            projectile.height = 80;
            projectile.aiStyle = -1;
            projectile.hostile = true;
            projectile.alpha = 255;
            projectile.scale = 1f;
            projectile.ignoreWater = true;
            projectile.tileCollide = false;
            projectile.penetrate = -1;
            Main.projFrames[projectile.type] = 4;
            projectile.timeLeft = 400;
        }
		
		public override Color? GetAlpha(Color lightColor)
		{
			return Color.White;
        }

        public int body = -1;
        public float rotValue = -1f;
        public Vector2 pos;
        int starNumber;

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

            Player player = Main.player[novacore.target];

            pos = novacore.Center;
            
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

            if (projectile.ai[1]++ > 180)
            {
                if (Collision.CanHit(projectile.position, projectile.width, projectile.height, player.Center, player.width, player.height))
                {
                    Vector2 fireTarget = projectile.Center;
                    float rot = BaseUtility.RotationTo(projectile.Center, player.Center);
                    fireTarget = BaseUtility.RotateVector(projectile.Center, fireTarget, rot);
                    BaseAI.FireProjectile(player.Center, fireTarget, ModContent.ProjectileType<NovaBlast>(), projectile.damage / 4, 0f, 4f);
                }
                projectile.ai[1] = 0;
            }
        }

        public override bool PreDraw(SpriteBatch sb, Color lightColor)
        {
			Vector2 vector43 = projectile.position + new Vector2(projectile.width, projectile.height) / 2f + Vector2.UnitY * projectile.gfxOffY - Main.screenPosition;
			Texture2D value107 = Main.projectileTexture[617];
			Color color68 = projectile.GetAlpha(lightColor);
			Vector2 origin17 = new Vector2(value107.Width, value107.Height) / 2f;
            Color color73 = color68 * 0.8f;
            color73.A /= 2;
            Color color74 = Color.Lerp(color68, Color.Black, 0.5f);
            color74.A = color68.A;
            float num299 = 0.95f + (projectile.rotation * 0.75f).ToRotationVector2().Y * 0.1f;
            color74 *= num299;
            float scale15 = 0.6f + projectile.scale * 0.6f * num299;
            sb.Draw(Main.extraTexture[50], vector43, null, color74, 0f - projectile.rotation + 0.35f, origin17, scale15, SpriteEffects.None, 0);
            sb.Draw(Main.extraTexture[50], vector43, null, color68, 0f - projectile.rotation, origin17, projectile.scale, SpriteEffects.None, 0);
            sb.Draw(value107, vector43, null, color73, (0f - projectile.rotation) * 0.7f, origin17, projectile.scale, SpriteEffects.None, 0);
            sb.Draw(Main.extraTexture[50], vector43, null, color68 * 0.8f, projectile.rotation * 0.5f, origin17, projectile.scale * 0.9f, SpriteEffects.None, 0);
            color68.A = 0;
			return false;
        }
    }
}