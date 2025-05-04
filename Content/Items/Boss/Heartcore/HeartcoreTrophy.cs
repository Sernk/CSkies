using Terraria.ModLoader;
using Terraria.ID;

namespace CSkies.Content.Items.Boss.Heartcore
{
    public class HeartcoreTrophy : ModItem
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Heartcore Trophy");
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
			Item.createTile = Mod.Find<ModTile>("HeartcoreTrophyTile").Type;
		}
    }
}