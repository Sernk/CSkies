using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using CSkies.Content.Items.Materials;
using CSkies.Content.Projectiles.Star;

namespace CSkies.Content.Items.Boss.Starcore
{
    public class Starsaber : ModItem
	{
		public override void SetStaticDefaults()
		{
            // Tooltip.SetDefault("Fires Homing stars");
		}

		public override void SetDefaults()
		{
			Item.damage = 40;
			Item.DamageType = DamageClass.Magic;
			Item.width = 48;
			Item.height = 48;
			Item.useTime = 30;
			Item.useAnimation = 30;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.shoot = ModContent.ProjectileType<StarPro>();
			Item.shootSpeed = 8f;
			Item.knockBack = 5;
            Item.value = Item.sellPrice(0, 2, 0, 0);
            Item.rare = ItemRarityID.Pink;
            Item.UseSound = SoundID.Item15;
            Item.autoReuse = true;
		}

		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ItemID.GreenPhaseblade);
            recipe.AddIngredient(ModContent.ItemType<Stelarite>(), 5);
            recipe.AddIngredient(ModContent.ItemType<CosmicStar>(), 2);
            recipe.AddTile(TileID.MythrilAnvil);
			recipe.Register();
		}


        public override void PostDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, float rotation, float scale, int whoAmI)
        {
            Texture2D texture = ModContent.Request<Texture2D>("CSkies/Glowmasks/Starsaber_Glow").Value;
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
