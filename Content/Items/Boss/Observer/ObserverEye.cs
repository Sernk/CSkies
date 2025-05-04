using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace CSkies.Content.Items.Boss.Observer
{
    public class ObserverEye : ModItem
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Eye of the Observer");
            // Tooltip.SetDefault(@"Provides spelunker, night vision, dangersense, and hunter");
        }

        public override void SetDefaults()
        {
            Item.width = 30;
            Item.height = 28;
            Item.value = Item.sellPrice(0, 1, 0, 0);
            Item.rare = ItemRarityID.Green;
            Item.accessory = true;
            Item.expert = true;
            Item.expertOnly = true;
        }

        public override void UpdateEquip(Player player)
        {
            player.findTreasure = true;
            player.dangerSense = true;
            player.nightVision = true;
            player.detectCreature = true;
        }
    }
}