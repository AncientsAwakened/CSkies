using Terraria;
using Terraria.ModLoader;

namespace CSkies
{
	public class Titles : ModPlayer
    {
        public bool text = false;
        public float alphaText = 255f;
        public float alphaText2 = 255f;
        public float alphaText3 = 255f;
        public float alphaText4 = 255f;
        public int BossID = 0;

        public override void ResetEffects()
        {
            text = false;
        }

        public override void PreUpdate()
        {
            if (!CUtils.AnyProjectiles(ModContent.ProjectileType<Title>()))
            {
                alphaText = 255f;
                alphaText2 = 255f;
            }
        }
    }

    public class Title : ModProjectile
    {
        public override string Texture => "CSkies/BlankTex";
        public override void SetDefaults()
        {
            projectile.width = 1;
            projectile.height = 1;
            projectile.penetrate = -1;
            projectile.hostile = false;
            projectile.friendly = false;
            projectile.tileCollide = false;
            projectile.ignoreWater = true;
            projectile.timeLeft = 240;
        }

        public override void AI()
        {
            Player player = Main.player[projectile.owner];
            Titles modPlayer = player.GetModPlayer<Titles>();

            modPlayer.text = true;

            modPlayer.BossID = (int)projectile.ai[0];

            projectile.velocity.X = 0;
            projectile.velocity.Y = 0;

            if (projectile.timeLeft <= 45)
            {
                if (modPlayer.alphaText < 255f)
                {
                    modPlayer.alphaText += 10f;
                    modPlayer.alphaText2 += 10f;
                }
            }
            else
            {
                if (projectile.timeLeft <= 180)
                {
                    modPlayer.alphaText -= 5f;
                }
                if (modPlayer.alphaText > 0f)
                {
                    modPlayer.alphaText2 -= 5f;
                }
            }
        }
    }
}