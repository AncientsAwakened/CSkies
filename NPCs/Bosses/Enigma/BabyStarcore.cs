using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.IO;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace CSkies.NPCs.Bosses.Enigma
{
    public class BabyStarcore : ModNPC
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Starcore Mini");
        }

        public override void SetDefaults()
        {
            npc.width = 46;
            npc.height = 46;
            npc.aiStyle = -1;
            npc.damage = 45;
            npc.defense = 25;
            npc.lifeMax = 2000;
            npc.value = Item.sellPrice(0, 0, 0, 0);
            npc.HitSound = new LegacySoundStyle(3, 4, Terraria.Audio.SoundType.Sound);
            npc.DeathSound = new LegacySoundStyle(4, 14, Terraria.Audio.SoundType.Sound);
            npc.knockBackResist = 0f;
            npc.noGravity = true;
            npc.netAlways = true;
            npc.noTileCollide = true;
        }

        public override void AI()
        {
            Lighting.AddLight(npc.Center, Color.LimeGreen.R / 150, Color.LimeGreen.G / 150, Color.LimeGreen.B / 150);

            if (!npc.HasPlayerTarget)
            {
                npc.TargetClosest();
            }
            
            Player player = Main.player[npc.target];

            BaseAI.AISkull(npc, ref npc.ai, true, 10, 350, .18f, .025f);

            if (npc.velocity.X > 0)
            {
                npc.rotation += .09f;
            }
            else if (npc.velocity.X < 0)
            {
                npc.rotation -= .09f;
            }

            BaseAI.ShootPeriodic(npc, player.position, player.width, player.height, ModContent.ProjectileType<Starcore.Starshot>(), ref npc.ai[2], Main.rand.Next(30, 50), npc.damage / 2, 10, true);
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Color drawColor)
        {
            Texture2D texture2D13 = Main.npcTexture[npc.type];
            Texture2D BladeTex = mod.GetTexture("NPCs/Bosses/Enigma/BabyStarcoreBack");

            BaseDrawing.DrawTexture(spriteBatch, BladeTex, 0, npc.position, npc.width, npc.height, npc.scale, npc.rotation, 0, 1, new Rectangle(0, 0, BladeTex.Width, BladeTex.Height), drawColor, true);

            BaseDrawing.DrawTexture(spriteBatch, texture2D13, 0, npc.position, npc.width, npc.height, npc.scale, 0, 0, 1, new Rectangle(0, 0, texture2D13.Width, texture2D13.Height), drawColor, true);

            return false;
        }
    }
}