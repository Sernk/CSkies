using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.GameContent;
using Terraria.ModLoader;
using Terraria.ID;
using CSkies.Utilities;
using CSkies.Utilities.Base.BaseMod.Base;

namespace CSkies.Content.NPCs.Bosses.ObserverVoid
{
    class Vortex : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Vortex");
        }

        public override void SetDefaults()
        {
            Projectile.width = 60;
            Projectile.height = 60;
            Projectile.timeLeft = 180;
            Projectile.aiStyle = -1;
            Projectile.alpha = 255;
            Projectile.tileCollide = false;
            Projectile.penetrate = -1;
        }

        public override void AI()
        {
            Lighting.AddLight(Projectile.Center, 0, 0f, .15f);
            Projectile.rotation += 0.1f;
            Projectile.velocity *= 0;
            if (Projectile.alpha > 0)
            {
                Projectile.alpha -= 3;
            }
            else
            {
                Projectile.alpha = 0;
            }
            if (Main.netMode != NetmodeID.MultiplayerClient)
            {
                Projectile.ai[0]++;
            }
            if (Projectile.ai[0] > 120)
            {
                Projectile.scale = Projectile.ai[1];

                if (Projectile.ai[0] == 121)
                {
                    Projectile.netUpdate = true;
                }
                if (Projectile.ai[1] > 0)
                {
                    Projectile.ai[1] -= .01f;
                }

                if (Projectile.ai[0] <= 0 && Main.netMode != NetmodeID.MultiplayerClient)
                {
                    Projectile.active = false;
                    Projectile.netUpdate = true;
                }
            }

            for (int u = 0; u < Main.maxPlayers; u++)
            {
                Player target = Main.player[u];

                if (target.active && Vector2.Distance(Projectile.Center, target.Center) < 160 * Projectile.ai[1] && !target.immune)
                {
                    float num3 = 3f;
                    Vector2 vector = target.Center;
                    float num4 = Projectile.Center.X - vector.X;
                    float num5 = Projectile.Center.Y - vector.Y;
                    float num6 = (float)Math.Sqrt(num4 * num4 + num5 * num5);
                    num6 = num3 / num6;
                    num4 *= num6;
                    num5 *= num6;
                    int num7 = 6;
                    target.velocity.X = (target.velocity.X * (num7 - 1) + num4) / num7;
                    target.velocity.Y = (target.velocity.Y * (num7 - 1) + num5) / num7;
                }
            }
        }


        /*public override bool PreDraw(ref Color lightColor)
        {
            SpriteBatch spritebatch = Main.spriteBatch;
            Texture2D Tex = TextureAssets.Projectile[Projectile.type].Value;
            Texture2D Vortex = ModContent.Request<Texture2D>("CSkies/Content/NPCs/Bosses/ObserverVoid/Vortex1").Value;
            Rectangle frame = new Rectangle(0, 0, Tex.Width, Tex.Height);
            BaseDrawing.DrawTexture(spritebatch, Vortex, 0, Projectile.position, Projectile.width, Projectile.height, Projectile.scale, Projectile.rotation, 0, 1, frame, Projectile.GetAlpha(Color.White * 0.8f), true);
            BaseDrawing.DrawTexture(spritebatch, Tex, 0, Projectile.position, Projectile.width, Projectile.height, Projectile.scale, -Projectile.rotation, 0, 1, frame, Projectile.GetAlpha(Color.White * 0.8f), true);
            return false;
        }*/
    }
}
