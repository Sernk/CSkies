using Terraria;
using Terraria.GameContent.ItemDropRules;
using Terraria.ModLoader;

namespace CSkies.Content.Items.Boss.Heartcore
{
    public class HeartcoreBag : ModItem
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
            itemLoot.Add(ItemDropRule.Common(ModContent.ItemType<FurySoulMask>(), 7));
            itemLoot.Add(ItemDropRule.Common(ModContent.ItemType<HeartSoul>(), 1, 10, 15));
            itemLoot.Add(ItemDropRule.Common(ModContent.ItemType<HeartcoreShield>(), 1));

            itemLoot.Add(ItemDropRule.OneFromOptions(1,
               ModContent.ItemType<MeteorShower>(),
               ModContent.ItemType<BlazeBuster>(),
               ModContent.ItemType<FlamingSoul>()
           ));
        }
	}
}