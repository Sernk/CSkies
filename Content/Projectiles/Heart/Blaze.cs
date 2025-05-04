using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CSkies.Content.Projectiles.Heart
{
    public class Blaze : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.width = 4;
            Projectile.height = 4;
            Projectile.aiStyle = -1;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.DamageType = DamageClass.Ranged;
            Projectile.extraUpdates = 100;
            Projectile.timeLeft = 400;
            Projectile.penetrate = -1;
            Projectile.tileCollide = false;
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
                    int num448 = Dust.NewDust(vector33, Projectile.width, Projectile.height, DustID.SolarFlare, 0f, 0f, 200, default, .8f);
                    Main.dust[num448].position = vector33;
                    Main.dust[num448].scale = Main.rand.Next(70, 110) * 0.013f;
                    Main.dust[num448].velocity *= 0.2f;
                    Main.dust[num448].noGravity = true;
                }
            }
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.AddBuff(BuffID.Daybreak, 200);
            int b = Projectile.NewProjectile(target.GetSource_FromThis(), target.position, Vector2.Zero, ModContent.ProjectileType<BlazeBoom>(), Projectile.damage, Projectile.knockBack, Main.myPlayer);
            Main.projectile[b].Center = target.Center;
            for (int num468 = 0; num468 < 8; num468++)
            {
                int num469 = Dust.NewDust(new Vector2(target.position.X, target.position.Y), target.width, target.height, DustID.Torch, -target.velocity.X * 0.2f,
                    -target.velocity.Y * 0.2f, 100, default, 2f);
                Main.dust[num469].noGravity = true;
                Main.dust[num469].velocity *= 2;
            }
        }
    }
}
