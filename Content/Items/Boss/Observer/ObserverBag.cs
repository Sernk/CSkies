using Terraria;
using Terraria.ModLoader;
using Terraria.GameContent.ItemDropRules;

namespace CSkies.Content.Items.Boss.Observer
{
    public class ObserverBag : ModItem
	{
        
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Treasure Bag");
			// Tooltip.SetDefault("{$CommonItemTooltip.RightClickToOpen}");
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
            itemLoot.Add(ItemDropRule.Common(ModContent.ItemType<ObserverMask>(), 7));
            itemLoot.Add(ItemDropRule.Common(ModContent.ItemType<CometFragment>(), 1, 10, 15));
            itemLoot.Add(ItemDropRule.Common(ModContent.ItemType<ObserverEye>(), 1));

            itemLoot.Add(ItemDropRule.OneFromOptions(1,
               ModContent.ItemType<Comet>(),
               ModContent.ItemType<CometDagger>(),
               ModContent.ItemType<CometFan>(),
               ModContent.ItemType<CometJavelin>(),
               ModContent.ItemType<CometPortal>()
           ));
        }
	}
}