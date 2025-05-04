using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using CSkies.Content.Items.Boss.Observer; 

namespace CSkies.Content.Items.Boss.Void
{
    public class Singularity : ModItem
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("The Singularity");
            // Tooltip.SetDefault(@"Throws a vortex on a chain that attracts enemies towards it");
        }

        public override void SetDefaults()
        {
            Item.autoReuse = true;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.useAnimation = 24;
            Item.useTime = 24;
            Item.knockBack = 15f;
            Item.width = 20;
            Item.height = 20;
            Item.damage = 150;
            Item.shoot = ModContent.ProjectileType<Projectiles.Void.Singularity>();
            Item.shootSpeed = 14f;
            Item.UseSound = SoundID.Item10;
            Item.rare = ItemRarityID.Purple;
            Item.value = Item.sellPrice(0, 20, 0, 0);
            Item.DamageType = DamageClass.Melee;
            Item.noMelee = true;
            Item.noUseGraphic = true;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ModContent.ItemType<Comet>(), 1);
            recipe.AddIngredient(ModContent.ItemType<VoidFragment>(), 5);
            recipe.AddTile(TileID.LunarCraftingStation);
            recipe.Register();
        }
    }
}