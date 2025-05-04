using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using CSkies.Utilities;

namespace CSkies.Content.Items.Boss.Void
{
    public class ObserverVoidEye : ModItem
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Eye of the Abyss");
            /* Tooltip.SetDefault(@"Provides spelunker, night vision, dangersense, and hunter
10% increased critical strike chance
Pressing the accessory ability key will cause a vortex that drags in enemies within 10 blocks of you to appear
You can only use this ability once every 5 minutes"); */
        }

        public override void SetDefaults()
        {
            Item.width = 30;
            Item.height = 28;
            Item.value = Item.sellPrice(0, 10, 0, 0);
            Item.rare = ItemRarityID.Purple;
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
            
            player.GetModPlayer<CPlayer>().VoidEye = true;
        }
    }
}