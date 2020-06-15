using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace CSkies.Items.Armor.Starsteel
{
	[AutoloadEquip(EquipType.Head)]
	public class StarsteelHelm : ModItem
	{
		public override void SetStaticDefaults()
		{
			base.SetStaticDefaults();
			DisplayName.SetDefault("Starsteel Helmet");
			Tooltip.SetDefault(@"Changes stats based on which Starsteel Augment is equipped");
		}

		public override void SetDefaults()
		{
			item.width = 22;
			item.height = 24;
            item.value = Item.sellPrice(0, 3, 0, 0);
            item.rare = ItemRarityID.LightRed;
		}

		public override void UpdateEquip(Player player)
		{
			player.findTreasure = true;
			player.pickSpeed -= 0.15f;

			Lighting.AddLight((int)player.Center.X, (int)player.Center.Y, 1f, 0.95f, .8f);
		}

		public override bool IsArmorSet(Item head, Item body, Item legs)
		{
			return body.type == mod.ItemType("StoneSoldierPlate") && legs.type == mod.ItemType("StoneSoldierGreaves") ;
        }

        public override void UpdateArmorSet(Player player)
		{
			bool Melee = BasePlayer.HasAccessory(player, ModContent.ItemType<StarsteelAugmentMe>(), true, false);
			bool Ranged = BasePlayer.HasAccessory(player, ModContent.ItemType<StarsteelAugmentMe>(), true, false);
			bool Magic = BasePlayer.HasAccessory(player, ModContent.ItemType<StarsteelAugmentMe>(), true, false);
			bool Summon = BasePlayer.HasAccessory(player, ModContent.ItemType<StarsteelAugmentMe>(), true, false);

			CPlayer modPlayer = player.GetModPlayer<CPlayer>();
			modPlayer.Starsteel = true;

			if (Melee)
			{
				player.setBonus = @"+9% Melee Speed
Melee critical hits will cause a piercing star to fall";
				modPlayer.StarsteelBonus = 1;
				item.defense = 23;
				player.meleeSpeed += .09f;
				return;
			}
			if (Ranged)
			{
				player.setBonus = @"20% reduced ammo consumption
Ranged critical hits will cause a piercing star to fall";
				player.ammoCost80 = true;
				modPlayer.StarsteelBonus = 2;
				item.defense = 5;
				return;
			}
			if (Magic)
			{
				player.setBonus = @"Increases maximum mana by 90
Magic critical hits will cause a piercing star to fall";
				modPlayer.StarsteelBonus = 3;
				item.defense = 9;
				player.statManaMax2 += 90;
				return;
			}
			if (Summon)
			{
				player.setBonus = @"A guardian star watches over you by firing mini stars at enemies.";
				modPlayer.StarsteelBonus = 4;
				item.defense = 2;
				return;
			}

			player.setBonus = @"ERROR: AUGMENT SLOT EMPTY. 
PLEASE EQUIP AUGMENT TO ACTIVATE ABILITY SYSTEM";
			modPlayer.StarsteelBonus = 0;
		}
		
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.MiningHelmet);
            recipe.AddIngredient(null, "StoneShell", 6);
            recipe.AddTile(TileID.MythrilAnvil);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
}