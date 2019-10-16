using Terraria;
using Terraria.ModLoader;

namespace CSkies.Items.Boss
{
    public class HeartcoreShield : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Shield of the Core");
            Tooltip.SetDefault(@"Provides knockback immunity
Allows you to do a quick dash
Being stuck while equipped with this causes stars to fall");
        }

        public override void SetDefaults()
        {
            item.width = 30;
            item.height = 28;
            item.value = Item.sellPrice(0, 5, 0, 0);
            item.rare = 5;
            item.accessory = true;
            item.expert = true;
            item.expertOnly = true;
        }

        public override void UpdateEquip(Player player)
        {
            player.dash = 3;
            player.noKnockback = true;
            player.GetModPlayer<CPlayer>().StarShield = true;
        }


        public override bool CanEquipAccessory(Player player, int slot)
        {
            if (slot < 10)
            {
                int maxAccessoryIndex = 5 + player.extraAccessorySlots;
                for (int i = 3; i < 3 + maxAccessoryIndex; i++)
                {
                    if (slot != i && player.armor[i].type == ModContent.ItemType<StarcoreShield>())
                    {
                        return false;
                    }
                }
            }
            return true;
        }
    }
}