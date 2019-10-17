using Terraria;
using Terraria.ModLoader;

namespace CSkies.Buffs
{
    public class Drone : ModBuff
	{
		public override void SetDefaults()
		{
			DisplayName.SetDefault("Star Drone");
			Description.SetDefault(@"Summons a star drone to fight for you");
			Main.buffNoSave[Type] = true;
			Main.buffNoTimeDisplay[Type] = true;
		}

		public override void Update(Player player, ref int buffIndex)
		{
            CPlayer modPlayer = player.GetModPlayer<CPlayer>();
			if (player.ownedProjectileCounts[ModContent.ProjectileType<Projectiles.Minions.StarDrone>()] > 0)
			{
				modPlayer.Drone = true;
			}
			if (!modPlayer.Drone)
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