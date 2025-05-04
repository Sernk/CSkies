using Terraria;
using Terraria.ModLoader;
using CSkies.Utilities;
using CSkies.Content.Projectiles.Minions;

namespace CSkies.Content.Buffs
{
    public class Rune : ModBuff
	{
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Fury Rune");
			// Description.SetDefault(@"Summons a fury rune to fight for you");
			Main.buffNoSave[Type] = true;
			Main.buffNoTimeDisplay[Type] = true;
		}

		public override void Update(Player player, ref int buffIndex)
		{
            CPlayer modPlayer = player.GetModPlayer<CPlayer>();
			if (player.ownedProjectileCounts[ModContent.ProjectileType<HeartRune>()] > 0)
			{
				modPlayer.Rune = true;
			}
			if (!modPlayer.Rune)
			{
				player.DelBuff(buffIndex);
				buffIndex--;
			}
			else
			{
				player.buffTime[buffIndex] = 18000;
			}
		}
	}
}