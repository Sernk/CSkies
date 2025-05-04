using Terraria.ModLoader;
using Terraria.ID;

namespace CSkies.Content.Items.Boss.Starcore
{
    [AutoloadEquip(EquipType.Head)]
	public class StarcoreMask : ModItem
    {
        public override void SetStaticDefaults()
        {
            base.SetStaticDefaults();
            // DisplayName.SetDefault("Starcore Mask");
		}

        public override void SetDefaults()
        {
            Item.width = 22;
            Item.height = 26;
            Item.rare = ItemRarityID.LightRed;
            Item.vanity = true;
        }
    }
}