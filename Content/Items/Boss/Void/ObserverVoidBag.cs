using CSkies.Content.Items.Boss.Heartcore;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.GameContent;
using Terraria.GameContent.ItemDropRules;
using Terraria.ModLoader;

namespace CSkies.Content.Items.Boss.Void
{
    public class ObserverVoidBag : ModItem
	{
        
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Treasure Bag");
			// Tooltip.SetDefault("{$CommonItemTooltip.RightClickToOpen}");
		}

		public override void SetDefaults()
		{
			Item.maxStack = 999;
			Item.consumable = true;
			Item.width = 36;
			Item.height = 32;
            Item.expert = true; Item.expertOnly = true;
		}

        public override bool CanRightClick()
		{
			return true;
        }
        public override void ModifyItemLoot(ItemLoot itemLoot)
        {
            itemLoot.Add(ItemDropRule.Common(ModContent.ItemType<VOIDMask>(), 7));
            itemLoot.Add(ItemDropRule.Common(ModContent.ItemType<VoidFragment>(), 1, 10, 15));
            itemLoot.Add(ItemDropRule.Common(ModContent.ItemType<ObserverVoidEye>(), 1));

            itemLoot.Add(ItemDropRule.OneFromOptions(1,
               ModContent.ItemType<Singularity>(),
               ModContent.ItemType<VoidFan>(),
               ModContent.ItemType<VoidShot>(),
               ModContent.ItemType<VoidJavelin>(),
               ModContent.ItemType<VoidWings>(),
               ModContent.ItemType<VoidPortal>()
           ));
        }
        public override void PostDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, float rotation, float scale, int whoAmI)
        {
            Texture2D glow = ModContent.Request<Texture2D>("CSkies/Glowmasks/ObserverVoidBag_Glow").Value;
            spriteBatch.Draw
                (
                glow,
                new Vector2
                (
                    Item.position.X - Main.screenPosition.X + Item.width * 0.5f,
                    Item.position.Y - Main.screenPosition.Y + Item.height - glow.Height * 0.5f + 2f
                ),
                new Rectangle(0, 0, glow.Width, glow.Height),
                lightColor,
                rotation,
                glow.Size() * 0.5f,
                scale,
                SpriteEffects.None,
                0f
                );
        }

        public override bool PreDrawInInventory(SpriteBatch spriteBatch, Vector2 position, Rectangle frame, Color drawColor, Color itemColor, Vector2 origin, float scale)
        {
            Texture2D glow = ModContent.Request<Texture2D>("CSkies/Glowmasks/ObserverVoidBag_Glow").Value;
            Texture2D texture2 = TextureAssets.Item[Item.type].Value;
            spriteBatch.Draw(texture2, position, null, drawColor, 0, origin, scale, SpriteEffects.None, 0f);
            for (int i = 0; i < 4; i++)
            {
                //Vector2 offsetPositon = Vector2.UnitY.RotatedBy(MathHelper.PiOver2 * i) * 2;
                spriteBatch.Draw(glow, position, null, drawColor, 0, origin, scale, SpriteEffects.None, 0f);

            }

            return false;
        }
    }
}