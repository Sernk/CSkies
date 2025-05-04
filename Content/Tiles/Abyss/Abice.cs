using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using CSkies.Content.Dusts;

namespace CSkies.Content.Tiles.Abyss
{
    public class Abice : ModTile
    {
        public override void SetStaticDefaults()
        {
            Main.tileSolid[Type] = true;
            Main.tileMergeDirt[Type] = true;
            Main.tileBlendAll[this.Type] = false;
			Main.tileMerge[TileID.SnowBlock][Type] = true;
            HitSound = SoundID.Tink;
            DustType = ModContent.DustType<VoidDust>();
            //drop = mod.ItemType("BlackIce");   //put your CustomBlock name
            AddMapEntry(new Color(60, 60, 100));
            TileID.Sets.Ices[Type] = true;
            MinPick = 225;
        }
    }
}