using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using CSkies.Content.Projectiles.Void;
using CSkies.Content.Items.Boss.Observer;

namespace CSkies.Content.Items.Boss.Void
{
    public class VoidShot : ModItem
	{
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Abyss Tome");
			// Tooltip.SetDefault("Releases a homing singularity that drags in enemies and explodes on contact");
		}

		public override void SetDefaults()
		{
			Item.mana = 35;
			Item.damage = 160;
			Item.useStyle = ItemUseStyleID.Shoot;
			Item.shootSpeed = 9f;
			Item.shoot = ModContent.ProjectileType<VoidMagic>();
			Item.width = 28;
			Item.height = 30;
			Item.UseSound = SoundID.Item117;
			Item.useAnimation = 35;
			Item.useTime = 35;
			Item.autoReuse = true;
			Item.noMelee = true;
			Item.knockBack = 8f;
			Item.rare = ItemRarityID.Purple;
			Item.value = Item.sellPrice(0, 10, 0, 0);
			Item.DamageType = DamageClass.Magic;
			Item.noUseGraphic = true;
		}

		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ModContent.ItemType<Skyshot>(), 1);
            recipe.AddIngredient(ModContent.ItemType<VoidFragment>(), 5);
            recipe.AddTile(TileID.LunarCraftingStation);
			recipe.Register();
		}
	}
}
