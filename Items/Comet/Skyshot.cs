using Terraria.ID;
using Terraria.ModLoader;

namespace CSkies.Items.Comet
{
    public class Skyshot : ModItem
    {
        public override void SetDefaults()
        {
            item.damage = 19;
            item.magic = true;
            item.mana = 12;
            item.width = 8;
            item.height = 8;
            item.useTime = 30;
            item.useAnimation = 30;
            item.useStyle = ItemUseStyleID.HoldingOut;
            item.noMelee = true;
            item.knockBack = 2;
            item.value = 1000;
            item.rare = ItemRarityID.Green;
            item.UseSound = SoundID.Item8;
            item.autoReuse = true;
            item.shoot = mod.ProjectileType("CometShot");
            item.shootSpeed = 8f;
        }

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Skyshot");
            Tooltip.SetDefault("Fires a homing comet towards your cursor");
        }
    }
}