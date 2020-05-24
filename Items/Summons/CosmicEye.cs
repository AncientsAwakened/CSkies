using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace CSkies.Items.Summons
{
	public class CosmicEye : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Cosmic Eye");
			Tooltip.SetDefault(@"It's observing you.
Summons the Observer
Can only be used at night");
            Main.RegisterItemAnimation(item.type, new DrawAnimationVertical(6, 4));
            ItemID.Sets.SortingPriorityBossSpawns[item.type] = 13;
        }

		public override void SetDefaults()
		{
			item.width = 14;
			item.height = 24;
			item.rare = ItemRarityID.Green;
			item.useAnimation = 45;
			item.useTime = 45;
			item.useStyle = ItemUseStyleID.HoldingUp;
            item.maxStack = 20;
			item.consumable = true;
            item.noUseGraphic = true;
		}

		public override bool CanUseItem(Player player)
		{
			return !Main.dayTime && !NPC.AnyNPCs(mod.NPCType("Observer"));
		}

		public override bool UseItem(Player player)
		{
			NPC.SpawnOnPlayer(player.whoAmI, mod.NPCType("Observer"));
			Main.PlaySound(SoundID.Roar, player.position, 0);
			return true;
		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.SuspiciousLookingEye, 1);
			recipe.AddIngredient(null, "CosmicLens", 3);
			recipe.AddTile(TileID.Anvils);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
}
