using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ModLoader;
using Terraria.ID;
using System;

namespace CSkies.Content.Items.Armor.Starsteel
{
    public class FallingStarProj : ModProjectile
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
			// DisplayName.SetDefault("Star");
            Main.projFrames[Projectile.type] = 3;
		}
		
		public override Color? GetAlpha(Color lightColor)
		{
			return Color.White;
		}
		
		public override void AI()
        {
            Projectile.frame = (int)Projectile.ai[0];

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

        public override void OnKill(int i)
        {
            SoundEngine.PlaySound(SoundID.Item10, Projectile.position);
            for (int num479 = 0; num479 < 10; num479++)
            {
                Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, ModContent.DustType<Dusts.StarDust>(), Projectile.velocity.X * 0.1f, Projectile.velocity.Y * 0.1f, 150, default, 1.2f);
            }
            for (int num480 = 0; num480 < 3; num480++)
            {
                Gore.NewGore(Projectile.GetSource_FromThis(), Projectile.position, new Vector2(Projectile.velocity.X * 0.05f, Projectile.velocity.Y * 0.05f), Main.rand.Next(16, 18), 1f);
            }
        }
    }
}