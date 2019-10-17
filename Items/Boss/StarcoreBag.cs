using Terraria;
using Terraria.ModLoader;

namespace CSkies.Items.Boss
{
    public class StarcoreBag : ModItem
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

        public override int BossBagNPC => mod.NPCType("Starcore");

        public override bool CanRightClick()
		{
			return true;
        }

        public override void OpenBossBag(Player player)
        {
            if (Main.rand.Next(7) == 0)
            {
                player.QuickSpawnItem(mod.ItemType("StarcoreMask"));
            }
            string[] lootTableA = { "Starsaber", "StormStaff", "StarDroneUnit", "Railscope" };
            int lootA = Main.rand.Next(lootTableA.Length);

            player.QuickSpawnItem(mod.ItemType(lootTableA[lootA]));

            player.QuickSpawnItem(ModContent.ItemType<Star.Stelarite>(), Main.rand.Next(10, 15));

            player.QuickSpawnItem(mod.ItemType("StarcoreShield"));
        }
	}
}