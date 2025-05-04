using CSkies.Content.Items.Boss.Heartcore;
using Terraria;
using Terraria.GameContent.ItemDropRules;
using Terraria.ModLoader;

namespace CSkies.Content.Items.Boss.Starcore
{
    public class StarcoreBag : ModItem
	{
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Treasure Bag");
			//Tooltip.SetDefault("{$CommonItemTooltip.RightClickToOpen}");
		}

		public override void SetDefaults()
		{
			Item.maxStack = 999;
			Item.consumable = true;
			Item.width = 36;
			Item.height = 32;
            Item.expert = true;
            Item.expertOnly = true;
		}

        public override bool CanRightClick()
		{
			return true;
        }
        public override void ModifyItemLoot(ItemLoot itemLoot)
        {
            itemLoot.Add(ItemDropRule.Common(ModContent.ItemType<StarcoreMask>(), 7));
            itemLoot.Add(ItemDropRule.Common(ModContent.ItemType<Stelarite>(), 1, 10, 15));
            itemLoot.Add(ItemDropRule.Common(ModContent.ItemType<StarcoreShield>(), 1));

            itemLoot.Add(ItemDropRule.OneFromOptions(1,
               ModContent.ItemType<Starsaber>(),
               ModContent.ItemType<StormStaff>(),
               ModContent.ItemType<StarDroneUnit>(),
               ModContent.ItemType<Railscope>()
           ));
        }
	}
}