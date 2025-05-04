using Microsoft.Xna.Framework;
using Terraria.ModLoader;
using Terraria.ID;

namespace CSkies.Content.Items.Boss.Starcore
{
    public class Stelarite : ModItem
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Starsteel");
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
