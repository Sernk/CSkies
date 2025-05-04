using Terraria.ModLoader;
using Terraria.Graphics.Effects;

namespace CSkies
{
    public class ShockwaveBoom : ModProjectile
    {
        public override string Texture => "CSkies/BlankTex";

        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Shockwave Boom");
        }

        public override void SetDefaults()
        {
            Projectile.width = 4;
            Projectile.height = 4;
            Projectile.friendly = true;
            Projectile.alpha = 0;
            Projectile.penetrate = -1;
            Projectile.timeLeft = 200;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
        }

        public override void AI()
        {
            float progress = (180f - Projectile.timeLeft) / 60f;
            float pulseCount = 1;
            float rippleSize = 1;
            float speed = 20;
            if (Projectile.ai[0] > 0)
            {
                rippleSize = Projectile.ai[0];
            }
            if (Projectile.ai[1] > 0)
            {
                speed = Projectile.ai[1];
            }
            Filters.Scene["Shockwave"].GetShader().UseProgress(progress).UseOpacity(100f * (1 - progress / 3f));
            Projectile.localAI[1]++;
            if (Projectile.localAI[1] >= 0 && Projectile.localAI[1] <= 60)
            {
                if (!Filters.Scene["Shockwave"].IsActive())
                {
                    Filters.Scene.Activate("Shockwave", Projectile.Center).GetShader().UseColor(pulseCount, rippleSize, speed).UseTargetPosition(Projectile.Center);
                }
            }
        }
        public override void OnKill(int timeLeft)
        {
            Filters.Scene["Shockwave"].Deactivate();
        }
    }
}
