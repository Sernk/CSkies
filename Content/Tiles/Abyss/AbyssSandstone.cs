using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using CSkies.Content.Dusts;

namespace CSkies.Content.Tiles.Abyss
{
    public class AbyssSandstone : ModTile
    {
        public override void SetStaticDefaults()
        {
            Main.tileSolid[Type] = true;
            Terraria.ID.TileID.Sets.Conversion.Sandstone[Type] = true;
            Main.tileBlendAll[Type] = true;
            Main.tileBlockLight[Type] = true;
            DustType = ModContent.DustType<VoidDust>();
            //drop = mod.ItemType("AbyssSandstone");   //put your CustomBlock name
            AddMapEntry(new Color(40, 30, 50));
            MinPick = 65;
        }
    }
}