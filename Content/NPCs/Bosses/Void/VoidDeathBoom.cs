using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;
using CSkies.Utilities.Base.BaseMod.Base;

namespace CSkies.Content.NPCs.Bosses.Void
{
    public class VoidDeath : ModNPC
    {
        public override string Texture => "CSkies/BlankTex";
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("VOID's Defeat");
            NPCID.Sets.ShouldBeCountedAsBoss[NPC.type] = true;
        }

        public override void SetDefaults()
        {
            NPC.width = 230;
            NPC.height = 142;
            NPC.friendly = false;
            NPC.lifeMax = 1;
            NPC.dontTakeDamage = true;
            NPC.noGravity = true;
            NPC.aiStyle = -1;
            NPC.alpha = 255;
            for (int k = 0; k < NPC.buffImmune.Length; k++)
            {
                NPC.buffImmune[k] = true;
            }
        }

        public override void AI()
        {
            if (NPC.ai[1] > 180)
            {
                int p = Projectile.NewProjectile(NPC.GetSource_FromThis(), NPC.Center.X, NPC.Center.Y, 0f, 0f, ModContent.ProjectileType<ShockwaveBoom>(), 0, 1, Main.myPlayer, 0, 12);
                Main.projectile[p].Center = NPC.Center;
                NPC.life = 0;
                NPC.netUpdate = true;
            }
            else
            {
                NPC.ai[1]++;
                NPC.ai[0]++;
                if (NPC.ai[0] > 4)
                {
                    NPC.ai[0] = 0;
                    SoundEngine.PlaySound(SoundID.Item14, NPC.position);
                    for (int i = 0; i < 2; i++)
                    {
                        Vector2 Pos = new Vector2(NPC.position.X + Main.rand.Next(0, 230), NPC.position.Y - Main.rand.Next(0, 142));
                        Projectile.NewProjectile(NPC.GetSource_FromThis(), Pos, Vector2.Zero, ModContent.ProjectileType<VoidDeathBoom>(), 0, 0, Main.myPlayer);
                    }
                }
            }
        }
    }

    public class VoidDeathBoom : ModProjectile
    {
        public override string Texture => "CSkies/Content/NPCs/Bosses/Void/VoidDeathBoom";
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Blast");     
            Main.projFrames[Projectile.type] = 7;
        }

        public override void SetDefaults()
        {
            Projectile.width = 98;
            Projectile.height = 98;
            Projectile.penetrate = -1;
            Projectile.hostile = true;
            Projectile.damage = 50;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
            Projectile.timeLeft = 600;
            Projectile.alpha = 80;
        }

        bool draw = true;
        public override void AI()
        {
            if (!draw)
            {
                draw = true;
            }
            else
            {
                draw = false;
            }
            
            if (++Projectile.frameCounter >= 3)
            {
                Projectile.frameCounter = 0;
                if (++Projectile.frame >= 7)
                {
                    Projectile.Kill();

                }
            }
            Projectile.velocity.X *= 0.00f;
            Projectile.velocity.Y *= 0.00f;

        }

        public override void OnKill(int timeLeft)
        {
            Projectile.timeLeft = 0;
        }

        public float auraPercent = 0f;
        public bool auraDirection = true;

        public override bool PreDraw(ref Color lightColor)
        {
            SpriteBatch sb = Main.spriteBatch;
            Rectangle frame = BaseDrawing.GetFrame(Projectile.frame, TextureAssets.Projectile[Projectile.type].Value.Width, TextureAssets.Projectile[Projectile.type].Value.Height / 7, 0, 2);

            if (!draw)
            {
                return false;
            }
            Texture2D Tex = TextureAssets.Projectile[Projectile.type].Value;
            BaseDrawing.DrawTexture(sb, Tex, 0, Projectile.position, Projectile.width, Projectile.height, Projectile.scale, Projectile.rotation, 0, 7, frame, Projectile.GetAlpha(Color.White), true);
            return false;
        }
    }
}
