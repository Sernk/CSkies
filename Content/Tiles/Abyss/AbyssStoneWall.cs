using Microsoft.Xna.Framework;
using Terraria.ID;
using Terraria.ModLoader;

namespace CSkies.Content.Tiles.Abyss
{
    public class AbyssStoneWall : ModWall
	{
		public override void SetStaticDefaults()
		{
			DustType = ModContent.DustType<Dusts.VoidDust>();
            AddMapEntry(new Color(10, 10, 20));
            HitSound = SoundID.Tink;
        }

		public override void NumDust(int i, int j, bool fail, ref int num)
		{
			num = fail ? 1 : 3;
		}
    }
}