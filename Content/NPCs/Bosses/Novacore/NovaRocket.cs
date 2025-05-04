using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace CSkies.Content.NPCs.Bosses.Novacore
{
    public class NovaRocket : ModProjectile
    {
		public override void SetDefaults()
		{
			Projectile.width = 14;
			Projectile.height = 14;
			Projectile.aiStyle = 80;
			Projectile.hostile = true;
			Projectile.penetrate = -1;
			Projectile.tileCollide = false;
		}

        public override void AI()
		{
			if (Projectile.ai[0] == 0f && Projectile.ai[1] > 0f)
			{
				ref float reference = ref Projectile.ai[1];
				ref float reference32 = ref reference;
				float num15 = reference;
				reference32 = num15 - 1f;
			}
			else if (Projectile.ai[0] == 0f && Projectile.ai[1] == 0f)
			{
				Projectile.ai[0] = 1f;
				Projectile.ai[1] = Player.FindClosest(Projectile.position, Projectile.width, Projectile.height);
				Projectile.netUpdate = true;
				float num639 = Projectile.velocity.Length();
				Projectile.velocity = Vector2.Normalize(Projectile.velocity) * (num639 + 4f);
				for (int num640 = 0; num640 < 8; num640++)
				{
					Vector2 spinningpoint9 = Vector2.UnitX * -8f;
					spinningpoint9 += -Vector2.UnitY.RotatedBy(num640 * (float)Math.PI / 4f) * new Vector2(2f, 8f);
					spinningpoint9 = spinningpoint9.RotatedBy(Projectile.rotation - (float)Math.PI / 2f);
					int num641 = Dust.NewDust(Projectile.Center, 0, 0, 228);
					Main.dust[num641].scale = 1.5f;
					Main.dust[num641].noGravity = true;
					Main.dust[num641].position = Projectile.Center + spinningpoint9;
					Main.dust[num641].velocity = Projectile.velocity * 0f;
				}
			}
			else if (Projectile.ai[0] == 1f)
			{
				Projectile.tileCollide = true;
				ref float reference = ref Projectile.localAI[1];
				ref float reference33 = ref reference;
				float num15 = reference;
				reference33 = num15 + 1f;
				float num642 = 180f;
				float num643 = 0f;
				float num644 = 30f;
				if (Projectile.localAI[1] == num642)
				{
					Projectile.Kill();
					return;
				}
				if (Projectile.localAI[1] >= num643 && Projectile.localAI[1] < num643 + num644)
				{
					Vector2 v4 = Main.player[(int)Projectile.ai[1]].Center - Projectile.Center;
					float num645 = Projectile.velocity.ToRotation();
					float num646 = v4.ToRotation();
					double num647 = num646 - num645;
					if (num647 > Math.PI)
					{
						num647 -= Math.PI * 2.0;
					}
					if (num647 < -Math.PI)
					{
						num647 += Math.PI * 2.0;
					}
					Projectile.velocity = Projectile.velocity.RotatedBy(num647 * 0.20000000298023224);
				}
				if (Projectile.localAI[1] % 5f == 0f)
				{
					for (int num648 = 0; num648 < 4; num648++)
					{
						Vector2 spinningpoint10 = Vector2.UnitX * -8f;
						spinningpoint10 += -Vector2.UnitY.RotatedBy(num648 * (float)Math.PI / 4f) * new Vector2(2f, 4f);
						spinningpoint10 = spinningpoint10.RotatedBy(Projectile.rotation - (float)Math.PI / 2f);
						int num649 = Dust.NewDust(Projectile.Center, 0, 0, 228);
						Main.dust[num649].scale = 1.5f;
						Main.dust[num649].noGravity = true;
						Main.dust[num649].position = Projectile.Center + spinningpoint10;
						Main.dust[num649].velocity = Projectile.velocity * 0f;
					}
				}
			}
			Projectile.rotation = Projectile.velocity.ToRotation() + (float)Math.PI / 2f;
			if (++Projectile.frameCounter >= 3)
			{
				Projectile.frameCounter = 0;
				if (++Projectile.frame >= 3)
				{
					Projectile.frame = 0;
				}
			}
			for (int num650 = 0; num650 < 1f + Projectile.ai[0]; num650++)
			{
				Vector2 value21 = Vector2.UnitY.RotatedBy(Projectile.rotation) * 8f * (num650 + 1);
				int num651 = Dust.NewDust(Projectile.Center, 0, 0, 228);
				Main.dust[num651].position = Projectile.Center + value21;
				Main.dust[num651].scale = 1f;
				Main.dust[num651].noGravity = true;
			}
			int num652 = 0;
			while (true)
			{
				if (num652 < 255)
				{
					Player player7 = Main.player[num652];
					if (player7.active && !player7.dead && Vector2.Distance(player7.Center, Projectile.Center) <= 42f)
					{
						break;
					}
					num652++;
					continue;
				}
				return;
			}
			Projectile.Kill();
		}

		public override void OnKill(int timeLeft)
		{
			SoundEngine.PlaySound(SoundID.Item14, Projectile.position);
			Projectile.position = Projectile.Center;
			Projectile.width = (Projectile.height = 112);
			Projectile.position.X -= Projectile.width / 2;
			Projectile.position.Y -= Projectile.height / 2;
			for (int num359 = 0; num359 < 4; num359++)
			{
				Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, ModContent.DustType<Dusts.CDust>(), 0f, 0f, 100, default, 1.5f);
			}
			for (int num360 = 0; num360 < 40; num360++)
			{
				int num361 = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, 228, 0f, 0f, 0, default, 2.5f);
				Main.dust[num361].noGravity = true;
				Dust dust2 = Main.dust[num361];
				dust2.velocity *= 3f;
				num361 = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, 228, 0f, 0f, 100, default, 1.5f);
				dust2 = Main.dust[num361];
				dust2.velocity *= 2f;
				Main.dust[num361].noGravity = true;
			}
			for (int num362 = 0; num362 < 1; num362++)
			{
				int num363 = Gore.NewGore(Projectile.GetSource_FromThis(), Projectile.position + new Vector2(Projectile.width * Main.rand.Next(100) / 100f, Projectile.height * Main.rand.Next(100) / 100f) - Vector2.One * 10f, default, Main.rand.Next(61, 64));
				Gore gore = Main.gore[num363];
				gore.velocity *= 0.3f;
				Main.gore[num363].velocity.X += Main.rand.Next(-10, 11) * 0.05f;
				Main.gore[num363].velocity.Y += Main.rand.Next(-10, 11) * 0.05f;
			}
			Projectile.Damage();
		}
	}
}