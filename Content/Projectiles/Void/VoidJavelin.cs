using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;
using CSkies.Utilities;

namespace CSkies.Content.Projectiles.Void
{
    public class VoidJavelin : Javelin
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Void Javelin");
        }

        public override void SetDefaults()
        {
            Projectile.width = 10;
            Projectile.height = 10;
            Projectile.aiStyle = -1;
            Projectile.friendly = true;
            Projectile.DamageType = DamageClass.Melee;
            Projectile.penetrate = 1;
            Projectile.GetGlobalProjectile<Buffs.ImplaingProjectile>().CanImpale = true;
            Projectile.GetGlobalProjectile<Buffs.ImplaingProjectile>().damagePerImpaler = 20;
            maxStickingJavelins = 12;
            rotationOffset = (float)Math.PI / 4;
        }

        public override bool PreDraw(ref Color lightColor)
        {
            SpriteBatch spriteBatch = Main.spriteBatch;
            Texture2D texture = TextureAssets.Projectile[Projectile.type].Value;
            spriteBatch.Draw(texture, new Vector2(Projectile.Center.X - Main.screenPosition.X, Projectile.Center.Y - Main.screenPosition.Y + 2),
                        new Rectangle(0, 0, texture.Width, texture.Height), Color.White, Projectile.rotation,
                        new Vector2(Projectile.width * 0.5f, Projectile.height * 0.5f), 1f, SpriteEffects.None, 0f);
            return false;
        }

        public override void OnKill(int timeLeft)
        {
            if (Projectile.ai[0] == 1f)
            {
                SoundEngine.PlaySound(SoundID.Item89);
                int p = Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center.X, Projectile.Center.Y, Projectile.velocity.X, Projectile.velocity.Y, ModContent.ProjectileType<VoidBoom>(), Projectile.damage, Projectile.knockBack, Projectile.owner, 0f, 0f);
                Main.projectile[p].Center = Projectile.Center;
            }
        }
    }
}
