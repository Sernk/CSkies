using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ModLoader;
using Terraria.ID;

namespace CSkies.Content.NPCs.Bosses.Starcore
{
    public class Starsphere : ModProjectile
	{
		public override void SetDefaults()
		{
			Projectile.width = 14;
			Projectile.height = 14;
			Projectile.aiStyle = 1;
			Projectile.hostile = true;
			Projectile.penetrate = 1;
			Projectile.aiStyle = -1;
			Projectile.timeLeft = 1200;
            Main.projFrames[Projectile.type] = 5;
        }
		
		public override Color? GetAlpha(Color lightColor)
		{
			return Color.White;
		}
		
		public override void AI()
        {
            Lighting.AddLight(Projectile.Center, 0f, .5f, 0f);
            if (Projectile.ai[0]++ > 20)
            {
                Projectile.velocity *= .93f;
            }

            if (Projectile.ai[0] > 120)
            {
                Projectile.Kill();
            }
            if (Projectile.frameCounter++ > 5)
            {
                Projectile.frameCounter = 0;
                if (Projectile.frame++ > 3)
                {
                    Projectile.frame = 0;
                }
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
                if (num293 != Projectile.whoAmI && Main.projectile[num293].active && Main.projectile[num293].owner == Projectile.owner && Main.projectile[num293].type == ModContent.ProjectileType<StormRing>() && Main.projectile[num293].getRect().Intersects(value19))
                {
                    Main.projectile[num293].ai[1] = 1f;
                    Main.projectile[num293].velocity = (Projectile.Center - Main.projectile[num293].Center) / 5f;
                    Main.projectile[num293].netUpdate = true;
                }
            }
            Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center.X, Projectile.Center.Y, 0f, 0f, ModContent.ProjectileType<StormRing>(), Projectile.damage / 4, 0f, Projectile.owner, 0f, 0f);
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            Projectile.velocity *= .90f;
            return false;
        }
    }
}