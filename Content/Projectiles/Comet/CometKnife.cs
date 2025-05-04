using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.GameContent;
using Terraria.ModLoader;
using CSkies.Utilities.Base.BaseMod.Base;

namespace CSkies.Content.Projectiles.Comet
{
    public class CometKnife : ModProjectile
	{
		public override void SetDefaults()
        {
            Projectile.DamageType = DamageClass.Ranged;
            Projectile.width = 14;
			Projectile.height = 14;
			Projectile.aiStyle = 1;
			Projectile.friendly = true;
			Projectile.penetrate = 1;
			Projectile.timeLeft = 200;
		}
		
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Comet Knife");
		}
		
		public override Color? GetAlpha(Color lightColor)
		{
			return Color.White;
		}

        public override void PostAI()
        {
            Projectile.alpha += 2;
            if (Projectile.alpha >= 255)
            {
                Projectile.active = false;
            }

            for (int m = Projectile.oldPos.Length - 1; m > 0; m--)
            {
                Projectile.oldPos[m] = Projectile.oldPos[m - 1];
            }
            Projectile.oldPos[0] = Projectile.position;
        }

        public override void OnKill(int timeleft)
        {
            int pieCut = 20;
            for (int m = 0; m < pieCut; m++)
            {
                int dustID = Dust.NewDust(new Vector2(Projectile.Center.X - 1, Projectile.Center.Y - 1), 2, 2, 17, 0f, 0f, 100, Color.White, 1.6f);
                Main.dust[dustID].velocity = BaseUtility.RotateVector(default, new Vector2(6f, 0f), m / (float)pieCut * 6.28f);
                Main.dust[dustID].noLight = false;
                Main.dust[dustID].noGravity = true;
            }
            for (int m = 0; m < pieCut; m++)
            {
                int dustID = Dust.NewDust(new Vector2(Projectile.Center.X - 1, Projectile.Center.Y - 1), 2, 2, 17, 0f, 0f, 100, Color.White, 2f);
                Main.dust[dustID].velocity = BaseUtility.RotateVector(default, new Vector2(9f, 0f), m / (float)pieCut * 6.28f);
                Main.dust[dustID].noLight = false;
                Main.dust[dustID].noGravity = true;
            }
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
		{
			Projectile.ai[0] += 0.1f;
			Projectile.velocity *= 0.75f;
		}

        public override bool PreDraw(ref Color lightColor)
        {
            SpriteBatch sb = Main.spriteBatch;
            Texture2D tex = TextureAssets.Projectile[Projectile.type].Value;
            BaseDrawing.DrawAfterimage(sb, tex, 0, Projectile, 2.5f, 1, 3, true, 0f, 0f, Projectile.GetAlpha(Color.White * 0.8f));
            BaseDrawing.DrawTexture(sb, tex, 0, Projectile, Projectile.GetAlpha(Color.White * 0.8f));
            return false;
        }
    }
}