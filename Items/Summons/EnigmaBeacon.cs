﻿using Terraria;
using CSkies.NPCs.Bosses.Enigma;
using Terraria.ID;
using Terraria.ModLoader;

namespace CSkies.Items.Summons
{
	public class EnigmaBeacon : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Enigma Beacon");
			Tooltip.SetDefault(@"It's flashing strangely, as if something is tracking it.
Summons Enigma
Non-Consumable");
            ItemID.Sets.SortingPriorityBossSpawns[item.type] = 13;
        }

		public override void SetDefaults()
		{
			item.width = 28;
			item.height = 32;
			item.rare = ItemRarityID.Green;
			item.useAnimation = 45;
			item.useTime = 45;
			item.useStyle = ItemUseStyleID.HoldingUp;
			item.consumable = false;
            item.noUseGraphic = true;
		}

		public override bool CanUseItem(Player player)
		{
			return !NPC.AnyNPCs(mod.NPCType("Enigma"));
		}

		public override bool UseItem(Player player)
		{
            NPC.NewNPC((int)player.position.X + (Main.rand.Next(-200, 200)), (int)player.position.Y - 300, ModContent.NPCType<Enigma>());
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
