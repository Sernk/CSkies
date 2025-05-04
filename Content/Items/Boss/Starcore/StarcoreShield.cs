using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using CSkies.Content.Items.Boss.Heartcore;
using CSkies.Utilities;

namespace CSkies.Content.Items.Boss.Starcore
{
    //[AutoloadEquip(EquipType.Shield)]
    public class StarcoreShield : ModItem
    {
        public override void SetDefaults()
        {
            Item.width = 30;
            Item.height = 32;
            Item.value = Item.sellPrice(0, 5, 0, 0);
            Item.rare = ItemRarityID.Pink;
            Item.expert = true; Item.expertOnly = true;
            Item.accessory = true;
            Item.defense = 3;
        }
        public override void SetStaticDefaults()
        {            // DisplayName.SetDefault("Starsteel Shield");
            /* Tooltip.SetDefault(@"Provides knockback immunity
Allows you to do a quick dash
Being stuck while equipped with this causes stars to fall"); */
        }

        public override void UpdateEquip(Player player)
        {
            player.dash = 3;
            player.noKnockback = true;
            player.GetModPlayer<CPlayer>().StarShield = true;
            player.dash = 2;
        }

        public override bool CanEquipAccessory(Player player, int slot, bool modded)
        {
            if (slot < 10)
            {
                int maxAccessoryIndex = 5 + player.extraAccessorySlots;
                for (int i = 3; i < 3 + maxAccessoryIndex; i++)
                {
                    if (slot != i && player.armor[i].type == ModContent.ItemType<HeartcoreShield>())
                    {
                        return false;
                    }
                }
            }
            return true;
        }
    }
}