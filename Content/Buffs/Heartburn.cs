using Terraria;
using Terraria.ModLoader;
using CSkies.Utilities;
using Terraria.ID;

namespace CSkies.Content.Buffs
{
    public class Heartburn : ModBuff
	{
		public override void SetStaticDefaults()
		{
			Main.debuff[Type] = true;
			Main.pvpBuff[Type] = false;
			Main.buffNoSave[Type] = true;
            BuffID.Sets.LongerExpertDebuff[Type] = true;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            player.GetModPlayer<CPlayer>().Heartburn = true;
        }
	}
}
