using Microsoft.Xna.Framework;
using Terraria.ModLoader;

namespace CSkies.Utilities.Base.NPCs
{
    public abstract class ParentNPC : ModNPC
	{	
		public virtual void SetAI(float[] ai, int aiType)
		{ 
		}

		public virtual Vector4 GetFrameV4()
		{ 
			return new Vector4(0, 0, 1, 1); 
		}
	}
}
