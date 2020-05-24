using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CSkies.Items.Star
{
    public class StormStaff : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Starstorm");
			Tooltip.SetDefault("Summons electrically charged stars from the sky");
			Item.staff[item.type] = true;
		}

	    public override void SetDefaults()
	    {
	        item.damage = 30;
	        item.magic = true;
	        item.mana = 5;
	        item.width = 50;
	        item.height = 50;
	        item.useTime = 40;
	        item.useAnimation = 40;
	        item.useStyle = ItemUseStyleID.HoldingOut;
	        item.noMelee = true;
	        item.knockBack = 6.75f;
            item.value = Item.buyPrice(0, 2, 0, 0);
            item.rare = ItemRarityID.Pink;
            item.UseSound = SoundID.Item20;
            item.autoReuse = true;
	        item.shoot = mod.ProjectileType("Starstorm");
	        item.shootSpeed = 10f;
		}

	    public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.MeteorStaff);
            recipe.AddIngredient(mod, "Stelarite", 5);
            recipe.AddIngredient(mod, "CosmicStar", 2);
            recipe.AddTile(TileID.MythrilAnvil);
	        recipe.SetResult(this);
	        recipe.AddRecipe();
		}

	    public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
	    {
			float speed = item.shootSpeed;
			int num112 = Main.rand.Next(1, 3);
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
    }
}
