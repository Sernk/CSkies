using Terraria;
using Terraria.ModLoader;
using CSkies.Utilities;
using CSkies.Content.Projectiles.Minions;

namespace CSkies.Content.Buffs
{
    public class Gazer : ModBuff
	{
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Gazer");
			// Description.SetDefault(@"Summons an abyss gazer to fight for you");
			Main.buffNoSave[Type] = true;
			Main.buffNoTimeDisplay[Type] = true;
		}

		public override void Update(Player player, ref int buffIndex)
		{
            CPlayer modPlayer = player.GetModPlayer<CPlayer>();
			if (player.ownedProjectileCounts[ModContent.ProjectileType<Projectiles.Minions.Gazer>()] > 0)
			{
				modPlayer.Gazer = true;
			}
			if (!modPlayer.Gazer)
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