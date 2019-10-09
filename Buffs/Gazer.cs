using Terraria;
using Terraria.ModLoader;

namespace CSkies.Buffs
{
    public class Gazer : ModBuff
	{
		public override void SetDefaults()
		{
			DisplayName.SetDefault("Gazer");
			Description.SetDefault(@"Summons an abyss gazer to fight for you");
			Main.buffNoSave[Type] = true;
			Main.buffNoTimeDisplay[Type] = true;
		}

		public override void Update(Player player, ref int buffIndex)
		{
            CPlayer modPlayer = player.GetModPlayer<CPlayer>(mod);
			if (player.ownedProjectileCounts[mod.ProjectileType("Gazer")] > 0)
			{
				modPlayer.Gazer = true;
			}
			if (!modPlayer.Gazer)
			{
				player.DelBuff(buffIndex);
				buffIndex--;
			}
			else
			{
				player.buffTime[buffIndex] = 18000;
			}
		}
	}
}