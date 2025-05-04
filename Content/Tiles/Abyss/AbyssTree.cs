using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria.GameContent;
using Terraria.ModLoader;
using Terraria;
using Terraria.ID;
using Terraria.DataStructures;
using Terraria.Enums;
using CSkies.Content.NPCs.Other;

namespace CSkies.Content.Tiles.Abyss
{
    public class ExampleTree : ModTree
    {
        private Asset<Texture2D> texture;
        private Asset<Texture2D> branchesTexture;
        private Asset<Texture2D> topsTexture;

        public override TreePaintingSettings TreeShaderSettings => new TreePaintingSettings
        {
            UseSpecialGroups = true,
            SpecialGroupMinimalHueValue = 11f / 72f,
            SpecialGroupMaximumHueValue = 0.25f,
            SpecialGroupMinimumSaturationValue = 0.88f,
            SpecialGroupMaximumSaturationValue = 1f
        };

        public override void SetStaticDefaults()
        {
            GrowsOnTileId = new int[1]
            {
                ModContent.TileType<AbyssGrass>()
            };
            texture = ModContent.Request<Texture2D>("CSkies/Content/Tiles/Abyss/AbyssTree");
            branchesTexture = ModContent.Request<Texture2D>("CSkies/Content/Tiles/Abyss/AbyssTreeBranches");
            topsTexture = ModContent.Request<Texture2D>("CSkies/Content/Tiles/Abyss/AbyssTreetop");

        }

        public override Asset<Texture2D> GetTexture()
        {
            return texture;
        }


        public override void SetTreeFoliageSettings(Tile tile, ref int xoffset, ref int treeFrame, ref int floorY, ref int topTextureFrameWidth, ref int topTextureFrameHeight)
        {

        }

        public override Asset<Texture2D> GetBranchTextures() => branchesTexture;

        public override Asset<Texture2D> GetTopTextures() => topsTexture;

        public override int DropWood()
        {
            return ItemID.Wood; //AbyssWood
        }
    }
}