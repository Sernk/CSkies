using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.GameContent.Generation;
using Terraria.WorldBuilding;
using CSkies.Content.Tiles.Abyss;
using CSkies.Content.Tiles;
using CSkies.Utilities.Base.BaseMod.Base;

namespace CSkies.Utilities.Worldgen
{
    public class InWorld : GenAction
    {
        public InWorld()
        {

        }

        public override bool Apply(Point origin, int x, int y, params object[] args)
        {
            if (x < 0 || x > Main.maxTilesX || y < 0 || y > Main.maxTilesY)
                return Fail();
            return UnitApply(origin, x, y, args);
        }
    }
}