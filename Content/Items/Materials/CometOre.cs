using Terraria.ModLoader;
using Terraria.ID;
using CSkies.Content.Tiles;

namespace CSkies.Content.Items.Materials
{
    public class CometOre : ModItem
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Comet Ore");
        }

        public override void SetDefaults()
        {
            Item.width = 22;
            Item.height = 22;
            Item.maxStack = 99;
            Item.rare = ItemRarityID.Blue;
            Item.maxStack = 9999;
            Item.useTurn = true;
            Item.autoReuse = true;
            Item.useAnimation = 15;
            Item.useTime = 10;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.rare = 1;
            Item.value = Terraria.Item.sellPrice(0, 0, 8, 0);
            Item.consumable = true;
            Item.createTile = ModContent.TileType<CometOreTile>();
        }
    }
}
