using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using CSkies.Content.Items.Materials;
using CSkies.Content.NPCs.Bosses.Starcore;

namespace CSkies.Content.Items.Summons
{
    public class Transmitter : ModItem
	{
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Transmitter");
			/* Tooltip.SetDefault(@"It's displaying coordinates somewhere in the atmosphere...
Summons Starcore
Can only be used at night"); */
            ItemID.Sets.SortingPriorityBossSpawns[Item.type] = 13;
        }

		public override void SetDefaults()
		{
			Item.width = 14;
			Item.height = 24;
			Item.rare = ItemRarityID.Pink;
			Item.useAnimation = 45;
			Item.useTime = 45;
			Item.useStyle = ItemUseStyleID.HoldUp;
            Item.maxStack = 20;
			Item.consumable = true;
            Item.noUseGraphic = true;
            Item.value = 400000;
        }

		public override bool CanUseItem(Player player)
		{
			return !Main.dayTime && !NPC.AnyNPCs(ModContent.NPCType<Starcore>());
		}

		public override bool? UseItem(Player player)
		{
			NPC.SpawnOnPlayer(player.whoAmI, ModContent.NPCType<Starcore>());
			SoundEngine.PlaySound(SoundID.Roar, player.position);
			return true;
		}

		public override void AddRecipes()
		{
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ItemID.MythrilBar, 5);
            recipe.AddIngredient(ItemID.FallenStar, 5);
            recipe.AddIngredient(ModContent.ItemType<CosmicStar>(), 3);
			recipe.AddTile(TileID.MythrilAnvil);
			recipe.Register();

            Recipe recipe1 = CreateRecipe();
            recipe1.AddIngredient(ItemID.OrichalcumBar, 5);
            recipe1.AddIngredient(ItemID.FallenStar, 5);
            recipe1.AddIngredient(ModContent.ItemType<CosmicStar>(), 3);
            recipe1.AddTile(TileID.MythrilAnvil);
            recipe1.Register();
        }
	}
}
