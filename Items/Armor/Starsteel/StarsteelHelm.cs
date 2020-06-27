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

		public override bool IsArmorSet(Item head, Item body, Item legs)
		{
			return body.type == mod.ItemType("StarsteelPlate") && legs.type == mod.ItemType("StarsteelBoots") ;
        }

        public override void UpdateArmorSet(Player player)
		{
			CPlayer modPlayer = player.GetModPlayer<CPlayer>();
			modPlayer.Starsteel = true;

			if (modPlayer.StarsteelBonus == 1)
			{
				player.setBonus = @"+9% Melee Speed
Melee critical hits will cause a piercing star to fall";
				item.defense = 23;
				player.meleeSpeed += .09f;
				return;
			}
			if (modPlayer.StarsteelBonus == 2)
			{
				player.setBonus = @"20% reduced ammo consumption
Ranged critical hits will cause a piercing star to fall";
				player.ammoCost80 = true;
				item.defense = 5;
				return;
			}
			if (modPlayer.StarsteelBonus == 3)
			{
				player.setBonus = @"Increases maximum mana by 90
Magic critical hits will cause a piercing star to fall";
				item.defense = 9;
				player.statManaMax2 += 90;
				return;
			}
			if (modPlayer.StarsteelBonus == 4)
			{
				player.setBonus = @"A guardian star watches over you by firing mini stars at enemies.";
				item.defense = 2; 
				if (player.whoAmI == Main.myPlayer)
				{
					if (player.ownedProjectileCounts[ModContent.ProjectileType<StarGuardian>()] < 1)
					{
						Projectile.NewProjectile(player.Center.X, player.Center.Y, 0f, -1f, ModContent.ProjectileType<StarGuardian>(), (int)(60 * player.minionDamage), 0f, Main.myPlayer, 0f, 0f);
					}
				}
				return;
			}

			player.setBonus = @"ERROR: AUGMENT SLOT EMPTY. 
PLEASE EQUIP AUGMENT TO ACTIVATE ABILITY SYSTEM";
			modPlayer.StarsteelBonus = 0;
		}
		
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(null, "Stelarite", 10);
            recipe.AddTile(TileID.MythrilAnvil);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
}