using Terraria.ID;
using Microsoft.Xna.Framework;
using Terraria.ModLoader;

namespace CSkies.Content.Items.Materials
{
    public class CosmicStar : ModItem
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Cosmic Star");
            // Tooltip.SetDefault("A star said to be from the edges of the universe");
            ItemID.Sets.ItemIconPulse[Item.type] = true;
            ItemID.Sets.ItemNoGravity[Item.type] = true;
        }

        public override void SetDefaults()
        {
            Item.width = 22;
            Item.height = 22;
			Item.maxStack = 99;
            Item.rare = ItemRarityID.Pink;
            Item.value = 10000;
        }

        public override Color? GetAlpha(Color lightColor)
        {
            return Color.White * 0.8f; 
        }
    }
}
