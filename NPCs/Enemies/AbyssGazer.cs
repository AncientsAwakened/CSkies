using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CSkies.NPCs.Enemies
{
	public class AbyssGazer : ModNPC
	{
		public override void SetStaticDefaults()
		{
            DisplayName.SetDefault("Abyss Gazer");
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
        }

        public override void NPCLoot()
		{
            if (!NPC.AnyNPCs(ModContent.NPCType<Bosses.Void.Void>()))
            {
                BaseAI.DropItem(npc, mod.ItemType("VoidLens"), 1, 1, 50, true);
            }
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
					int dustID = Dust.NewDust(npc.position, npc.width, npc.height, 17, npc.velocity.X * 0.2f, npc.velocity.Y * 0.2f, 0, Color.White, 1f);
                    Main.dust[dustID].velocity = new Vector2(MathHelper.Lerp(-1f, 1f, (float)Main.rand.NextDouble()), MathHelper.Lerp(-1f, 1f, (float)Main.rand.NextDouble()));
				}
			}else
			{
				for (int m = 0; m < 5; m++)
				{
					Dust.NewDust(npc.position, npc.width, npc.height, 17, npc.velocity.X * 0.2f, npc.velocity.Y * 0.2f, 0, Color.White, 1.1f);
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
            BaseAI.AISkull(npc, ref npc.ai, true, 6, 350, .015f, .02f);
            if (npc.ai[1] <= 400f)
            {
                BaseAI.ShootPeriodic(npc, player.position, player.width, player.height, ModContent.ProjectileType<Bosses.ObserverVoid.BlackHoleProj>(), ref npc.ai[2], 100, npc.damage / 2, 7, true);
            }
			if (npc.ai[0] < 200) { BaseAI.LookAt(player.Center, npc, 1); } else { if (npc.timeLeft > 10) { npc.timeLeft = 10; } npc.spriteDirection = -npc.direction; }
			npc.rotation = 0;
        }

        public float auraPercent = 0f;
        public bool auraDirection = true;

        public override bool PreDraw(SpriteBatch sb, Color dColor)
		{
            Texture2D Tex = Main.npcTexture[npc.type];

            if (auraDirection) { auraPercent += 0.1f; auraDirection = auraPercent < 1f; }
            else { auraPercent -= 0.1f; auraDirection = auraPercent <= 0f; }
            BaseDrawing.DrawAura(sb, Tex, 0, npc, auraPercent, 2f, 0f, 0f, npc.GetAlpha(Colors.COLOR_GLOWPULSE));
			return false;
		}
	}
}