using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace CSkies.Content.NPCs.Bosses.Novacore
{
    public class NovaBlast : ModProjectile
    {
		public override void SetStaticDefaults()
		{
			ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
		}
		public override void SetDefaults()
        {
            Projectile.width = 24;
            Projectile.height = 24;
            Projectile.aiStyle = 5;
            Projectile.hostile = true;
            Projectile.penetrate = 2;
            Projectile.alpha = 255;
            Projectile.tileCollide = false;
            Projectile.extraUpdates = 1;
        }

        public override void AI()
		{
			if (Projectile.soundDelay == 0)
			{
				Projectile.soundDelay = 20 + Main.rand.Next(40);
				SoundEngine.PlaySound(SoundID.Item9, Projectile.position);
			}
			Projectile.alpha -= 15;
			int num75 = 150;
			if (Projectile.Center.Y >= Projectile.ai[1])
			{
				num75 = 0;
			}
			if (Projectile.alpha < num75)
			{
				Projectile.alpha = num75;
			}

			Projectile.rotation = Projectile.velocity.ToRotation() - (float)Math.PI / 2f;

			if (Main.rand.Next(16) == 0)
			{
				Vector2 value3 = Vector2.UnitX.RotatedByRandom(1.5707963705062866).RotatedBy(Projectile.velocity.ToRotation());
				int num77 = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, 58, Projectile.velocity.X * 0.5f, Projectile.velocity.Y * 0.5f, 150, default, 1.2f);
				Main.dust[num77].velocity = value3 * 0.66f;
				Main.dust[num77].position = Projectile.Center + value3 * 12f;
			}
			if (Main.rand.Next(48) == 0)
			{
				int num78 = Gore.NewGore(Projectile.GetSource_FromThis(), Projectile.Center, new Vector2(Projectile.velocity.X * 0.2f, Projectile.velocity.Y * 0.2f), 16);
				Gore gore = Main.gore[num78];
				gore.velocity *= 0.66f;
				gore = Main.gore[num78];
				gore.velocity += Projectile.velocity * 0.3f;
			}
		}

		public override void OnKill(int timeLeft)
		{
			SoundEngine.PlaySound(SoundID.Item10, Projectile.position);
			Projectile.velocity /= 2f;
			for (int num611 = 0; num611 < 40; num611++)
			{
				Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, 58, Projectile.velocity.X * 0.1f, Projectile.velocity.Y * 0.1f, 150, default, 1.2f);
			}
			for (int num612 = 0; num612 < 2; num612++)
			{
				Gore.NewGore(Projectile.GetSource_FromThis(), Projectile.position, new Vector2(Projectile.velocity.X * 0.05f, Projectile.velocity.Y * 0.05f), 16);
			}
        }

		public override Color? GetAlpha(Color lightColor)
		{
			Color value2 = Color.Lerp(lightColor, Color.White, 0.5f) * (1f - Projectile.alpha / 255f);
			Color value3 = Color.Lerp(Color.Purple, Color.White, 0.33f);
			float amount = 0.25f + (float)Math.Cos(Projectile.localAI[0]) * 0.25f;
			return Color.Lerp(value2, value3, amount);
		}

		public override bool PreDraw(ref Color lightColor)
		{
            SpriteBatch sb = Main.spriteBatch;
            Texture2D value13 = TextureAssets.Projectile[Projectile.type].Value;
			Rectangle rectangle3 = new Rectangle(0, 0, value13.Width, value13.Height);
			Vector2 origin4 = rectangle3.Size() / 2f;
			origin4.Y = 70f;
			
			Color color38 = Projectile.GetAlpha(lightColor);
			float num169 = Projectile.scale;
			float rotation23 = Projectile.rotation;

			sb.Draw(value13, Projectile.Center + Vector2.Zero - Main.screenPosition + new Vector2(0f, Projectile.gfxOffY), rectangle3, color38, rotation23, origin4, num169, SpriteEffects.None, 0);

			sb.Draw(ModContent.Request<Texture2D>("CSkies/Content/NPCs/Bosses/Novacore/NovaBlast_Nova").Value, Projectile.Center - Main.screenPosition + new Vector2(0f, Projectile.gfxOffY), rectangle3, Color.White, Projectile.localAI[0], origin4, Projectile.scale, SpriteEffects.None, 0);
			return false;
		}
	}
}