using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using CSkies.NPCs.Enemies;

namespace CSkies.NPCs.Other
{
    [AutoloadBossHead]
	public class AbyssVoid : ModNPC
	{
        public override string Texture => "CSkies/NPCs/Bosses/ObserverVoid/DarkVortex";
        public override string BossHeadTexture => "CSkies/NPCs/Other/AbyssVoid_Head_Boss";
        public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Abyss Gate");
        }

		public override void SetDefaults()
		{
			npc.aiStyle = -1;
			npc.lifeMax = 20000;
			npc.damage = 0;
			npc.defense = 20;
			npc.knockBackResist = 0f;
            npc.width = 264;
            npc.height = 264;
            npc.HitSound = SoundID.NPCHit1;
			npc.DeathSound = SoundID.NPCDeath1;
			npc.noGravity = true;
			npc.noTileCollide = true;
			npc.alpha = 0;
			npc.dontTakeDamage = true;
			npc.boss = false;
            npc.npcSlots = 0;
        }

		public override bool CheckActive()
		{
			return false;
		}		

		public override void AI()
		{
            npc.rotation += .01f;
            npc.timeLeft = 10;
            if (Main.netMode != 1 && npc.ai[1]++ > 450)
            {
                npc.ai[1] = 0;
                for (int a = 0; a < Main.rand.Next(4); a++)
                {
                    int n = NPC.NewNPC((int)npc.Center.X, (int)npc.Center.Y, ModContent.NPCType<AbyssGazer>());
                    Main.npc[n].Center = npc.Center;
                }
            }
        }

        public Color GetGlowAlpha()
        {
            return Colors.COLOR_GLOWPULSE;
        }

        public override bool PreDraw(SpriteBatch spritebatch, Color drawColor)
        {
            Texture2D T = Main.npcTexture[npc.type];
            BaseDrawing.DrawTexture(spritebatch, T, 0, npc.position, npc.width, npc.height, npc.scale, npc.rotation, 0, 1, npc.frame, GetGlowAlpha(), true);
            return false;
        }

        public override bool StrikeNPC(ref double damage, int defense, ref float knockback, int hitDirection, ref bool crit)
        {
            damage = 0;
            return false;
        }
    }

    public class VoidHandler : ModWorld
    {
        public override void PostUpdate()
        {
            bool anyVoidExist = NPC.AnyNPCs(mod.NPCType("AbyssVoid"));
            if (Main.netMode != 1 && NPC.downedMoonlord && !anyVoidExist)
            {
                SpawnVoid();
            }
        }

        public void SpawnVoid()
        {
            int VoidHeight = 140;
            int boundary = Main.maxTilesX / 15;

            Point spawnTilePos = new Point(Main.rand.Next(boundary, Main.maxTilesX - boundary), VoidHeight);				
			Vector2 spawnPos = new Vector2(spawnTilePos.X * 16, spawnTilePos.Y * 16);
			bool anyVoidExist = NPC.AnyNPCs(mod.NPCType("AbyssVoid"));			
			if (!anyVoidExist)
			{
                int whoAmI = NPC.NewNPC((int)spawnPos.X, (int)spawnPos.Y, ModContent.NPCType<AbyssVoid>());			
				if (Main.netMode == 2 && whoAmI != -1 && whoAmI < 200)
				{					
					NetMessage.SendData(MessageID.SyncNPC, number: whoAmI);
				}			
			}
        }
    }
}
