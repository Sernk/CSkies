using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using CSkies.Content.Projectiles.Minions;

namespace CSkies.Content.Items.Boss.Void
{
    public class VoidPortal : ModItem
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Abyss Portal");
            // Tooltip.SetDefault(@"Summons an abyss gazer to fight for you");
            Main.RegisterItemAnimation(Item.type, new DrawAnimationVertical(5, 4));
            ItemID.Sets.ItemNoGravity[Item.type] = true;
        }

        public override Color? GetAlpha(Color lightColor)
        {
            return Color.White * 0.8f;
        }

        public override void SetDefaults()
        {
            Item.damage = 170;
            Item.DamageType = DamageClass.Summon;
            Item.mana = 10;
            Item.width = 24;
            Item.height = 24;
            Item.useTime = 36;
            Item.useAnimation = 36;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.noUseGraphic = true;
            Item.noMelee = true;
            Item.knockBack = 3;
            Item.UseSound = SoundID.Item44;
            Item.shoot = ModContent.ProjectileType<Gazer>();
            Item.shootSpeed = 10f;
            Item.buffType = ModContent.BuffType<Buffs.Gazer>();
            Item.autoReuse = true;
            Item.rare = ItemRarityID.Purple;
            Item.value = Item.sellPrice(0, 1, 0, 0);
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
            float num74 = player.GetWeaponKnockback(Item, knockback);
            player.itemTime = Item.useTime;
            Vector2 vector2 = player.RotatedRelativePoint(player.MountedCenter, true);
            vector2.X = Main.mouseX + Main.screenPosition.X;
            vector2.Y = Main.mouseY + Main.screenPosition.Y;
            Projectile.NewProjectile(source, vector2.X, vector2.Y, 0, 0, ModContent.BuffType<Buffs.Gazer>(), damage, num74, i, 0f, 0f);
            return false;
        }

        public override void PostUpdate()
        {
            Lighting.AddLight(Item.Center, Color.White.ToVector3() * 0.8f * 0.55f * Main.essScale);
        }
    }
}