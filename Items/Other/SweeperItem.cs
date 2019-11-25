using Terraria.ID;
using Terraria;
using Terraria.ModLoader;
using CSkies.NPCs.Critters;

namespace CSkies.Items.Other
{
    public class SweeperItem : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Chicken");
        }

        public override void SetDefaults()
        {
            item.width = 36;
            item.height = 30;
            item.maxStack = 999;
            item.value = Item.buyPrice(0, 0, 5, 0);
            item.rare = 1;
            item.useAnimation = 30;
            item.useTime = 30;
            item.useStyle = 4;
            item.consumable = true;
            item.makeNPC = (short)ModContent.NPCType<Sweeper>();
        }
    }
}
