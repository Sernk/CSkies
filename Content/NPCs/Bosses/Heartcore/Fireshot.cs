using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.Graphics.Shaders;
using Terraria.ID;
using Terraria.ModLoader;
using CSkies.Utilities.Base.BaseMod.Base;

namespace CSkies.Content.NPCs.Bosses.Heartcore
{
    public class Fireshot : ModProjectile
    {
    	
        public override void SetDefaults()
        {
            Projectile.width = 32;
            Projectile.height = 32;
            Projectile.friendly = false;
            Projectile.hostile = true;
            Projectile.ignoreWater = true;
            Projectile.penetrate = 1;
            Projectile.extraUpdates = 1;
        }

        public override void AI()
        {
            if (Main.rand.Next(2) == 0)
            {
                int dustnumber = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.SolarFlare, 0f, 0f, 200, default, 0.5f);
                Main.dust[dustnumber].velocity *= 0.3f;
            }
            Projectile.rotation = Projectile.velocity.ToRotation() + 1.57079637f;
        }

        public override void OnKill(int timeLeft)
        {
            for (int num468 = 0; num468 < 5; num468++)
            {
                int num469 = Dust.NewDust(new Vector2(Projectile.Center.X, Projectile.Center.Y), Projectile.width, Projectile.height, DustID.SolarFlare, -Projectile.velocity.X * 0.2f,
                    -Projectile.velocity.Y * 0.2f, 100, default, 2f);
                Main.dust[num469].noGravity = true;
            }
            //SoundEngine.PlaySound(new Terraria.Audio.LegacySoundStyle(2, 124, Terraria.Audio.SoundType.Sound));
            Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, new Vector2(0, 0), ModContent.ProjectileType<FireBoom>(), Projectile.damage, 2);
        }

        public override bool PreDraw(ref Color lightColor)
        {
            SpriteBatch sb = Main.spriteBatch;
            int r = GameShaders.Armor.GetShaderIdFromItemId(ItemID.LivingFlameDye);
            BaseDrawing.DrawAfterimage(sb, TextureAssets.Projectile[Projectile.type].Value, r, Projectile, .7f, 1, 5, false, 0, 0);
            BaseDrawing.DrawTexture(sb, TextureAssets.Projectile[Projectile.type].Value, r, Projectile, Color.White, false);
            return false;
        }
    }
}