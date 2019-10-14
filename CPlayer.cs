using Terraria;
using Terraria.ModLoader;
using CSkies.NPCs.Other;
using Terraria.GameInput;
using Microsoft.Xna.Framework;
using CSkies.NPCs.Bosses.Void;
using CSkies.NPCs.Bosses.ObserverVoid;

namespace CSkies
{
    public class CPlayer : ModPlayer
    {
        public bool Watcher = false;
        public bool Gazer = false;

        public bool ZoneComet = false;
        public bool ZoneVoid = false;

        public bool VoidEye = false;
        public bool VoidCD = false;

        public override void ResetEffects()
        {
            Watcher = false;
            Gazer = false;

            VoidEye = false;
            VoidCD = false;
        }

        public override void Initialize()
        {
            Watcher = false;
            Gazer = false;
            VoidEye = false;
            VoidCD = false;
            ZoneVoid = false;
            ZoneComet = false;
        }

        public override void UpdateBiomes()
        {
            ZoneVoid = NearVoid();
            ZoneComet = CWorld.CometTiles > 30;
        }
        public override void UpdateBiomeVisuals()
        {
            bool useVoid = ZoneVoid || NearVoidBoss();

            player.ManageSpecialBiomeVisuals("CSkies:AbyssSky", useVoid);
        }

        public bool NearVoid()
        {
            if (NPC.AnyNPCs(ModContent.NPCType<AbyssVoid>()))
            {
                int v = BaseAI.GetNPC(player.Center, ModContent.NPCType<AbyssVoid>(), 2500);
                if (v != -1)
                {
                    return true;
                }
            }
            return false;
        }

        public bool NearVoidBoss()
        {
            if (NPC.AnyNPCs(ModContent.NPCType<Void>()) || NPC.AnyNPCs(ModContent.NPCType<VoidTransition1>()) || NPC.AnyNPCs(ModContent.NPCType<VoidTransition2>()) || NPC.AnyNPCs(ModContent.NPCType<ObserverVoid>()))
            {
                int v = BaseAI.GetNPC(player.Center, ModContent.NPCType<Void>(), 2500);
                int vt1 = BaseAI.GetNPC(player.Center, ModContent.NPCType<VoidTransition1>(), 2500);
                int vt2 = BaseAI.GetNPC(player.Center, ModContent.NPCType<VoidTransition2>(), 2500);
                int ov = BaseAI.GetNPC(player.Center, ModContent.NPCType<ObserverVoid>(), 2500);
                if (v != -1 || vt1 != -1 || vt2 != -1 || ov != -1)
                {
                    return true;
                }
            }
            return false;
        }

        public int VortexScale = 0;

        public override void PostUpdate()
        {
            if (VoidEye)
            {
                CritAll(10);
            }
        }

        public override void ProcessTriggers(TriggersSet triggersSet)
        {
            if (VoidEye)
            {
                if (CSkies.AccessoryAbilityKey.JustPressed && !player.HasBuff(ModContent.BuffType<Buffs.VECooldown>()))
                {
                    player.AddBuff(ModContent.BuffType<Buffs.VECooldown>(), 3000);
                    Projectile.NewProjectile(player.Center, Vector2.Zero, ModContent.ProjectileType<Projectiles.VoidEyeVortex>(), 60, 0, Main.myPlayer);
                }
            }
        }

        public void CritAll(int critAddon)
        {
            player.meleeCrit += critAddon;
            player.rangedCrit += critAddon;
            player.magicCrit += critAddon;
            player.thrownCrit += critAddon;
        }
    }
}