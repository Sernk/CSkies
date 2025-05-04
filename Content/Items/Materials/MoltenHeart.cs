using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;
using CSkies.Utilities;

namespace CSkies.Content.Items.Materials
{
    public class MoltenHeart : ModItem
	{
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Molten Heart");
			// Tooltip.SetDefault("It's uncomfortably hot to the touch");
		}

	    public override void SetDefaults()
	    {
	        Item.width = 22;
	        Item.height = 22;
            Item.value = Item.buyPrice(0, 1, 0, 0);
            Item.rare = ModContent.RarityType<CSkiesRarity>();
        }

        public override void PostDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, float rotation, float scale, int whoAmI)
        {
            Texture2D texture = ModContent.Request<Texture2D>("CSkies/Glowmasks/MoltenHeart_Glow").Value;
            spriteBatch.Draw
            (
                texture,
                new Vector2
                (
                    Item.position.X - Main.screenPosition.X + Item.width * 0.5f,
                    Item.position.Y - Main.screenPosition.Y + Item.height - texture.Height * 0.5f + 2f
                ),
                new Rectangle(0, 0, texture.Width, texture.Height),
                Color.White,
                rotation,
                texture.Size() * 0.5f,
                scale,
                SpriteEffects.None,
                0f
            );
        }
    }
}
