using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;
using CSkies.Utilities.Base.BaseMod.Base;

namespace CSkies.Content.Projectiles.Void
{
    public class VoidMagic : ModProjectile
    {
        public override string Texture => "CSkies/Content/Projectiles/Void/Singularity";
        public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Vortex");
		}
        public override void SetDefaults()
        {
            Projectile.width = 32;
            Projectile.height = 32;
            Projectile.aiStyle = -1;
            Projectile.friendly = true;
            Projectile.alpha = 255;
            Projectile.ignoreWater = true;
            Projectile.DamageType = DamageClass.Magic;
            Projectile.penetrate = 3;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 8;
        }
		
		public override void AI()
        {
            Projectile.ai[0] += 1f;
            int num1002 = 0;
            if (Projectile.velocity.Length() <= 4f)
            {
                num1002 = 1;
            }
            Projectile.alpha -= 15;
            if (Projectile.alpha < 0)
            {
                Projectile.alpha = 0;
            }
            if (num1002 == 0)
            {
                Projectile.rotation -= 0.104719758f;
                if (Main.rand.Next(3) == 0)
                {
                    if (Main.rand.Next(2) == 0)
                    {
                        Vector2 vector124 = Vector2.UnitY.RotatedByRandom(6.2831854820251465);
                        Dust dust27 = Main.dust[Dust.NewDust(Projectile.Center - vector124 * 30f, 0, 0, ModContent.DustType<Dusts.VoidDust>(), 0f, 0f, 0, default, 1f)];
                        dust27.noGravity = true;
                        dust27.position = Projectile.Center - vector124 * Main.rand.Next(10, 21);
                        dust27.velocity = vector124.RotatedBy(1.5707963705062866, default) * 6f;
                        dust27.scale = 0.5f + Main.rand.NextFloat();
                        dust27.fadeIn = 0.5f;
                        dust27.customData = this;
                    }
                    else
                    {
                        Vector2 vector125 = Vector2.UnitY.RotatedByRandom(6.2831854820251465);
                        Dust dust28 = Main.dust[Dust.NewDust(Projectile.Center - vector125 * 30f, 0, 0, ModContent.DustType<Dusts.VoidDust>(), 0f, 0f, 0, default, 1f)];
                        dust28.noGravity = true;
                        dust28.position = Projectile.Center - vector125 * 30f;
                        dust28.velocity = vector125.RotatedBy(-1.5707963705062866, default) * 3f;
                        dust28.scale = 0.5f + Main.rand.NextFloat();
                        dust28.fadeIn = 0.5f;
                        dust28.customData = this;
                    }
                }
                if (Projectile.ai[0] >= 30f)
                {
                    Projectile.velocity *= 0.98f;
                    Projectile.scale += 0.00744680827f;
                    if (Projectile.scale > 1.3f)
                    {
                        Projectile.scale = 1.3f;
                    }
                    Projectile.rotation -= 0.0174532924f;
                }
                if (Projectile.velocity.Length() < 4.1f)
                {
                    Projectile.velocity.Normalize();
                    Projectile.velocity *= 4f;
                    Projectile.ai[0] = 0f;
                }
            }
            else if (num1002 == 1)
            {
                Projectile.rotation -= 0.104719758f;
                for (int num1003 = 0; num1003 < 1; num1003++)
                {
                    if (Main.rand.Next(2) == 0)
                    {
                        Vector2 vector126 = Vector2.UnitY.RotatedByRandom(6.2831854820251465);
                        Dust dust29 = Main.dust[Dust.NewDust(Projectile.Center - vector126 * 30f, 0, 0, ModContent.DustType<Dusts.VoidDust>(), 0f, 0f, 0, default, 1f)];
                        dust29.noGravity = true;
                        dust29.position = Projectile.Center - vector126 * Main.rand.Next(10, 21);
                        dust29.velocity = vector126.RotatedBy(1.5707963705062866, default) * 6f;
                        dust29.scale = 0.9f + Main.rand.NextFloat();
                        dust29.fadeIn = 0.5f;
                        dust29.customData = this;
                        vector126 = Vector2.UnitY.RotatedByRandom(6.2831854820251465);
                        dust29 = Main.dust[Dust.NewDust(Projectile.Center - vector126 * 30f, 0, 0, ModContent.DustType<Dusts.VoidDust>(), 0f, 0f, 0, default, 1f)];
                        dust29.noGravity = true;
                        dust29.position = Projectile.Center - vector126 * Main.rand.Next(10, 21);
                        dust29.velocity = vector126.RotatedBy(1.5707963705062866, default) * 6f;
                        dust29.scale = 0.9f + Main.rand.NextFloat();
                        dust29.fadeIn = 0.5f;
                        dust29.customData = this;
                    }
                    else
                    {
                        Vector2 vector127 = Vector2.UnitY.RotatedByRandom(6.2831854820251465);
                        Dust dust30 = Main.dust[Dust.NewDust(Projectile.Center - vector127 * 30f, 0, 0, ModContent.DustType<Dusts.VoidDust>(), 0f, 0f, 0, default, 1f)];
                        dust30.noGravity = true;
                        dust30.position = Projectile.Center - vector127 * Main.rand.Next(20, 31);
                        dust30.velocity = vector127.RotatedBy(-1.5707963705062866, default) * 5f;
                        dust30.scale = 0.9f + Main.rand.NextFloat();
                        dust30.fadeIn = 0.5f;
                        dust30.customData = this;
                    }
                }
                if (Projectile.ai[0] % 30f == 0f && Projectile.ai[0] < 241f && Main.myPlayer == Projectile.owner)
                {
                    Vector2 vector128 = Vector2.UnitY.RotatedByRandom(6.2831854820251465) * 12f;
                    Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center.X, Projectile.Center.Y, vector128.X, vector128.Y, ProjectileID.NebulaArcanumSubshot, Projectile.damage / 2, 0f, Projectile.owner, 0f, Projectile.whoAmI);
                }
                Vector2 vector129 = Projectile.Center;
                float num1004 = 800f;
                bool flag58 = false;
                int num1005 = 0;
                if (Projectile.ai[1] == 0f)
                {
                    for (int num1006 = 0; num1006 < 200; num1006++)
                    {
                        if (Main.npc[num1006].CanBeChasedBy(this, false))
                        {
                            Vector2 center13 = Main.npc[num1006].Center;
                            if (Projectile.Distance(center13) < num1004 && Collision.CanHit(new Vector2(Projectile.position.X + Projectile.width / 2, Projectile.position.Y + Projectile.height / 2), 1, 1, Main.npc[num1006].position, Main.npc[num1006].width, Main.npc[num1006].height))
                            {
                                num1004 = Projectile.Distance(center13);
                                vector129 = center13;
                                flag58 = true;
                                num1005 = num1006;
                            }
                        }
                    }
                    if (flag58)
                    {
                        if (Projectile.ai[1] != num1005 + 1)
                        {
                            Projectile.netUpdate = true;
                        }
                        Projectile.ai[1] = num1005 + 1;
                    }
                    flag58 = false;
                }
                if (Projectile.ai[1] != 0f)
                {
                    int num1007 = (int)(Projectile.ai[1] - 1f);
                    if (Main.npc[num1007].active && Main.npc[num1007].CanBeChasedBy(this, true) && Projectile.Distance(Main.npc[num1007].Center) < 1000f)
                    {
                        flag58 = true;
                        vector129 = Main.npc[num1007].Center;
                    }
                }
                if (!Projectile.friendly)
                {
                    flag58 = false;
                }
                if (flag58)
                {
                    float num1008 = 4f;
                    int num1009 = 8;
                    Vector2 vector130 = new Vector2(Projectile.position.X + Projectile.width * 0.5f, Projectile.position.Y + Projectile.height * 0.5f);
                    float num1010 = vector129.X - vector130.X;
                    float num1011 = vector129.Y - vector130.Y;
                    float num1012 = (float)Math.Sqrt(num1010 * num1010 + num1011 * num1011);
                    num1012 = num1008 / num1012;
                    num1010 *= num1012;
                    num1011 *= num1012;
                    Projectile.velocity.X = (Projectile.velocity.X * (num1009 - 1) + num1010) / num1009;
                    Projectile.velocity.Y = (Projectile.velocity.Y * (num1009 - 1) + num1011) / num1009;
                }
            }
            if (Projectile.alpha < 150)
            {
                Lighting.AddLight(Projectile.Center, 0.1f, 0.1f, 0.2f);
            }
            if (Projectile.ai[0] >= 600f)
            {
                Projectile.Kill();
                return;
            }

            for (int u = 0; u < Main.maxNPCs; u++)
            {
                NPC target = Main.npc[u];

                if (target.type != NPCID.TargetDummy && target.active && !target.boss && target.chaseable && Vector2.Distance(Projectile.Center, target.Center) < 160)
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

        public override void OnHitNPC (NPC target, NPC.HitInfo hit, int damageDone)
		{
            Projectile.ai[0] = 1f;
        }
		
		public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough, ref Vector2 hitboxCenterFrac)
        {
            width = 16;
            height = 16;
            return true;
        }

        public override void OnKill(int timeLeft)
        {
            Projectile.position = Projectile.Center;
            Projectile.width = (Projectile.height = 176);
            Projectile.Center = Projectile.position;
            Projectile.maxPenetrate = -1;
            Projectile.penetrate = -1;
            Projectile.Damage();
            SoundEngine.PlaySound(SoundID.Item14, Projectile.position);
            for (int num93 = 0; num93 < 4; num93++)
            {
                int num94 = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, ModContent.DustType<Dusts.VoidDust>(), 0f, 0f, 100, default, 1.5f);
                Main.dust[num94].position = Projectile.Center + Vector2.UnitY.RotatedByRandom(3.1415927410125732) * (float)Main.rand.NextDouble() * Projectile.width / 2f;
            }
            for (int num95 = 0; num95 < 30; num95++)
            {
                int num96 = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, ModContent.DustType<Dusts.VoidDust>(), 0f, 0f, 200, default, 3.7f);
                Main.dust[num96].position = Projectile.Center + Vector2.UnitY.RotatedByRandom(3.1415927410125732) * (float)Main.rand.NextDouble() * Projectile.width / 2f;
                Main.dust[num96].noGravity = true;
                Main.dust[num96].velocity *= 3f;
                num96 = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, ModContent.DustType<Dusts.VoidDust>(), 0f, 0f, 100, default, 1.5f);
                Main.dust[num96].position = Projectile.Center + Vector2.UnitY.RotatedByRandom(3.1415927410125732) * (float)Main.rand.NextDouble() * Projectile.width / 2f;
                Main.dust[num96].velocity *= 2f;
                Main.dust[num96].noGravity = true;
                Main.dust[num96].fadeIn = 1f;
            }
            for (int num97 = 0; num97 < 10; num97++)
            {
                int num98 = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, ModContent.DustType<Dusts.VoidDust>(), 0f, 0f, 0, default, 2.7f);
                Main.dust[num98].position = Projectile.Center + Vector2.UnitX.RotatedByRandom(3.1415927410125732).RotatedBy(Projectile.velocity.ToRotation(), default) * Projectile.width / 2f;
                Main.dust[num98].noGravity = true;
                Main.dust[num98].velocity *= 3f;
            }
            for (int num99 = 0; num99 < 10; num99++)
            {
                int num100 = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, ModContent.DustType<Dusts.VoidDust>(), 0f, 0f, 0, default, 1.5f);
                Main.dust[num100].position = Projectile.Center + Vector2.UnitX.RotatedByRandom(3.1415927410125732).RotatedBy(Projectile.velocity.ToRotation(), default) * Projectile.width / 2f;
                Main.dust[num100].noGravity = true;
                Main.dust[num100].velocity *= 3f;
            }
        }

        // chain voodoo
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