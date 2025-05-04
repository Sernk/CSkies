using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ModLoader;
using Terraria.ID;

namespace CSkies.Content.Projectiles.Heart
{
    public class FirePro : ModProjectile
	{
		public override void SetDefaults()
		{
			Projectile.width = 38;
			Projectile.height = 38;
			Projectile.aiStyle = 1;
			Projectile.friendly = true;
			Projectile.penetrate = 1;
			Projectile.aiStyle = -1;
			Projectile.DamageType = DamageClass.Melee;
			Projectile.timeLeft = 300;
		}
		
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Fireball");
            Main.projFrames[Projectile.type] = 4;
		}
		
		public override Color? GetAlpha(Color lightColor)
		{
			return Color.White * 0.8f;
		}
		
		int HomeOnTarget()
		{
			const bool homingCanAimAtWetEnemies = true;
			const float homingMaximumRangeInPixels = 500;

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
						Projectile.Distance(Main.npc[selectedTarget].Center) > distance) 
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
			if (Main.rand.Next(2) == 0)
            {
                int dustnumber = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.Torch, 0f, 0f, 200, default, 0.8f);
                Main.dust[dustnumber].velocity *= 0.3f;
			}

            if (Projectile.frameCounter++ > 5)
            {
                Projectile.frameCounter = 0;
                if (Projectile.frame++ > 2)
                {
                    Projectile.frame = 0;
                }
            }
			
			const int aislotHomingCooldown = 0;
			const int homingDelay = 30;
			const float desiredFlySpeedInPixelsPerFrame = 14; 
			const float amountOfFramesToLerpBy = 15; 

			Projectile.ai[aislotHomingCooldown]++;
			if(Projectile.ai[aislotHomingCooldown] > homingDelay)
			{
				Projectile.ai[aislotHomingCooldown] = homingDelay; 

				int foundTarget = HomeOnTarget();
				if(foundTarget != -1)
				{
					NPC n = Main.npc[foundTarget];
					Vector2 desiredVelocity = Projectile.DirectionTo(n.Center) * desiredFlySpeedInPixelsPerFrame;
					Projectile.velocity = Vector2.Lerp(Projectile.velocity, desiredVelocity, 1f / amountOfFramesToLerpBy);
				}
			}
		}

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.AddBuff(BuffID.Daybreak, 200);
        }

        public override void OnKill(int timeleft)
        {
            SoundEngine.PlaySound(SoundID.Item14, Projectile.position);
            int b = Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.position, Vector2.Zero, ModContent.ProjectileType<ProBoom>(), Projectile.damage, Projectile.knockBack, Main.myPlayer);
            Main.projectile[b].Center = Projectile.Center;
            int num290 = Main.rand.Next(3, 7);
            for (int num291 = 0; num291 < num290; num291++)
            {
                int num292 = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.Torch, 0f, 0f, 100, default, 2.1f);
                Main.dust[num292].velocity *= 2f;
                Main.dust[num292].noGravity = true;
            };
        }
    }
}