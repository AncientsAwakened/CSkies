using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace CSkies.Items.Boss.Observer
{
    public class ObserverEye : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Eye of the Observer");
            Tooltip.SetDefault(@"Provides spelunker, night vision, dangersense, and hunter");
        }

        public override void SetDefaults()
        {
            item.width = 30;
            item.height = 28;
            item.value = Item.sellPrice(0, 1, 0, 0);
            item.rare = ItemRarityID.Green;
            item.accessory = true;
            item.expert = true;
            item.expertOnly = true;
        }

        public override void UpdateEquip(Player player)
        {
            player.findTreasure = true;
            player.dangerSense = true;
            player.nightVision = true;
            player.detectCreature = true;
        }
    }
}