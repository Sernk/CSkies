using Microsoft.Xna.Framework;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using CSkies.Utilities.Base.BaseMod.Base;

namespace CSkies.Content.NPCs.Bosses.FurySoul
{
    public class Furyrang : ModProjectile
	{
        public override void SetDefaults()
        {
            Projectile.width = 66;
            Projectile.height = 66;
            Projectile.aiStyle = -1;
            Projectile.timeLeft = 3600;
            Projectile.hostile = true;
            Projectile.tileCollide = false;
            Projectile.damage = 200;
            Projectile.penetrate = -1;
            Projectile.DamageType = DamageClass.Melee;
        }

        public override Color? GetAlpha(Color lightColor)
        {
            return Color.White;
        }

        public int master = -1;
		
		public int dustDelay = 0;

		public override void AI()
		{
            if (master >= 0 && (Main.npc[master] == null || !Main.npc[master].active || Main.npc[master].type != ModContent.NPCType<FurySoul>())) master = -1;
            if (master == -1)
            {
                master = BaseAI.GetNPC(Projectile.Center, ModContent.NPCType<FurySoul>(), -1, null);
                if (master == -1) master = -2;
            }
            if (master == -1) { return; }
			if (master < 0 || !Main.npc[master].active || Main.npc[master].life <= 0) { Projectile.Kill(); return; }

            if (Main.rand.Next(2) == 0)
            {
                int dustnumber = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.Torch, 0f, 0f, 200, default, 0.5f);
                Main.dust[dustnumber].velocity *= 0.3f;
            }

            BaseAI.AIBoomerang(Projectile, ref Projectile.ai, Main.npc[master].position, Main.npc[master].width, Main.npc[master].height, true, 40, 35, 15f, .4f, true);
		}
	}
}