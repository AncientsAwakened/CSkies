using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Graphics.Shaders;
using Terraria.ID;
using Terraria.ModLoader;

namespace CSkies.Items.Summons
{
    public class PassionRune : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Rune of Passion");
			Tooltip.SetDefault(@"The heart on it glows at the same rate your own beats
Summons Heartcore
Can only be used at night");
            ItemID.Sets.SortingPriorityBossSpawns[item.type] = 13;
        }

		public override void SetDefaults()
		{
			item.width = 40;
			item.height = 40;
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
			return !Main.dayTime && !NPC.AnyNPCs(mod.NPCType("Heartcore"));
		}

		public override bool UseItem(Player player)
		{
			NPC.SpawnOnPlayer(player.whoAmI, mod.NPCType("Heartcore"));
			Main.PlaySound(SoundID.Roar, player.position, 0);
			return true;
		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.SuspiciousLookingEye, 1);
			recipe.AddIngredient(null, "MoltenHeart", 3);
			recipe.AddTile(TileID.LunarCraftingStation);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}

        public override void PostDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, float rotation, float scale, int whoAmI)
        {
            int r = GameShaders.Armor.GetShaderIdFromItemId(ItemID.LivingFlameDye);
            Texture2D texture = mod.GetTexture("Glowmasks/" + GetType().Name + "_Glow");
			Rectangle frame = BaseDrawing.GetFrame(0, texture.Width, texture.Height, 0, 0);
			BaseDrawing.DrawTexture(spriteBatch, texture, r, item.position, item.width, item.height, item.scale, 0f, 0, 1, frame);
		}

        public override void PostDrawInInventory(SpriteBatch spriteBatch, Vector2 position, Rectangle frame, Color drawColor, Color itemColor, Vector2 origin, float scale)
        {
            int r = GameShaders.Armor.GetShaderIdFromItemId(ItemID.LivingFlameDye);
            Texture2D texture = mod.GetTexture("Glowmasks/" + GetType().Name + "_Glow");
            BaseDrawing.DrawTexture(spriteBatch, texture, r, position, item.width, item.height, item.scale, 0f, 0, 1, frame);
        }
    }
}
