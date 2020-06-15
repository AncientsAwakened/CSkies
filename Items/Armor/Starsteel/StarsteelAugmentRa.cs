using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using System.Collections.Generic;

namespace CSkies.Items.Armor.Starsteel
{
    public class StarsteelAugmentRa : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Ranged Starsteel Augment");
            Tooltip.SetDefault(@"If wearing starsteel Armor, accessory effects double.");
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
            if (player.GetModPlayer<CPlayer>().Starsteel)
            {
                player.rangedDamage += .18f;
                player.rangedCrit += 8;
            }
            else
            {
                player.rangedDamage += .09f;
                player.rangedCrit += 4;
            }
        }

        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
            bool Starsteel = Main.player[item.owner].GetModPlayer<CPlayer>().Starsteel;

            string DamageAmount = (Starsteel ? 18 : 9) + "% increased ranged damage";
            string CritAmount = (Starsteel ? 8 : 4) + "% increased ranged critical strike chance";

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
                            player.armor[i].type == ModContent.ItemType<StarsteelAugmentSu>())
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