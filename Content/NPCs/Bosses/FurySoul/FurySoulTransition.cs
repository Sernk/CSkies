using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.GameContent;
using Terraria.ModLoader;
using Terraria.ID;
using CSkies.Utilities.Base.BaseMod.Base;

namespace CSkies.Content.NPCs.Bosses.FurySoul
{
    public class FurySoulTransition : ModNPC
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("???");
            Main.npcFrameCount[NPC.type] = 6;
        }

        public override void SetDefaults()
        {
            NPC.width = 230;
            NPC.height = 142;
            NPC.friendly = false;
            NPC.lifeMax = 1;
            NPC.dontTakeDamage = true;
            NPC.noTileCollide = true;
            NPC.noGravity = true;
            NPC.aiStyle = -1;
            NPC.timeLeft = 10;
            for (int k = 0; k < NPC.buffImmune.Length; k++)
            {
                NPC.buffImmune[k] = true;
            }
            //music = mod.GetSoundSlot(SoundType.Music, "Sounds/Music/silence");
        }

        bool title = false;

        public override void AI()
        {
            NPC.velocity *= 0;

            if (NPC.ai[3]++ > 120)
            {
                if (!title)
                {
                    CSkies.ShowTitle(NPC, 6);
                    title = true;
                }
                //music = mod.GetSoundSlot(SoundType.Music, "Sounds/Music/FurySoul");
                if (++NPC.ai[0] >= 12 && Main.netMode != NetmodeID.MultiplayerClient)
                {
                    if (++NPC.ai[1] >= 15)
                    {
                        int n = NPC.NewNPC(NPC.GetSource_FromThis(), (int)NPC.Center.X, (int)NPC.Center.Y, ModContent.NPCType<FurySoul>(), 0, 10);
                        Main.npc[n].Center = NPC.Center;
                        Main.npc[n].velocity = NPC.velocity;
                        int p = Projectile.NewProjectile(NPC.GetSource_FromThis(), NPC.Center.X, NPC.Center.Y, 0f, 0f, ModContent.ProjectileType<ShockwaveBoom>(), 0, 1, Main.myPlayer, 0, 12);
                        Main.projectile[p].Center = NPC.Center;
                        NPC.active = false;
                    }
                    NPC.ai[0] = 0;
                    NPC.netUpdate = true;
                }
            }
        }

        public override void FindFrame(int frameHeight)
        {
            if (NPC.frameCounter++ > 6)
            {
                NPC.frameCounter = 0;
                if (NPC.ai[3] > 120)
                {
                    if (NPC.frame.Y != frameHeight * 3)
                    {
                        NPC.frame.Y += frameHeight;
                    }
                }
                else
                {
                    NPC.frame.Y += frameHeight;
                }
                if (NPC.frame.Y > frameHeight * 5)
                {
                    NPC.frame.Y = 0;
                }
            }
        }

        public float auraPercent = 0f;
        public bool auraDirection = true;

        public override bool PreDraw(SpriteBatch sb, Vector2 screenPos, Color drawColor)
        {
            Texture2D tex = TextureAssets.Npc[NPC.type].Value;

            if (auraDirection) { auraPercent += 0.1f; auraDirection = auraPercent < 1f; }
            else { auraPercent -= 0.1f; auraDirection = auraPercent <= 0f; }

            BaseDrawing.DrawAura(sb, tex, 0, NPC, auraPercent, 2f, 0f, 0f, NPC.GetAlpha(Color.White));
            return false;
        }
    }
}