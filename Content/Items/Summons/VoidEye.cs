using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using CSkies.Content.NPCs.Bosses.ObserverVoid;
using CSkies.Content.Items.Materials;

namespace CSkies.Content.Items.Summons
{
	public class VoidEye : ModItem
	{
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Void Eye");
			/* Tooltip.SetDefault(@"The abyss stares back
			Summons Observer Void
			Can only be used at night"); */
            Main.RegisterItemAnimation(Item.type, new DrawAnimationVertical(6, 4));
            ItemID.Sets.SortingPriorityBossSpawns[Item.type] = 13;
        }

		public override void SetDefaults()
		{
			Item.width = 14;
			Item.height = 24;
			Item.rare = ItemRarityID.Green;
			Item.useAnimation = 45;
			Item.useTime = 45;
			Item.useStyle = ItemUseStyleID.HoldUp;
            Item.maxStack = 20;
			Item.consumable = true;
            Item.noUseGraphic = true;
			Item.value = 1000000;
        }

		public override bool CanUseItem(Player player)
		{
			return !Main.dayTime && !NPC.AnyNPCs(ModContent.NPCType<ObserverVoid>());
		}

		public override bool? UseItem(Player player)
		{
			NPC.SpawnOnPlayer(player.whoAmI, ModContent.NPCType<ObserverVoid>());
			SoundEngine.PlaySound(SoundID.Roar, player.position);
			return true;
		}

		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ModContent.ItemType<VoidLens>(), 5);
            recipe.AddIngredient(ModContent.ItemType<CosmicEye>(), 1);
			recipe.AddTile(TileID.LunarCraftingStation);
			recipe.Register();
		}
	}
}
