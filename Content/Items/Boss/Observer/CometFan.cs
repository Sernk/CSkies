using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework.Graphics;
using CSkies.Content.Items.Materials;
using CSkies.Content.Projectiles.Comet;

namespace CSkies.Content.Items.Boss.Observer
{
    public class CometFan : ModItem
	{
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Nova Fan");
		}
		
		public override void SetDefaults()
		{
			Item.rare = ItemRarityID.Green;
			Item.noUseGraphic = true;
			Item.UseSound = SoundID.Item1;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.damage = 15;
			Item.useAnimation = 21;
			Item.useTime = 21;
			Item.width = 26;
			Item.height = 26;
			Item.shoot = ModContent.ProjectileType<CometKnife>();
			Item.shootSpeed = 18f;
			Item.knockBack = 2.5f;
			Item.DamageType = DamageClass.Ranged;
			Item.value = Item.sellPrice(0, 1, 0, 0);
			Item.autoReuse = true;
		}


        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ModContent.ItemType<CometBar>(), 5);
            recipe.AddIngredient(ModContent.ItemType<CometFragment>(), 8);
            recipe.AddTile(TileID.Anvils);
            recipe.Register();
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
		{
			int numberProjectiles = 2 + Main.rand.Next(3); // 2 or 4 shots
			for (int i = 0; i < numberProjectiles; i++)
			{
				Vector2 perturbedSpeed = new Vector2(velocity.X, velocity.Y).RotatedByRandom(MathHelper.ToRadians(14)); // 14 degree spread.
				// If you want to randomize the speed to stagger the projectiles
				// float scale = 1f - (Main.rand.NextFloat() * .3f);
				// perturbedSpeed = perturbedSpeed * scale; 
				Projectile.NewProjectile(source, position.X, position.Y, perturbedSpeed.X, perturbedSpeed.Y, type, damage, knockback, player.whoAmI);
			}
			return false; // return false because we don't want tmodloader to shoot projectile
		}
		
		public override void PostDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, float rotation, float  scale, int whoAmI)
		{
			Texture2D texture = ModContent.Request<Texture2D>("CSkies/Glowmasks/CometFan_Glow").Value;
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