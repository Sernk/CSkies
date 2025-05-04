using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace CSkies.Content.NPCs.Bosses.Starcore
{
    public class Starstatic : ModProjectile
	{
        public int damage = 0;

		public override void SetStaticDefaults()
		{
            // DisplayName.SetDefault("Star Static");
            Main.projFrames[Projectile.type] = 4;
        }

        public override void SetDefaults()
        {
            Projectile.tileCollide = false;
            Projectile.width = 24;
            Projectile.height = 24;
            Projectile.aiStyle = -1;
            Projectile.penetrate = -1;
            Projectile.hostile = true;
            Projectile.ignoreWater = true;
            Projectile.timeLeft = 600;
        }

        public override void AI()
        {
            Lighting.AddLight(Projectile.Center, 0f, .4f, 0f);
            Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.ToRadians(90f);
            if (Projectile.frameCounter++ > 5)
            {
                Projectile.frameCounter = 0;
                if (Projectile.frame++ > 2)
                {
                    Projectile.frame = 0;
                }
            }
        }

        public override Color? GetAlpha(Color lightColor)
        {
            return Color.White;
        }

        public override void OnKill(int timeLeft)
        {
            int dustID = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, ModContent.DustType<Dusts.StarDust>(), Projectile.velocity.X * 0.2f, Projectile.velocity.Y * 0.2f, 10, Color.White, 1f);
            Main.dust[dustID].velocity = new Vector2(MathHelper.Lerp(-1f, 1f, (float)Main.rand.NextDouble()), MathHelper.Lerp(-1f, 1f, (float)Main.rand.NextDouble()));
        }
	}
}