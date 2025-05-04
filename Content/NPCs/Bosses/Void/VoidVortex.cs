using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.GameContent;
using Terraria.ModLoader;

namespace CSkies.Content.NPCs.Bosses.Void
{
    public class VoidVortex : ModProjectile
	{
        public override void SetStaticDefaults()
		{
            // DisplayName.SetDefault("Void Vortex");
            Main.projFrames[Projectile.type] = 4;
        }

        public override void SetDefaults()
        {
            Projectile.tileCollide = false;
            Projectile.width = 30;
            Projectile.height = 30;
            Projectile.aiStyle = -1;
            Projectile.penetrate = -1;
            Projectile.friendly = false;
            Projectile.hostile = true;
            Projectile.ignoreWater = true;
            Projectile.timeLeft = 180;
        }

        public override void AI()
        {
            Lighting.AddLight(Projectile.Center, 0, .1f, .3f);

            for (int u = 0; u < Main.maxPlayers; u++)
            {
                Player target = Main.player[u];

                if (target.active && Vector2.Distance(Projectile.Center, target.Center) < 100 && !target.immune)
                {
                    float num3 = 6f;
                    Vector2 vector = new Vector2(target.position.X + target.width / 2, target.position.Y + target.height / 2);
                    float num4 = Projectile.Center.X - vector.X;
                    float num5 = Projectile.Center.Y - vector.Y;
                    float num6 = (float)Math.Sqrt(num4 * num4 + num5 * num5);
                    num6 = num3 / num6;
                    num4 *= num6;
                    num5 *= num6;
                    int num7 = 3;
                    target.velocity.X = (target.velocity.X * (num7 - 1) + num4) / num7;
                    target.velocity.Y = (target.velocity.Y * (num7 - 1) + num5) / num7;
                }
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

        public float auraPercent = 0f;
        public bool auraDirection = true;

        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D texture = TextureAssets.Projectile[Projectile.type].Value;
            int frameHeight = texture.Height / Main.projFrames[Projectile.type];
            Rectangle frame = new Rectangle(0, Projectile.frame * frameHeight, texture.Width, frameHeight);

            if (auraDirection)
            {
                auraPercent += 0.1f;
                auraDirection = auraPercent < 1f;
            }
            else
            {
                auraPercent -= 0.1f;
                auraDirection = auraPercent <= 0f;
            }

            Main.EntitySpriteDraw(
                texture,
                Projectile.Center - Main.screenPosition,
                frame,
                Projectile.GetAlpha(lightColor),
                Projectile.rotation,
                frame.Size() / 2f,
                Projectile.scale,
                Projectile.spriteDirection == 1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally,
                0
            );

            DrawAuraEffect(texture, frame, Projectile.GetAlpha(Color.White), auraPercent);

            return false; 
        }

        private void DrawAuraEffect(Texture2D texture, Rectangle frame, Color color, float percent)
        {
            for (int i = 0; i < 6; i++)
            {
                float scale = Projectile.scale * (1f + percent * 0.5f);
                float rotation = Projectile.rotation + MathHelper.TwoPi * i / 6f;
                Vector2 offset = Vector2.UnitX.RotatedBy(rotation) * percent * 30f;

                Main.EntitySpriteDraw(
                    texture,
                    Projectile.Center + offset - Main.screenPosition,
                    frame,
                    color * 0.7f,
                    rotation,
                    frame.Size() / 2f,
                    scale,
                    Projectile.spriteDirection == 1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally,
                    0
                );
            }
        }
    }
}