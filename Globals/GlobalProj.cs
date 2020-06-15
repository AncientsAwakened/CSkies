using Terraria;
using Terraria.ModLoader;

namespace CSkies
{
	public class GlobalProj : GlobalProjectile
    {
        public override void OnHitPlayer(Projectile projectile, Player target, int damage, bool crit)
        {
            if (target.GetModPlayer<CPlayer>().StarsteelBonus == 1 && Main.rand.Next(10) == 0)
            {

            }
        }
    }
}