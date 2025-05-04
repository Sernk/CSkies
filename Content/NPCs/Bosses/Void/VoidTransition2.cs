using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.GameContent;
using Terraria.ModLoader;
using Terraria.ID;
using CSkies.Utilities.Base.BaseMod.Base;

namespace CSkies.Content.NPCs.Bosses.Void
{
    public class VoidTransition2 : ModNPC
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("???");
            Main.npcFrameCount[NPC.type] = 19;
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
           // music = mod.GetSoundSlot(SoundType.Music, "Sounds/Music/Void");
        }

        bool title = false;

        public override void AI()
        {
            if (!title)
            {
                CSkies.ShowTitle(NPC, 4);
                title = true;
            }

            if (++NPC.ai[0] >= 5 && Main.netMode != NetmodeID.MultiplayerClient)
            {
                if (++NPC.ai[1] >= 19)
                {
                    int p = Projectile.NewProjectile(NPC.GetSource_FromThis(), NPC.Center.X, NPC.Center.Y, 0f, 0f, ModContent.ProjectileType<ShockwaveBoom>(), 0, 1, Main.myPlayer, 0, 12);
                    Main.projectile[p].Center = NPC.Center;
                    int n = NPC.NewNPC(NPC.GetSource_FromThis(), (int)NPC.Center.X, (int)NPC.Center.Y, ModContent.NPCType<Void>(), 0, 30);
                    Main.npc[n].Center = NPC.Center;
                    Main.npc[n].velocity = NPC.velocity;
                    NPC.active = false;
                }
                NPC.ai[0] = 0;
                NPC.netUpdate = true;
            }
            NPC.velocity *= .98f;
        }

        public override void FindFrame(int frameHeight)
        {
            NPC.frame.Y = frameHeight *= (int)NPC.ai[1];
        }

        public float auraPercent = 0f;
        public bool auraDirection = true;

        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            Texture2D tex = TextureAssets.Npc[NPC.type].Value;
            BaseDrawing.DrawAura(spriteBatch, tex, 0, NPC, auraPercent, 2f, 0f, 0f, NPC.GetAlpha(Color.White));
            return false;
        }
    }
}