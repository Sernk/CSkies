using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.GameContent;
using Terraria.ModLoader;
using CSkies.Utilities;
using CSkies.Utilities.Base.BaseMod.Base;

namespace CSkies.Content.NPCs.Bosses.Starcore
{
    public class Starblast : ModProjectile
	{
        public int damage = 0;

		public override void SetStaticDefaults()
		{
            // DisplayName.SetDefault("Starbeam");
		}

        public override void SetDefaults()
        {
            Projectile.tileCollide = false;
            Projectile.width = 30;
            Projectile.height = 30;
            Projectile.aiStyle = -1;
            Projectile.penetrate = -1;
            Projectile.hostile = true;
            Projectile.ignoreWater = true;
            Projectile.timeLeft = 600;
        }

        public override void AI()
        {
            Lighting.AddLight(Projectile.Center, 0f, .3f, 0f);
            Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.ToRadians(90f);
            for (int m = Projectile.oldPos.Length - 1; m > 0; m--)
            {
                Projectile.oldPos[m] = Projectile.oldPos[m - 1];
            }
            Projectile.oldPos[0] = Projectile.position;
        }

        public override void OnKill(int timeLeft)
        {
            int dustID = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, ModContent.DustType<Dusts.StarDust>(), Projectile.velocity.X * 0.2f, Projectile.velocity.Y * 0.2f, 10, Color.White, 1f);
            Main.dust[dustID].velocity = new Vector2(MathHelper.Lerp(-1f, 1f, (float)Main.rand.NextDouble()), MathHelper.Lerp(-1f, 1f, (float)Main.rand.NextDouble()));
        }

        public override bool PreDraw(ref Color lightColor)
        {
            SpriteBatch sb = Main.spriteBatch;
            BaseDrawing.DrawAfterimage(sb, TextureAssets.Projectile[Projectile.type].Value, 0, Projectile, 2.5f, 1, 3, true, 0f, 0f, Projectile.GetAlpha(Colors.COLOR_GLOWPULSE));
            BaseDrawing.DrawTexture(sb, TextureAssets.Projectile[Projectile.type].Value, 0, Projectile, Projectile.GetAlpha(Colors.COLOR_GLOWPULSE), true);
            return false;
		}		
	}
}