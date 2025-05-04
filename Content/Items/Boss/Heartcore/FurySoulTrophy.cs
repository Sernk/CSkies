using Terraria.ModLoader;
using Terraria.ID;
using CSkies.Content.Tiles;

namespace CSkies.Content.Items.Boss.Heartcore
{
    public class FurySoulTrophy : ModItem
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Fury Soul Trophy");
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
            Item.useStyle = ItemUseStyleID.Swing;
			Item.consumable = true;
			Item.value = 2000;
			Item.rare = ItemRarityID.Blue;
			Item.createTile = ModContent.TileType<Tiles.FurySoulTrophy>();
            Item.expert = true;
            Item.expertOnly = true;
        }
    }
}