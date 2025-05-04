using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using CSkies.Utilities.Base.BaseMod.Base;

namespace CSkies.Content.Tiles.Abyss
{
    public class AbyssWall : ModWall
	{
		public override void SetStaticDefaults()
		{
			DustType = ModContent.DustType<Dusts.VoidDust>();
            AddMapEntry(new Color(10, 10, 30));
            HitSound = SoundID.Tink;
            Main.wallLargeFrames[Type] = 2;
        }

		public override void NumDust(int i, int j, bool fail, ref int num)
		{
			num = fail ? 1 : 3;
		}

        public static Color Glow(Color c) => new Color(c.R / 3, c.G / 3, c.B / 3);

        public override void PostDraw(int i, int j, SpriteBatch spriteBatch)
        {
            Tile tile = Main.tile[i, j];
            Vector2 zero = new Vector2(Main.offScreenRange, Main.offScreenRange);
            if (Main.drawToScreen)
            {
                zero = Vector2.Zero;
            }
            int height = tile.TileFrameY == 36 ? 18 : 16;
            BaseDrawing.DrawWallTexture(spriteBatch, ModContent.Request<Texture2D>("CSkies/Glowmasks/AbyssWall_Glow").Value, i, j, false, Glow);
        }
    }
}