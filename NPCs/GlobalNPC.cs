using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
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

        public void ClearPoolWithExceptions(IDictionary<int, float> pool)
        {
            try
            {
                Dictionary<int, float> keepPool = new Dictionary<int, float>();
                foreach (var kvp in pool)
                {
                    int npcID = kvp.Key;
                    ModNPC mnpc = NPCLoader.GetNPC(npcID);
                    if (mnpc != null && mnpc.mod != null) //splitting so you can add other exceptions if need be
                    {
                        if (mnpc.mod.Name.Equals("GRealm")) //do not remove GRealm spawns!
                        {
                            keepPool.Add(npcID, kvp.Value);
                        }
                    }
                }
                pool.Clear();

                foreach (var newkvp in keepPool)
                {
                    pool.Add(newkvp.Key, newkvp.Value);
                }

                keepPool.Clear();
            }
            catch (Exception e)
            {
                if (Main.netMode != 1)
                {
                    BaseUtility.Chat(e.StackTrace);
                }
            }
        }

        public override void EditSpawnPool(IDictionary<int, float> pool, NPCSpawnInfo spawnInfo)
        {
            if (spawnInfo.player.ZoneTowerNebula || spawnInfo.player.ZoneTowerSolar || spawnInfo.player.ZoneTowerStardust || spawnInfo.player.ZoneTowerVortex ||
                Main.eclipse ||
                Main.invasionType == InvasionID.MartianMadness ||
                Main.invasionType == InvasionID.CachedPumpkinMoon ||
                Main.invasionType == InvasionID.CachedFrostMoon)
            {
                return;
            }

            if (spawnInfo.player.GetModPlayer<CPlayer>().ZoneObservatory)
            {
                ClearPoolWithExceptions(pool);
                if (Main.hardMode)
                {
                    pool.Add(mod.NPCType("Starprobe"), .2f);
                    pool.Add(mod.NPCType("Sweeper"), .08f);
                    pool.Add(mod.NPCType("Stabber"), .02f);
                }
            }

            if (spawnInfo.player.GetModPlayer<CPlayer>().ZoneVoid)
            {
                if (NPC.downedMoonlord)
                {
                    ClearPoolWithExceptions(pool);
                    pool.Add(mod.NPCType("AbyssEye"), .15f);
                    pool.Add(mod.NPCType("FlailingHate"), .05f);
                    pool.Add(mod.NPCType("Fleyer"), .2f);

                    if (spawnInfo.player.ZoneDesert)
                    {
                        pool.Add(mod.NPCType("AbyssGhoul"), .15f);
                    }
                }
            }
        }
    }
}
