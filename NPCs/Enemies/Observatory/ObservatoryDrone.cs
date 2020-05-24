using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using CSkies.Dusts;

namespace CSkies.NPCs.Enemies.Observatory
{
    public class ObservatoryDrone : ModNPC
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Observatory Drone");
		}

		public override void SetDefaults()
		{
            npc.width = 38;
            npc.height = 38;
            npc.value = 0;
            npc.npcSlots = 1;
            npc.aiStyle = -1;
            npc.lifeMax = 1500;
            npc.defense = 30;
            npc.damage = 50;
			npc.HitSound = SoundID.NPCHit4;
			npc.DeathSound = SoundID.NPCDeath14;
            npc.knockBackResist = 0.3f;
			npc.noGravity = true;
			npc.noTileCollide = true;
		}

		public override void HitEffect(int hitDirection, double damage)
		{		
			bool isDead = npc.life <= 0;
			for (int m = 0; m < (isDead ? 25 : 5); m++)
			{
				int dustType = ModContent.DustType<StarDust>();
				Dust.NewDust(npc.position, npc.width, npc.height, dustType, npc.velocity.X * 0.2f, npc.velocity.Y * 0.2f, 100, Color.White, isDead ? 2f : 1.1f);
			}
		}

		float shootAI = 0;
		public override void AI()
		{
		    BaseAI.AISkull(npc, ref npc.ai, false, 6f, 350f, 0.6f, 0.15f);
			Player player = Main.player[npc.target];
			bool playerActive = player != null && player.active && !player.dead;
            if (shootAI < 60)
            {
                BaseAI.LookAt(player.Center, npc, 3, 0, .1f, false);
            }
            if (Main.netMode != NetmodeID.MultiplayerClient && playerActive)
			{
				shootAI++;
				if(shootAI >= 90)
				{
					shootAI = 0;
                    int projType = mod.ProjType("NeutralizerP");

                    if (Collision.CanHit(npc.position, npc.width, npc.height, player.position, player.width, player.height))
                    {
                        Projectile.NewProjectile(npc.Center.X, npc.Center.Y, 0f, 0f, projType, (int)(npc.damage * 0.25f), 3f, Main.myPlayer, npc.whoAmI);

                    }
                }
			}
		}

        public override bool PreDraw(SpriteBatch spritebatch, Color dColor)
        {
            Texture2D glowTex = mod.GetTexture("Glowmasks/ObservatoryDrone_Glow");
            BaseDrawing.DrawTexture(spritebatch, Main.npcTexture[npc.type], 0, npc, dColor);
            BaseDrawing.DrawTexture(spritebatch, glowTex, 0, npc, Colors.COLOR_GLOWPULSE);
            return false;
        }
    }
}