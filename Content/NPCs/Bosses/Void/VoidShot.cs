using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;
using CSkies.Utilities.Base.BaseMod.Base;

namespace CSkies.Content.NPCs.Bosses.Void
{
    public class VoidShot : ModProjectile
    {
        public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Shadow Sphere");
		}
    	
        public override void SetDefaults()
        {
            Projectile.width = 38;
            Projectile.height = 38;
            Projectile.aiStyle = -1;
            Projectile.hostile = true;
            Projectile.timeLeft = 660;
            Projectile.light = 0.5f;
            Projectile.alpha = 255;
        }

        public override void AI()
        {
            if (Projectile.alpha > 0)
            {
                Projectile.alpha -= 5;
            }
            else
            {
                Projectile.alpha = 0;
            }

            for (int m = Projectile.oldPos.Length - 1; m > 0; m--)
            {
                Projectile.oldPos[m] = Projectile.oldPos[m - 1];
            }
            Projectile.oldPos[0] = Projectile.position;

            Projectile.frameCounter++;
            if (Projectile.frameCounter > 6)
            {
                Projectile.frameCounter = 0;
                Projectile.frame++;
                if (Projectile.frame > 4)
                {
                    Projectile.frame = 0;
                }
            }

            if (Projectile.ai[1]++ > 60)
            {
                Projectile.velocity *= 0.98f;
            }
            if (Projectile.ai[1]++ > 120)
            {
                Projectile.Kill();
            }
        }

        public override void OnKill(int timeLeft)
        {
            int damage = 120;
            if (Main.rand.Next(2) == 0) // + lasers
            {
                SoundEngine.PlaySound(SoundID.Item73, Projectile.position);
                int a = Projectile.NewProjectile(Projectile.GetSource_FromThis(), new Vector2(Projectile.Center.X, Projectile.Center.Y), new Vector2(0f, -12f), ModContent.ProjectileType<VoidBlast>(), damage, 3);
                Main.projectile[a].Center = Projectile.Center;
                int b = Projectile.NewProjectile(Projectile.GetSource_FromThis(), new Vector2(Projectile.Center.X, Projectile.Center.Y), new Vector2(0f, 12f), ModContent.ProjectileType<VoidBlast>(), damage, 3);
                Main.projectile[b].Center = Projectile.Center;
                int c = Projectile.NewProjectile(Projectile.GetSource_FromThis(), new Vector2(Projectile.Center.X, Projectile.Center.Y), new Vector2(-12f, 0f), ModContent.ProjectileType<VoidBlast>(), damage, 3);
                Main.projectile[c].Center = Projectile.Center;
                int d = Projectile.NewProjectile(Projectile.GetSource_FromThis(), new Vector2(Projectile.Center.X, Projectile.Center.Y), new Vector2(12f, 0f), ModContent.ProjectileType<VoidBlast>(), damage, 3);
                Main.projectile[d].Center = Projectile.Center;
            }
            else // x lasers
            {
                SoundEngine.PlaySound(SoundID.Item73, Projectile.position);
                int a = Projectile.NewProjectile(Projectile.GetSource_FromThis(), new Vector2(Projectile.Center.X, Projectile.Center.Y), new Vector2(8f, 8f), ModContent.ProjectileType<VoidBlast>(), damage, 3);
                Main.projectile[a].Center = Projectile.Center;
                int b = Projectile.NewProjectile(Projectile.GetSource_FromThis(), new Vector2(Projectile.Center.X, Projectile.Center.Y), new Vector2(8f, -8f), ModContent.ProjectileType<VoidBlast>(), damage, 3);
                Main.projectile[b].Center = Projectile.Center;
                int c = Projectile.NewProjectile(Projectile.GetSource_FromThis(), new Vector2(Projectile.Center.X, Projectile.Center.Y), new Vector2(-8f, 8f), ModContent.ProjectileType<VoidBlast>(), damage, 3);
                Main.projectile[c].Center = Projectile.Center;
                int d = Projectile.NewProjectile(Projectile.GetSource_FromThis(), new Vector2(Projectile.Center.X, Projectile.Center.Y), new Vector2(-8f, -8f), ModContent.ProjectileType<VoidBlast>(), damage, 3);
                Main.projectile[d].Center = Projectile.Center;
            }
        }

        public override bool PreDraw(ref Color lightColor)
        {
            SpriteBatch spriteBatch = Main.spriteBatch;
            Texture2D t = TextureAssets.Projectile[Projectile.type].Value;
            Rectangle frame = BaseDrawing.GetFrame(Projectile.frame, TextureAssets.Projectile[Projectile.type].Value.Width, TextureAssets.Projectile[Projectile.type].Value.Height / 5, 0, 0);
            BaseDrawing.DrawAfterimage(spriteBatch, t, 0, Projectile, 1, 1, 3, true, 0, 0, Projectile.GetAlpha(Color.White * 0.8f), frame, 5);
            BaseDrawing.DrawTexture(spriteBatch, t, 0, Projectile, Projectile.GetAlpha(Color.White * 0.8f), true);
            return false;
        }
    }
}
 