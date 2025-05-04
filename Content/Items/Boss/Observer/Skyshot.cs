using Terraria.ID;
using Terraria.ModLoader;

namespace CSkies.Content.Items.Boss.Observer
{
    public class Skyshot : ModItem
    {
        public override void SetDefaults()
        {
            Item.damage = 19;
            Item.DamageType = DamageClass.Magic;
            Item.mana = 12;
            Item.width = 8;
            Item.height = 8;
            Item.useTime = 30;
            Item.useAnimation = 30;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.noMelee = true;
            Item.knockBack = 2;
            Item.value = 1000;
            Item.rare = ItemRarityID.Green;
            Item.UseSound = SoundID.Item8;
            Item.autoReuse = true;
            Item.shoot = ModContent.ProjectileType<Projectiles.Comet.CometShot>();
            Item.shootSpeed = 8f;
        }

        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Skyshot");
            // Tooltip.SetDefault("Fires a homing comet towards your cursor");
        }
    }
}