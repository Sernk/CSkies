using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;

namespace CSkies.Utilities.Base.BaseMod
{
    public class MProjectile : GlobalProjectile
    {
        public static Projectile LastShaderDrawObject { get; private set; }

        public override void PostDraw(Projectile projectile, Color lightColor)
        {
            LastShaderDrawObject = projectile;

            base.PostDraw(projectile, lightColor);
        }
    }
}