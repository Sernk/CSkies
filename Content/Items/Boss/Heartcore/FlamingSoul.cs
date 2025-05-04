using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.DataStructures;
using CSkies.Content.Buffs;
using CSkies.Content.Projectiles.Minions;
using CSkies.Utilities;
using CSkies.Content.Items.Boss.Starcore;
namespace CSkies.Content.Items.Boss.Heartcore
{
    public class FlamingSoul : ModItem
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Flaming Soul");
            // Tooltip.SetDefault(@"Summons a fury rune to fight with you");
            Main.RegisterItemAnimation(Item.type, new DrawAnimationVertical(5, 4));
        }

        public override void SetDefaults()
        {
            Item.damage = 120;
            Item.DamageType = DamageClass.Summon;
            Item.mana = 10;
            Item.width = 26;
            Item.height = 28;
            Item.useTime = 36;
            Item.useAnimation = 36;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.noMelee = true;
            Item.knockBack = 3;
            Item.rare = ModContent.RarityType<CSkiesRarity>();
            Item.UseSound = SoundID.Item44;
            Item.shoot = ModContent.ProjectileType<HeartRune>();
            Item.shootSpeed = 10f;
            Item.buffType = ModContent.BuffType<Rune>();
            Item.autoReuse = true;
            Item.value = Item.sellPrice(0, 12, 0, 0);
            Item.noUseGraphic = true;
        }
		
		public override void UseStyle(Player player, Rectangle heldItemFrame)
		{
			if (player.whoAmI == Main.myPlayer && player.itemTime == 0)
			{
				player.AddBuff(Item.buffType, 3600, true);
			}
		}

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            int i = Main.myPlayer;
            int num73 = damage;
            float num74 = knockback;
            num74 = player.GetWeaponKnockback(Item, num74);
            player.itemTime = Item.useTime;
            Vector2 vector2 = player.RotatedRelativePoint(player.MountedCenter, true);
            vector2.X = Main.mouseX + Main.screenPosition.X;
            vector2.Y = Main.mouseY + Main.screenPosition.Y;
            Projectile.NewProjectile(source, vector2.X, vector2.Y, 0, 0, ModContent.ProjectileType<HeartRune>(), num73, num74, i, 0f, 0f);
            return false;
        }

        public override void AddRecipes()  //How to craft this sword
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ModContent.ItemType<StarDroneUnit>(), 1);
            recipe.AddIngredient(ModContent.ItemType<HeartSoul>(), 8);
            recipe.AddTile(TileID.LunarCraftingStation);
            recipe.Register();
        }
    }
}