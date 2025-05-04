using Terraria;
using Terraria.ModLoader;
using CSkies.Utilities;

namespace CSkies.Content.Buffs
{
    public class Watcher : ModBuff
	{
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Watcher");
			// Description.SetDefault(@"Summons a watcher to fight for you");
			Main.buffNoSave[Type] = true;
			Main.buffNoTimeDisplay[Type] = true;
		}

		public override void Update(Player player, ref int buffIndex)
		{
            CPlayer modPlayer = player.GetModPlayer<CPlayer>();
			if (player.ownedProjectileCounts[ModContent.ProjectileType<Projectiles.Minions.Watcher>()] > 0)
			{
				modPlayer.Watcher = true;
			}
			if (!modPlayer.Watcher)
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