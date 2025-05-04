using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.ModLoader;
using Terraria.ID;

namespace CSkies.Content.NPCs.Bosses.ObserverVoid
{
    public class VoidLaser : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Void Beam");
		}

        public override void SetDefaults()
        {
            Projectile.width = 18;
            Projectile.height = 18;
            Projectile.hostile = true;
            Projectile.friendly = false;
            Projectile.tileCollide = false;
            Projectile.usesLocalNPCImmunity = true;
            CooldownSlot = 1;
            Projectile.penetrate = -1;
            Projectile.alpha = 255;
        }

        public override void AI()
        {
            Lighting.AddLight(Projectile.Center, 0, 0f, .15f);
            Vector2? vector69 = null;
            if (Projectile.velocity.HasNaNs() || Projectile.velocity == Vector2.Zero)
            {
                Projectile.velocity = -Vector2.UnitY;
            }
            if (Main.npc[(int)Projectile.ai[1]].active && Main.npc[(int)Projectile.ai[1]].type == ModContent.NPCType<ObserverVoid>())
            {
                Vector2 offset = new Vector2(30f, 30f);
                Vector2 offsetElipse = Utils.Vector2FromElipse(Main.npc[(int)Projectile.ai[1]].localAI[0].ToRotationVector2(), offset * Main.npc[(int)Projectile.ai[1]].localAI[1]);
                Projectile.position = Main.npc[(int)Projectile.ai[1]].Center + offsetElipse - new Vector2(Projectile.width, Projectile.height) / 2f;
            }
            if (Projectile.velocity.HasNaNs() || Projectile.velocity == Vector2.Zero)
            {
                Projectile.velocity = -Vector2.UnitY;
            }
            if (Projectile.localAI[0] == 0f)
            {
                SoundEngine.PlaySound(SoundID.Zombie104, Projectile.position);
            }
            if (Projectile.localAI[0]++ >= 180f)
            {
                Projectile.Kill();
                return;
            }
            Projectile.scale = (float)Math.Sin(Projectile.localAI[0] * 3.14159274f / 180f) * 10f * 0.4f;
            if (Projectile.scale > 0.4f)
            {
                Projectile.scale = 0.4f;
            }
            float velocity = Projectile.velocity.ToRotation();
            velocity += Projectile.ai[0];
            Projectile.rotation = velocity - 1.57079637f;
            Projectile.velocity = velocity.ToRotationVector2();
            Vector2 samplingPoint = Projectile.Center;
            if (vector69.HasValue)
            {
                samplingPoint = vector69.Value;
            }
            float num799 = 3f;
            float width = Projectile.width;
            float[] array3 = new float[(int)num799];
            Collision.LaserScan(samplingPoint, Projectile.velocity, width * Projectile.scale, 2400f, array3);
            float num801 = 0f;
            for (int num802 = 0; num802 < array3.Length; num802++)
            {
                num801 += array3[num802];
            }
            num801 /= num799;
            float amount = 0.5f;
            Projectile.localAI[1] = MathHelper.Lerp(Projectile.localAI[1], num801, amount);
            Vector2 vector70 = Projectile.Center + Projectile.velocity * (Projectile.localAI[1] - 14f);
            for (int num803 = 0; num803 < 2; num803++)
            {
                float laserVelocity = Projectile.velocity.ToRotation() + ((Main.rand.Next(2) == 1) ? -1f : 1f) * 1.57079637f;
                float rand = (float)Main.rand.NextDouble() * 2f + 2f;
                Vector2 dustVelocity = new Vector2((float)Math.Cos(laserVelocity) * rand, (float)Math.Sin(laserVelocity) * rand);
                int dust = Dust.NewDust(vector70, 0, 0, 229, dustVelocity.X, dustVelocity.Y, 0, default, 1f);
                Main.dust[dust].noGravity = true;
                Main.dust[dust].scale = 1.7f;
            }
            if (Main.rand.Next(5) == 0)
            {
                Vector2 laserVelocity = Projectile.velocity.RotatedBy(1.5707963705062866, default) * ((float)Main.rand.NextDouble() - 0.5f) * Projectile.width;
                int dust = Dust.NewDust(vector70 + laserVelocity - Vector2.One * 4f, 8, 8, 31, 0f, 0f, 100, default, 1.5f);
                Main.dust[dust].velocity *= 0.5f;
                Main.dust[dust].velocity.Y = -Math.Abs(Main.dust[dust].velocity.Y);
            }
            DelegateMethods.v3_1 = new Vector3(0.3f, 0.65f, 0.7f);
            Utils.PlotTileLine(Projectile.Center, Projectile.Center + Projectile.velocity * Projectile.localAI[1], Projectile.width * Projectile.scale, DelegateMethods.CastLight);
        }

        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
            float n = 0f;
            if (Collision.CheckAABBvLineCollision(targetHitbox.TopLeft(), targetHitbox.Size(), Projectile.Center, Projectile.Center + Projectile.velocity * Projectile.localAI[1], 30f * Projectile.scale, ref n))
                return true;

            return false;
        }

        public override bool PreDraw(ref Color lightColor)
        {
            if (Projectile.velocity == Vector2.Zero)
                return false;

            Texture2D tex2 = TextureAssets.Projectile[Projectile.type].Value;
            float num210 = Projectile.localAI[1];
            Color c_ = new Color(255, 255, 255, 127);
            Vector2 value20 = Projectile.Center.Floor();
            num210 -= Projectile.scale * 10.5f;
            Vector2 vector41 = new Vector2(Projectile.scale);
            DelegateMethods.f_1 = 1f;
            DelegateMethods.c_1 = c_;
            DelegateMethods.i_1 = 54000 - (int)Main.time / 2;
            Vector2 vector42 = Projectile.oldPos[0] + new Vector2(Projectile.width, Projectile.height) / 2f + Vector2.UnitY * Projectile.gfxOffY - Main.screenPosition;
            Utils.DrawLaser(Main.spriteBatch, tex2, value20 - Main.screenPosition, value20 + Projectile.velocity * num210 - Main.screenPosition, vector41, new Utils.LaserLineFraming(DelegateMethods.TurretLaserDraw));
            DelegateMethods.c_1 = new Color(255, 255, 255, 127) * 0.75f * Projectile.Opacity;
            Utils.DrawLaser(Main.spriteBatch, tex2, value20 - Main.screenPosition, value20 + Projectile.velocity * num210 - Main.screenPosition, vector41 / 2f, new Utils.LaserLineFraming(DelegateMethods.TurretLaserDraw));
            return false;
        }

    }
}
