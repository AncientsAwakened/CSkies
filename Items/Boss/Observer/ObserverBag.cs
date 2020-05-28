using Terraria;
using Terraria.ModLoader;

namespace CSkies.Items.Boss.Observer
{
    public class ObserverBag : ModItem
	{
        
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Treasure Bag");
			Tooltip.SetDefault("{$CommonItemTooltip.RightClickToOpen}");
		}

		public override void SetDefaults()
		{
			item.maxStack = 999;
			item.consumable = true;
			item.width = 36;
			item.height = 32;
            item.expert = true;
            item.expertOnly = true;
		}

        public override int BossBagNPC => mod.NPCType("Observer");

        public override bool CanRightClick()
		{
			return true;
        }

        public override void OpenBossBag(Player player)
        {
            if (Main.rand.Next(7) == 0)
            {
                player.QuickSpawnItem(mod.ItemType("ObserverMask"));
            }
            string[] lootTableA = { "Comet", "CometDagger", "CometFan", "CometJavelin", "CometPortal", "Comet Shot" };
            int lootA = Main.rand.Next(lootTableA.Length);
            int Drop = mod.ItemType(lootTableA[lootA]);
            if (Drop == mod.ItemType("CometDagger"))
            {
                player.QuickSpawnItem(mod.ItemType(lootTableA[lootA]), Main.rand.Next(50, 201));
            }
            else
            {
                player.QuickSpawnItem(mod.ItemType(lootTableA[lootA]));
            }

            player.QuickSpawnItem(ModContent.ItemType<Boss.Observer.CometFragment>(), Main.rand.Next(10, 15));

            player.QuickSpawnItem(mod.ItemType("ObserverEye"));
        }
	}
}