using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using System;

namespace CSkies.Content.Tiles.Abyss
{
    class AbyssSand : ModTile
    {

        public override void SetStaticDefaults()
        {
            Main.tileSolid[Type] = true;
            Main.tileMergeDirt[Type] = true;
            Main.tileBlendAll[Type] = true;
            //ItemDrop/* tModPorter Note: Removed. Tiles and walls will drop the item which places them automatically. Use RegisterItemDrop to alter the automatic drop if necessary. */ = Mod.Find<ModItem>("AbyssSand").Type;
            //soundStyle/* tModPorter Note: Removed. Integrate into HitSound */ = 18;
            AddMapEntry(new Color(35, 22, 50));
            //SetModCactus(new Abysstus())/* tModPorter Note: Removed. Assign GrowsOnTileId to this tile type in ModCactus.SetStaticDefaults instead */;
            //SetModPalmTree(new AbyssPalmTree())/* tModPorter Note: Removed. Assign GrowsOnTileId to this tile type in ModPalmTree.SetStaticDefaults instead */;
            TileID.Sets.Conversion.Sand[Type] = true;
            DustType = ModContent.DustType<Dusts.VoidDust>();
        }

        public override bool TileFrame(int i, int j, ref bool resetFrame, ref bool noBreak)
        {
            Tile tile = Main.tile[i, j];
            Tile tile2 = Main.tile[i, j - 1];
            Tile tile3 = Main.tile[i, j + 1];
            int tileType = tile.TileType;
            if (!WorldGen.noTileActions && tile.HasTile && (tileType == Type))
            {
                if (Main.netMode == NetmodeID.SinglePlayer)
                {
                    if (tile3 != null && !tile3.HasTile)
                    {
                        bool flag18 = !(tile2.HasTile && (TileID.Sets.BasicChest[tile2.TileType] || TileID.Sets.BasicChestFake[tile2.TileType] || tile2.TileType == 323));
                        if (flag18)
                        {
                            int damage = 10;
                            int projectileType = 0;
                            if (tileType == Type)
                            {
                                projectileType = ModContent.ProjectileType<AbyssSandBall>();
                                damage = 0;
                            }
                            tile.ClearTile();
                            int num77 = Projectile.NewProjectile(Main.LocalPlayer.GetSource_FromThis(), i * 16 + 8, j * 16 + 8, 0f, 0.41f, projectileType, damage, 0f, Main.myPlayer, 0f, 0f);
                            Main.projectile[num77].ai[0] = 1f;
                            WorldGen.SquareTileFrame(i, j, true);
                        }
                    }
                }
                else if (Main.netMode == NetmodeID.Server && tile3 != null && !tile3.HasTile)
                {
                    bool flag19 = !(tile2.HasTile && (TileID.Sets.BasicChest[tile2.TileType] || TileID.Sets.BasicChestFake[tile2.TileType] || tile2.TileType == 323));
                    if (flag19)
                    {
                        int damage2 = 10;
                        int projectileType = 0;
                        if (tileType == Type)
                        {
                            projectileType = ModContent.ProjectileType<AbyssSandBall>();
                            damage2 = 0;
                        }

                        tile.HasTile = false;
                        bool flag20 = false;
                        for (int m = 0; m < 1000; m++)
                        {
                            if (Main.projectile[m].active && Main.projectile[m].owner == Main.myPlayer && Main.projectile[m].type == projectileType && Math.Abs(Main.projectile[m].timeLeft - 3600) < 60 && Main.projectile[m].Distance(new Vector2(i * 16 + 8, j * 16 + 10)) < 4f)
                            {
                                flag20 = true;
                                break;
                            }
                        }
                        if (!flag20)
                        {
                            int num79 = Projectile.NewProjectile(Main.LocalPlayer.GetSource_FromThis(), i * 16 + 8, j * 16 + 8, 0f, 2.5f, projectileType, damage2, 0f, Main.myPlayer, 0f, 0f);
                            Main.projectile[num79].velocity.Y = 0.5f;
                            Projectile expr_7AAA_cp_0 = Main.projectile[num79];
                            expr_7AAA_cp_0.position.Y += 2f;
                            Main.projectile[num79].netUpdate = true;
                        }
                        NetMessage.SendTileSquare(-1, i, j, 1, TileChangeType.None);
                        WorldGen.SquareTileFrame(i, j, true);
                    }
                }
            }
            return true;
        }

        /*public override int SaplingGrowthType(ref int style)/* tModPorter Note: Removed. Use ModTree.SaplingGrowthType 
        {
            style = 0;
            return ModContent.TileType<AbyssPalmSapling>();
        }*/
    }
}