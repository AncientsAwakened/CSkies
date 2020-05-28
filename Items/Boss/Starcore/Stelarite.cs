using Microsoft.Xna.Framework;
using Terraria.ModLoader;
using Terraria.ID;

namespace CSkies.Items.Boss.Starcore
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
            item.rare = ItemRarityID.Pink;
            item.value = 10000;
        }

        public override Color? GetAlpha(Color lightColor)
        {
            return Colors.COLOR_GLOWPULSE;
        }
    }
}
