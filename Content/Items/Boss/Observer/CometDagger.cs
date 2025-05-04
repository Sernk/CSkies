using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using CSkies.Content.Projectiles.Comet;
using CSkies.Content.Items.Materials;

namespace CSkies.Content.Items.Boss.Observer
{
    public class CometDagger : ModItem
	{
		public override void SetDefaults()
		{
            Item.damage = 20;            
            Item.DamageType = DamageClass.Ranged;
            Item.width = 20;
            Item.height = 20;
			Item.useTime = 14;
            Item.useAnimation = 14;
            Item.maxStack = 999;
            Item.noUseGraphic = true;
            Item.useStyle = ItemUseStyleID.Swing;
			Item.knockBack = 1;
			Item.value = 10;
			Item.rare = ItemRarityID.Green;
			Item.shootSpeed = 12f;
			Item.shoot = ModContent.ProjectileType<CometKnife>();
			Item.UseSound = SoundID.Item1;
			Item.autoReuse = true;
            Item.consumable = true;
            Item.noMelee = true;
        }

        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Comet Dagger");
            // Tooltip.SetDefault("");
        }
        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe(50);
            recipe.AddIngredient(ModContent.ItemType<CometBar>(), 1);
            recipe.AddIngredient(ModContent.ItemType<CometFragment>(), 1);
            recipe.AddTile(TileID.Anvils);
            recipe.Register();
		}
    }
}
