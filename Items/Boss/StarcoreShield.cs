using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace CSkies.Items.Boss
{
    //[AutoloadEquip(EquipType.Shield)]
    public class StarcoreShield : ModItem
    {
        public override void SetDefaults()
        {
            item.width = 30;
            item.height = 32;
            item.value = Item.sellPrice(0, 5, 0, 0);
            item.rare = ItemRarityID.Pink;
            item.expert = true; item.expertOnly = true;
            item.accessory = true;
            item.defense = 3;
        }
        public override void SetStaticDefaults()
        {            DisplayName.SetDefault("Starsteel Shield");
            Tooltip.SetDefault(@"Provides knockback immunity
Allows you to do a quick dash
Being stuck while equipped with this causes stars to fall");
        }

        public override void UpdateEquip(Player player)
        {
            player.dash = 3;
            player.noKnockback = true;
            player.GetModPlayer<CPlayer>().StarShield = true;
            player.dash = 2;
        }

        public override bool CanEquipAccessory(Player player, int slot)
        {
            if (slot < 10)
            {
                int maxAccessoryIndex = 5 + player.extraAccessorySlots;
                for (int i = 3; i < 3 + maxAccessoryIndex; i++)
                {
                    if (slot != i && player.armor[i].type == ModContent.ItemType<HeartcoreShield>())
                    {
                        return false;
                    }
                }
            }
            return true;
        }
    }
}