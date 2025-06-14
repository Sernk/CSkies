using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace CSkies.Content.Dusts
{
    public class HeartDust : ModDust
	{
		public override void OnSpawn(Dust dust)
		{
			dust.velocity.Y *= 3f;
			dust.velocity.X *= 3f;
			dust.scale *= 2f;
			dust.noGravity = true;
			dust.noLight = false;
		}
		
		public override Color? GetAlpha(Dust dust, Color lightColor)
		{
			return Color.White;
		}
	}
}