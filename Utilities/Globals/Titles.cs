using Terraria;
using Terraria.ModLoader;

namespace CSkies.Utilities.Globals
{
	public class Titles : ModPlayer
    {
        public bool text = false;
        public float alphaText = 255f;
        public float alphaText2 = 255f;
        public float alphaText3 = 255f;
        public float alphaText4 = 255f;
        public int BossID = 0;

        public override void ResetEffects()
        {
            text = false;
        }

        public override void PreUpdate()
        {
            if (!CUtils.AnyProjectiles(ModContent.ProjectileType<Title>()))
            {
                alphaText = 255f;
                alphaText2 = 255f;
            }
        }
    }

    public class Title : ModProjectile
    {
        public override string Texture => "CSkies/BlankTex";
        public override void SetDefaults()
        {
            Projectile.width = 1;
            Projectile.height = 1;
            Projectile.penetrate = -1;
            Projectile.hostile = false;
            Projectile.friendly = false;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
            Projectile.timeLeft = 240;
        }

        public override void AI()
        {
            Player player = Main.player[Projectile.owner];
            Titles modPlayer = player.GetModPlayer<Titles>();

            modPlayer.text = true;

            modPlayer.BossID = (int)Projectile.ai[0];

            Projectile.velocity.X = 0;
            Projectile.velocity.Y = 0;

            if (Projectile.timeLeft <= 45)
            {
                if (modPlayer.alphaText < 255f)
                {
                    modPlayer.alphaText += 10f;
                    modPlayer.alphaText2 += 10f;
                }
            }
            else
            {
                if (Projectile.timeLeft <= 180)
                {
                    modPlayer.alphaText -= 5f;
                }
                if (modPlayer.alphaText > 0f)
                {
                    modPlayer.alphaText2 -= 5f;
                }
            }
        }
    }
}