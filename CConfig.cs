using Terraria.ModLoader.Config;
using System.IO;
using Terraria;
using Terraria.IO;
using Terraria.ModLoader;
using System.ComponentModel;
using System.Collections.Generic;

namespace CSkies
{
    public class CConfigClient : ModConfig
    {
        public override ConfigScope Mode => ConfigScope.ClientSide;

        public static CConfigClient Instance; // See ExampleConfigServer.Instance for info.

        [DefaultValue(true)]
        [Label("Boss Titles")]
        [Tooltip("Enables Zelda-Style Boss intros to all the Celestial Skies bosses")]
        public bool BossIntroText;
    }
}
