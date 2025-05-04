using Terraria.ModLoader;
using Terraria.ID;

namespace CSkies.Content.Items.Blocks
{
    public class Starglass : ModItem
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
            Item.rare = ItemRarityID.Lime;
            //Item.createTile = Mod.Find<ModTile>("Starglass").Type;
        }

        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Starglass");
        }

    }
}