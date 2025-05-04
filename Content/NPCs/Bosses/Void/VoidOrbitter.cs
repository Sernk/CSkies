using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;
using CSkies.Utilities.Base.BaseMod.Base;

namespace CSkies.Content.NPCs.Bosses.Void
{
    public class VoidOrbitter : ModProjectile
	{
        public override string Texture => "CSkies/Content/NPCs/Bosses/ObserverVoid/Vortex";
        public override void SetStaticDefaults()
		{
            // DisplayName.SetDefault("Void Cyclone");
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
            Projectile.timeLeft = 600;
        }

		public int body = -1;
		public float rotValue = -1f;
        public Vector2 pos;

        public override void AI()
        {
            Lighting.AddLight(Projectile.Center, 0, .1f, .3f);
            if (!NPC.AnyNPCs(ModContent.NPCType<Void>()))
            {
                Projectile.active = false;
            }

            Projectile.rotation += 0.1f;

            if (body == -1)
            {
                int npcID = BaseAI.GetNPC(Projectile.Center, ModContent.NPCType<Void>(), 400f, null);
                if (npcID >= 0) body = npcID;
            }
            if (body == -1) return;
            NPC observer = Main.npc[body];
            if (observer == null || observer.life <= 0 || !observer.active || observer.type != ModContent.NPCType<Void>()) { Projectile.active = false; return; }

            pos = observer.Center;

            for (int m = Projectile.oldPos.Length - 1; m > 0; m--)
            {
                Projectile.oldPos[m] = Projectile.oldPos[m - 1];
            }
            Projectile.oldPos[0] = Projectile.position;

            int starNumber = 4;

            Projectile.timeLeft = 180;

            if (rotValue == -1f) rotValue = Projectile.ai[0] % starNumber * ((float)Math.PI * 2f / starNumber);
            rotValue += 0.08f;
            while (rotValue > (float)Math.PI * 2f) rotValue -= (float)Math.PI * 2f;

            Projectile.Center = BaseUtility.RotateVector(observer.Center, observer.Center + new Vector2(200, 0f), rotValue);

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

            if (observer.ai[0] < 3 || observer.ai[0] > 4)
            {
                Projectile.scale -= .05f;
                if (Projectile.scale <= 0)
                {
                    Projectile.active = false;
                    Projectile.netUpdate = true;
                }
            }
        }

        public override bool PreDraw(ref Color lightColor)
        {
            SpriteBatch spriteBatch = Main.spriteBatch;
            Texture2D Tex = ModContent.Request<Texture2D>("CSkies/Content/NPCs/Bosses/ObserverVoid/Vortex").Value;
            Texture2D Vortex = ModContent.Request<Texture2D>("CSkies/Content/NPCs/Bosses/ObserverVoid/Vortex1").Value;
            Rectangle frame = new Rectangle(0, 0, Tex.Width, Tex.Height);
            BaseDrawing.DrawTexture(spriteBatch, Vortex, 0, Projectile.position, Projectile.width, Projectile.height, Projectile.scale, Projectile.rotation, 0, 1, frame, Projectile.GetAlpha(Color.White), true);
            BaseDrawing.DrawTexture(spriteBatch, Tex, 0, Projectile.position, Projectile.width, Projectile.height, Projectile.scale, -Projectile.rotation, 0, 1, frame, Projectile.GetAlpha(Color.White), true);
            return false;
        }
    }
}