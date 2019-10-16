using Microsoft.Xna.Framework;
using Terraria.ModLoader;

namespace CSkies.Items.Star
{
    public class Stelarite : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Stelarite");
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
