using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using CSkies.Content.Dusts;

namespace CSkies.Content.Tiles.Abyss
{
    public class AbyssStone : ModTile
    {
        public override void SetStaticDefaults()
        {
            Main.tileSolid[Type] = true;
			Main.tileMergeDirt[Type] = true;
            Main.tileBlockLight[Type] = true;
            HitSound = SoundID.Tink;
            DustType = ModContent.DustType<VoidDust>();
            AddMapEntry(new Color(20, 20, 50));
			MinPick = 225;
        }
    }
}