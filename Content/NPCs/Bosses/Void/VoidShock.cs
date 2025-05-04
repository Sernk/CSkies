using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Utilities;
using CSkies.Utilities;

namespace CSkies.Content.NPCs.Bosses.Void
{
    public class VoidShock : ModProjectile
    {

        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 25;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 1;
        }
        public override void SetDefaults()
        {
            Projectile.penetrate = -1;
            Projectile.width = 14;
            Projectile.height = 14;
            Projectile.aiStyle = -1;
            Projectile.hostile = true;
            Projectile.alpha = 255;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = true;
            Projectile.extraUpdates = 4;
            Projectile.timeLeft = 120;
        }

        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
            for (int i = 0; i < Projectile.oldPos.Length; i++)
            {
                projHitbox.X = (int)Projectile.oldPos[i].X;
                projHitbox.Y = (int)Projectile.oldPos[i].Y;
                if (projHitbox.Intersects(targetHitbox))
                {
                    return true;
                }
            }
            return base.Colliding(projHitbox, targetHitbox);
        }

        public override void AI()
        {
            Lighting.AddLight(Projectile.Center, 0.45f, 0f, 0.5f);

            CAI.LightningAI(Projectile, ref Projectile.ai, ref Projectile.localAI, 74);
        }

        public override bool PreDraw(ref Color lightColor)
        {

            SpriteBatch spriteBatch = Main.spriteBatch;
            CDrawing.LightningDraw(Projectile, spriteBatch, Color.White, new Color(30, 30, 50), ref Projectile.localAI);
            return false;
        }
    }
}
 