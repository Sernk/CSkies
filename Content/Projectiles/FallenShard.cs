using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using CSkies.Content.Items.Other;

namespace CSkies.Content.Projectiles
{
    public class FallenShard : ModProjectile
	{
		public override void SetDefaults()
        {
            Projectile.width = 16;
            Projectile.height = 16;
            Projectile.aiStyle = -1;
            Projectile.friendly = true;
            Projectile.penetrate = -1;
            Projectile.alpha = 50;
            Projectile.light = 1f;
        }
		
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Comet Knife");
		}
		
		public override Color? GetAlpha(Color lightColor)
		{
			return Color.White;
		}

        public override void AI()
        {
            if (Projectile.ai[1] == 0f && !Collision.SolidCollision(Projectile.position, Projectile.width, Projectile.height))
            {
                Projectile.ai[1] = 1f;
                Projectile.netUpdate = true;
            }
            if (Projectile.ai[1] != 0f)
            {
                Projectile.tileCollide = true;
            }
            if (Projectile.soundDelay == 0)
            {
                Projectile.soundDelay = 20 + Main.rand.Next(40);
                SoundEngine.PlaySound(SoundID.Item9, Projectile.position);
            }
            if (Projectile.localAI[0] == 0f)
            {
                Projectile.localAI[0] = 1f;
            }
            Projectile.alpha += (int)(25f * Projectile.localAI[0]);
            if (Projectile.alpha > 200)
            {
                Projectile.alpha = 200;
                Projectile.localAI[0] = -1f;
            }
            if (Projectile.alpha < 0)
            {
                Projectile.alpha = 0;
                Projectile.localAI[0] = 1f;
            }
            Projectile.rotation += (Math.Abs(Projectile.velocity.X) + Math.Abs(Projectile.velocity.Y)) * 0.01f * Projectile.direction;
            if (Projectile.ai[1] == 1f)
            {
                Projectile.light = 0.9f;
                if (Main.rand.Next(10) == 0)
                {
                    Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, 58, Projectile.velocity.X * 0.5f, Projectile.velocity.Y * 0.5f, DustID.Electric, Color.Blue, 1.2f);
                }
                if (Main.rand.Next(20) == 0)
                {
                    Gore.NewGore(Projectile.GetSource_FromThis(), Projectile.position, new Vector2(Projectile.velocity.X * 0.2f, Projectile.velocity.Y * 0.2f), Main.rand.Next(16, 18), 1f);
                    return;
                }
            }
        }

        public override void OnKill(int timeLeft)
        {
            SoundEngine.PlaySound(SoundID.Item10, Projectile.position);
            int num535 = 10;
            int num536 = 3;
            for (int num537 = 0; num537 < num535; num537++)
            {
                Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, 17, Projectile.velocity.X * 0.1f, Projectile.velocity.Y * 0.1f, DustID.Electric, Color.Blue, 1.2f);
            }
            for (int num538 = 0; num538 < num536; num538++)
            {
                int num539 = Main.rand.Next(16, 18);
                Gore.NewGore(Projectile.GetSource_FromThis(), Projectile.position, new Vector2(Projectile.velocity.X * 0.05f, Projectile.velocity.Y * 0.05f), num539, 1f);
            }
            if (Projectile.damage < 100)
            {
                for (int num540 = 0; num540 < 10; num540++)
                {
                    Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, 17, Projectile.velocity.X * 0.1f, Projectile.velocity.Y * 0.1f, DustID.Electric, Color.Blue, 1.2f);
                }
                for (int num541 = 0; num541 < 3; num541++)
                {
                    Gore.NewGore(Projectile.GetSource_FromThis(), Projectile.position, new Vector2(Projectile.velocity.X * 0.05f, Projectile.velocity.Y * 0.05f), Main.rand.Next(16, 18), 1f);
                }
            }

            Item.NewItem(Projectile.GetSource_FromThis(), (int)Projectile.position.X, (int)Projectile.position.Y, Projectile.width, Projectile.height, ModContent.ItemType<CometShard>(), 1, false, 0, false, false);
        }
    }
}