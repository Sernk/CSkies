using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using CSkies.Content.Items.Boss.Starcore;
using CSkies.Utilities;

namespace CSkies.Content.Items.Armor.Starsteel
{
    [AutoloadEquip(EquipType.Legs)]
	public class StarsteelBoots : ModItem
	{
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Starsteel Boots");
			// Tooltip.SetDefault(@"4% increased damage & critical strike chance");
		}

		public override void SetDefaults()
		{
			Item.width = 22;
			Item.height = 24;
			Item.value = Item.sellPrice(0, 1, 80, 0);
			Item.rare = ItemRarityID.LightRed;
			Item.defense = 11;
		}

		public override void UpdateEquip(Player player)
		{
			player.GetDamage(DamageClass.Generic) += 0.04f;
            player.GetCritChance(DamageClass.Generic) += 4;
        }

		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(ModContent.ItemType<Stelarite>(), 8);
			recipe.AddTile(TileID.MythrilAnvil);
			recipe.Register();
		}
	}
}