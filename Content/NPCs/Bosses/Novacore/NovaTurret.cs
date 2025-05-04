using Microsoft.Xna.Framework;
using Terraria;
using Terraria.GameContent;
using Terraria.ModLoader;
using System;
using Microsoft.Xna.Framework.Graphics;
using CSkies.Utilities.Base.BaseMod.Base;
using CSkies.Utilities;
using CSkies.Content.NPCs.Bosses.Heartcore;

namespace CSkies.Content.NPCs.Bosses.Novacore
{
    public class NovaTurret : ModProjectile
	{
		public override void SetDefaults()
        {
            Projectile.width = 80;
            Projectile.height = 80;
            Projectile.aiStyle = -1;
            Projectile.hostile = true;
            Projectile.alpha = 255;
            Projectile.scale = 1f;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = false;
            Projectile.penetrate = -1;
            Main.projFrames[Projectile.type] = 4;
            Projectile.timeLeft = 400;
        }
		
		public override Color? GetAlpha(Color lightColor)
		{
			return Color.White;
        }

        public int body = -1;
        public float rotValue = -1f;
        public Vector2 pos;
        int starNumber;

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

            Player player = Main.player[novacore.target];

            pos = novacore.Center;
            
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

            if (Projectile.ai[1]++ > 180)
            {
                if (Collision.CanHit(Projectile.position, Projectile.width, Projectile.height, player.Center, player.width, player.height))
                {
                    Vector2 fireTarget = Projectile.Center;
                    float rot = BaseUtility.RotationTo(Projectile.Center, player.Center);
                    fireTarget = BaseUtility.RotateVector(Projectile.Center, fireTarget, rot);
                    Vector2 direction = player.Center - Projectile.Center;
                    direction.Normalize();
                    direction *= 10f;
                    Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, direction, ModContent.ProjectileType<NovaBlast>(), 90, 1f, Main.myPlayer);
                }

                Projectile.ai[1] = 0;
            }
        }

        public override bool PreDraw(ref Color lightColor)
        {
            SpriteBatch sb = Main.spriteBatch;
            Vector2 vector43 = Projectile.position + new Vector2(Projectile.width, Projectile.height) / 2f + Vector2.UnitY * Projectile.gfxOffY - Main.screenPosition;
			Texture2D value107 = TextureAssets.Projectile[617].Value;
			Color color68 = Projectile.GetAlpha(lightColor);
			Vector2 origin17 = new Vector2(value107.Width, value107.Height) / 2f;
            Color color73 = color68 * 0.8f;
            color73.A /= 2;
            Color color74 = Color.Lerp(color68, Color.Black, 0.5f);
            color74.A = color68.A;
            float num299 = 0.95f + (Projectile.rotation * 0.75f).ToRotationVector2().Y * 0.1f;
            color74 *= num299;
            float scale15 = 0.6f + Projectile.scale * 0.6f * num299;
            sb.Draw(TextureAssets.Extra[50].Value, vector43, null, color74, 0f - Projectile.rotation + 0.35f, origin17, scale15, SpriteEffects.None, 0);
            sb.Draw(TextureAssets.Extra[50].Value, vector43, null, color68, 0f - Projectile.rotation, origin17, Projectile.scale, SpriteEffects.None, 0);
            sb.Draw(value107, vector43, null, color73, (0f - Projectile.rotation) * 0.7f, origin17, Projectile.scale, SpriteEffects.None, 0);
            sb.Draw(TextureAssets.Extra[50].Value, vector43, null, color68 * 0.8f, Projectile.rotation * 0.5f, origin17, Projectile.scale * 0.9f, SpriteEffects.None, 0);
            color68.A = 0;
			return false;
        }
    }
}