using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using CSkies.Content.Projectiles.Star;
using CSkies.Content.Items.Materials;

namespace CSkies.Content.Items.Boss.Starcore
{
    public class Railscope : ModItem
	{
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Railscope");
			// Tooltip.SetDefault(@"Allows you to view slightly further by moving your mouse to the edge of the screen");
		}

		public override void SetDefaults()
        {
            Item.autoReuse = true;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.useAnimation = 50;
            Item.useTime = 50;
            Item.width = 24;
            Item.height = 28;
            Item.UseSound = SoundID.Item12;
            Item.knockBack = 0.75f;
            Item.damage = 20;
            Item.shootSpeed = 5f;
            Item.noMelee = true;
            Item.rare = ItemRarityID.Pink;
            Item.DamageType = DamageClass.Ranged;
            Item.value = Item.sellPrice(0, 2, 0, 0);
            Item.shoot = ModContent.ProjectileType<Starlaser>();
        }

        public override void HoldItem(Player player)
        {
            player.scope = true;
        }

        public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ItemID.SpaceGun);
            recipe.AddIngredient(ModContent.ItemType<Stelarite>(), 5);
            recipe.AddIngredient(ModContent.ItemType<CosmicStar>(), 2);
            recipe.AddTile(TileID.MythrilAnvil);
			recipe.Register();
		}
		
		public override Vector2? HoldoutOffset()
		{
			return new Vector2(-12, -6);
		}
    }
}
