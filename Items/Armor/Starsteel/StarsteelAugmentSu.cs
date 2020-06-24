using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using System.Collections.Generic;
using Terraria.DataStructures;

namespace CSkies.Items.Armor.Starsteel
{
    public class StarsteelAugmentSu : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Summon Starsteel Augment");
            Tooltip.SetDefault(@"If wearing starsteel Armor, accessory effects double.");
            Main.RegisterItemAnimation(item.type, new DrawAnimationVertical(5, 13));
        }

        public override void SetDefaults()
        {
            item.width = 32;
            item.height = 26;
            item.value = Item.sellPrice(0, 3, 0, 0);
            item.rare = ItemRarityID.LightRed;
            item.accessory = true;
        }

        public override void UpdateEquip(Player player)
        {
            player.GetModPlayer<CPlayer>().StarsteelBonus = 4;
            if (player.GetModPlayer<CPlayer>().Starsteel)
            {
                player.minionDamage += .12f;
                player.maxMinions += 2;
            }
            else
            {
                player.rangedDamage += .06f;
                player.maxMinions += 1;
            }
        }

        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
            bool Starsteel = Main.player[item.owner].GetModPlayer<CPlayer>().Starsteel;

            string DamageAmount = (Starsteel ? 12 : 6) + "% increased minion damage";
            string CritAmount = "+" + (Starsteel ? 2 : 1) + "increased max minions";

            TooltipLine DamageTooltip = new TooltipLine(mod, "Damage Type", DamageAmount + @"
" + CritAmount);

            tooltips.Add(DamageTooltip);

            base.ModifyTooltips(tooltips);
        }

        public override bool CanEquipAccessory(Player player, int slot)
        {
            if (slot < 10)
            {
                int maxAccessoryIndex = 5 + player.extraAccessorySlots;
                for (int i = 3; i < 3 + maxAccessoryIndex; i++)
                {
                    if (slot != i)
                    {
                        if (player.armor[i].type == ModContent.ItemType<StarsteelAugmentMa>() ||
                            player.armor[i].type == ModContent.ItemType<StarsteelAugmentMe>() ||
                            player.armor[i].type == ModContent.ItemType<StarsteelAugmentRa>())
                        {
                            return false;
                        }
                    }
                }
            }
            return true;
        }
    }
}