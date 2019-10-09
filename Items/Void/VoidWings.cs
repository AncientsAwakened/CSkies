using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Graphics.Shaders;
using Terraria.ID;
using Terraria.ModLoader;

namespace CSkies.Items.Void
{
    [AutoloadEquip(EquipType.Wings)]
	public class VoidWings : ModItem
	{
		public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Void Wings");
            Tooltip.SetDefault(@"Allows flight and slow fall
Holding down while flying allows you to hover");
        }

		public override void SetDefaults()
		{
			item.width = 28;
			item.height = 40;
            item.value = Item.sellPrice(0, 8, 0, 0);
            item.rare = 11;
			item.accessory = true;
		}
		
		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			player.wingTimeMax = 200;
		}

		public override void VerticalWingSpeeds(Player player, ref float ascentWhenFalling, ref float ascentWhenRising,
			ref float maxCanAscendMultiplier, ref float maxAscentMultiplier, ref float constantAscend)
		{
			ascentWhenFalling = 0.95f;
			ascentWhenRising = 0.15f;
			maxCanAscendMultiplier = 1f;
			maxAscentMultiplier = 4f;
			constantAscend = 0.17f;
		}

        public override void HorizontalWingSpeeds(Player player, ref float speed, ref float acceleration)
        {
            if (player.controlDown && player.controlJump && player.wingTime > 0f)
            {
                speed = 15f;
                acceleration *= 10f;
            }
            else
            {
                speed = 10f;
                acceleration *= 6.25f;
            }
        }

        public override bool WingUpdate(Player player, bool inUse)
        {
            if (player.controlDown && player.controlJump && player.wingTime > 0f && !player.merman)
            {
                player.velocity.Y *= 0.01f;
                if (player.velocity.Y > -2f && player.velocity.Y < 1f)
                {
                    player.velocity.Y = 1E-05f;
                }
            }

            if (inUse || player.jump > 0)
            {
                player.wingFrameCounter++;
                if (player.wingFrameCounter > 3)
                {
                    player.wingFrame++;
                    player.wingFrameCounter = 0;
                    if (player.wingFrame >= 4)
                    {
                        player.wingFrame = 0;
                    }
                }
            }
            else if (player.velocity.Y != 0f)
            {
                player.wingFrame = 1;
                if (player.wings == 32)
                {
                    player.wingFrame = 3;
                }
                if (player.wings == 29 && Main.rand.Next(5) == 0)
                {
                    int num92 = 4;
                    if (player.direction == 1)
                    {
                        num92 = -40;
                    }
                    int num93 = Dust.NewDust(new Vector2(player.position.X + (float)(player.width / 2) + (float)num92, player.position.Y + (float)(player.height / 2) - 15f), 30, 30, mod.DustType<Dusts.VoidDust>(), 0f, 0f, 100, default, 2.4f);
                    Main.dust[num93].noGravity = true;
                    Main.dust[num93].velocity *= 0.3f;
                    if (Main.rand.Next(10) == 0)
                    {
                        Main.dust[num93].fadeIn = 2f;
                    }
                    Main.dust[num93].shader = GameShaders.Armor.GetSecondaryShader(player.cWings, player);
                }
            }
            else
            {
                player.wingFrame = 0;
            }
            return true;
        }

        public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(null, "VoidFragment", 5);
            recipe.AddIngredient(ItemID.SoulofFlight, 15);
            recipe.AddTile(TileID.LunarCraftingStation);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
}