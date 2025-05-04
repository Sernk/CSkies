using Terraria;
using Terraria.ModLoader;
using CSkies.Utilities;

namespace CSkies.Content.Buffs
{
    public class VECooldown : ModBuff
	{
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Vortex Burnout");
			// Description.SetDefault("The Eye of the Abyss is disengaged and needs recharging");
			Main.debuff[Type] = true;
			Main.buffNoSave[Type] = true;
		}

		public override void Update(Player player, ref int buffIndex)
		{
            player.GetModPlayer<CPlayer>().VoidCD = true;
        }
	}
}
