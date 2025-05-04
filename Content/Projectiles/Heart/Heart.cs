using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ModLoader;
using Terraria.ID;

namespace CSkies.Content.Projectiles.Heart
{
    public class Heart : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.width = 24;
            Projectile.height = 24;
            Projectile.aiStyle = 5;
            Projectile.friendly = true;
            Projectile.penetrate = 2;
            Projectile.alpha = 50;
            Projectile.scale = 0.8f;
            Projectile.tileCollide = false;
        }

        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Heart");
        }

        public override Color? GetAlpha(Color lightColor)
        {
            return Color.White;
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.AddBuff(BuffID.Daybreak, 200);
        }

        public override void AI()
        {
            if (Projectile.position.Y > Projectile.ai[1])
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
            Projectile.rotation =
            Projectile.velocity.ToRotation() +
            MathHelper.ToRadians(90f);
            Projectile.light = 0.9f;
            if (Main.rand.Next(10) == 0)
            {
                Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, ModContent.DustType<Dusts.HeartDust>(), Projectile.velocity.X * 0.5f, Projectile.velocity.Y * 0.5f, 150, default, 1.2f);
            }
            if (Main.rand.Next(20) == 0)
            {
                Gore.NewGore(Projectile.GetSource_FromThis(), Projectile.position, new Vector2(Projectile.velocity.X * 0.2f, Projectile.velocity.Y * 0.2f), Main.rand.Next(16, 18), 1f);
                return;
            }
        }

        public override void OnKill(int timeleft)
        {
            SoundEngine.PlaySound(SoundID.Item14, Projectile.position);
            int num290 = Main.rand.Next(3, 7);
            for (int num291 = 0; num291 < num290; num291++)
            {
                int num292 = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, ModContent.DustType<Dusts.HeartDust>(), 0f, 0f, 100, default, 2.1f);
                Main.dust[num292].velocity *= 2f;
                Main.dust[num292].noGravity = true;
            };
        }
    }
}