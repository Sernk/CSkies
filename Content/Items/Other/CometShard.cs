using Terraria;
using Microsoft.Xna.Framework;
using Terraria.ModLoader;
using Terraria.ID;

namespace CSkies.Content.Items.Other
{
    public class CometShard : ModItem
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Comet Shard");
        }
        public override void SetDefaults()
        {
            Item.width = 22;
            Item.height = 22;
			Item.maxStack = 99;
            Item.rare = ItemRarityID.Green;
            Item.value = 100;
        }

        public override void Update(ref float gravity, ref float maxFallSpeed)
        {
            Lighting.AddLight((int)((Item.position.X + Item.width) / 16f), (int)((Item.position.Y + Item.height / 2) / 16f), 0.1f, 0.7f, 0.8f);
            if (Main.rand.Next(25) == 0)
            {
                Dust.NewDust(Item.position, Item.width, Item.height, 58, Item.velocity.X * 0.5f, Item.velocity.Y * 0.5f, 150, Color.Blue, 1.2f);
            }
            if (Main.rand.Next(50) == 0)
            {
                Gore.NewGore(Item.GetSource_FromThis(), Item.position, new Vector2(Item.velocity.X * 0.2f, Item.velocity.Y * 0.2f), Main.rand.Next(16, 18), 1f);
            }
        }

        public override Color? GetAlpha(Color lightColor)
        {
            return Color.White * 0.8f;
        }
    }
}
