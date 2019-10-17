using System;
using Terraria.Graphics.Shaders;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CSkies.Projectiles.Minions
{
    public class HeartRune : ModProjectile
    {
    	public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Heart Rune");
            ProjectileID.Sets.MinionSacrificable[projectile.type] = true;
            ProjectileID.Sets.Homing[projectile.type] = true;
            ProjectileID.Sets.MinionTargettingFeature[projectile.type] = true;
        }
    	
        public override void SetDefaults()
        {
            projectile.width = 18;
            projectile.height = 28;
            projectile.netImportant = true;
            projectile.friendly = true;
            projectile.ignoreWater = true;
            projectile.minionSlots = 1;
            projectile.timeLeft = 18000;
            projectile.penetrate = -1;
            projectile.tileCollide = false;
            projectile.timeLeft *= 5;
            projectile.minion = true;
        }

        public float maxDistToAttack = 360f;
        public Entity target = null;

        public override void AI()
        {
            projectile.rotation += .1f;
            if (scale < 1)
            {
                scale += .05f;
            }
            else
            {
                scale = 1;
            }
            Player player = Main.player[projectile.owner];
            CPlayer modPlayer = player.GetModPlayer<CPlayer>();
            if (player.dead || !player.HasBuff(ModContent.BuffType<Buffs.Rune>()))
            {
                projectile.Kill();
                modPlayer.Rune = false;
            }
            if (modPlayer.Rune)
            {
                projectile.timeLeft = 2;
            }

            Target();
            BaseAI.AIMinionFlier(projectile, ref projectile.ai, player, false, false, false, 70, 70, 400, 800, .2f, 6f, 6f, !CanShoot(target), false, (proj, owner) => { return (target == player ? null : target); }, Shoot);
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
                            int projID = Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, 0f, 0f, ModContent.ProjectileType<Heart.FirePro>(), projectile.damage, 0f, projectile.owner);
                            Main.projectile[projID].melee = false;
                            Main.projectile[projID].minion = true;
                            Main.projectile[projID].velocity = velocity;
                            Main.projectile[projID].velocity = velocity;
                            Main.projectile[projID].netUpdate = true;
                        }
                        else
                        {
                            Main.PlaySound(SoundID.Item20, projectile.position);
                            Projectile.NewProjectile(projectile.Center, new Vector2(8, 8), ModContent.ProjectileType<RuneWave>(), projectile.damage, 0f, Main.myPlayer, 0, projectile.whoAmI);
                            Projectile.NewProjectile(projectile.Center, new Vector2(-8, 8), ModContent.ProjectileType<RuneWave>(), projectile.damage, 0f, Main.myPlayer, 0, projectile.whoAmI);
                            Projectile.NewProjectile(projectile.Center, new Vector2(8, -8), ModContent.ProjectileType<RuneWave>(), projectile.damage, 0f, Main.myPlayer, 0, projectile.whoAmI);
                            Projectile.NewProjectile(projectile.Center, new Vector2(-8, -8), ModContent.ProjectileType<RuneWave>(), projectile.damage, 0f, Main.myPlayer, 0, projectile.whoAmI);
                            Projectile.NewProjectile(projectile.Center, new Vector2(8, 0), ModContent.ProjectileType<RuneWave>(), projectile.damage, 0f, Main.myPlayer, 0, projectile.whoAmI);
                            Projectile.NewProjectile(projectile.Center, new Vector2(-8, 0), ModContent.ProjectileType<RuneWave>(), projectile.damage, 0f, Main.myPlayer, 0, projectile.whoAmI);
                            Projectile.NewProjectile(projectile.Center, new Vector2(0, -8), ModContent.ProjectileType<RuneWave>(), projectile.damage, 0f, Main.myPlayer, 0, projectile.whoAmI);
                            Projectile.NewProjectile(projectile.Center, new Vector2(0, 8), ModContent.ProjectileType<RuneWave>(), projectile.damage, 0f, Main.myPlayer, 0, projectile.whoAmI);
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

        float scale = 0;

        public override bool PreDraw(SpriteBatch sb, Color lightColor)
        {
            int r = GameShaders.Armor.GetShaderIdFromItemId(ItemID.LivingFlameDye);

            Texture2D texture2D13 = Main.projectileTexture[projectile.type];
            Texture2D RingTex = mod.GetTexture("Projectiles/Minions/HeartRune_Ring");

            BaseDrawing.DrawTexture(sb, texture2D13, 0, projectile.position, projectile.width, projectile.height, 1f, 0, 0, 1, new Rectangle(0, 0, texture2D13.Width, texture2D13.Height), projectile.GetAlpha(Color.White), true);

            if (scale > 0)
            {
                BaseDrawing.DrawTexture(sb, RingTex, r, projectile.position, projectile.width, projectile.height, scale, projectile.rotation, 0, 1, new Rectangle(0, 0, RingTex.Width, RingTex.Height), Color.White, true);
            }

            return false;
        }
    }
}