using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.IO;
using Terraria;
using Terraria.GameContent;
using Terraria.ModLoader;
using Terraria.ID;
using CSkies.Utilities;
using CSkies.Utilities.Base.BaseMod.Base;

namespace CSkies.Content.Items.Armor.Starsteel
{
    public class StarGuardian : ModProjectile
	{
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Terra Crystal");
            Main.projFrames[Projectile.type] = 3;
		}

		public override void SetDefaults()
        {
            Projectile.width = 22;
            Projectile.height = 42;
            Projectile.aiStyle = -1;
            Projectile.friendly = true;
            Projectile.tileCollide = false;
            Projectile.penetrate = -1;
            Projectile.timeLeft *= 5;
            Projectile.light = 0.4f;
            Projectile.ignoreWater = true;
            Projectile.minion = true;
            Projectile.minionSlots = 0;
        }

        public override void AI()
		{
			Player player = Main.player[Projectile.owner];
			CPlayer modPlayer = player.GetModPlayer<CPlayer>();
			if (!modPlayer.Starsteel || modPlayer.StarsteelBonus != 4)
            {
				Projectile.Kill();
				return;
			}

			Vector2 PlayerPoint;
			PlayerPoint.X = player.Center.X - Projectile.width / 2;
			PlayerPoint.Y = player.Center.Y - Projectile.height / 2 + player.gfxOffY - 60f;

			Projectile.position = PlayerPoint;

			if (player.gravDir == -1f)
			{
				Projectile.position.Y = Projectile.position.Y + 120f;
				Projectile.rotation = 3.14f;
			}
			else
			{
				Projectile.rotation = 0f;
			}

			if (Projectile.owner == Main.myPlayer)
			{
				if (Projectile.ai[0] != 0f)
				{
					Projectile.ai[0] -= 1f;
					return;
				}
				float num396 = Projectile.position.X;
				float num397 = Projectile.position.Y;
				float num398 = 700f;
				bool flag11 = false;
				for (int num399 = 0; num399 < 200; num399++)
				{
					if (Main.npc[num399].CanBeChasedBy(this, true))
					{
						float num400 = Main.npc[num399].position.X + Main.npc[num399].width / 2;
						float num401 = Main.npc[num399].position.Y + Main.npc[num399].height / 2;
						float num402 = Math.Abs(Projectile.position.X + Projectile.width / 2 - num400) + Math.Abs(Projectile.position.Y + Projectile.height / 2 - num401);
						if (num402 < num398 && Collision.CanHit(Projectile.position, Projectile.width, Projectile.height, Main.npc[num399].position, Main.npc[num399].width, Main.npc[num399].height))
						{
							num398 = num402;
							num396 = num400;
							num397 = num401;
							flag11 = true;
						}
					}
				}
				if (flag11)
				{
					float num403 = 12f;
					Vector2 vector29 = new Vector2(Projectile.position.X + Projectile.width * 0.5f, Projectile.position.Y + Projectile.height * 0.5f);
					float num404 = num396 - vector29.X;
					float num405 = num397 - vector29.Y;
					float num406 = (float)Math.Sqrt(num404 * num404 + num405 * num405);
					num406 = num403 / num406;
					num404 *= num406;
					num405 *= num406;
					int p = Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center.X - 4f, Projectile.Center.Y, num404, num405, ModContent.ProjectileType<StarGuardianShot>(), Player.crystalLeafDamage, Player.crystalLeafKB, Projectile.owner, 0f, 0f);
					Main.projectile[p].minion = true;
					Main.projectile[p].minionSlots = 0;
					Projectile.ai[0] = 160;
					return;
				}
			}
		}

        public float auraPercent = 0f;
        public bool auraDirection = true;

        public override bool PreDraw(ref Color lightColor)
        {
            SpriteBatch sb = Main.spriteBatch;

            if (auraDirection) { auraPercent += 0.1f; auraDirection = auraPercent < 1f; }
            else { auraPercent -= 0.1f; auraDirection = auraPercent <= 0f; }

            Rectangle frame = BaseDrawing.GetFrame(Projectile.frame, TextureAssets.Projectile[Projectile.type].Value.Width, TextureAssets.Projectile[Projectile.type].Value.Height / 3, 0, 0);

            BaseDrawing.DrawAura(sb, TextureAssets.Projectile[Projectile.type].Value, 0, Projectile.position, Projectile.width, Projectile.height, auraPercent, 1.4f, Projectile.scale, Projectile.rotation, Projectile.direction, 3, frame, 0, 0, Color.White);
            BaseDrawing.DrawTexture(sb, TextureAssets.Projectile[Projectile.type].Value, 0, Projectile.position, Projectile.width, Projectile.height, Projectile.scale, Projectile.rotation, Projectile.direction, 3, frame, Projectile.GetAlpha(Color.White * 0.8f), true);

            return false;
        }

        public void MoveToPoint(Vector2 point)
		{
			float moveSpeed = 20f;
			float velMultiplier = 1f;
			Vector2 dist = point - Projectile.Center;
			float length = dist == Vector2.Zero ? 0f : dist.Length();
			if (length < moveSpeed)
			{
				velMultiplier = MathHelper.Lerp(0f, 1f, length / moveSpeed);
			}
			if (length < 200f)
			{
				moveSpeed *= 0.5f;
			}
			if (length < 100f)
			{
				moveSpeed *= 0.5f;
			}
			if (length < 50f)
			{
				moveSpeed *= 0.5f;
			}
			if (length < 10f)
			{
				moveSpeed *= 0.01f;
			}
			Projectile.velocity = length == 0f ? Vector2.Zero : Vector2.Normalize(dist);
			Projectile.velocity *= moveSpeed;
			Projectile.velocity *= velMultiplier;
		}
	}
}
