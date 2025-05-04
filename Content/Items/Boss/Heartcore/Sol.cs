using CSkies.Utilities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using CSkies.Content.Projectiles.Heart;
using CSkies.Content.Items.Boss.Starcore;

namespace CSkies.Content.Items.Boss.Heartcore
{
    public class Sol : ModItem
	{
		public override void SetDefaults()
		{
			Item.damage = 250;
			Item.DamageType = DamageClass.Melee;
			Item.width = 48;
			Item.height = 48;
			Item.useTime = 20;
			Item.useAnimation = 20;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.shoot = ModContent.ProjectileType<FirePro>();
			Item.shootSpeed = 9f;
			Item.knockBack = 7;
            Item.value = Item.sellPrice(0, 12, 0, 0);
            Item.rare = ModContent.RarityType<CSkiesRarity>();
            Item.UseSound = SoundID.Item15;
            Item.autoReuse = true;
		}

		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ModContent.ItemType<Starsaber>(), 1);
            recipe.AddIngredient(ModContent.ItemType<HeartSoul>(), 8);
            recipe.AddTile(TileID.LunarCraftingStation);
			recipe.Register();
		}

        public override void PostDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, float rotation, float scale, int whoAmI)
        {
            Texture2D texture = ModContent.Request<Texture2D>("CSkies/Glowmasks/Sol_Glow").Value;
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
