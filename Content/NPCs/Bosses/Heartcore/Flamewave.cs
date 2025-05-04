using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.GameContent;
using Terraria.Graphics.Shaders;
using Terraria.ID;
using Terraria.ModLoader;
using CSkies.Utilities.Base.BaseMod.Base;

namespace CSkies.Content.NPCs.Bosses.Heartcore
{
    public class Flamewave : ModProjectile
	{
		public override void SetDefaults()
		{
			Projectile.width = 34;
			Projectile.height = 34;
			Projectile.friendly = false;
            Projectile.hostile = true;
            Projectile.penetrate = 2;
			Projectile.aiStyle = -1;
			Projectile.timeLeft = 300;
			Projectile.penetrate = 5;
            Projectile.tileCollide = false;
            Projectile.extraUpdates = 1;
		}
        
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Flamewave");
		}
		
		public override Color? GetAlpha(Color lightColor)
		{
			return Color.White;
		}

        public override void OnHitPlayer(Player target, Player.HurtInfo info)
		{
            for (int num468 = 0; num468 < 5; num468++)
            {
                int num469 = Dust.NewDust(new Vector2(Projectile.Center.X, Projectile.Center.Y), Projectile.width, Projectile.height, DustID.SolarFlare, -Projectile.velocity.X * 0.2f,
                    -Projectile.velocity.Y * 0.2f, 100, default, 2f);
                Main.dust[num469].noGravity = true;
            }
		}
		
		public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
		{
			// Inflate some target hitboxes if they are beyond 8,8 size
			if (targetHitbox.Width > 8 && targetHitbox.Height > 8)
			{
				targetHitbox.Inflate(-targetHitbox.Width / 8, -targetHitbox.Height / 8);
			}
			// Return if the hitboxes intersects, which means the javelin collides or not
			return projHitbox.Intersects(targetHitbox);
		}
		
		public override void AI()
		{
			Projectile.rotation =
			Projectile.velocity.ToRotation() +
			MathHelper.ToRadians(90f);
		}

        public override void OnKill(int timeLeft)
        {
            for (int num468 = 0; num468 < 5; num468++)
            {
                int num469 = Dust.NewDust(new Vector2(Projectile.Center.X, Projectile.Center.Y), Projectile.width, Projectile.height, DustID.SolarFlare, -Projectile.velocity.X * 0.2f,
                    -Projectile.velocity.Y * 0.2f, 100, default, 2f);
                Main.dust[num469].noGravity = true;
            }
        }

        public override bool PreDraw(ref Color lightColor)
        {
            SpriteBatch sb = Main.spriteBatch;
            int r = GameShaders.Armor.GetShaderIdFromItemId(ItemID.LivingFlameDye);
            BaseDrawing.DrawAfterimage(sb, TextureAssets.Projectile[Projectile.type].Value, r, Projectile, .7f, 1, 5, false, 0, 0);
            BaseDrawing.DrawTexture(sb, TextureAssets.Projectile[Projectile.type].Value, r, Projectile, Color.White, true);
            return false;
        }
	}
}