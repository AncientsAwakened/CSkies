using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using CSkies.Projectiles.Star;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace CSkies.Projectiles.Minions
{
    public class StarDrone : ModProjectile
    {
        public override void SetDefaults()
        {
            projectile.netImportant = true;
            projectile.aiStyle = -1;
            projectile.width = 36;
            projectile.height = 36;
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
            Main.projFrames[projectile.type] = 4;
        }

        public float maxDistToAttack = 360f;
        public Entity target = null;

        public override void AI()
        {
            projectile.frameCounter++;
            if (projectile.frameCounter >= 5)
            {
                projectile.frameCounter = 0;
                projectile.frame++;
            }
            if (projectile.frame > 3)
            {
                projectile.frame = 0;
            }

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

            Target();

            BaseAI.AIMinionFlier(projectile, ref projectile.ai, player, false, false, false, 40, 40, 400, 800, .15f, 4.5f, 5f, !CanShoot(target), false, (proj, owner) => { return (target == player ? null : target); }, Shoot);
            projectile.position -= (player.oldPosition - player.position);
        }

        public bool CanShoot(Entity target)
        {
            return target != null && target is NPC && BaseUtility.CanHit(projectile.Hitbox, new Rectangle((int)target.Center.X, (int)target.Center.Y, 1, 1)) && Vector2.Distance(projectile.Center, target.Center) < 350;
        }

        public bool Shoot(Entity proj, Entity owner, Entity target)
        {
            if (CanShoot(target))
            {
                if (Main.myPlayer == projectile.owner)
                {
                    projectile.localAI[0]--;
                    if (projectile.localAI[0] <= 0)
                    {
                        projectile.localAI[0] = 30;
                        if (Main.rand.Next(4) != 0)
                        {
                            Main.PlaySound(SoundID.Item20, projectile.position);
                            Vector2 velocity = BaseUtility.RotateVector(default, new Vector2(5f, 0f), BaseUtility.RotationTo(projectile.Center, target.Center));
                            int projID = Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, 0f, 0f, ModContent.ProjectileType<MinionStar>(), projectile.damage, 0f, projectile.owner);
                            Main.projectile[projID].melee = false;
                            Main.projectile[projID].minion = true;
                            Main.projectile[projID].velocity = velocity;
                            Main.projectile[projID].velocity = velocity;
                            Main.projectile[projID].netUpdate = true;
                        }
                        else
                        {
                            Main.PlaySound(SoundID.Item20, projectile.position);
                            Projectile.NewProjectile(projectile.Center, new Vector2(8, 8), ModContent.ProjectileType<Static>(), projectile.damage, 0f, Main.myPlayer, 0, projectile.whoAmI);
                            Projectile.NewProjectile(projectile.Center, new Vector2(-8, 8), ModContent.ProjectileType<Static>(), projectile.damage, 0f, Main.myPlayer, 0, projectile.whoAmI);
                            Projectile.NewProjectile(projectile.Center, new Vector2(8, -8), ModContent.ProjectileType<Static>(), projectile.damage, 0f, Main.myPlayer, 0, projectile.whoAmI);
                            Projectile.NewProjectile(projectile.Center, new Vector2(-8, -8), ModContent.ProjectileType<Static>(), projectile.damage, 0f, Main.myPlayer, 0, projectile.whoAmI);
                            Projectile.NewProjectile(projectile.Center, new Vector2(8, 0), ModContent.ProjectileType<Static>(), projectile.damage, 0f, Main.myPlayer, 0, projectile.whoAmI);
                            Projectile.NewProjectile(projectile.Center, new Vector2(-8, 0), ModContent.ProjectileType<Static>(), projectile.damage, 0f, Main.myPlayer, 0, projectile.whoAmI);
                            Projectile.NewProjectile(projectile.Center, new Vector2(0, -8), ModContent.ProjectileType<Static>(), projectile.damage, 0f, Main.myPlayer, 0, projectile.whoAmI);
                            Projectile.NewProjectile(projectile.Center, new Vector2(0, 8), ModContent.ProjectileType<Static>(), projectile.damage, 0f, Main.myPlayer, 0, projectile.whoAmI);
                        }
                    }
                }
                return true;
            }
            projectile.localAI[0] = 0;
            return false;
        }

        public void Target()
        {
            Vector2 startPos = Main.player[projectile.owner].Center;
            if (target != null && target != Main.player[projectile.owner] && !CanTarget(target, startPos))
            {
                target = null;
            }
            if (target == null || target == Main.player[projectile.owner])
            {
                int[] npcs = BaseAI.GetNPCs(startPos, -1, default, maxDistToAttack);
                float prevDist = maxDistToAttack;
                foreach (int i in npcs)
                {
                    NPC npc = Main.npc[i];
                    float dist = Vector2.Distance(startPos, npc.Center);
                    if (CanTarget(npc, startPos) && dist < prevDist) { target = npc; prevDist = dist; }
                }
            }
            if (target == null) { target = Main.player[projectile.owner]; }
        }

        public bool CanTarget(Entity codable, Vector2 startPos)
        {
            if (codable is NPC npc)
            {
                return npc.active && npc.life > 0 && !npc.friendly && !npc.dontTakeDamage && npc.lifeMax > 5 && Vector2.Distance(startPos, npc.Center) < maxDistToAttack && BaseUtility.CanHit(projectile.Hitbox, npc.Hitbox);
            }
            return false;
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