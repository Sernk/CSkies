using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using CSkies.Content.NPCs.Bosses.Novacore;
using CSkies.Content.Items.Materials;
using CSkies.Content.NPCs.Bosses.Observer;
using System;

namespace CSkies.Content.Items.Summons
{
	public class CosmicEye : ModItem
	{
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Cosmic Eye");
			/* Tooltip.SetDefault(@"It's observing you.
			Summons the Observer
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
            Item.value = 100000;
        }

		public override bool CanUseItem(Player player)
        {
			return !Main.dayTime && !NPC.AnyNPCs(ModContent.NPCType<Observer>()); // Observer
        }

		public override bool? UseItem(Player player)
		{
			NPC.SpawnOnPlayer(player.whoAmI, ModContent.NPCType<Observer>());
			SoundEngine.PlaySound(SoundID.Roar, player.position);
			return true;
		}

		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ItemID.SuspiciousLookingEye, 1);
			recipe.AddIngredient(ModContent.ItemType<CosmicLens>(), 3);
			recipe.AddTile(TileID.Anvils);
			recipe.Register();
		}
	}
}
