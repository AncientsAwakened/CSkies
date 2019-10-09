using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using System;

namespace CSkies.Buffs
{
    public class Sucked : ModBuff
	{
		public override void SetDefaults()
		{
			DisplayName.SetDefault("Event Horizon");
			Description.SetDefault("'There is no escape.'");
			Main.debuff[Type] = true;
			Main.buffNoSave[Type] = true;
		}

		public override void Update(Player player, ref int buffIndex)
		{
            int N = BaseAI.GetNPC(player.Center, mod.NPCType< NPCs.Bosses.Void.Void>(), -1);
            if (N == -1 || Vector2.Distance(player.Center, Main.npc[N].Center) < 700)
            {
                player.DelBuff(buffIndex);
                buffIndex--;
            }
            else
            {
                player.buffTime[buffIndex] = 2;
            }
            NPC Void = Main.npc[N];
            float num3 = 24f;
            Vector2 vector = new Vector2(player.position.X + player.width / 4, player.position.Y + player.height / 4);
            float num4 = Void.Center.X - vector.X;
            float num5 = Void.Center.Y - vector.Y;
            float num6 = (float)Math.Sqrt(num4 * num4 + num5 * num5);
            num6 = num3 / num6;
            num4 *= num6;
            num5 *= num6;
            int num7 = 10;
            player.velocity.X = (player.velocity.X * (num7 - 1) + num4) / num7;
            player.velocity.Y = (player.velocity.Y * (num7 - 1) + num5) / num7;
        }
	}
}
