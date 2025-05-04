using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using System.Collections.Generic;
using CSkies.Utilities;


namespace CSkies.Content.Items.Boss.Heartcore
{
    public class HeartSoul : ModItem
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Heart Soul");
            // Tooltip.SetDefault(@"The soul of an enraged, runic creature");
            Main.RegisterItemAnimation(Item.type, new DrawAnimationVertical(5, 4));
            ItemID.Sets.ItemNoGravity[Item.type] = true;
            ItemID.Sets.ItemIconPulse[Item.type] = true;
        }

        public override void SetDefaults()
        {
            Item.width = 22;
            Item.height = 22;
            Item.maxStack = 99;
            Item.rare = ModContent.RarityType<CSkiesRarity>();
            Item.value = 100000;
        }

        public override Color? GetAlpha(Color lightColor)
        {
            return Color.White;
        }

        public override void PostUpdate()
        {
            Lighting.AddLight(Item.Center, NPCs.Bosses.FurySoul.FurySoul.Flame.ToVector3() * 0.55f * Main.essScale);
        }
    }
}