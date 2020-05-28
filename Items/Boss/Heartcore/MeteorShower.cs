using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CSkies.Items.Boss.Heartcore
{
    public class MeteorShower : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Meteor Shower");
			Tooltip.SetDefault("Summons flaming meteorites from the sky");
			Item.staff[item.type] = true;
		}

	    public override void SetDefaults()
	    {
	        item.damage = 180;
	        item.magic = true;
	        item.mana = 12;
	        item.width = 50;
	        item.height = 50;
	        item.useTime = 10;
	        item.useAnimation = 10;
	        item.useStyle = ItemUseStyleID.HoldingOut;
	        item.noMelee = true;
	        item.knockBack = 6.75f;
            item.value = Item.sellPrice(0, 12, 0, 0);
            item.rare = ItemRarityID.Red;
            item.UseSound = SoundID.Item88;
	        item.autoReuse = true;
	        item.shoot = mod.ProjectileType("Meteor0");
	        item.shootSpeed = 20f;
		}

	    public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(null, "StormStaff", 1);
            recipe.AddIngredient(null, "HeartSoul", 8);
            recipe.AddTile(TileID.LunarCraftingStation);
	        recipe.SetResult(this);
	        recipe.AddRecipe();
		}

	    public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
	    {
			float speed = item.shootSpeed;
			int num112 = Main.rand.Next(2, 4);
			for (int num113 = 0; num113 < num112; num113++)
			{
                Vector2 vector2 = new Vector2(player.position.X + player.width * 0.5f + Main.rand.Next(201) * -(float)player.direction + (Main.mouseX + Main.screenPosition.X - player.position.X), player.MountedCenter.Y - 600f);
				vector2.X = (vector2.X + player.Center.X) / 2f + Main.rand.Next(-200, 201);
				vector2.Y -= 100 * num113;
				float posX = Main.mouseX + Main.screenPosition.X - vector2.X + Main.rand.Next(-40, 41) * 0.03f;
                float posY = Main.mouseY + Main.screenPosition.Y - vector2.Y;
				if (posY < 0f)
				{
					posY *= -1f;
				}
				if (posY < 20f)
				{
					posY = 20f;
				}
				float pos = (float)Math.Sqrt(posX * posX + posY * posY);
				pos = speed / pos;
				posX *= pos;
				posY *= pos;
				float originX = posX;
				float originY = posY + Main.rand.Next(-40, 41) * 0.02f;
				Projectile.NewProjectile(vector2.X, vector2.Y, originX * 0.75f, originY * 0.75f, type, damage, knockBack, player.whoAmI, 0f, 0.5f + (float)Main.rand.NextDouble() * 0.3f);
			}
			return false;
		}


        public override void PostDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, float rotation, float scale, int whoAmI)
        {
            Texture2D texture = mod.GetTexture("Glowmasks/MeteorShower_Glow");
            spriteBatch.Draw
            (
                texture,
                new Vector2
                (
                    item.position.X - Main.screenPosition.X + item.width * 0.5f,
                    item.position.Y - Main.screenPosition.Y + item.height - texture.Height * 0.5f + 2f
                ),
                new Rectangle(0, 0, texture.Width, texture.Height),
                Color.White,
                rotation,
                texture.Size() * 0.5f,
                scale,
                SpriteEffects.None,
                0f
            );
        }


        public override void ModifyTooltips(System.Collections.Generic.List<TooltipLine> list)
        {
            foreach (TooltipLine line2 in list)
            {
                if (line2.mod == "Terraria" && line2.Name == "ItemName")
                {
                    line2.overrideColor = Colors.Rarity12;
                }
            }
        }
    }
}
