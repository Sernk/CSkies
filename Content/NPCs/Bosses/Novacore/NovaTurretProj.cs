using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.ModLoader;
using Terraria.ID;
using Microsoft.Xna.Framework.Graphics;
using System;
using CSkies.Utilities.Base.BaseMod.Base;

namespace CSkies.Content.NPCs.Bosses.Novacore
{
    public class NovaTurretProj : ModProjectile
	{
		public override void SetDefaults()
		{
			Projectile.width = 40;
			Projectile.height = 40;
			Projectile.aiStyle = -1;
			Projectile.hostile = true;
			Projectile.penetrate = 1;
			Projectile.aiStyle = -1;
			Projectile.timeLeft = 1200;
            Main.projFrames[Projectile.type] = 4;
        }
		
		public override Color? GetAlpha(Color lightColor)
		{
			return Color.White;
        }

        public int body = -1;
        public float rotValue = -1f;
        public Vector2 pos;

        public override void AI()
        {
            Lighting.AddLight(Projectile.Center, 0.5f, 0f, .5f);

            if (!NPC.AnyNPCs(ModContent.NPCType<Novacore>()))
            {
                Projectile.active = false;
            }

            if (body == -1)
            {
                int npcID = BaseAI.GetNPC(Projectile.Center, ModContent.NPCType<Novacore>(), 400f, null);
                if (npcID >= 0) body = npcID;
            }
            if (body == -1) return;
            NPC novacore = Main.npc[body];
            if (novacore == null || novacore.life <= 0 || !novacore.active || novacore.type != ModContent.NPCType<Novacore>()) { Projectile.active = false; return; }

            pos = novacore.Center;

            int starNumber = ((Novacore)novacore.ModNPC).TurretCount();

            if (Projectile.localAI[0] == 0)
            {
                starNumber = ((Novacore)novacore.ModNPC).TurretCount();
                Projectile.localAI[0]++;
                Projectile.netUpdate = true;
            }

            Projectile.rotation += .06f;
            float dist = ((Novacore)novacore.ModNPC).OrbitterDist;

            if (rotValue == -1f) rotValue = Projectile.ai[0] % starNumber * ((float)Math.PI * 2f / starNumber);
            rotValue += 0.04f;
            while (rotValue > (float)Math.PI * 2f) rotValue -= (float)Math.PI * 2f;

            Projectile.Center = BaseUtility.RotateVector(novacore.Center, novacore.Center + new Vector2(dist, 0f), rotValue);

            if (((Novacore)novacore.ModNPC).OrbitterDist == 300)
            {
                Projectile.Kill();
            }
        }

        public override void OnKill(int timeleft)
        {
            SoundEngine.PlaySound(SoundID.Item94, Projectile.position);
            int num290 = Main.rand.Next(3, 7);
            for (int num291 = 0; num291 < num290; num291++)
            {
                int num292 = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, 135, 0f, 0f, 100, default, 2.1f);
                Main.dust[num292].velocity *= 2f;
                Main.dust[num292].noGravity = true;
            }
            Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, Vector2.Zero, ModContent.ProjectileType<NovaTurret>(), Projectile.damage / 2, 0, Main.myPlayer, Projectile.ai[0]);
        }

        public float auraPercent = 0f;
        public bool auraDirection = true;

        public override bool PreDraw(ref Color lightColor)
        {
            SpriteBatch sb = Main.spriteBatch;
            if (auraDirection) { auraPercent += 0.1f; auraDirection = auraPercent < 1f; }
            else { auraPercent -= 0.1f; auraDirection = auraPercent <= 0f; }

            Rectangle frame = BaseDrawing.GetFrame(Projectile.frame, TextureAssets.Projectile[Projectile.type].Value.Width, TextureAssets.Projectile[Projectile.type].Value.Height / Main.projFrames[Projectile.type], 0, 0);

            BaseDrawing.DrawAura(sb, TextureAssets.Projectile[Projectile.type].Value, 0, Projectile.position, Projectile.width, Projectile.height, auraPercent, 1.5f, 1f, Projectile.rotation, Projectile.direction, 4, frame, 0f, 0f, null);
            BaseDrawing.DrawTexture(sb, TextureAssets.Projectile[Projectile.type].Value, 0, Projectile.position, Projectile.width, Projectile.height, Projectile.scale, Projectile.rotation, 0, 4, frame, Projectile.GetAlpha(Color.White * 0.8f), true);
            return false;
        }
    }
}