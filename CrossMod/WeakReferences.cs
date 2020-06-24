using CSkies.Items.Summons;
using CSkies.Items.Boss.Observer;
using CSkies.Items.Boss.Starcore;
using CSkies.Items.Boss.Void;
using CSkies.Items.Boss.Heartcore;
using CSkies.Items.Boxes;
using System;
using System.Collections.Generic;
using Terraria.ModLoader;
using Terraria;

namespace CSkies
{
    internal class WeakReferences
    {
        public static void PerformModSupport()
        {
            PerformBossChecklistSupport();
            PerformFargosSetup();
        }

        private static void PerformBossChecklistSupport()
        {
            Mod bossChecklist = ModLoader.GetMod("BossChecklist");

            CSkies mod = CSkies.inst;

            if (bossChecklist != null)
            {
                #region Observer
                bossChecklist.Call("AddBoss", 4f, mod.NPCType("Observer"), mod,
                    "The Observer",
                    (Func<bool>)(() => CWorld.downedObserver),
                    ModContent.ItemType<CosmicEye>(),
                    new List<int>
                    {
                        ModContent.ItemType<ObserverTrophy>(),
                        ModContent.ItemType<ObserverMask>(),
                        ModContent.ItemType<ObserverBox>()
                    },
                    new List<int>
                    {
                        ModContent.ItemType<ObserverBag>(),
                        ModContent.ItemType<ObserverEye>(),
                        ModContent.ItemType<Comet>(),
                        ModContent.ItemType<CometDagger>(),
                        ModContent.ItemType<CometFan>(),
                        ModContent.ItemType<CometJavelin>(),
                        ModContent.ItemType<CometPortal>(),
                        ModContent.ItemType<Skyshot>(),
                        ModContent.ItemType<CometFragment>()
                    },
                    "Use a [i: " + ModContent.ItemType<CosmicEye>() + "] at night",
                    "The observer has seen enough.",
                    "CSkies/CrossMod/BossChecklist/Observer",
                    "CSkies/NPCs/Bosses/Observer/Observer_Head_Boss");
                #endregion

                #region Starcore
                bossChecklist.Call("AddBoss", 6.5f, mod.NPCType("Starcore"), mod,
                    "Starcore",
                    (Func<bool>)(() => CWorld.downedStarcore),
                    ModContent.ItemType<Transmitter>(),
                    new List<int>
                    {
                        ModContent.ItemType<StarcoreTrophy>(),
                        ModContent.ItemType<StarcoreMask>(),
                        ModContent.ItemType<StarcoreBox>()
                    },
                    new List<int>
                    {
                        ModContent.ItemType<StarcoreBag>(),
                        ModContent.ItemType<StarcoreShield>(),
                        ModContent.ItemType<Starsaber>(),
                        ModContent.ItemType<Railscope>(),
                        ModContent.ItemType<StormStaff>(),
                        ModContent.ItemType<StarDroneUnit>(),
                        ModContent.ItemType<Stelarite>(),
                    },
                    "Use a [i: " + ModContent.ItemType<Transmitter>() + "] at night",
                    "Starcore returns to orbit.",
                    "CSkies/CrossMod/BossChecklist/Starcore",
                    "CSkies/NPCs/Bosses/Starcore/Starcore_Head_Boss");
                #endregion

                #region Observer Void
                bossChecklist.Call("AddBoss", 15f, mod.NPCType("ObserverVoid"), mod,
                    "Oberver Void",
                    (Func<bool>)(() => CWorld.downedObserverV),
                    ModContent.ItemType<VoidEye>(),
                    new List<int>
                    {
                        ModContent.ItemType<ObserverVoidMask>(),
                        ModContent.ItemType<ObserverVoidTrophy>(),
                        ModContent.ItemType<OVBox>()
                    },
                    new List<int>
                    {
                        ModContent.ItemType<Singularity>(),
                        ModContent.ItemType<VoidJavelin>(),
                        ModContent.ItemType<VoidFan>(),
                        ModContent.ItemType<VoidShot>(),
                        ModContent.ItemType<VoidPortal>(),
                        ModContent.ItemType<VoidWings>(),
                        ModContent.ItemType<VoidFragment>(),
                    },
                    "Use a [i: " + ModContent.ItemType<VoidEye>() + "] at night",
                    "Observer Void Returns to the darkness...",
                    "CSkies/CrossMod/BossChecklist/ObserverVoid",
                    "CSkies/NPCs/Bosses/ObserverVoid/ObserverVoid_Head_Boss");
                #endregion

                #region VOID
                bossChecklist.Call("AddBoss", 15f, mod.NPCType("Void"), mod,
                    "VOID",
                    (Func<bool>)(() => CWorld.downedObserverV),
                    ModContent.ItemType<VoidEye>(),
                    new List<int>
                    {
                        ModContent.ItemType<VOIDMask>(),
                        ModContent.ItemType<VOIDTrophy>(),
                        ModContent.ItemType<VOIDBox>()
                    },
                    new List<int>
                    {
                        ModContent.ItemType<ObserverVoidBag>(),
                        ModContent.ItemType<ObserverVoidEye>()
                    },
                    "Defeat Observer Void in expert mode",
                    "The sound of insane laugter rings through your ears...",
                    "CSkies/CrossMod/BossChecklist/Void",
                    "CSkies/NPCs/Bosses/ObserverVoid/Void_Head_Boss",
                    (Func<bool>)(() => CWorld.downedObserverV && Main.expertMode));
                #endregion

                #region Heartcore
                bossChecklist.Call("AddBoss", 16f, mod.NPCType("Heartcore"), mod,
                    "Heartcore",
                    (Func<bool>)(() => CWorld.downedHeartcore),
                    ModContent.ItemType<PassionRune>(),
                    new List<int>
                    {
                        ModContent.ItemType<HeartcoreTrophy>(),
                        ModContent.ItemType<HeartcoreMask>(),
                        ModContent.ItemType<HCBox>()
                    },
                    new List<int>
                    {
                        ModContent.ItemType<Sol>(),
                        ModContent.ItemType<BlazeBuster>(),
                        ModContent.ItemType<MeteorShower>(),
                        ModContent.ItemType<FlamingSoul>()
                    },
                    "Use a [i: " + ModContent.ItemType<PassionRune>() + "] at night",
                    "Heartcore rockets back into the stratosphere...",
                    "CSkies/CrossMod/BossChecklist/Heartcore",
                    "CSkies/NPCs/Bosses/Broodmother/Heartcore_Head_Boss");
                #endregion

                #region Fury Soul
                bossChecklist.Call("AddBoss", 16.5f, mod.NPCType("FurySoul"), mod,
                    "Fury Soul",
                    (Func<bool>)(() => CWorld.downedSoul),
                    ModContent.ItemType<PassionRune>(),
                    new List<int>
                    {
                        ModContent.ItemType<FurySoulTrophy>(),
                        ModContent.ItemType<FurySoulMask>(),
                        ModContent.ItemType<FSBox>()
                    },
                    new List<int>
                    {
                        ModContent.ItemType<HeartcoreBag>(),
                        ModContent.ItemType<HeartcoreShield>()
                    },
                    "Defeat Heartcore in expert mode",
                    "Heartcore vanishes in a blink of flame...",
                    "CSkies/CrossMod/BossChecklist/FurySoul",
                    "CSkies/NPCs/Bosses/FurySoul/FurySoul_Head_Boss",
                    null);
                #endregion

                // SlimeKing = 1f;
                // EyeOfCthulhu = 2f;
                // EaterOfWorlds = 3f;
                // QueenBee = 4f;
                // Skeletron = 5f;
                // WallOfFlesh = 6f;
                // TheTwins = 7f;
                // TheDestroyer = 8f;
                // SkeletronPrime = 9f;
                // Plantera = 10f;
                // Golem = 11f;
                // DukeFishron = 12f;
                // LunaticCultist = 13f;
                // Moonlord = 14f;
            }
        }

        private static void PerformFargosSetup()
        {
            Mod fargos = ModLoader.GetMod("Fargowiltas");
            if (fargos != null)
            {
                // AddSummon, order or value in terms of vanilla bosses, your mod internal name, summon   
                //item internal name, inline method for retrieving downed value, price to sell for in copper

                fargos.Call("AddSummon", 3.5f, "CSkies", "CosmicEye", (Func<bool>)(() => CWorld.downedObserver), 100000);
                fargos.Call("AddSummon", 9.7f, "CSkies", "Transmitter", (Func<bool>)(() => CWorld.downedStarcore), 400000);
                fargos.Call("AddSummon", 15f, "CSkies", "VoidEye", (Func<bool>)(() => CWorld.downedObserverV), 1000000);
                fargos.Call("AddSummon", 16f, "CSkies", "PassionRune", (Func<bool>)(() => CWorld.downedHeartcore), 1500000);
            }
        }
    }
}
