using Terraria;
using Terraria.ModLoader;

namespace CSkies.Buffs
{
    public class Heartburn : ModBuff
	{
		public override void SetDefaults()
		{
            DisplayName.SetDefault("Heartburn");
            Description.SetDefault("Your chest is burning, preventing you from healing above 80% of your max life");
			Main.debuff[Type] = true;
			Main.pvpBuff[Type] = false;
			Main.buffNoSave[Type] = true;
			longerExpertDebuff = true;
		}

        public override void Update(Player player, ref int buffIndex)
        {
            player.GetModPlayer<CPlayer>().Heartburn = true;
        }
	}
}
