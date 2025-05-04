using Microsoft.Xna.Framework;
using Terraria;
using Terraria.GameContent;
using Terraria.ModLoader;
using System;
using Microsoft.Xna.Framework.Graphics;
using Terraria.Graphics.Shaders;
using Terraria.ID;
using CSkies.Utilities.Base.BaseMod.Base;
using CSkies.Content.NPCs.Bosses.ObserverVoid;

namespace CSkies.Content.NPCs.Bosses.Heartcore
{
    public class Fire : ModProjectile
	{
		public override void SetDefaults()
        {
            Projectile.width = 80;
            Projectile.height = 80;
            Projectile.aiStyle = -1;
            Projectile.hostile = true;
            Projectile.alpha = 255;
            Projectile.scale = 1f;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = false;
            Projectile.penetrate = -1;
            Main.projFrames[Projectile.type] = 4;
            Projectile.timeLeft = 400;
        }
		
		public override Color? GetAlpha(Color lightColor)
		{
			return Color.White;
		}
        private int shootTimer;

        public override void AI()
        {
            Lighting.AddLight(Projectile.Center, 0f, .7f, 0f);
            Vector2 vector62 = -Vector2.UnitY.RotatedBy(6.28318548f * Projectile.ai[0] / 30f, default);
            float val = 0.75f + vector62.Y * 0.25f;
            float val2 = 0.8f - vector62.Y * 0.2f;
            float num728 = Math.Max(val, val2);

            Projectile.frameCounter++;
            if (Projectile.frameCounter >= 7)
            {
                Projectile.frameCounter = 0;
                if (Projectile.frame++ >= 3)
                {
                    Projectile.frame = 0;
                }
            }

            for (int num729 = 0; num729 < 1; num729++)
            {
                float num730 = 55f * num728;
                float num731 = 11f * num728;
                float num732 = 0.5f;
                int num733 = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.SolarFlare, 0f, 0f, 100, default, 0.5f);
                Main.dust[num733].noGravity = true;
                Main.dust[num733].velocity *= 2f;
                Main.dust[num733].position = ((float)Main.rand.NextDouble() * MathHelper.TwoPi).ToRotationVector2() * (num731 + num732 * (float)Main.rand.NextDouble() * num730) + Projectile.Center;
                Main.dust[num733].velocity = Main.dust[num733].velocity / 2f + Vector2.Normalize(Main.dust[num733].position - Projectile.Center);
            }

            shootTimer++;
            if (shootTimer >= 30) 
            {
                shootTimer = 0;
                int p = BaseAI.GetPlayer(Projectile.Center, -1);
                Player player = Main.player[p];

                Vector2 direction = player.Center - Projectile.Center;
                direction.Normalize();
                direction *= 10f;
                Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, direction, ModContent.ProjectileType<Fireshot>(), 90, 1f, Main.myPlayer);
            }
        }


        public float auraPercent = 0f;
        public bool auraDirection = true;

        public override bool PreDraw(ref Color lightColor)
        {
            SpriteBatch sb = Main.spriteBatch;
            int r = GameShaders.Armor.GetShaderIdFromItemId(ItemID.LivingFlameDye);
            if (auraDirection) { auraPercent += 0.1f; auraDirection = auraPercent < 1f; }
            else { auraPercent -= 0.1f; auraDirection = auraPercent <= 0f; }

            Rectangle frame = BaseDrawing.GetFrame(Projectile.frame, TextureAssets.Projectile[Projectile.type].Value.Width, TextureAssets.Projectile[Projectile.type].Value.Height / Main.projFrames[Projectile.type], 0, 0);

            BaseDrawing.DrawAura(sb, TextureAssets.Projectile[Projectile.type].Value, r, Projectile.position, Projectile.width, Projectile.height, auraPercent, 1.5f, 1f, Projectile.rotation, Projectile.direction, 4, frame, 0f, 0f, null);
            BaseDrawing.DrawTexture(sb, TextureAssets.Projectile[Projectile.type].Value, r, Projectile.position, Projectile.width, Projectile.height, Projectile.scale, Projectile.rotation, 0, 4, frame, Projectile.GetAlpha(Color.White * 0.8f), true);
            BaseDrawing.DrawAura(sb, ModContent.Request<Texture2D>("CSkies/Glowmasks/Fire_Heart").Value, 0, Projectile.position, Projectile.width, Projectile.height, auraPercent, 1.5f, 1f, Projectile.rotation, Projectile.direction, 4, frame, 0f, 0f, Projectile.GetAlpha(Color.White));
            return false;
        }
    }
}