using Microsoft.Xna.Framework;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CSkies.NPCs.Bosses.FurySoul
{
    public class Furyrang : ModProjectile
	{
        public override void SetDefaults()
        {
            projectile.width = 66;
            projectile.height = 66;
            projectile.aiStyle = -1;
            projectile.timeLeft = 3600;
            projectile.hostile = true;
            projectile.tileCollide = false;
            projectile.damage = 1;
            projectile.penetrate = -1;
            projectile.melee = true;
        }

        public override Color? GetAlpha(Color lightColor)
        {
            return Color.White;
        }

        public int master = -1;
		
		public int dustDelay = 0;

		public override void AI()
		{
            if (master >= 0 && (Main.npc[master] == null || !Main.npc[master].active || Main.npc[master].type != mod.NPCType("FurySoul"))) master = -1;
            if (master == -1)
            {
                master = BaseAI.GetNPC(projectile.Center, mod.NPCType("FurySoul"), -1, null);
                if (master == -1) master = -2;
            }
            if (master == -1) { return; }
			if (master < 0 || !Main.npc[master].active || Main.npc[master].life <= 0) { projectile.Kill(); return; }

            if (Main.rand.Next(2) == 0)
            {
                int dustnumber = Dust.NewDust(projectile.position, projectile.width, projectile.height, DustID.Fire, 0f, 0f, 200, default, 0.5f);
                Main.dust[dustnumber].velocity *= 0.3f;
            }

            BaseAI.AIBoomerang(projectile, ref projectile.ai, Main.npc[master].position, Main.npc[master].width, Main.npc[master].height, true, 40, 35, 8f, .3f, true);
		}
	}
}