using Terraria.ID;
using Microsoft.Xna.Framework;
using Terraria.ModLoader;

namespace CSkies.Items.Materials
{
    public class CosmicStar : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Cosmic Star");
            Tooltip.SetDefault("A star said to be from the edges of the universe");
            ItemID.Sets.ItemIconPulse[item.type] = true;
            ItemID.Sets.ItemNoGravity[item.type] = true;
        }

        public override void SetDefaults()
        {
            item.width = 22;
            item.height = 22;
			item.maxStack = 99;
            item.rare = 5;
            item.value = 10000;
        }

        public override Color? GetAlpha(Color lightColor)
        {
            return Colors.COLOR_GLOWPULSE;
        }
    }
}
