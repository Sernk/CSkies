using Terraria.ModLoader.Config;
using System.ComponentModel;

namespace CSkies
{
    public class CConfigClient : ModConfig
    {
        public override ConfigScope Mode => ConfigScope.ClientSide;

        public static CConfigClient Instance; // See ExampleConfigServer.Instance for info.

        [Header("$Mods.CSkies.Config.BuffSettings")]

        [DefaultValue(false)] 
        [LabelKey("$Mods.TremorMod.Config.AllowHealthBuffsTogether")] // Boss Titles
        [TooltipKey("$Mods.TremorMod.Config.AllowHealthBuffsTogetherTooltip")] // "Enables Zelda-Style Boss intros to all the Celestial Skies bosses
        public bool BossIntroText;
    }
}
