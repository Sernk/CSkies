using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Audio;
using Terraria.Graphics.Shaders;
using Terraria.ID;
using Terraria.ModLoader;
using CSkies.Content.NPCs.Bosses.Heartcore;
using CSkies.Content.Items.Materials;
using CSkies.Utilities.Base.BaseMod.Base;

namespace CSkies.Content.Items.Summons
{
    public class PassionRune : ModItem
	{
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Rune of Passion");
			/* Tooltip.SetDefault(@"The heart on it glows at the same rate your own beats
			Summons Heartcore
			Can only be used at night"); */
            ItemID.Sets.SortingPriorityBossSpawns[Item.type] = 13;
        }

		public override void SetDefaults()
		{
			Item.width = 40;
			Item.height = 40;
			Item.rare = ItemRarityID.Green;
			Item.useAnimation = 45;
			Item.useTime = 45;
			Item.useStyle = ItemUseStyleID.HoldUp;
            Item.maxStack = 20;
			Item.consumable = true;
            Item.noUseGraphic = true;
            Item.value = 1500000;
        }

		public override bool CanUseItem(Player player)
		{
			return !Main.dayTime && !NPC.AnyNPCs(ModContent.NPCType<Heartcore>());
		}

		public override bool? UseItem(Player player)
		{
			NPC.SpawnOnPlayer(player.whoAmI, ModContent.NPCType<Heartcore>());
			SoundEngine.PlaySound(SoundID.Roar, player.position);
			return true;
		}

		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ItemID.SuspiciousLookingEye, 1);
			recipe.AddIngredient(ModContent.ItemType<MoltenHeart>(), 3);
			recipe.AddTile(TileID.LunarCraftingStation);
			recipe.Register();
		}

        public override void PostDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, float rotation, float scale, int whoAmI)
        {
            int r = GameShaders.Armor.GetShaderIdFromItemId(ItemID.LivingFlameDye);
            Texture2D texture = ModContent.Request<Texture2D>("CSkies/Glowmasks/" + GetType().Name + "_Glow").Value;
			Rectangle frame = BaseDrawing.GetFrame(0, texture.Width, texture.Height, 0, 0);
			BaseDrawing.DrawTexture(spriteBatch, texture, r, Item.position, Item.width, Item.height, Item.scale, 0f, 0, 1, frame);
		}

        public override void PostDrawInInventory(SpriteBatch spriteBatch, Vector2 position, Rectangle frame, Color drawColor, Color itemColor, Vector2 origin, float scale)
        {
            int r = GameShaders.Armor.GetShaderIdFromItemId(ItemID.LivingFlameDye);
            Texture2D texture = ModContent.Request<Texture2D>("CSkies/Glowmasks/" + GetType().Name + "_Glow").Value;
            BaseDrawing.DrawTexture(spriteBatch, texture, r, position, Item.width, Item.height, Item.scale, 0f, 0, 1, frame);
        }
    }
}
