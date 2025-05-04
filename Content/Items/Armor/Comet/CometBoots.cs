using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using CSkies.Content.Items.Materials;

namespace CSkies.Content.Items.Armor.Comet
{
    [AutoloadEquip(EquipType.Legs)]
	public class CometBoots : ModItem
	{
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Cometsteel Boots");
            // Tooltip.SetDefault("7% increased ranged damage");
		}

		public override void SetDefaults()
		{
			Item.width = 22;
			Item.height = 18;
			Item.value = 5000;
			Item.rare = ItemRarityID.Green;
			Item.defense = 4;
		}
        public override void UpdateEquip(Player player)
        {
            player.GetDamage(DamageClass.Ranged) += 0.07f;
        }
        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
			recipe.AddIngredient(ModContent.ItemType<CometBar>(), 20);
			recipe.AddTile(TileID.Anvils);
            recipe.Register();
        }
	}
}