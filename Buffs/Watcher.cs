using Terraria;
using Terraria.ModLoader;

namespace CSkies.Buffs
{
    public class Watcher : ModBuff
	{
		public override void SetDefaults()
		{
			DisplayName.SetDefault("Watcher");
			Description.SetDefault(@"Summons a watcher to fight for you");
			Main.buffNoSave[Type] = true;
			Main.buffNoTimeDisplay[Type] = true;
		}

		public override void Update(Player player, ref int buffIndex)
		{
            CPlayer modPlayer = player.GetModPlayer<CPlayer>(mod);
			if (player.ownedProjectileCounts[mod.ProjectileType("Watcher")] > 0)
			{
				modPlayer.Watcher = true;
			}
			if (!modPlayer.Watcher)
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