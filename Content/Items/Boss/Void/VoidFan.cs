using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework.Graphics;
using CSkies.Content.Projectiles.Void;
using CSkies.Content.Items.Boss.Observer;

namespace CSkies.Content.Items.Boss.Void
{
    public class VoidFan : ModItem
	{
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Abyss Fan");
		}
		
		public override void SetDefaults()
		{
			Item.rare = ItemRarityID.Purple;
			Item.noUseGraphic = true;
			Item.UseSound = SoundID.Item1;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.damage = 110;
			Item.useAnimation = 16;
			Item.useTime = 16;
			Item.width = 26;
			Item.height = 26;
			Item.shoot = ModContent.ProjectileType<VoidKnife>();
			Item.shootSpeed = 18f;
			Item.knockBack = 0f;
			Item.DamageType = DamageClass.Ranged;
			Item.value = Item.sellPrice(0, 10, 0, 0);
			Item.autoReuse = true;
			Item.crit = 20;
		}

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ModContent.ItemType<CometFan>(), 1);
            recipe.AddIngredient(ModContent.ItemType<VoidFragment>(), 5);
            recipe.AddTile(TileID.LunarCraftingStation);
            recipe.Register();
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
		{
			int numberProjectiles = 2 + Main.rand.Next(3); // 2 or 4 shots
			for (int i = 0; i < numberProjectiles; i++)
			{
				Vector2 perturbedSpeed = new Vector2(velocity.X, velocity.Y).RotatedByRandom(MathHelper.ToRadians(14));
				Projectile.NewProjectile(source, position.X, position.Y, perturbedSpeed.X, perturbedSpeed.Y, type, damage, knockback, player.whoAmI);
			}
			return false;
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