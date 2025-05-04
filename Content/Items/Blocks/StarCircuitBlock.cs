using Terraria.ModLoader;
using Terraria.ID;

namespace CSkies.Content.Items.Blocks
{
    public class StarCircuitBlock : ModItem
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
            Item.createTile = Mod.Find<ModWall>("AbyssWall").Type;
        }

        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Starcircuit Block");
        }
    }
}
