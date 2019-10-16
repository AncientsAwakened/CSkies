using Terraria;
using Terraria.ID;
using Microsoft.Xna.Framework;
using Terraria.ModLoader;

namespace CSkies.Items.Void
{
    public class VoidFragment : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Void Fragment");
            Tooltip.SetDefault("A fragment from a broken world");
            ItemID.Sets.ItemIconPulse[item.type] = true;
            ItemID.Sets.ItemNoGravity[item.type] = true;
        }

        public override void SetDefaults()
        {
            item.width = 22;
            item.height = 22;
			item.maxStack = 99;
            item.rare = 2;
            item.value = 10000;
        }

        public override Color? GetAlpha(Color lightColor)
        {
            return Color.White;
        }

        public override void PostUpdate()
        {
            Lighting.AddLight(item.Center, Color.Black.ToVector3() * 0.55f * Main.essScale);
        }
    }
}
