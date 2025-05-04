using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace CSkies.Content.Projectiles.Heart
{
    public class ProBoom : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Fury Blast");     
            Main.projFrames[Projectile.type] = 7;     
        }

        public override void SetDefaults()
        {
            Projectile.width = 98;
            Projectile.height = 98;
            Projectile.penetrate = -1;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.DamageType = DamageClass.Melee;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
            Projectile.timeLeft = 600;
        }

        public override void AI()
        {
            if (++Projectile.frameCounter >= 5)
            {
                Projectile.frameCounter = 0;
                if (++Projectile.frame >= 6)
                {
                    Projectile.Kill();

                }
            }
            Projectile.velocity.X *= 0.00f;
            Projectile.velocity.Y *= 0.00f;

        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.AddBuff(BuffID.Daybreak, 200);
        }

        public override void OnKill(int timeLeft)
        {
            Projectile.timeLeft = 0;
        }

        public override Color? GetAlpha(Color lightColor)
        {
            return Color.White;
        }
    }
}
