using Terraria;
using Terraria.ModLoader;
using CSkies.NPCs.Other;
using Terraria.GameInput;
using Microsoft.Xna.Framework;
using CSkies.NPCs.Bosses.Void;
using CSkies.NPCs.Bosses.ObserverVoid;
using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;
using Terraria.ID;
using Terraria.Graphics.Shaders;

namespace CSkies
{
    public partial class CPlayer : ModPlayer
    {
        public bool Watcher = false;
        public bool Gazer = false;
        public bool Drone = false;
        public bool Rune = false;

        public bool ZoneComet = false;
        public bool ZoneVoid = false;
        public bool ZoneCSky = false;
        public bool ZoneObservatory = false;

        public bool VoidEye = false;
        public bool VoidCD = false;
        public bool StarShield = false;
        public bool HeartShield = false;

        public float HeartringScale = 0;
        public float HeartringRot = 0;

        public bool Heartburn = false;

        public override void ResetEffects()
        {
            Watcher = false;
            Gazer = false;
            Drone = false;
            Rune = false;

            VoidEye = false;
            VoidCD = false;

            Heartburn = false;
        }

        public override void Initialize()
        {
            Watcher = false;
            Gazer = false;
            Drone = false;
            Rune = false;

            VoidEye = false;
            VoidCD = false;

            ZoneVoid = false;
            ZoneComet = false;
            ZoneObservatory = false;

            Heartburn = false;
        }

        public override void UpdateBiomes()
        {
            ZoneVoid = NearVoid() || CWorld.AbyssTiles > 50;
            ZoneComet = CWorld.CometTiles > 30;
            ZoneObservatory = CWorld.ObservatoryTiles > 30 && !ZoneVoid;
        }

        public override void UpdateBiomeVisuals()
        {
            bool useVoid = ZoneVoid || NearVoidBoss();
            bool useCSky = ZoneCSky;

            player.ManageSpecialBiomeVisuals("CSkies:AbyssSky", useVoid);
            player.ManageSpecialBiomeVisuals("CSkies:CSky", useCSky);
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
            if (NPC.AnyNPCs(ModContent.NPCType<NPCs.Bosses.Void.Void>()) || NPC.AnyNPCs(ModContent.NPCType<VoidTransition1>()) || NPC.AnyNPCs(ModContent.NPCType<VoidTransition2>()) || NPC.AnyNPCs(ModContent.NPCType<ObserverVoid>()))
            {
                int v = BaseAI.GetNPC(player.Center, ModContent.NPCType<NPCs.Bosses.Void.Void>(), 2500);
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
            if (HeartShield)
            {
                if (player.statLife < player.statLifeMax2)
                {
                    player.moveSpeed *= 1.1f;
                    player.allDamage += .25f;
                    player.statDefense -= 8;

                    HeartringRot += .02f;
                    if (HeartringScale >= 1f)
                    {
                        HeartringScale = 1f;
                    }
                    else
                    {
                        HeartringScale += .02f;
                    }
                }
                else
                {
                    player.moveSpeed *= .9f;
                    player.statDefense += 8;
                    HeartringRot -= .02f;
                    if (HeartringScale <= 0)
                    {
                        HeartringScale = 0f;
                    }
                    else
                    {
                        HeartringScale -= .02f;
                    }
                }
            }
            else
            {
                HeartringRot -= .02f;
                if (HeartringScale <= 0)
                {
                    HeartringScale = 0f;
                }
                else
                {
                    HeartringScale -= .02f;
                }
            }
        }

        public override void OnHitByNPC(NPC npc, int damage, bool crit)
        {
            if (StarShield)
            {
                for (int n = 0; n < 3; n++)
                {
                    float x = player.position.X + Main.rand.Next(-400, 400);
                    float y = player.position.Y - Main.rand.Next(500, 800);
                    Vector2 vector = new Vector2(x, y);
                    float num13 = player.position.X + player.width / 2 - vector.X;
                    float num14 = player.position.Y + player.height / 2 - vector.Y;
                    num13 += Main.rand.Next(-100, 101);
                    int num15 = 23;
                    float num16 = (float)Math.Sqrt(num13 * num13 + num14 * num14);
                    num16 = num15 / num16;
                    num13 *= num16;
                    num14 *= num16;
                    int num17 = Projectile.NewProjectile(x, y, num13, num14, ModContent.ProjectileType<Projectiles.Star.ShieldStar>(), 30, 5f, player.whoAmI, 0f, 0f);
                    Main.projectile[num17].ai[1] = player.position.Y;
                }
            }
            if (HeartShield)
            {
                for (int n = 0; n < 6; n++)
                {
                    float x = player.position.X + Main.rand.Next(-400, 400);
                    float y = player.position.Y - Main.rand.Next(500, 800);
                    Vector2 vector = new Vector2(x, y);
                    float num13 = player.position.X + player.width / 2 - vector.X;
                    float num14 = player.position.Y + player.height / 2 - vector.Y;
                    num13 += Main.rand.Next(-100, 101);
                    int num15 = 23;
                    float num16 = (float)Math.Sqrt(num13 * num13 + num14 * num14);
                    num16 = num15 / num16;
                    num13 *= num16;
                    num14 *= num16;
                    int num17 = Projectile.NewProjectile(x, y, num13, num14, ModContent.ProjectileType<Projectiles.Heart.Meteor0>(), 30, 5f, player.whoAmI, 0f, 0f);
                    Main.projectile[num17].ai[1] = player.position.Y;
                }
            }
        }

        public override void ProcessTriggers(TriggersSet triggersSet)
        {
            if (VoidEye)
            {
                if (CSkies.AccessoryAbilityKey.JustPressed && !player.HasBuff(ModContent.BuffType<Buffs.VECooldown>()))
                {
                    player.AddBuff(ModContent.BuffType<Buffs.VECooldown>(), 3000);
                    Projectile.NewProjectile(player.Center, Vector2.Zero, ModContent.ProjectileType<Projectiles.Void.VoidEyeVortex>(), 60, 0, Main.myPlayer);
                }
            }
        }

        public override void UpdateBadLifeRegen()
        {
            if (Heartburn)
            {
                if (player.statLife > player.statLifeMax2 * .8f)
                {
                    player.lifeRegen -= 30;
                }
                else
                {
                    player.lifeRegen = 0;
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

    public partial class CPlayer : ModPlayer
    {
        public override void ModifyDrawLayers(List<PlayerLayer> list)
        {
            AddPlayerLayer(list, glAfterAll, list[list.Count - 1]);
        }

        public static void AddPlayerLayer(List<PlayerLayer> layers, PlayerLayer layer, PlayerLayer parent)
        {
            int index = layers.IndexOf(parent);
            if (index != -1)
            {
                layers.Insert(index + 1, layer);
            }
        }

        public PlayerLayer glAfterAll = new PlayerLayer("CSkies", "glAfterAll", delegate (PlayerDrawInfo edi)
        {
            Mod mod = CSkies.inst;
            Player drawPlayer = edi.drawPlayer;
            CPlayer cPlayer = drawPlayer.GetModPlayer<CPlayer>();

            if (drawPlayer.mount.Active)
            {
                return;
            }

            if (drawPlayer.GetModPlayer<CPlayer>().HeartringScale > 0)
            {
                Texture2D Ring = mod.GetTexture("NPCs/Bosses/FurySoul/FuryRing");
                int r = GameShaders.Armor.GetShaderIdFromItemId(ItemID.LivingFlameDye);
                BaseDrawing.DrawTexture(Main.spriteBatch, Ring, r, drawPlayer.position, drawPlayer.width, drawPlayer.height, cPlayer.HeartringScale, cPlayer.HeartringRot, 0, 1, new Rectangle(0, 0, Ring.Width, Ring.Height), BaseDrawing.GetLightColor(new Vector2(drawPlayer.position.X, drawPlayer.position.Y)), true);
            }
        });

        public static bool HasAndCanDraw(Player player, int type)
        {
            int dum = 0;
            bool dummy = false;

            return HasAndCanDraw(player, type, ref dummy, ref dum);
        }

        public static bool HasAndCanDraw(Player player, int type, ref bool social, ref int slot)
        {
            if (player.wereWolf || player.merman)
            {
                return false;
            }

            ModItem mitem = ItemLoader.GetItem(type);
            if (mitem != null)
            {
                Item item = mitem.item;
                if (item.headSlot > 0)
                {
                    return BasePlayer.HasHelmet(player, type) && BaseDrawing.ShouldDrawHelmet(player, type);
                }
                else if (item.bodySlot > 0)
                {
                    return BasePlayer.HasChestplate(player, type) && BaseDrawing.ShouldDrawChestplate(player, type);
                }
                else if (item.legSlot > 0)
                {
                    return BasePlayer.HasLeggings(player, type) && BaseDrawing.ShouldDrawLeggings(player, type);
                }
                else if (item.accessory)
                {
                    return BasePlayer.HasAccessory(player, type, true, true, ref social, ref slot) && BaseDrawing.ShouldDrawAccessory(player, type);
                }
            }

            return false;
        }
    }
}