using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace CSkies.Items.Void
{
    public class VoidPortal : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Abyss Portal");
            Tooltip.SetDefault(@"Summons an abyss gazer to fight for you");
            Main.RegisterItemAnimation(item.type, new DrawAnimationVertical(5, 4));
            ItemID.Sets.ItemNoGravity[item.type] = true;
        }

        public override Color? GetAlpha(Color lightColor)
        {
            return Colors.COLOR_GLOWPULSE;
        }

        public override void SetDefaults()
        {
            item.damage = 170;
            item.summon = true;
            item.mana = 10;
            item.width = 24;
            item.height = 24;
            item.useTime = 36;
            item.useAnimation = 36;
            item.useStyle = 1;
            item.noUseGraphic = true;
            item.noMelee = true;
            item.knockBack = 3;
            item.UseSound = SoundID.Item44;
            item.shoot = mod.ProjectileType("Gazer");
            item.shootSpeed = 10f;
            item.buffType = mod.BuffType("Gazer");
            item.autoReuse = true;
            item.rare = 11;
            item.value = Item.sellPrice(0, 1, 0, 0);
        }
		
		public override void UseStyle(Player player)
		{
			if (player.whoAmI == Main.myPlayer && player.itemTime == 0)
			{
				player.AddBuff(item.buffType, 3600, true);
			}
		}

        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            int i = Main.myPlayer;
            float num74 = player.GetWeaponKnockback(item, knockBack);
            player.itemTime = item.useTime;
            Vector2 vector2 = player.RotatedRelativePoint(player.MountedCenter, true);
            vector2.X = Main.mouseX + Main.screenPosition.X;
            vector2.Y = Main.mouseY + Main.screenPosition.Y;
            Projectile.NewProjectile(vector2.X, vector2.Y, 0, 0, mod.ProjectileType("Gazer"), damage, num74, i, 0f, 0f);
            return false;
        }

        public override void PostUpdate()
        {
            Lighting.AddLight(item.Center, Colors.COLOR_GLOWPULSE.ToVector3() * 0.55f * Main.essScale);
        }
    }
}