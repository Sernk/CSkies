using Terraria;
using Terraria.ID;
using Microsoft.Xna.Framework;
using Terraria.ModLoader;

namespace CSkies.Content.Items.Boss.Void
{
    public class VoidFragment : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Void Fragment");
            //Tooltip.SetDefault("A fragment from a broken world");
            ItemID.Sets.ItemIconPulse[Item.type] = true;
            ItemID.Sets.ItemNoGravity[Item.type] = true;
        }

        public override void SetDefaults()
        {
            Item.width = 22;
            Item.height = 22;
			Item.maxStack = 99;
            Item.rare = ItemRarityID.Green;
            Item.value = 10000;
        }

        public override Color? GetAlpha(Color lightColor)
        {
            return Color.White;
        }

        public override void PostUpdate()
        {
            Lighting.AddLight(Item.Center, Color.Black.ToVector3() * 0.55f * Main.essScale);
        }
    }
}
