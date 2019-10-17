using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using CSkies.NPCs.Bosses.Heartcore;

namespace CSkies.NPCs.Bosses.FurySoul
{
	public class FuryMinion : ModNPC
	{
		public override void SetStaticDefaults()
		{
            DisplayName.SetDefault("Fury Spirit");
            Main.npcFrameCount[npc.type] = 4;
		}		

        public override void SetDefaults()
        {
            npc.width = 42;
            npc.height = 38;
            npc.value = BaseUtility.CalcValue(0, 0, 2, 0);
            npc.npcSlots = 1;
            npc.aiStyle = -1;
            npc.lifeMax = 100;
            npc.defense = 5;
            npc.damage = 30;
            npc.HitSound = SoundID.NPCHit49;
            npc.DeathSound = SoundID.NPCDeath51;
            npc.knockBackResist = 0.7f;
            npc.alpha = 255;
            npc.noTileCollide = true;
        }

        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            if (spawnInfo.player.GetModPlayer<CPlayer>().ZoneVoid)
            {
                return .05f;
            }
            return 0;
        }

        public override void HitEffect(int hitDirection, double damage)
		{
			if (Main.netMode == 2) { return; }
			if (npc.life <= 0)
			{
				for (int m = 0; m < 20; m++)
				{
					int dustID = Dust.NewDust(npc.position, npc.width, npc.height, DustID.Fire, npc.velocity.X * 0.2f, npc.velocity.Y * 0.2f, 0, Color.White, 2f);
                    Main.dust[dustID].velocity = new Vector2(MathHelper.Lerp(-1f, 1f, (float)Main.rand.NextDouble()), MathHelper.Lerp(-1f, 1f, (float)Main.rand.NextDouble()));
				}
			}
            else
			{
				for (int m = 0; m < 5; m++)
				{
					Dust.NewDust(npc.position, npc.width, npc.height, DustID.SolarFlare, npc.velocity.X * 0.2f, npc.velocity.Y * 0.2f, 0, Color.White, 1.1f);
				}
			}
		}

        int frame = 0;
        public override void FindFrame(int frameHeight)
		{
            if (npc.frameCounter++ > 7)
            {
                frame++;
                npc.frameCounter = 0;
                if (frame > 3)
                {
                    frame = 0;
                }
            }
			npc.frame = BaseDrawing.GetFrame(frame, 32, 42, 0, 0);
		}

        public override void AI()
		{
            if (npc.alpha > 0)
            {
                npc.alpha -= 5;
            }
            else
            {
                npc.alpha = 0;
            }
			npc.noGravity = true;
			npc.TargetClosest(true);
			Player player = Main.player[npc.target];
			for (int m = npc.oldPos.Length - 1; m > 0; m--)
			{
				npc.oldPos[m] = npc.oldPos[m - 1];
			}
			npc.oldPos[0] = npc.position;
            BaseAI.AISkull(npc, ref npc.ai, true, 10, 350, .03f, .04f);
            if (npc.ai[1] <= 400f)
            {
                BaseAI.ShootPeriodic(npc, player.position, player.width, player.height, ModContent.ProjectileType<Fireshot>(), ref npc.ai[2], Main.rand.Next(80, 130), npc.damage / 2, 7, true);
            }
			npc.rotation = 0;
        }

        public override bool PreDraw(SpriteBatch sb, Color dColor)
		{
            Texture2D Tex = Main.npcTexture[npc.type];

            Rectangle f = BaseDrawing.GetFrame(frame, Main.npcTexture[npc.type].Width, Main.npcTexture[npc.type].Height / Main.npcFrameCount[npc.type], 0, 0);

            BaseDrawing.DrawTexture(sb, Tex, 0, npc.position, npc.width, npc.height, npc.scale, npc.rotation, 0, 4, f, npc.GetAlpha(Colors.COLOR_GLOWPULSE), true);
            return false;
		}
	}
}