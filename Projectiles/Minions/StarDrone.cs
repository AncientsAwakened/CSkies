using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using CSkies.Projectiles.Star;
using Microsoft.Xna.Framework.Graphics;

namespace CSkies.Projectiles.Minions
{
    public class StarDrone : ModProjectile
    {
        public override void SetDefaults()
        {
            projectile.netImportant = true;
            projectile.CloneDefaults(533);
            aiType = 533;
            projectile.width = 62;
            projectile.height = 62;
            projectile.friendly = true;
            projectile.minion = true;
            projectile.minionSlots = 1;
            projectile.penetrate = -1;
            projectile.timeLeft = 300;
            projectile.ignoreWater = true;
            projectile.tileCollide = false;
        }

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Star Drone");
            ProjectileID.Sets.MinionSacrificable[projectile.type] = true;
            ProjectileID.Sets.Homing[projectile.type] = true;
            ProjectileID.Sets.MinionTargettingFeature[projectile.type] = true;
        }
        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            if (projectile.velocity.X != oldVelocity.X)
            {
                projectile.velocity.X = oldVelocity.X;
            }
            if (projectile.velocity.Y != oldVelocity.Y)
            {
                projectile.velocity.Y = oldVelocity.Y;
            }
            return false;
        }

        float shoot = 0;
        public override bool PreAI()
        {
            Player player = Main.player[projectile.owner];
            CPlayer modPlayer = player.GetModPlayer<CPlayer>();
            if (player.dead)
            {
                modPlayer.Drone = false;
            }
            if (modPlayer.Drone)
            {
                projectile.timeLeft = 2;
            }
            player.AddBuff(mod.BuffType("Drone"), 3600);

            int n = BaseAI.GetNPC(projectile.Center, -1, 500);
            if (Main.npc[n].friendly)
            {
                n = -1;
            }
            if (n != -1)
            {
                NPC target = Main.npc[n];
                int p = BaseAI.ShootPeriodic(projectile, target.position, target.width, target.height, ModContent.ProjectileType<StarPro>(), ref shoot, 120, projectile.damage, 9, true);
                Main.projectile[p].melee = true;
                Main.projectile[p].minion = true;
            }
            return true;
        }

        public override bool PreDraw(SpriteBatch sb, Color dColor)
        {
            Texture2D Tex = Main.projectileTexture[projectile.type];
            Texture2D Glow = mod.GetTexture("Glowmasks/StarDrone_Glow");

            Rectangle frame = BaseDrawing.GetFrame(projectile.frame, Main.projectileTexture[projectile.type].Width, Main.projectileTexture[projectile.type].Height / 4, 0, 0);
            BaseDrawing.DrawTexture(sb, Tex, 0, projectile.position, projectile.width, projectile.height, projectile.scale, projectile.rotation, projectile.direction, 4, frame, dColor, true);
            BaseDrawing.DrawTexture(sb, Glow, 0, projectile.position, projectile.width, projectile.height, projectile.scale, projectile.rotation, projectile.direction, 4, frame, Color.White, true);
            return false;
        }
    }
}