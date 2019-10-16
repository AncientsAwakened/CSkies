using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CSkies.Items.Summons
{
    public class Transmitter : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Transmitter");
			Tooltip.SetDefault(@"It's displaying coordinates somewhere in the atmosphere...
Summons Starcore
Can only be used at night");
        }

		public override void SetDefaults()
		{
			item.width = 14;
			item.height = 24;
			item.rare = 2;
			item.useAnimation = 45;
			item.useTime = 45;
			item.useStyle = 4;
            item.maxStack = 20;
			item.consumable = true;
            item.noUseGraphic = true;
		}

		public override bool CanUseItem(Player player)
		{
			return !Main.dayTime && !NPC.AnyNPCs(mod.NPCType("Starcore"));
		}

		public override bool UseItem(Player player)
		{
			NPC.SpawnOnPlayer(player.whoAmI, mod.NPCType("Starcore"));
			Main.PlaySound(SoundID.Roar, player.position, 0);
			return true;
		}

		public override void AddRecipes()
		{
			/*ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.SuspiciousLookingEye, 1);
			recipe.AddIngredient(null, "CosmicLens", 3);
			recipe.AddTile(TileID.Anvils);
			recipe.SetResult(this);
			recipe.AddRecipe();*/
		}
	}
}
