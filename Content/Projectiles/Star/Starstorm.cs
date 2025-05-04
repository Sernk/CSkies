using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ModLoader;
using Terraria.ID;
using System;

namespace CSkies.Content.Projectiles.Star
{
    public class Starstorm : ModProjectile
	{
		public override void SetDefaults()
        {
            Projectile.width = 24;
            Projectile.height = 24;
            Projectile.aiStyle = -1;
            Projectile.friendly = true;
            Projectile.penetrate = 1;
            Projectile.alpha = 50;
            Projectile.scale = 0.8f;
            Projectile.tileCollide = false;
            Projectile.extraUpdates = 1;
        }
		
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Star");
		}
		
		public override Color? GetAlpha(Color lightColor)
		{
			return Color.White;
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
            Projectile.rotation += (Math.Abs(Projectile.velocity.X) + Math.Abs(Projectile.velocity.Y)) * 0.01f * Projectile.direction;
            Projectile.light = 0.9f;
            if (Main.rand.Next(10) == 0)
            {
                Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, ModContent.DustType<Dusts.StarDust>(), Projectile.velocity.X * 0.5f, Projectile.velocity.Y * 0.5f, 150, default, 1.2f);
            }
            if (Main.rand.Next(20) == 0)
            {
                Gore.NewGore(Projectile.GetSource_FromThis(), Projectile.position, new Vector2(Projectile.velocity.X * 0.2f, Projectile.velocity.Y * 0.2f), Main.rand.Next(16, 18), 1f);
                return;
            }
        }

        public override void OnKill(int timeleft)
        {
            SoundEngine.PlaySound(SoundID.Item94, Projectile.position);
            int num290 = Main.rand.Next(3, 7);
            for (int num291 = 0; num291 < num290; num291++)
            {
                int num292 = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, ModContent.DustType<Dusts.StarDust>(), 0f, 0f, 100, default, 2.1f);
                Main.dust[num292].velocity *= 2f;
                Main.dust[num292].noGravity = true;
            }
            for (int num293 = 0; num293 < 1000; num293++)
            {
                Rectangle value19 = new Rectangle((int)Projectile.Center.X - 40, (int)Projectile.Center.Y - 40, 80, 80);
                if (num293 != Projectile.whoAmI && Main.projectile[num293].active && Main.projectile[num293].owner == Projectile.owner && Main.projectile[num293].type == ModContent.ProjectileType<StarRing>() && Main.projectile[num293].getRect().Intersects(value19))
                {
                    Main.projectile[num293].ai[1] = 1f;
                    Main.projectile[num293].velocity = (Projectile.Center - Main.projectile[num293].Center) / 5f;
                    Main.projectile[num293].netUpdate = true;
                }
            }
            Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center.X, Projectile.Center.Y, 0f, 0f, ModContent.ProjectileType<StarRing>(), Projectile.damage / 4, 0f, Projectile.owner, 0f, 0f);
        }
    }
}