using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;
using CSkies.Utilities.Base.BaseMod.Base;

namespace CSkies.Content.NPCs.Bosses.Observer
{
    public class StarProj : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.width = 30;
            Projectile.height = 30;
            Projectile.aiStyle = 0;
            Projectile.friendly = false;
            Projectile.hostile = true;
            Projectile.timeLeft = 180;
            Projectile.penetrate = 1;
            Projectile.tileCollide = true;
        }

		public override void SetStaticDefaults()
		{
		    // DisplayName.SetDefault("Cosmic Star");
		}

        public override void AI()
        {
            Projectile.rotation += .2f;
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            SoundEngine.PlaySound(SoundID.Item89);
            Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center.X, Projectile.Center.Y - 15, Projectile.velocity.X, Projectile.velocity.Y, Mod.Find<ModProjectile>("Shock").Type, Projectile.damage, Projectile.knockBack, Projectile.owner, 0f, 0f);
            return true;
        }

        public override void OnKill(int timeLeft)
        {
            int dustID = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, 17, Projectile.velocity.X * 0.2f, Projectile.velocity.Y * 0.2f, 0, Color.White, 1f);
            Main.dust[dustID].velocity = new Vector2(MathHelper.Lerp(-1f, 1f, (float)Main.rand.NextDouble()), MathHelper.Lerp(-1f, 1f, (float)Main.rand.NextDouble()));
        }

        public override bool PreDraw(ref Color lightColor)
        {
            SpriteBatch sb = Main.spriteBatch;
            BaseDrawing.DrawTexture(sb, TextureAssets.Projectile[Projectile.type].Value, 0, Projectile, Projectile.GetAlpha(Color.White * 0.8f), true);
            return false;
        }
    }
}
