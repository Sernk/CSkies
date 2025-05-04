using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CSkies.Content.Tiles.Abyss
{
    public class AbyssMoss : ModTile
    {
        public override void SetStaticDefaults()
        {
            Main.tileSolid[Type] = true;
            //SetModTree(new AbyssTree())/* tModPorter Note: Removed. Assign GrowsOnTileId to this tile type in ModTree.SetStaticDefaults instead */;
            TileID.Sets.Conversion.Grass[Type] = true;
            Main.tileBlendAll[Type] = true;
            Main.tileMergeDirt[Type] = true;
            Main.tileBlockLight[Type] = true;
            TileID.Sets.NeedsGrassFraming[Type] = true;
            DustType = Mod.Find<ModDust>("VoidDust").Type;
            AddMapEntry(new Color(70, 50, 90));
        }

        public override void RandomUpdate(int i, int j)
        {
            if (!Framing.GetTileSafely(i, j - 1).HasTile && Main.rand.Next(40) == 0)
            {
                if (!Framing.GetTileSafely(i, j - 1).HasTile && Main.rand.Next(20) == 0)
                {
                    int style = Main.rand.Next(23);
                    if (PlaceObject(i, j - 1, ModContent.TileType<AbyssFoliage>(), false, style))
                        NetMessage.SendObjectPlacement(-1, i, j - 1, ModContent.TileType<AbyssFoliage>(), style, 0, -1, -1);
                }
            }
        }

        public static bool PlaceObject(int x, int y, int type, bool mute = false, int style = 0, int random = -1, int direction = -1)
        {
            if (!TileObject.CanPlace(x, y, type, style, direction, out TileObject toBePlaced, false))
            {
                return false;
            }
            toBePlaced.random = random;
            if (TileObject.Place(toBePlaced) && !mute)
            {
                WorldGen.SquareTileFrame(x, y, true);
            }
            return false;
        }

        /*public override int SaplingGrowthType(ref int style)
        {
            style = 0;
            return mod.TileType("AbyssSapling");
        }*/
    }
}