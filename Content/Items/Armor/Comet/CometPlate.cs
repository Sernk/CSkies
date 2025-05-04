using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using CSkies.Content.Items.Materials;

namespace CSkies.Content.Items.Armor.Comet
{
    [AutoloadEquip(EquipType.Body)]
	public class CometPlate : ModItem
	{
		public override void SetStaticDefaults()
		{
			base.SetStaticDefaults();
			// DisplayName.SetDefault("Cometsteel Platemail");
			// Tooltip.SetDefault("6% increased ranged damage");
		}

		public override void SetDefaults()
		{
			Item.width = 24;
			Item.height = 24;
			Item.value = 6000;
			Item.rare = ItemRarityID.Green;
			Item.defense = 5;
		}

		public override void UpdateEquip(Player player)
		{
			player.GetDamage(DamageClass.Ranged) += 0.06f;
        }

		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ModContent.ItemType<CometBar>(), 25);
            recipe.AddTile(TileID.Anvils);
			recipe.Register();
		}
	}
}