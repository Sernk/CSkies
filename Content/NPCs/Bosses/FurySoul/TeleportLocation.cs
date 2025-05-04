using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;
using CSkies.Content.NPCs.Bosses.Heartcore;
using Terraria.Graphics.Shaders;
using CSkies.Utilities.Base.BaseMod.Base;

namespace CSkies.Content.NPCs.Bosses.FurySoul
{
	public class TeleportLocation : ModNPC
	{
		public override void SetStaticDefaults()
		{
            // DisplayName.SetDefault("Ring of Fire");
		}		

        public override void SetDefaults()
        {
            NPC.width = 90;
            NPC.height = 90;
            NPC.npcSlots = 0;
            NPC.aiStyle = -1;
            NPC.lifeMax = 100;
            NPC.noTileCollide = true;
            NPC.dontTakeDamage = true;
            NPC.scale = 0;
            NPC.noGravity = true;
        }

        public override void AI()
		{
            NPC.rotation = Main.npc[(int)NPC.ai[0]].rotation;

            if (NPC.scale < 1)
            {
                if (Main.npc[(int)NPC.ai[0]].ai[0] == 3)
                {
                    NPC.scale += .1f;
                }
                else
                {
                    NPC.scale += .05f;
                }
            }
            else
            {
                NPC.scale = 1;
            }
        }

        public override bool PreDraw(SpriteBatch sb, Vector2 screenPos, Color drawColor)
		{
            Texture2D Tex = TextureAssets.Npc[NPC.type].Value;
            int r = GameShaders.Armor.GetShaderIdFromItemId(ItemID.LivingFlameDye);

            Rectangle f = BaseDrawing.GetFrame(0, TextureAssets.Npc[NPC.type].Value.Width, TextureAssets.Npc[NPC.type].Value.Height / Main.npcFrameCount[NPC.type], 0, 0);

            BaseDrawing.DrawTexture(sb, Tex, r, NPC.position, NPC.width, NPC.height, NPC.scale, NPC.rotation, NPC.direction, 1, f, NPC.GetAlpha(Color.White * 0.8f), true);
            return false;
		}
	}
}