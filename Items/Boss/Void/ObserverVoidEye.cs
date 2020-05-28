using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace CSkies.Items.Boss.Void
{
    public class ObserverVoidEye : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Eye of the Abyss");
            Tooltip.SetDefault(@"Provides spelunker, night vision, dangersense, and hunter
10% increased critical strike chance
Pressing the accessory ability key will cause a vortex that drags in enemies within 10 blocks of you to appear
You can only use this ability once every 5 minutes");
        }

        public override void SetDefaults()
        {
            item.width = 30;
            item.height = 28;
            item.value = Item.sellPrice(0, 10, 0, 0);
            item.rare = ItemRarityID.Purple;
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
            
            player.GetModPlayer<CPlayer>().VoidEye = true;
        }
    }
}