using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using CSkies.Utilities;

namespace CSkies.Content.Buffs
{
    public class Cometspark : ModBuff
	{
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Cometspark");
			// Description.SetDefault("Spreads to nearby allies, stay away!");
			Main.debuff[Type] = true;
			Main.pvpBuff[Type] = true;
			Main.buffNoSave[Type] = true;
			BuffID.Sets.LongerExpertDebuff[Type] = true;
        }

		public override void Update(Player player, ref int buffIndex)
		{
            player.GetModPlayer<CPlayer>().Cometspark = true;
            player.statDefense -= 25;
		}

		public override void Update(NPC npc, ref int buffIndex)
		{
			//npc.GetGlobalNPC<CGlobalNPC>().Cometspark = true;
            //npc.defense -= 25;
		}
	}
}
