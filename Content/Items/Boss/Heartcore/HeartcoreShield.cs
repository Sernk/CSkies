using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using CSkies.Content.Items.Boss.Starcore;
using CSkies.Utilities;

namespace CSkies.Content.Items.Boss.Heartcore
{
    public class HeartcoreShield : ModItem
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Shield of the Core");
            /* Tooltip.SetDefault(@"Provides knockback immunity
            Allows you to do a fiery dash
            Being stuck while equipped with this causes meteors to fall
            Above half health, you are slowed by 10%, but gain 8 defense
            Below half health, you gain 10% speed, 25% damage, but defense is reduced by 8"); */
        }

        public override void SetDefaults()
        {
            Item.width = 30;
            Item.height = 28;
            Item.value = Item.sellPrice(0, 5, 0, 0);
            Item.rare = ItemRarityID.Pink;
            Item.accessory = true;
            Item.expert = true;
            Item.expertOnly = true;
        }

        public override void UpdateEquip(Player player)
        {
            player.dash = 3;
            player.noKnockback = true;
            player.GetModPlayer<CPlayer>().StarShield = true;
        }


        public override bool CanEquipAccessory(Player player, int slot, bool modded)
        {
            if (slot < 10)
            {
                int maxAccessoryIndex = 5 + player.extraAccessorySlots;
                for (int i = 3; i < 3 + maxAccessoryIndex; i++)
                {
                    if (slot != i && player.armor[i].type == ModContent.ItemType<StarcoreShield>())
                    {
                        return false;
                    }
                }
            }
            return true;
        }
    }
}