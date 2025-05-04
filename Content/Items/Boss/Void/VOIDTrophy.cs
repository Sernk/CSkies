using Terraria.ModLoader;
using Terraria.ID;

namespace CSkies.Content.Items.Boss.Void
{
    public class VOIDTrophy : ModItem
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("VOID Trophy");
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
            Item.rare = ItemRarityID.Purple;
            Item.useStyle = ItemUseStyleID.Swing;
			Item.consumable = true;
			Item.value = 2000;
			Item.rare = 1;
			Item.createTile = ModContent.TileType<Tiles.VOIDTrophy>();
            Item.expert = true;
            Item.expertOnly = true;
        }
    }
}