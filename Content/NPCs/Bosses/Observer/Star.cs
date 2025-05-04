using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;
using System.IO;
using Terraria.Audio;
using CSkies.Utilities;
using CSkies.Utilities.Base.BaseMod.Base;

namespace CSkies.Content.NPCs.Bosses.Observer
{
    public class Star : ModProjectile
	{
        public int damage = 0;

		public override void SetStaticDefaults()
		{
            // DisplayName.SetDefault("Star");
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
            if (Main.expertMode)
            {
                damage = Projectile.damage / 4;
            }
            else
            {
                damage = Projectile.damage / 2;
            }

            if (!NPC.AnyNPCs(ModContent.NPCType<Observer>()))
            {
                Projectile.active = false;
            }

            if (body == -1)
            {
                int npcID = BaseAI.GetNPC(Projectile.Center, ModContent.NPCType<Observer>(), 400f, null);
                if (npcID >= 0) body = npcID;
            }
            if (body == -1) return;
            NPC observer = Main.npc[body];
            if (observer == null || observer.life <= 0 || !observer.active || observer.type != ModContent.NPCType<Observer>()) { Projectile.active = false; return; }

            Player player = Main.player[observer.target];

            pos = observer.Center;

            int starNumber = ((Observer)observer.ModNPC).StarCount;

            Projectile.rotation += .1f;

            if (((Observer)observer.ModNPC).internalAI[0] == 0)
            {
                Projectile.timeLeft = 120;
                float dist = ((Observer)observer.ModNPC).internalAI[1];

                if (rotValue == -1f) rotValue = Projectile.ai[0] % starNumber * ((float)Math.PI * 2f / starNumber);
                rotValue += 0.04f;
                while (rotValue > (float)Math.PI * 2f) rotValue -= (float)Math.PI * 2f;

                Projectile.Center = BaseUtility.RotateVector(observer.Center, observer.Center + new Vector2(dist, 0f), rotValue);

                if (Projectile.ai[1]++ > 180)
                {
                    if (Collision.CanHit(Projectile.position, Projectile.width, Projectile.height, player.Center, player.width, player.height))
                    {
                        Vector2 fireTarget = Projectile.Center;
                        float rot = BaseUtility.RotationTo(Projectile.Center, player.Center);
                        fireTarget = BaseUtility.RotateVector(Projectile.Center, fireTarget, rot);
                        //BaseAI.FireProjectile(player.Center, Projectile, ModContent.ProjectileType<StarProj>(), damage, 0f, 4f);
                    }
                    Projectile.ai[1] = 0;
                }
            }
            else if (((Observer)observer.ModNPC).internalAI[0] == 1)
            {
                Projectile.ai[1] = 0;
                Projectile.tileCollide = true;
                const int homingDelay = 0;
                const float desiredFlySpeedInPixelsPerFrame = 10;
                const float amountOfFramesToLerpBy = 20;

                internalAI[0]++;
                if (internalAI[0] > homingDelay)
                {
                    internalAI[0] = homingDelay;

                    int foundTarget = HomeOnTarget();
                    if (foundTarget != -1)
                    {
                        Player target = Main.player[foundTarget];
                        Vector2 desiredVelocity = Projectile.DirectionTo(target.Center) * desiredFlySpeedInPixelsPerFrame;
                        Projectile.velocity = Vector2.Lerp(Projectile.velocity, desiredVelocity, 1f / amountOfFramesToLerpBy);
                    }
                }
            }
        }

        private int HomeOnTarget()
        {
            const float homingMaximumRangeInPixels = 10000000;

            int selectedTarget = -1;
            for (int i = 0; i < Main.maxPlayers; i++)
            {
                Player target = Main.player[i];
                if (target.active)
                {
                    float distance = Projectile.Distance(target.Center);
                    if (distance <= homingMaximumRangeInPixels && (selectedTarget == -1 || Projectile.Distance(Main.player[selectedTarget].Center) > distance))
                    {
                        selectedTarget = i;
                    }
                }
            }

            return selectedTarget;
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            SoundEngine.PlaySound(SoundID.Item89);
            Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center.X, Projectile.Center.Y - 20, Projectile.velocity.X, Projectile.velocity.Y, ModContent.ProjectileType<Starshock>(), Projectile.damage, Projectile.knockBack, Projectile.owner, 0f, 0f);
            return true;
        }

        public override void OnKill(int timeLeft)
        {
            int dustID = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, 17, Projectile.velocity.X * 0.2f, Projectile.velocity.Y * 0.2f, 10, Color.White, 1f);
            Main.dust[dustID].velocity = new Vector2(MathHelper.Lerp(-1f, 1f, (float)Main.rand.NextDouble()), MathHelper.Lerp(-1f, 1f, (float)Main.rand.NextDouble()));
        }

        public override bool PreDraw(ref Color lightColor)
        {
            SpriteBatch sb = Main.spriteBatch;
            BaseDrawing.DrawAfterimage(sb, TextureAssets.Projectile[Projectile.type].Value, 0, Projectile, 2.5f, 1, 3, true, 0f, 0f, Projectile.GetAlpha(Color.White * 0.8f));
            BaseDrawing.DrawTexture(sb, TextureAssets.Projectile[Projectile.type].Value, 0, Projectile, Projectile.GetAlpha(Color.White * 0.8f), true);
            return false;
		}		
	}
}