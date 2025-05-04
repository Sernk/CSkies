using Terraria.ModLoader;
using Terraria.ID;
using CSkies.Utilities;
using Terraria;
using CSkies.Content.Items.Materials;
using CSkies.Content.Tiles.HeartAltars;

namespace CSkies.Content.Items.Blocks
{
    public class MagmaAltar : ModItem
	{
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Magma Altar");
        }

        public override void SetDefaults()
		{
			Item.useStyle = ItemUseStyleID.Swing;
			Item.useTurn = true;
			Item.useAnimation = 15;
			Item.useTime = 10;
			Item.autoReuse = true;
			Item.consumable = true;
			Item.createTile = ModContent.TileType<HeartAltar>();
			Item.width = 24;
			Item.height = 24;
            Item.rare = ModContent.RarityType<CSkiesRarity>();
            Item.value = 10000;
			Item.accessory = true;
		}

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ItemID.LifeCrystal, 5);
            recipe.AddIngredient(ItemID.Meteorite, 20);
            recipe.AddIngredient(ModContent.ItemType<MoltenHeart>(), 1);
            recipe.AddTile(TileID.LunarCraftingStation);
            recipe.Register();
        }
    }
}
