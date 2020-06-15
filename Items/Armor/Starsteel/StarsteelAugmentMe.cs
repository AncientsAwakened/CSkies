using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using System.Collections.Generic;

namespace CSkies.Items.Armor.Starsteel
{
    public class StarsteelAugmentMe : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Melee Starsteel Augment");
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
                player.meleeDamage += .1f;
                player.meleeCrit += 10;
            }
            else
            {
                player.meleeDamage += .05f;
                player.meleeCrit += 5;
            }
        }

        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
            bool Starsteel = Main.player[item.owner].GetModPlayer<CPlayer>().Starsteel;

            string DamageAmount = (Starsteel ? 10 : 5) + "% increased melee damage & critical strike chance";

            TooltipLine DamageTooltip = new TooltipLine(mod, "Damage Type", DamageAmount);
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
                            player.armor[i].type == ModContent.ItemType<StarsteelAugmentRa>() ||
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