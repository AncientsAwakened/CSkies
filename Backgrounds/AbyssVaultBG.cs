using Terraria;
using Terraria.ModLoader;

namespace CSkies.Backgrounds
{
    public class AbyssVaultBG : ModUgBgStyle
    {
        public override bool ChooseBgStyle()
        {
            return !Main.gameMenu && Main.LocalPlayer.GetModPlayer<CPlayer>().ZoneVoid;
        }

        public override void FillTextureArray(int[] textureSlots)
        {
            textureSlots[0] = mod.GetBackgroundSlot("Backgrounds/AbyssVaultBG");
            textureSlots[1] = mod.GetBackgroundSlot("Backgrounds/AbyssVaultBG");
            textureSlots[2] = mod.GetBackgroundSlot("Backgrounds/AbyssVaultBG");
            textureSlots[3] = mod.GetBackgroundSlot("Backgrounds/AbyssVaultBG");
        }
    }
}
