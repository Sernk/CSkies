using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CSkies.Content.Items.Materials
{
    public class CometBar : ModItem
    {
        public override void SetDefaults()
        {
            Item.width = 30;
            Item.height = 24;
            Item.maxStack = 99;
			Item.useTurn = true;
            Item.autoReuse = true;
            Item.useAnimation = 15;
            Item.value = 16000;
            Item.rare = ItemRarityID.Green;
            Item.useTime = 10;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.consumable = true;
        }

        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Comet Bar");
            // Tooltip.SetDefault("Said to be a piece of the sky itself");
        }

		public override void AddRecipes()
        {  
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ModContent.ItemType<CometOre>(), 3);
            recipe.AddTile(TileID.Hellforge);
            recipe.Register();
        }
    }
}
