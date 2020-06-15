using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CSkies.NPCs.Other
{
    public class EnigmaSpeaker : ModNPC
	{
        public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Mysterious Speaker");
        }

		public override void SetDefaults()
		{
			npc.aiStyle = -1;
			npc.lifeMax = 1;
			npc.damage = 0;
			npc.knockBackResist = 0f;
            npc.width = 28;
            npc.height = 28;
            npc.HitSound = SoundID.NPCHit1;
			npc.DeathSound = SoundID.NPCDeath1;
			npc.noGravity = false;
			npc.noTileCollide = false;
			npc.alpha = 0;
			npc.dontTakeDamage = true;
			npc.boss = false;
            npc.npcSlots = 0;
        }

		public override bool CheckActive()
		{
			return false;
        }

        public float rotationspeed = 0.2f;

        public override void AI()
        {
            if (npc.collideY)
            {
                npc.ai[1]++;
            }
            switch ((int)npc.ai[0])
            {
                case 0:

                    if (npc.ai[1] == 120)
                    {
                        CombatText.NewText(npc.Hitbox, Color.LimeGreen, "WH-- HEY!!!", true);
                    }
                    if (npc.ai[1] == 240)
                    {
                        CombatText.NewText(npc.Hitbox, Color.LimeGreen, "YOU!!! NATIVE LIFEFORM! DO YOU HAVE ANY IDEA WHAT YOU JUST DESTROYED?!", true);
                    }
                    if (npc.ai[1] == 360)
                    {
                        CombatText.NewText(npc.Hitbox, Color.LimeGreen, "MY BEAUTIFUL STARCORE! Just a pile of no more than SCRAP!!!", true);
                    }
                    if (npc.ai[1] == 480)
                    {
                        CombatText.NewText(npc.Hitbox, Color.LimeGreen, "You'll be paying for this very shortly...", true);
                    }
                    if (npc.ai[1] == 600)
                    {
                        CombatText.NewText(npc.Hitbox, Color.LimeGreen, "MARK", true);
                    }
                    if (npc.ai[1] == 660)
                    {
                        CombatText.NewText(npc.Hitbox, Color.LimeGreen, "MY", true);
                    }
                    if (npc.ai[1] == 720)
                    {
                        CombatText.NewText(npc.Hitbox, Color.LimeGreen, "WORDS!", true);
                    }
                    if (npc.ai[1] == 780)
                    {
                        npc.life = 0;
                        npc.netUpdate = true;
                    }

                    break;

                case 1:
                    if (npc.ai[1] == 120)
                    {
                        CombatText.NewText(npc.Hitbox, Color.LimeGreen, "GAAAAAAAAAH!! AGAIN?!", true);
                    }
                    if (npc.ai[1] == 240)
                    {
                        CombatText.NewText(npc.Hitbox, Color.LimeGreen, "YOU ARE QUITE THE INSUFFERABLE PEST!", true);
                    }
                    if (npc.ai[1] == 360)
                    {
                        CombatText.NewText(npc.Hitbox, Color.LimeGreen, "BELIVE ME, THE NEXT TIME YOU FIGHT ONE OF MY CREATIONS...", true);
                    }
                    if (npc.ai[1] == 480)
                    {
                        CombatText.NewText(npc.Hitbox, Color.LimeGreen, "YOU'LL BE SORR--", true);
                    }
                    if (npc.ai[1] == 500)
                    {
                        npc.life = 0;
                        npc.netUpdate = true;
                    }
                    break;

                case 2:
                    if (npc.ai[1] == 120)
                    {
                        CombatText.NewText(npc.Hitbox, Color.LimeGreen, "...", true);
                    }
                    if (npc.ai[1] == 240)
                    {
                        CombatText.NewText(npc.Hitbox, Color.LimeGreen, "*SLAM*", true);
                    }
                    if (npc.ai[1] == 360)
                    {
                        CombatText.NewText(npc.Hitbox, Color.LimeGreen, "AAAAAAAAAAAAAAAAAAAAAAAAAAA--", true);
                    }
                    if (npc.ai[1] == 400)
                    {
                        npc.life = 0;
                        npc.netUpdate = true;
                    }
                    break;

                case 3:
                    if (npc.ai[1] == 120)
                    {
                        CombatText.NewText(npc.Hitbox, Color.LimeGreen, "THAT TEARS IT!!!", true);
                    }
                    if (npc.ai[1] == 240)
                    {
                        CombatText.NewText(npc.Hitbox, Color.LimeGreen, "YOU ARE QUITE THE INSUFFERABLE PEST!", true);
                    }
                    if (npc.ai[1] == 360)
                    {
                        CombatText.NewText(npc.Hitbox, Color.LimeGreen, "BELIVE ME, THE NEXT TIME YOU FIGHT ONE OF MY CREATIONS...", true);
                    }
                    if (npc.ai[1] == 480)
                    {
                        CombatText.NewText(npc.Hitbox, Color.LimeGreen, "YOU'LL BE SORR--", true);
                    }
                    if (npc.ai[1] == 500)
                    {
                        npc.life = 0;
                        npc.netUpdate = true;
                    }
                    break;

            }
        }

        public override void NPCLoot()
        {
            for (int num468 = 0; num468 < 12; num468++)
            {
                int num469 = Dust.NewDust(new Vector2(npc.position.X, npc.position.Y), npc.width, npc.height, DustID.Fire, -npc.velocity.X * 0.2f,
                    -npc.velocity.Y * 0.2f, 100, default, 2f);
                Main.dust[num469].noGravity = false;
            }
        }

        public override bool PreDraw(SpriteBatch spritebatch, Color drawColor)
        {
            Texture2D T = Main.npcTexture[npc.type];
            BaseDrawing.DrawTexture(spritebatch, T, 0, npc.position, npc.width, npc.height, npc.scale, npc.rotation, 0, 1, npc.frame, drawColor, true);
            return false;
        }

        public override bool StrikeNPC(ref double damage, int defense, ref float knockback, int hitDirection, ref bool crit)
        {
            damage = 0;
            return false;
        }
    }
}
