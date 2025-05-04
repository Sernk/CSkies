using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using CSkies.Content.Dusts;

namespace CSkies.Content.Tiles.Abyss
{
    public class AbyssGrass : ModTile
    {
        public static int _type;

        public override void SetStaticDefaults()
        {
            Main.tileSolid[Type] = true;
            TileID.Sets.Conversion.Grass[Type] = true;
            Main.tileBlendAll[Type] = true;
            Main.tileMergeDirt[Type] = true;
            Main.tileBlockLight[Type] = true;
            TileID.Sets.NeedsGrassFraming[Type] = true;
            DustType = ModContent.DustType<VoidDust>();
            AddMapEntry(new Color(61, 42, 84));
            //ItemDrop/* tModPorter Note: Removed. Tiles and walls will drop the item which places them automatically. Use RegisterItemDrop to alter the automatic drop if necessary. */ = ItemID.DirtBlock;
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

        /*public override int SaplingGrowthType(ref int style)/* tModPorter Note: Removed. Use ModTree.SaplingGrowthType 
        {
            style = 0;
            return ModContent.TileType<AbyssSapling>();
        }*/
    }
}