using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using CSkies.Utilities;
using CSkies.Content.Items.Boss.Starcore;
using CSkies.Content.Projectiles.Heart;

namespace CSkies.Content.Items.Boss.Heartcore
{
    public class BlazeBuster : ModItem
	{
		public override void SetStaticDefaults()
		{
			//DisplayName.SetDefault("Blaze Breaker");
			//Tooltip.SetDefault("Fires an immensely powerful piercing laser that explodes on contact with enemies");
		}

		public override void SetDefaults()
        {
            Item.autoReuse = true;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.useAnimation = 50;
            Item.useTime = 50;
            Item.width = 46;
            Item.height = 28;
            Item.UseSound = SoundID.Item14;
            Item.knockBack = 0.75f;
            Item.damage = 180;
            Item.shootSpeed = 5f;
            Item.noMelee = true;
            Item.rare = ModContent.RarityType<CSkiesRarity>();
            Item.DamageType = DamageClass.Ranged;
            Item.value = Item.sellPrice(0, 12, 0, 0);
            Item.shoot = ModContent.ProjectileType<Blaze>();
        }

        public override void HoldItem(Player player)
        {
            player.scope = true;
        }

        public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(ModContent.ItemType<Railscope>());
            recipe.AddIngredient(ModContent.ItemType<HeartSoul>(), 8);
            recipe.AddTile(TileID.LunarCraftingStation);
			recipe.Register();
		}
		
		public override Vector2? HoldoutOffset()
		{
			return new Vector2(-12, -6);
		}
    }
}
