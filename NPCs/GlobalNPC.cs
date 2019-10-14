using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CSkies
{
    public class CGlobalNPC : GlobalNPC
    {
        public override void NPCLoot(NPC npc)
        {
            if (npc.type == NPCID.SkeletronHead && !CWorld.MeteorMessage)
            {
                CWorld.MeteorMessage = true;
                if (Main.netMode != 1) BaseUtility.Chat("Pieces of the sky begin to fall", new Color(136, 151, 255), true); int num144 = 12;
                Vector2 vector = new Vector2(npc.Center.X, npc.Center.Y - 1000);
                float num147 = Main.rand.Next(-100, 101);
                float num148 = Main.rand.Next(200) + 100;
                float num149 = (float)Math.Sqrt(num147 * num147 + num148 * num148);
                num149 = num144 / num149;
                num147 *= num149;
                num148 *= num149;
                Projectile.NewProjectile(vector.X, vector.Y, num147, num148, ModContent.ProjectileType<Projectiles.FallenShard>(), 1000, 10f, Main.myPlayer, 0f, 0f);
            }
        }
    }
}
