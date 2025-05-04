using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Localization;
using CSkies.Content.Items.Materials;
using CSkies.Utilities;

namespace CSkies.Content.Items.Armor.Comet
{
    [AutoloadEquip(EquipType.Head)]
	public class CometVisor : ModItem
	{
        public static LocalizedText SetBonusText { get; private set; }
        public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Cometsteel Visor");
			// Tooltip.SetDefault("6% increased ranged damage");
			SetBonusText = this.GetLocalization("SetBonus").WithFormatArgs("Ranged projectiles have a chance to inflict cometspark on targets");
        }

		public override void SetDefaults()
		{
			Item.width = 24;
			Item.height = 20;
			Item.value = 10000;
			Item.rare = ItemRarityID.Green;
			Item.defense = 4;
		}

        public override bool IsArmorSet(Item head, Item body, Item legs)
		{
            return body.type == ModContent.ItemType<CometPlate>() && legs.type == ModContent.ItemType<CometBoots>();
        }

		public override void UpdateArmorSet(Player player)
		{
            player.setBonus = SetBonusText.Value;
            player.setBonus = "Ranged projectiles have a chance to inflict cometspark on targets";
            player.GetModPlayer<CPlayer>().CometSet = true;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
			recipe.AddIngredient(ModContent.ItemType<CometBar>(), 15);
			recipe.AddTile(TileID.Anvils);
            recipe.Register();
        }
	}
}