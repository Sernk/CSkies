using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using CSkies.Content.Items.Materials;
using CSkies.Content.Projectiles.Star;

namespace CSkies.Content.Items.Boss.Starcore
{
    public class StormStaff : ModItem
	{
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Starstorm");
			// Tooltip.SetDefault("Summons electrically charged stars from the sky");
			Item.staff[Item.type] = true;
		}

	    public override void SetDefaults()
	    {
	        Item.damage = 30;
	        Item.DamageType = DamageClass.Magic;
	        Item.mana = 5;
	        Item.width = 50;
	        Item.height = 50;
	        Item.useTime = 40;
	        Item.useAnimation = 40;
	        Item.useStyle = ItemUseStyleID.Shoot;
	        Item.noMelee = true;
	        Item.knockBack = 6.75f;
            Item.value = Item.buyPrice(0, 2, 0, 0);
            Item.rare = ItemRarityID.Pink;
            Item.UseSound = SoundID.Item20;
            Item.autoReuse = true;
	        Item.shoot = ModContent.ProjectileType<Starstorm>();
	        Item.shootSpeed = 10f;
		}

	    public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ItemID.MeteorStaff);
            recipe.AddIngredient(ModContent.ItemType<Stelarite>(), 5);
            recipe.AddIngredient(ModContent.ItemType<CosmicStar>(), 2);
            recipe.AddTile(TileID.MythrilAnvil);
	        recipe.Register();
		}

	    public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
	    {
			float speed = Item.shootSpeed;
			int num112 = Main.rand.Next(1, 3);
			for (int num113 = 0; num113 < num112; num113++)
			{
                Vector2 vector2 = new Vector2(player.position.X + player.width * 0.5f + Main.rand.Next(201) * -(float)player.direction + (Main.mouseX + Main.screenPosition.X - player.position.X), player.MountedCenter.Y - 600f);
				vector2.X = (vector2.X + player.Center.X) / 2f + Main.rand.Next(-200, 201);
				vector2.Y -= 100 * num113;
				float posX = Main.mouseX + Main.screenPosition.X - vector2.X + Main.rand.Next(-40, 41) * 0.03f;
                float posY = Main.mouseY + Main.screenPosition.Y - vector2.Y;
				if (posY < 0f)
				{
					posY *= -1f;
				}
				if (posY < 20f)
				{
					posY = 20f;
				}
				float pos = (float)Math.Sqrt(posX * posX + posY * posY);
				pos = speed / pos;
				posX *= pos;
				posY *= pos;
				float originX = posX;
				float originY = posY + Main.rand.Next(-40, 41) * 0.02f;
				Projectile.NewProjectile(source, vector2.X, vector2.Y, originX * 0.75f, originY * 0.75f, type, damage, knockback, player.whoAmI, 0f, 0.5f + (float)Main.rand.NextDouble() * 0.3f);
			}
			return false;
		}


        public override void PostDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, float rotation, float scale, int whoAmI)
        {
            Texture2D texture = ModContent.Request<Texture2D>("CSkies/Glowmasks/MeteorShower_Glow").Value;
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
