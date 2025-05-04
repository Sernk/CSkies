using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace CSkies.Content.NPCs.Bosses.Heartcore
{
    public class HeartcoreDefeat : ModNPC
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Broken Heart");
            Main.npcFrameCount[NPC.type] = 23;
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
            //Music = Mod.GetSoundSlot(SoundType.Music, "Sounds/Music/silence");
        }

        public override void AI()
        {
            if (++NPC.ai[0] >= 12 && Main.netMode != NetmodeID.MultiplayerClient)
            {
                if (++NPC.ai[1] >= 23)
                {
                    NPC.active = false;
                }
                NPC.ai[0] = 0;
                NPC.netUpdate = true;
            }
            NPC.velocity *= 0;
        }

        public override void FindFrame(int frameHeight)
        {
            NPC.frame.Y = frameHeight *= (int)NPC.ai[1];
        }

        public override Color? GetAlpha(Color drawColor)
        {
            return Color.White;
        }
    }
}