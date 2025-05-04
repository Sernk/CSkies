using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using CSkies.Content.Dusts;

namespace CSkies.Content.Tiles.Abyss
{
    public class AbyssDoor : ModTile
    {
        public override void SetStaticDefaults()
        {
            Main.tileSolid[Type] = true;
			Main.tileMergeDirt[Type] = true;
            Main.tileBlockLight[Type] = true;
            Main.tileMerge[Type][ModContent.TileType<AbyssBricks>()] = true;
            HitSound = SoundID.Tink;
            DustType = ModContent.DustType<VoidDust>();
            AddMapEntry(new Color(10, 10, 50));
			MinPick = 99999;
        }

        public override bool CanExplode(int i, int j)
        {
            return false;
        }

        public override bool CanKillTile(int i, int j, ref bool blockDamaged)
        {
            return false;
        }
    }
}