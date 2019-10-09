using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;

namespace CSkies.Items.Boss
{
    public class ObserverVoidBag : ModItem
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
            item.expert = true; item.expertOnly = true;
		}

        public override int BossBagNPC => mod.NPCType("ObserverVoid");

        public override bool CanRightClick()
		{
			return true;
        }

        public override void OpenBossBag(Player player)
        {
            if (Main.rand.Next(7) == 0)
            {
                player.QuickSpawnItem(mod.ItemType("VOIDMask"));
            }
            string[] lootTableA = { "Singularity", "VoidFan", "VoidShot", "VoidJavelin", "VoidWings" };
            int lootA = Main.rand.Next(lootTableA.Length);
            player.QuickSpawnItem(mod.ItemType(lootTableA[lootA]));

            player.QuickSpawnItem(mod.ItemType<Void.VoidFragment>(), Main.rand.Next(10, 15));

            player.QuickSpawnItem(mod.ItemType("ObserverVoidEye"));
        }

        public override void PostDrawInInventory(SpriteBatch spriteBatch, Vector2 position, Rectangle frame, Color drawColor, Color itemColor, Vector2 origin, float scale)
        {
            Texture2D glow = mod.GetTexture("Glowmasks/ObserverVoidBag_Glow"); 
            spriteBatch.Draw
            (
               glow,
               new Vector2
               (
                   item.position.X - Main.screenPosition.X + item.width * 0.5f,
                   item.position.Y - Main.screenPosition.Y + item.height - glow.Height * 0.5f + 2f
               ),
               new Rectangle(0, 0, glow.Width, glow.Height),
               drawColor,
               0f,
               glow.Size() * 0.5f,
               scale,
               SpriteEffects.None,
               0f
           );
        }

        public override void PostDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, float rotation, float scale, int whoAmI)
        {
            Texture2D glow = mod.GetTexture("Glowmasks/ObserverVoidBag_Glow");
            spriteBatch.Draw
            (
               glow,
               new Vector2
               (
                   item.position.X - Main.screenPosition.X + item.width * 0.5f,
                   item.position.Y - Main.screenPosition.Y + item.height - glow.Height * 0.5f + 2f
               ),
               new Rectangle(0, 0, glow.Width, glow.Height),
               lightColor,
               rotation,
               glow.Size() * 0.5f,
               scale,
               SpriteEffects.None,
               0f
           );
        }
    }
}