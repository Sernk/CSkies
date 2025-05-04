using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace CSkies.Content.Items.Blocks
{
    public class StarCircuitWall : ModItem
    {
        public override void SetDefaults()
        {

            Item.width = 16;
            Item.height = 16;
            Item.maxStack = 999;
            Item.useTurn = true;
            Item.autoReuse = true;
            Item.useAnimation = 15;
            Item.useTime = 10;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.consumable = true;
            Item.createWall = Mod.Find<ModWall>("AbyssStoneWall").Type;
        }


        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("StarcircuitWall Wall");
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe(4);
            recipe.AddIngredient(ModContent.ItemType<StarCircuitBlock>());
            recipe.AddTile(TileID.WorkBenches);
            recipe.Register();
        }
    }
}