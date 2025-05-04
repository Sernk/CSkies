using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Fargowiltas.NPCs;
using Terraria.ID;
using CSkies.Content.Items.Summons;

namespace CSkies.Utilities
{
    public class Shop : GlobalNPC, ILocalizedModType // новый способ что бы Mutant продавал веши с мода
    {
        public string LocalizationCategory => "ShopSystem";
        public override void Load()
        {
            string BossSumonn = this.GetLocalization("Chat.BossSumonn").Value;
        }

        public override void ModifyShop(NPCShop shop) // Pre Hardmode Shop, Hardmode Shop, Post Moon Lord Shop
        {
            string BossSumonn = this.GetLocalization("Chat.BossSumonn").Value;
            var BossCs1 = new Condition(BossSumonn,  () => CSystem._Observer);
            var BossCs2 = new Condition(BossSumonn, () => CSystem._ObserverVoid);
            var BossCs3 = new Condition(BossSumonn, () => CSystem._Starcore);
            var BossCs4 = new Condition(BossSumonn, () => CSystem._Heartcore);
            if (shop.NpcType == ModContent.NPCType<Mutant>() && shop.Name == "Pre Hardmode Shop")
            {
                shop.Add(ModContent.ItemType<CosmicEye>(), BossCs1);
            }
            if (shop.NpcType == ModContent.NPCType<Mutant>() && shop.Name == "Hardmode Shop")
            {
                shop.Add(ModContent.ItemType<Transmitter>(), BossCs3);
            }
            if (shop.NpcType == ModContent.NPCType<Mutant>() && shop.Name == "Post Moon Lord Shop")
            {
                shop.Add(ModContent.ItemType<VoidEye>(), BossCs2);
                shop.Add(ModContent.ItemType<PassionRune>(),BossCs4);
            }
        }
    }
}