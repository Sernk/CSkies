using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using CSkies.Content.Items.Boss.Observer;

namespace CSkies.Content.Items.Boss.Void
{
    public class VoidJavelin : ModItem
	{
		public override void SetDefaults()
		{
			Item.damage = 250;
			Item.DamageType = DamageClass.Melee;
			Item.width = 22;
			Item.noUseGraphic = true;
			Item.maxStack = 1;
			Item.consumable = false;
			Item.height = 44;
			Item.useTime = 15;
			Item.useAnimation = 15;
			Item.shoot = ModContent.ProjectileType<Projectiles.Void.VoidJavelin>();
			Item.shootSpeed = 16;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.knockBack = 4;
			Item.value = Item.sellPrice(0, 10, 0, 0);
			Item.rare = ItemRarityID.Purple;
			Item.UseSound = SoundID.Item1;
			Item.autoReuse = true;
			Item.crit = 3;
		}

		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Void Javelin");
			// Tooltip.SetDefault("");
		}

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ModContent.ItemType<CometJavelin>(), 1);
            recipe.AddIngredient(ModContent.ItemType<VoidFragment>() ,5);
            recipe.AddTile(TileID.Anvils);
            recipe.Register();
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
		{
			Vector2 perturbedSpeed = new Vector2(velocity.X, velocity.Y).RotatedByRandom(MathHelper.ToRadians(02));
			velocity.X = perturbedSpeed.X;
			velocity.Y = perturbedSpeed.Y;
			return true;
		}
	}
}
