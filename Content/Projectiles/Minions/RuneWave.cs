using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.GameContent;
using Terraria.Graphics.Shaders;
using Terraria.ID;
using Terraria.ModLoader;
using CSkies.Utilities.Base.BaseMod.Base;

namespace CSkies.Content.Projectiles.Minions
{
    public class RuneWave : ModProjectile
	{
		public override void SetDefaults()
		{
			Projectile.width = 10;
			Projectile.height = 10;
			Projectile.friendly = true;
			Projectile.penetrate = 2;
			Projectile.aiStyle = -1;
			Projectile.timeLeft = 1200;
			ProjectileID.Sets.TrailCacheLength[Projectile.type] = 5;
			ProjectileID.Sets.TrailingMode[Projectile.type] = 2;
			Projectile.penetrate = 1;
		}

        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Hydra Slash");
		}
		
		public override Color? GetAlpha(Color lightColor)
		{
			return Color.White;
		}

		public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
		{
			Projectile.ai[0] += 0.1f;
			Projectile.velocity *= 0.75f;
		}
		
		public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
		{
			if (targetHitbox.Width > 8 && targetHitbox.Height > 8)
			{
				targetHitbox.Inflate(-targetHitbox.Width / 8, -targetHitbox.Height / 8);
			}
			return projHitbox.Intersects(targetHitbox);
		}
		
		public override void AI()
		{
			Projectile.rotation =
			Projectile.velocity.ToRotation() +
			MathHelper.ToRadians(90f);
        }

        public override void OnKill(int timeLeft)
        {
            for (int num468 = 0; num468 < 5; num468++)
            {
                int num469 = Dust.NewDust(new Vector2(Projectile.Center.X, Projectile.Center.Y), Projectile.width, Projectile.height, DustID.Torch, -Projectile.velocity.X * 0.2f,
                    -Projectile.velocity.Y * 0.2f, 100, default, 2f);
                Main.dust[num469].noGravity = true;
            }
        }

        public override bool PreDraw(ref Color lightColor)
        {
            SpriteBatch sb = Main.spriteBatch;

            int r = GameShaders.Armor.GetShaderIdFromItemId(ItemID.LivingFlameDye);
            BaseDrawing.DrawAfterimage(sb, TextureAssets.Projectile[Projectile.type].Value, r, Projectile, .7f, 1, 5, false, 0, 0);
            BaseDrawing.DrawTexture(sb, TextureAssets.Projectile[Projectile.type].Value, r, Projectile, Color.White, true);
            return false;
        }
    }
}