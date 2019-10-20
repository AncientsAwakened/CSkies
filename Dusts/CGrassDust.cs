using Terraria;
using Terraria.ModLoader;

namespace AAMod.Dusts
{
    public class CGrassDust : ModDust
	{
        public override void OnSpawn(Dust dust)
        {
            dust.noGravity = false;
        }
    }
}