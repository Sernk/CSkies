using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using CSkies.Content.Items.Materials;
using CSkies.Utilities.Base.BaseMod.Base;

namespace CSkies.Content.Items.Boss.Observer
{
    public class Comet : ModItem
	{
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("The Comet");			
		}		
		
        public override void SetDefaults()
        {
            Item.width = 30;
            Item.height = 30;
            Item.maxStack = 1;
            Item.rare = ItemRarityID.Green;
            Item.value = BaseUtility.CalcValue(0, 0, 90, 50);
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.useAnimation = 45;
            Item.useTime = 45;
            Item.UseSound = SoundID.Item1;
            Item.damage = 24;
            Item.knockBack = 7;
            Item.DamageType = DamageClass.Melee;
            Item.shoot = ModContent.ProjectileType<Projectiles.Comet.Comet>();
            Item.shootSpeed = 12;
            Item.noUseGraphic = true;
            Item.noMelee = true;
            Item.channel = true;		
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe(this.Type);
            recipe.AddIngredient(ModContent.ItemType<CometFragment>(), 3);
            recipe.AddIngredient(ModContent.ItemType<CometBar>(), 5);
            recipe.AddIngredient(ItemID.BlueMoon, 1);
            recipe.AddTile(TileID.Hellforge);
            recipe.Register();
        }
    }
}