using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace CSkies.Content.Projectiles.Star
{
    public class Starlaser : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.width = 4;
            Projectile.height = 4;
            Projectile.aiStyle = 0;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.DamageType = DamageClass.Ranged;
            Projectile.extraUpdates = 100;
            Projectile.timeLeft = 400;
            Projectile.penetrate = -1;
            Projectile.tileCollide = true;
        }

        public override void AI()
        {
            Projectile.localAI[0] += 1f;
            if (Projectile.localAI[0] > 9f)
            {
                for (int num447 = 0; num447 < 4; num447++)
                {
                    Vector2 vector33 = Projectile.position;
                    vector33 -= Projectile.velocity * (num447 * 0.25f);
                    Projectile.alpha = 255;
                    int num448 = Dust.NewDust(vector33, Projectile.width, Projectile.height, ModContent.DustType<Dusts.StarDust>(), 0f, 0f, 200, default, .8f);
                    Main.dust[num448].position = vector33;
                    Main.dust[num448].scale = Main.rand.Next(70, 110) * 0.013f;
                    Main.dust[num448].velocity *= 0f;
                    Main.dust[num448].noGravity = true;
                }
            }
        }

    }
}
