using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace CSkies.Items.Summons
{
	public class VoidEye : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Void Eye");
			Tooltip.SetDefault(@"The abyss stares back
Summons Observer Void
Can only be used at night");
            Main.RegisterItemAnimation(item.type, new DrawAnimationVertical(6, 4));
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
			return !Main.dayTime && !NPC.AnyNPCs(mod.NPCType("ObserverVoid"));
		}

		public override bool UseItem(Player player)
		{
			NPC.SpawnOnPlayer(player.whoAmI, mod.NPCType("ObserverVoid"));
			Main.PlaySound(SoundID.Roar, player.position, 0);
			return true;
		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(null, "VoidLens", 5);
            recipe.AddIngredient(null, "CosmicEye", 1);
			recipe.AddTile(TileID.LunarCraftingStation);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
}
