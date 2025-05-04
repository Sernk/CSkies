using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using CSkies.Content.Items.Boss.Starcore;
using CSkies.Utilities;

namespace CSkies.Content.Items.Armor.Starsteel
{
    [AutoloadEquip(EquipType.Body)]
	public class StarsteelPlate : ModItem
	{
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Starsteel Chestplate");
			/* Tooltip.SetDefault(@"5% increased damage
4% Increased critical strike chance"); */
		}

		public override void SetDefaults()
		{
			Item.width = 22;
			Item.height = 24;
			Item.value = Item.sellPrice (0, 2, 40, 0);
			Item.rare = ItemRarityID.LightRed;
			Item.defense = 16;
		}
		
		public override void UpdateEquip(Player player)
		{
			player.GetDamage(DamageClass.Generic) += 0.05f;
			player.GetCritChance(DamageClass.Generic) += 4;
        }
		
		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ModContent.ItemType<Stelarite>(), 10);
            recipe.AddTile(TileID.MythrilAnvil);
			recipe.Register();
		}
	}
}