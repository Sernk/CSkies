using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;
using CSkies.Utilities;
using CSkies.Utilities.Base.BaseMod.Base;

namespace CSkies.Content.Projectiles.Void
{
    public class VoidEyeVortex : ModProjectile
    {
        public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Singularity");
		}
        public override void SetDefaults()
        {
            Projectile.width = 20;
            Projectile.height = 20;
            Projectile.friendly = true;
            Projectile.penetrate = -1; 
            Projectile.DamageType = DamageClass.Melee;
            Projectile.knockBack = 0;
            Projectile.alpha = 255;
            Projectile.tileCollide = false;
        }
		
		public override void AI()
		{
            Projectile.Center = Main.player[Projectile.owner].Center;
            if (Projectile.ai[0] == 0)
            {
                if (Projectile.alpha > 0)
                {
                    Projectile.alpha -= 5;
                    Projectile.netUpdate = true;
                }
                else
                {
                    Projectile.ai[0] = 1;
                }
            }
            else if (Projectile.ai[0] == 1)
            {
                if (Main.netMode != NetmodeID.MultiplayerClient && Projectile.ai[1]++ > 120)
                {
                    Projectile.ai[0] = 2;
                    Projectile.netUpdate = true;
                }
            }
            else
            {
                if (Projectile.alpha < 255)
                {
                    Projectile.alpha += 5;
                }
                else
                {
                    Projectile.active = false;
                    Projectile.netUpdate = true;
                }
            }

            for (int u = 0; u < Main.maxNPCs; u++)
            {
                NPC target = Main.npc[u];

                if (target.active && !target.boss && target.type != NPCID.TargetDummy && Vector2.Distance(Projectile.Center, target.Center) < 300)
                {
                    float num3 = 6f;
                    Vector2 vector = new Vector2(target.position.X + target.width / 2, target.position.Y + target.height / 2);
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
 
        public override bool PreDraw(ref Color lightColor)
        {
            SpriteBatch spriteBatch = Main.spriteBatch;
            Texture2D Tex = TextureAssets.Projectile[Projectile.type].Value;
            Texture2D Vortex = ModContent.Request<Texture2D>("CSkies/Content/NPCs/Bosses/ObserverVoid/Vortex1").Value;
            Rectangle frame = new Rectangle(0, 0, Tex.Width, Tex.Height);
            BaseDrawing.DrawTexture(spriteBatch, Vortex, 0, Projectile.position, Projectile.width, Projectile.height, Projectile.scale, Projectile.rotation, 0, 1, frame, Projectile.GetAlpha(Color.White * 0.8f), true);
            BaseDrawing.DrawTexture(spriteBatch, Tex, 0, Projectile.position, Projectile.width, Projectile.height, Projectile.scale, -Projectile.rotation, 0, 1, frame, Projectile.GetAlpha(Color.White * 0.8f), true);
            return false;
        }
    }
}