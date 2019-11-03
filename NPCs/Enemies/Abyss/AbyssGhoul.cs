using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Linq;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;


namespace CSkies.NPCs.Enemies.Abyss
{
    public class AbyssGhoul : ModNPC
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Eldritch Ghoul");
            Main.npcFrameCount[npc.type] = 8;
        }
        public override void SetDefaults()
        {
            npc.width = 36;
            npc.height = 48;
            npc.damage = 75;
            npc.friendly = false;
            npc.defense = 34;
            npc.lifeMax = 1000;
            npc.HitSound = SoundID.NPCHit37;
            npc.DeathSound = SoundID.NPCDeath40;
            npc.value = 90f;
            npc.knockBackResist = 0.3f;
            npc.aiStyle = 3;
            aiType = NPCID.DesertGhoul;
            animationType = NPCID.DesertGhoul;
        }

        public override Color? GetAlpha(Color drawColor)
        {
            return Colors.COLOR_GLOWPULSE;
        }

        public override void HitEffect(int hitDirection, double damage)
        {
            if (npc.life <= 0)
            {
                for (int d = 0; d < 3; d++)
                {
                    Dust.NewDust(npc.position, npc.width, npc.height, ModContent.DustType<Dusts.VoidDust>());
                }
            }
        }
    }
}