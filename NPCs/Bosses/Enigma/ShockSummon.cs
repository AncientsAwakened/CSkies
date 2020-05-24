using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CSkies.NPCs.Bosses.Enigma
{
    public class ShockSummon : ModProjectile
	{
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Enigma Portal");
            Main.projFrames[projectile.type] = 4;
        }

		public override void SetDefaults()
		{
            projectile.width = 46;
            projectile.height = 46;
            projectile.aiStyle = -1;
            projectile.ignoreWater = true;
            projectile.tileCollide = false;
            projectile.timeLeft = 300;
        }

        public override void AI()
        {
            if (projectile.frameCounter++ > 5)
            {
                projectile.frameCounter = 0;
                if (projectile.frame++ > 2)
                {
                    projectile.frame = 0;
                }
            }
            projectile.velocity = Vector2.Zero;
            Lighting.AddLight(projectile.Center, new Vector3(0.3f, 0.4f, 0.9f) * projectile.Opacity);

            if (projectile.ai[0]++ == 180 && Main.netMode != NetmodeID.MultiplayerClient)
            {
                Main.PlaySound(mod.GetLegacySoundSlot(SoundType.Custom, "Sounds/Sounds/Shock"), projectile.position);
                Projectile.NewProjectile(projectile.position, new Vector2(0, 10), mod.ProjectileType("Shocking"), projectile.damage / 4, 4, Main.myPlayer);
            }
        }
    }
}
