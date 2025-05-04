using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.GameContent;
using Terraria.ModLoader;
using Terraria.ID;
using Terraria.Graphics.Effects;
using CSkies;

namespace CSkies.Content.NPCs.Bosses.Novacore
{
    public class NovacoreIntro : ModNPC
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("???");
            Main.npcFrameCount[NPC.type] = 8;
            NPCID.Sets.TrailCacheLength[NPC.type] = 20;
            NPCID.Sets.TrailingMode[NPC.type] = 0;
        }

        public override void SetDefaults()
        {
            NPC.width = 198;
            NPC.height = 198;
            NPC.friendly = false;
            NPC.lifeMax = 1;
            NPC.dontTakeDamage = true;
            NPC.noTileCollide = true;
            NPC.noGravity = true;
            NPC.aiStyle = -1;
            NPC.timeLeft = 10;
            NPC.alpha = 255;
            for (int k = 0; k < NPC.buffImmune.Length; k++)
            {
                NPC.buffImmune[k] = true;
            }
            //Music = Mod.GetSoundSlot(SoundType.Music, "Sounds/Music/Novacore1");
        }

        bool title = false;
        int ShineAlpha = 255;
        int Fadeout = 0;
        public override void AI()
        {
            NPC.rotation += .06f;
            NPC.velocity *= 0;
            NPC.ai[0]++;
            if (Main.netMode != NetmodeID.Server && !Filters.Scene["WhiteFlash"].IsActive())
            {
                Filters.Scene.Activate("WhiteFlash", NPC.Center).GetShader().UseOpacity(NPC.ai[0] * 5);
            }
            Filters.Scene["WhiteFlash"].GetShader().UseOpacity(NPC.ai[0] * 5);

            if (NPC.ai[0] == 120 && Main.netMode != NetmodeID.Server)
            {
                int p = Projectile.NewProjectile(NPC.GetSource_FromThis(), NPC.Center.X, NPC.Center.Y, 0f, 0f, ModContent.ProjectileType<ShockwaveBoom>(), 0, 1, Main.myPlayer, 0, 12);
                Main.projectile[p].Center = NPC.Center;
            }
            if (NPC.ai[0] > 60)
            {
                if (!title)
                {
                    CSkies.ShowTitle(NPC, 7);
                    title = true;
                }

                if (NPC.ai[0] > 120)
                {
                    if (NPC.alpha > 0)
                    {
                        NPC.alpha -= 10;
                    }
                    else
                    {
                        NPC.alpha = 0;
                    }
                }

                if (NPC.alpha > 0)
                {
                    Fadeout += 5;
                    ShineAlpha -= 5;
                    if (ShineAlpha <= 30)
                    {
                        ShineAlpha = 30;
                    }
                }
                else
                {
                    Fadeout -= 5;
                    ShineAlpha += 3;
                    if (ShineAlpha >= 255)
                    {
                        ShineAlpha = 255;
                    }
                }
            }

            if (Main.netMode != NetmodeID.MultiplayerClient)
            {
                if (NPC.ai[0] > 300 && Main.netMode != NetmodeID.Server && Filters.Scene["WhiteFlash"].IsActive())
                {
                    Filters.Scene["WhiteFlash"].Deactivate();
                }
                if (NPC.ai[0] > 360)
                {
                    int n = NPC.NewNPC(NPC.GetSource_FromThis(), (int)NPC.position.X, (int)NPC.position.Y, ModContent.NPCType<Novacore>(), 0, 10);
                    Main.npc[n].Center = NPC.Center;
                    Main.npc[n].velocity = NPC.velocity;
                    Main.npc[n].frame.Y = NPC.frame.Y;
                    NPC.active = false;
                    NPC.netUpdate = true;
                }
                
            }
        }

        int Frame = 0;
        public override void FindFrame(int frameHeight)
        {
            if (NPC.frameCounter++ > 5)
            {
                NPC.frameCounter = 0;
                Frame++;
                if (Frame > 7)
                {
                    Frame = 0;
                }
            }
            NPC.frame.Y = Frame * frameHeight;
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            Texture2D BladeTex = ModContent.Request<Texture2D>("CSkies/Content/NPCs/Bosses/Novacore/NovacoreBack").Value;

            Texture2D BladeGlowTex = ModContent.Request<Texture2D>("CSkies/Glowmasks/NovacoreBack_Glow").Value;

            Texture2D Base = TextureAssets.Npc[NPC.type].Value;

            Texture2D BaseGlow = ModContent.Request<Texture2D>("CSkies/Glowmasks/NovacoreIntro_Glow").Value;

            Vector2 drawOrigin = new Vector2(NPC.width * .5f, NPC.height * .5f);

            spriteBatch.End();
            spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.Additive);

            if (ShineAlpha < 255) //Simple bool
            {
                for (int k = 0; k < NPC.oldPos.Length; k++)
                {
                    Color Alpha = new Color(255 - Fadeout, 120 - Fadeout, 255 - Fadeout, ShineAlpha) * (NPC.oldPos.Length - k / NPC.oldPos.Length);

                    //spriteBatch.Draw(BladeGlowTex, npc.position - Main.screenPosition + drawOrigin, npc.frame, Alpha, npc.rotation, drawOrigin, 1f + k, SpriteEffects.None, 0f);

                    //spriteBatch.Draw(BaseGlow, npc.position - Main.screenPosition + drawOrigin, npc.frame, Alpha, 0, drawOrigin, 1f + k, SpriteEffects.None, 0f);
                }
            }

            spriteBatch.Draw(BladeTex, NPC.position - Main.screenPosition + drawOrigin, NPC.frame, NPC.GetAlpha(drawColor), NPC.rotation, drawOrigin, 1f, SpriteEffects.None, 0f);

            spriteBatch.Draw(BladeGlowTex, NPC.position - Main.screenPosition + drawOrigin, NPC.frame, NPC.GetAlpha(Color.White), NPC.rotation, drawOrigin, 1f, SpriteEffects.None, 0f);

            spriteBatch.Draw(Base, NPC.position - Main.screenPosition + drawOrigin, NPC.frame, NPC.GetAlpha(drawColor), 0, drawOrigin, 1f, SpriteEffects.None, 0f);

            spriteBatch.Draw(BaseGlow, NPC.position - Main.screenPosition + drawOrigin, NPC.frame, NPC.GetAlpha(Color.White), 0, drawOrigin, 1f, SpriteEffects.None, 0f);
            return false;
        }
    }
}
