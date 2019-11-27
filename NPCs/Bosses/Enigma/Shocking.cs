using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CSkies.NPCs.Bosses.Enigma
{
    internal class Shocking : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Shocking Orthrus Breath");
        }

        public override void SetDefaults()
        {
            projectile.width = 30;
            projectile.height = 30;
            projectile.hostile = true;
            projectile.friendly = false;
            projectile.ignoreWater = true;
            projectile.penetrate = -1;
            projectile.alpha = 15;
            projectile.aiStyle = -1;
			projectile.timeLeft = 40; //timed it so all the frames pass through before it dies
			Main.projFrames[projectile.type] = 14;
        }

        public override Color? GetAlpha(Color lightColor)
        {
            return Color.White;
        }
		
		int Summon = -1;

        public override bool PreAI()
        {
			if(Summon == -1)
			{
				int projID = BaseAI.GetProjectile(projectile.Center, mod.ProjectileType("ShockSummon"), Main.myPlayer, 500f, null);	
				if(projID >= 0) Summon = projID;
			}
			if(Summon == -1) return false;				
			Projectile Vortex = Main.projectile[Summon];
			if(Vortex == null || !Vortex.active || Vortex.type != mod.ProjectileType("ShockSummon")){ projectile.Kill(); return false; }

			//Fun fact: this technique is what the shadowbeam staff does!
			if(Main.netMode != 1 && projectile.timeLeft % 3 == 0) //so it doesn't do this every tick, which would be laggy
			{
				projectile.Center = Vortex.Center; //reset to start chain movement
				for(int m = 0; m < 18; m++) //this + velocity ends up ~540 in length, same as the texture
				{
					projectile.Center += projectile.velocity * 30f; //move to new point in the chain
					projectile.Damage(); //inflcit damage
				}
			}
			projectile.Center = Vortex.Center;
			BaseAI.LookAt(projectile.Center + projectile.velocity, projectile.Center, ref projectile.rotation, ref projectile.spriteDirection, 2, 0f, 0.1f, false);

            if (++projectile.frameCounter >= 3)
            {
                projectile.frameCounter = 0;
                if (++projectile.frame >= 14)
                {
					projectile.frame = 14;
                }
            }
			return false; //so it doesn't add velocity and try to move
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            target.AddBuff(BuffID.Electrified, 300);
        }

        public override bool PreDraw(SpriteBatch sb, Color dColor)
        {
            Vector2 drawOffset = BaseUtility.RotateVector(Vector2.Zero, new Vector2(540 * 0.5f, 50), projectile.rotation);
            BaseDrawing.DrawTexture(sb, Main.projectileTexture[projectile.type], 0, projectile.position + drawOffset, projectile.width, projectile.height, projectile.scale, projectile.rotation, projectile.spriteDirection, Main.projFrames[projectile.type], new Rectangle(0, projectile.frame * 50, 570, 50), GetAlpha(Color.White), true, default);			
			return false;
		}
    }
}