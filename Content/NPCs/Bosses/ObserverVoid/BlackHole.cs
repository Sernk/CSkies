using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;
using System.IO;
using Terraria.Audio;
using CSkies.Utilities.Base.BaseMod.Base;

namespace CSkies.Content.NPCs.Bosses.ObserverVoid
{
    public class BlackHole : ModProjectile
	{
        public int damage = 0;

		public override void SetStaticDefaults()
		{
            // DisplayName.SetDefault("Black Hole");
            Main.projFrames[Projectile.type] = 4;
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

		public int body = -1;
		public float rotValue = -1f;
        public Vector2 pos;

        public float[] internalAI = new float[1];

        public override void SendExtraAI(BinaryWriter writer)
        {
            base.SendExtraAI(writer);
            if (Main.netMode == NetmodeID.Server || Main.dedServ)
            {
                writer.Write(internalAI[0]);
            }
        }

        public override void ReceiveExtraAI(BinaryReader reader)
        {
            base.ReceiveExtraAI(reader);
            if (Main.netMode == NetmodeID.MultiplayerClient)
            {
                internalAI[0] = reader.ReadFloat();
            }
        }

        public override void AI()
        {
            Lighting.AddLight(Projectile.Center, 0, 0f, .15f);
            if (Projectile.frameCounter > 5)
            {
                Projectile.frame++;
                Projectile.frameCounter = 0;
                if (Projectile.frame > 3)
                {
                    Projectile.frame = 0;
                }
            }

            if (!NPC.AnyNPCs(ModContent.NPCType<ObserverVoid>()))
            {
                Projectile.active = false;
            }

            if (body == -1)
            {
                int npcID = BaseAI.GetNPC(Projectile.Center, ModContent.NPCType<ObserverVoid>(), 400f, null);
                if (npcID >= 0) body = npcID;
            }
            if (body == -1) return;
            NPC observer = Main.npc[body];
            if (observer == null || observer.life <= 0 || !observer.active || observer.type != ModContent.NPCType<ObserverVoid>()) { Projectile.active = false; return; }

            Player player = Main.player[observer.target];

            pos = observer.Center;

            for (int m = Projectile.oldPos.Length - 1; m > 0; m--)
            {
                Projectile.oldPos[m] = Projectile.oldPos[m - 1];
            }
            Projectile.oldPos[0] = Projectile.position;

            int starNumber = ((ObserverVoid)observer.ModNPC).StarCount;

            if (((ObserverVoid)observer.ModNPC).internalAI[0] == 0)
            {
                Projectile.timeLeft = 180;
                float dist = ((ObserverVoid)observer.ModNPC).internalAI[1];

                if (rotValue == -1f) rotValue = Projectile.ai[0] % starNumber * ((float)Math.PI * 2f / starNumber);
                rotValue += 0.08f;
                while (rotValue > (float)Math.PI * 2f) rotValue -= (float)Math.PI * 2f;

                for (int m = Projectile.oldPos.Length - 1; m > 0; m--)
                {
                    Projectile.oldPos[m] = Projectile.oldPos[m - 1];
                }
                Projectile.oldPos[0] = Projectile.position;

                Projectile.Center = BaseUtility.RotateVector(observer.Center, observer.Center + new Vector2(dist, 0f), rotValue);

                if (Projectile.ai[1]++ > 180)
                {
                    if (Collision.CanHit(Projectile.position, Projectile.width, Projectile.height, player.Center, player.width, player.height))
                    {
                        Projectile target = Main.projectile[Projectile.damage];
                        Vector2 fireTarget = Projectile.Center;
                        float rot = BaseUtility.RotationTo(Projectile.Center, player.Center);
                        fireTarget = BaseUtility.RotateVector(Projectile.Center, fireTarget, rot);
                        BaseAI.FireProjectile(player.Center, target, ModContent.ProjectileType<BlackHoleProj>(), damage, 0f, 4f);
                    }
                    Projectile.ai[1] = 0;
                }
            }
            else if (((ObserverVoid)observer.ModNPC).internalAI[0] == 1)
            {
                Projectile.Kill();
            }
        }

        public override void OnKill(int timeLeft)
        {
            SoundEngine.PlaySound(SoundID.Item89);
            int p = Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center.X, Projectile.Center.Y, Projectile.velocity.X, Projectile.velocity.Y, ModContent.ProjectileType<Vortex>(), Projectile.damage, Projectile.knockBack, Projectile.owner, 0f, 0f);
            Main.projectile[p].Center = Projectile.Center;
        }

        public override bool PreDraw(ref Color lightColor)
        {
            SpriteBatch spriteBatch = Main.spriteBatch;
            Rectangle frame = BaseDrawing.GetFrame(Projectile.frame, TextureAssets.Projectile[Projectile.type].Value.Width, TextureAssets.Projectile[Projectile.type].Value.Height / 4, 0, 0);
            BaseDrawing.DrawTexture(spriteBatch, TextureAssets.Projectile[Projectile.type].Value, 0, Projectile.position, Projectile.width, Projectile.height, Projectile.scale, Projectile.rotation, Projectile.direction, 4, frame, Color.White, true);
            return false;
		}		
	}
}