using Microsoft.Xna.Framework;
using Terraria;

namespace CSkies
{
    public class Colors
    {
        public static Color COLOR_GLOWPULSE => Color.White * (Main.mouseTextColor / 255f);

        public static Color Flash => BaseUtility.MultiLerpColor(Main.LocalPlayer.miscCounter % 100 / 100f, Color.Transparent, Color.White, Color.Transparent);
    }

    public static class CUtils
    {
        public static void DropLoot(this Entity ent, int type, int stack = 1)
        {
            Item.NewItem(ent.Hitbox, type, stack);
        }

        public static void DropLoot(this Entity ent, int type, float chance)
        {
            if (Main.rand.NextDouble() < chance)
            {
                Item.NewItem(ent.Hitbox, type);
            }
        }

        public static void DropLoot(this Entity ent, int type, int min, int max)
        {
            Item.NewItem(ent.Hitbox, type, Main.rand.Next(min, max));
        }

        public static bool AnyProjectiles(int type)
        {
            for (int i = 0; i < Main.maxProjectiles; i++)
            {
                if (Main.projectile[i].active && Main.projectile[i].type == type)
                {
                    return true;
                }
            }
            return false;
        }

        public static int CountProjectiles(int type)
        {
            int p = 0;
            for (int i = 0; i < Main.maxProjectiles; i++)
            {
                if (Main.projectile[i].active && Main.projectile[i].type == type)
                {
                    p++;
                }
            }
            return p;
        }
    }
}
