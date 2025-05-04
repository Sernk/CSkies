using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;

namespace CSkies.Utilities.Base.BaseMod
{
    public class MNPC : GlobalNPC
    {
        public override bool PreDraw(NPC npc, SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            LastDrawnNPC = npc;

            return base.PreDraw(npc, spriteBatch, screenPos, drawColor);
        }

        public static NPC LastDrawnNPC { get; private set; }
    }
}