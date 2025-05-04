using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using CSkies.Content.Dusts;

namespace CSkies.Content.Tiles.Abyss
{
    public class HardenedAbyssSand : ModTile
    {
        public override void SetStaticDefaults()
        {
            Main.tileSolid[Type] = true;
            Terraria.ID.TileID.Sets.Conversion.HardenedSand[Type] = true;
            Main.tileBlendAll[Type] = true;
            Main.tileBlockLight[Type] = true;
            DustType = ModContent.DustType<VoidDust>();
            //drop = mod.ItemType("HardenedAbyssSand");   //put your CustomBlock name
            AddMapEntry(new Color(30, 17, 50));
            MinPick = 65;
        }
    }
}