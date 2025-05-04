using Terraria.ModLoader;
using Terraria.ID;

namespace CSkies.Content.Items.Boss.Observer
{
    public class ObserverTrophy : ModItem
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Observer Trophy");
		}

        public override void SetDefaults()
		{
			Item.width = 32;
			Item.height = 32;
			Item.maxStack = 99;
			Item.useTurn = true;
			Item.autoReuse = true;
			Item.useAnimation = 15;
			Item.useTime = 10;
            Item.rare = ItemRarityID.Blue;
            Item.useStyle = ItemUseStyleID.Swing;
			Item.consumable = true;
			Item.value = 2000;
			Item.rare = 1;
			Item.createTile = ModContent.TileType<Content.Tiles.ObserverTrophy>();
		}
    }
}