using Terraria.ModLoader;
using Terraria.ID;

namespace CSkies.Content.Items.Boss.Void
{
    [AutoloadEquip(EquipType.Head)]
    public class VOIDMask : ModItem
    {
        public override void SetStaticDefaults()
        {
            base.SetStaticDefaults();
            // DisplayName.SetDefault("VOID Mask");
        }

        public override void SetDefaults()
        {
            Item.width = 22;
            Item.height = 26;
            Item.rare = ItemRarityID.Purple;
            Item.vanity = true;
            Item.expert = true;
            Item.expertOnly = true;
        }
    }
}