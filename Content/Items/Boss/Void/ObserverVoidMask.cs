using Terraria.ModLoader;
using Terraria.ID;

namespace CSkies.Content.Items.Boss.Void
{
    [AutoloadEquip(EquipType.Head)]
    public class ObserverVoidMask : ModItem
    {
        public override void SetStaticDefaults()
        {
            base.SetStaticDefaults();
            // DisplayName.SetDefault("Observer Void Mask");
        }

        public override void SetDefaults()
        {
            Item.width = 22;
            Item.height = 26;
            Item.rare = ItemRarityID.Purple;
            Item.vanity = true;
        }
    }
}