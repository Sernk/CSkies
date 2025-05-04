using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using CSkies.Content.Items.Materials;

namespace CSkies.Utilities
{
    public class CSkiesRarity : ModRarity
    {
        public override Color RarityColor => new Color(225, 0, 229, 255);
        public override void SetStaticDefaults()
        {
            ItemID.Sets.IsLavaImmuneRegardlessOfRarity[ModContent.ItemType<MoltenHeart>()] = true;
        }
    }
}