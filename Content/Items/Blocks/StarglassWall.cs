using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using CSkies.Content.Items.Blocks;

namespace CSkies.Content.Items.Blocks
{
    public class StarglassWall : ModItem
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
            //Item.createWall = Mod.Find<ModWall>("StarglassWall").Type;
        }

        
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("StarglassWall Wall");
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe(4);
            recipe.AddIngredient(ModContent.ItemType<Starglass>());
            recipe.AddTile(TileID.WorkBenches);
            recipe.Register();
        }
    }
}
