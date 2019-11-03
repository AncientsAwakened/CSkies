using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.Audio;

namespace CSkies.Projectiles.Void
{
    public class VoidJavelin : Javelin
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Void Javelin");
        }

        public override void SetDefaults()
        {
            projectile.width = 10;
            projectile.height = 10;
            projectile.aiStyle = -1;
            projectile.friendly = true;
            projectile.melee = true;
            projectile.penetrate = 1;
            projectile.GetGlobalProjectile<Buffs.ImplaingProjectile>().CanImpale = true;
            projectile.GetGlobalProjectile<Buffs.ImplaingProjectile>().damagePerImpaler = 20;
            maxStickingJavelins = 12;
            rotationOffset = (float)Math.PI / 4;
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            Texture2D texture = Main.projectileTexture[projectile.type];
            spriteBatch.Draw(texture, new Vector2(projectile.Center.X - Main.screenPosition.X, projectile.Center.Y - Main.screenPosition.Y + 2),
                        new Rectangle(0, 0, texture.Width, texture.Height), Color.White, projectile.rotation,
                        new Vector2(projectile.width * 0.5f, projectile.height * 0.5f), 1f, SpriteEffects.None, 0f);
            return false;
        }

        public override void Kill(int timeLeft)
        {
            if (projectile.ai[0] == 1f)
            {
                Main.PlaySound(new LegacySoundStyle(2, 89, SoundType.Sound));
                int p = Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, projectile.velocity.X, projectile.velocity.Y, mod.ProjectileType("VoidBoom"), projectile.damage, projectile.knockBack, projectile.owner, 0f, 0f);
                Main.projectile[p].Center = projectile.Center;
            }
        }
    }
}
