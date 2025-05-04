using Microsoft.Xna.Framework;
using Terraria.ModLoader;

namespace CSkies.Content.Tiles.Abyss
{
    public class AbyssLeafWall : ModWall
	{
		public override void SetStaticDefaults()
		{
			//DustType = Mod.Find<ModDust>("RazeleafDust").Type;
			AddMapEntry(new Color(10, 5, 30));
            Terraria.ID.WallID.Sets.Conversion.Grass[Type] = true;
        }

		public override void NumDust(int i, int j, bool fail, ref int num)
		{
			num = fail ? 1 : 3;
		}
    }
}