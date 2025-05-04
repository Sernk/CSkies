using System.Collections.Generic;
using Terraria.ModLoader;
using Terraria.ID;
using CSkies.Utilities;
using Terraria;

namespace CSkies.Content.Items.Boss.Heartcore
{
    [AutoloadEquip(EquipType.Head)]
	public class HeartcoreMask : ModItem
    {
        public override void SetStaticDefaults()
        {
            base.SetStaticDefaults();
            // DisplayName.SetDefault("Heartcore Mask");
		}

        public override void SetDefaults()
        {
            Item.width = 22;
            Item.height = 26;
            Item.rare = ModContent.RarityType<CSkiesRarity>();
            Item.vanity = true;
        }
    }
}