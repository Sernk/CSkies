using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.GameContent;
using Terraria.ModLoader;
using CSkies.Utilities;
using CSkies.Utilities.Base.BaseMod.Base;

namespace CSkies.Content.Projectiles.Void
{
    public class VoidKnife : ModProjectile
	{
		public override void SetDefaults()
        {
            Projectile.DamageType = DamageClass.Ranged;
            Projectile.width = 14;
			Projectile.height = 14;
			Projectile.aiStyle = 1;
			Projectile.friendly = true;
			Projectile.penetrate = 1;
			Projectile.timeLeft = 60;
            Projectile.extraUpdates = 1;
		}
		
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Void Knife");
		}
		
		public override Color? GetAlpha(Color lightColor)
		{
			return Color.White;
		}

        public override void OnKill(int timeleft)
        {
            int pieCut = 20;
            for (int m = 0; m < pieCut; m++)
            {
                int dustID = Dust.NewDust(new Vector2(Projectile.Center.X - 1, Projectile.Center.Y - 1), 2, 2, ModContent.DustType<Dusts.VoidDust>(), 0f, 0f, 100, Color.White, 1.6f);
                Main.dust[dustID].velocity = BaseUtility.RotateVector(default, new Vector2(6f, 0f), m / (float)pieCut * 6.28f);
                Main.dust[dustID].noLight = false;
                Main.dust[dustID].noGravity = true;
            }
            for (int m = 0; m < pieCut; m++)
            {
                int dustID = Dust.NewDust(new Vector2(Projectile.Center.X - 1, Projectile.Center.Y - 1), 2, 2, ModContent.DustType<Dusts.VoidDust>(), 0f, 0f, 100, Color.White, 2f);
                Main.dust[dustID].velocity = BaseUtility.RotateVector(default, new Vector2(9f, 0f), m / (float)pieCut * 6.28f);
                Main.dust[dustID].noLight = false;
                Main.dust[dustID].noGravity = true;
            }
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            for (int u = 0; u < Main.maxNPCs; u++)
            {
                NPC t = Main.npc[u];

                if (t.active && !t.boss && Vector2.Distance(Projectile.Center, t.Center) < 160)
                {
                    float num3 = 6f;
                    Vector2 vector = new Vector2(target.position.X + target.width / 2, target.position.Y + target.height / 2);
                    float num4 = Projectile.Center.X - vector.X;
                    float num5 = Projectile.Center.Y - vector.Y;
                    float num6 = (float)Math.Sqrt(num4 * num4 + num5 * num5);
                    num6 = num3 / num6;
                    num4 *= num6;
                    num5 *= num6;
                    int num7 = 6;
                    target.velocity.X = (target.velocity.X * (num7 - 1) + num4) / num7;
                    target.velocity.Y = (target.velocity.Y * (num7 - 1) + num5) / num7;
                }
            }
            Projectile.ai[0] += 0.1f;
            Projectile.velocity *= 0.75f;
        }
		
		int HomeOnTarget()
		{
			const bool homingCanAimAtWetEnemies = true;
			const float homingMaximumRangeInPixels = 1000;

			int selectedTarget = -1;
			for (int i = 0; i < Main.maxNPCs; i++)
			{
				NPC n = Main.npc[i];
				if(n.CanBeChasedBy(Projectile) && (!n.wet || homingCanAimAtWetEnemies))
				{
					float distance = Projectile.Distance(n.Center);
					if(distance <= homingMaximumRangeInPixels &&
						(
						selectedTarget == -1 ||  //there is no selected target
						Projectile.Distance(Main.npc[selectedTarget].Center) > distance) //or we are closer to this target than the already selected target
						)
					{
						selectedTarget = i;
					}
				}
			}
			return selectedTarget;
		}

        public override void AI()
        {
            const int aislotHomingCooldown = 0;
            const int homingDelay = 10;
            const float desiredFlySpeedInPixelsPerFrame = 20; //How fast can it go once in homing mode?
            const float amountOfFramesToLerpBy = 20; // minimum of 1, How quickly can it turn?

            Projectile.ai[aislotHomingCooldown]++;
            if (Projectile.ai[aislotHomingCooldown] > homingDelay)
            {
                Projectile.ai[aislotHomingCooldown] = homingDelay; //cap this value 

                int foundTarget = HomeOnTarget();
                if (foundTarget != -1)
                {
                    NPC n = Main.npc[foundTarget];
                    Vector2 desiredVelocity = Projectile.DirectionTo(n.Center) * desiredFlySpeedInPixelsPerFrame;
                    Projectile.velocity = Vector2.Lerp(Projectile.velocity, desiredVelocity, 1f / amountOfFramesToLerpBy);
                }
            }
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