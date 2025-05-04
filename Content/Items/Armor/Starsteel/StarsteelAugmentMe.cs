using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using System.Collections.Generic;
using Terraria.DataStructures;
using CSkies.Utilities;

namespace CSkies.Content.Items.Armor.Starsteel
{
    public class StarsteelAugmentMe : ModItem
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Melee Starsteel Augment");
            // Tooltip.SetDefault(@"If wearing starsteel Armor, accessory effects double.");
            Main.RegisterItemAnimation(Item.type, new DrawAnimationVertical(5, 13));
        }

        public override void SetDefaults()
        {
            Item.width = 32;
            Item.height = 26;
            Item.value = Item.sellPrice(0, 3, 0, 0);
            Item.rare = ItemRarityID.LightRed;
            Item.accessory = true;
        }

        public override void UpdateEquip(Player player)
        {
            player.GetModPlayer<CPlayer>().StarsteelBonus = 1;
            if (player.GetModPlayer<CPlayer>().Starsteel)
            {
                player.GetDamage(DamageClass.Melee) += .1f;
                player.GetCritChance(DamageClass.Melee) += 10;
            }
            else
            {
                player.GetDamage(DamageClass.Melee) += .05f;
                player.GetCritChance(DamageClass.Melee) += 5;
            }
        }

        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
            bool Starsteel = Main.player[Item.playerIndexTheItemIsReservedFor].GetModPlayer<CPlayer>().Starsteel;

            string DamageAmount = (Starsteel ? 10 : 5) + "% increased melee damage & critical strike chance";

            TooltipLine DamageTooltip = new TooltipLine(Mod, "Damage Type", DamageAmount);
            tooltips.Add(DamageTooltip);

            base.ModifyTooltips(tooltips);
        }

        public override bool CanEquipAccessory(Player player, int slot, bool modded)
        {
            if (slot < 10)
            {
                int maxAccessoryIndex = 5 + player.extraAccessorySlots;
                for (int i = 3; i < 3 + maxAccessoryIndex; i++)
                {
                    if (slot != i)
                    {
                        if (player.armor[i].type == ModContent.ItemType<StarsteelAugmentMa>() ||
                            player.armor[i].type == ModContent.ItemType<StarsteelAugmentRa>() ||
                            player.armor[i].type == ModContent.ItemType<StarsteelAugmentSu>())
                        {
                            return false;
                        }
                    }
                }
            }
            return true;
        }
    }
}