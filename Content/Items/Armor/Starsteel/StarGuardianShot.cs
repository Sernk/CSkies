using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using CSkies.Content.Dusts;

namespace CSkies.Content.Items.Armor.Starsteel
{
    public class StarGuardianShot : ModProjectile
	{
		public override void SetDefaults()
		{
			Projectile.width = 14;
			Projectile.height = 14;
			Projectile.aiStyle = 1;
			Projectile.friendly = true;
			Projectile.penetrate = 1;
			Projectile.aiStyle = 27;
            Projectile.minion = true;
            Projectile.minionSlots = 0;
            Projectile.timeLeft = 300;
			ProjectileID.Sets.TrailCacheLength[Projectile.type] = 5;
			ProjectileID.Sets.TrailingMode[Projectile.type] = 2;
		}
		
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Star");
		}
		
		public override Color? GetAlpha(Color lightColor)
		{
			return Color.White * 0.8f;
        }
		
		public override void AI()
        {
            if (Main.rand.Next(2) == 0)
			{
				int dustnumber = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, ModContent.DustType<StarDust>(), 0f, 0f, 200, Color.Cyan, 0.8f);
				Main.dust[dustnumber].velocity *= 0.3f;
			}

            Projectile.rotation -= Projectile.velocity.X * .04f;
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            if (Projectile.velocity.X != oldVelocity.X)
            {
                Projectile.position.X = Projectile.position.X + Projectile.velocity.X;
                Projectile.velocity.X = -oldVelocity.X;
                Projectile.damage = (int)(Projectile.damage * 1.2);
            }
            if (Projectile.velocity.Y != oldVelocity.Y)
            {
                Projectile.position.Y = Projectile.position.Y + Projectile.velocity.Y;
                Projectile.velocity.Y = -oldVelocity.Y;
                Projectile.damage = (int)(Projectile.damage * 1.2);
            }

            return false;
        }

        public override void OnKill(int a)
        {
            for (int num468 = 0; num468 < 5; num468++)
            {
                int num469 = Dust.NewDust(new Vector2(Projectile.Center.X, Projectile.Center.Y), Projectile.width, Projectile.height, ModContent.DustType<StarDust>(), -Projectile.velocity.X * 0.2f,
                    -Projectile.velocity.Y * 0.2f, 100, Color.Cyan, 2f);
                Main.dust[num469].noGravity = true;
            }
        }
    }
}