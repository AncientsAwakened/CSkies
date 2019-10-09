using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CSkies.NPCs.Bosses.Observer
{
    public class Starbeam : ModProjectile
    {
    	public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Starbeam");
		}

        public override void SetDefaults()
        {
            projectile.width = 10;
            projectile.height = 10;
            projectile.hostile = true;
            projectile.scale = 2f;
            projectile.ignoreWater = true;
            projectile.penetrate = -1;
			projectile.timeLeft = 1000;
        }

		public bool playedSound = false;		
		public int dontDrawDelay = 2;		
        public override void AI()
        {
			if(!playedSound)
			{
				playedSound = true;
				Main.PlaySound(SoundID.Item12, (int)projectile.Center.X, (int)projectile.Center.Y);				
			}
			Effects();
			if(projectile.velocity.Length() < 12f)
			{
				projectile.velocity.X *= 1.05f;
				projectile.velocity.Y *= 1.05f;
			}
        	projectile.rotation = (float)Math.Atan2(projectile.velocity.Y, projectile.velocity.X) + 1.57f;
        }

		public virtual void Effects()
		{
	        Lighting.AddLight(projectile.Center, (255 - projectile.alpha) * 0.05f / 255f, (255 - projectile.alpha) * 0.5f / 255f, (255 - projectile.alpha) * 0.5f / 255f);		
		}

		public override Color? GetAlpha(Color lightColor)
		{
			return Color.White;
		}

        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
        {
			dontDrawDelay = Math.Max(0, dontDrawDelay - 1);
			return dontDrawDelay == 0;
		}
    }
}