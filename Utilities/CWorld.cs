using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using Terraria.WorldBuilding;
using Terraria.GameContent.Generation;
using CSkies.Content.Projectiles;
using CSkies.Utilities.Base.BaseMod.Base;
using CSkies.Content.Tiles;
using CSkies.Content.Tiles.Abyss;
using Terraria.Localization;

namespace CSkies
{
    public class CWorld : ModSystem, ILocalizedModType
    {
        public string LocalizationCategory => "CWorldSystem";
        public static string Sky;

        public override void Load()
        {
            string downedMoonlord = this.GetLocalization("Chat.downedMoonlord").Value; // The death of a titan rips open a gateway from the dark below
            string downedObserverV = this.GetLocalization("Chat.downedObserverV").Value; // The sound of arcane stone cracking echoes across the land...
            string Sky = this.GetLocalization("Chat.Sky").Value; // The sky has fallen somewhere in the world
        }
        public static bool MeteorMessage = false;
        public static bool downedObserver = false;
        public static bool downedObserverV = false;
        public static bool downedVoid = false;

        public static bool downedStarcore = false;
        public static bool downedHeart = false;
        public static bool downedHeartcore = false;
        public static bool downedSoul = false;

        public static int CometTiles = 0;
        public static int AbyssTiles = 0;

        public static int VaultCount = 0;
        public static bool KillDoors = false;
        public static bool AbyssBiome = false;

        public static bool Altar = false;
        public static bool Altar1 = false;
        public static bool Altar2 = false;
        public static bool Altar3 = false;
        public static bool Altar4 = false;

        public override void OnWorldLoad()
        {
            downedObserver = false;
            downedObserverV = false;
            downedVoid = false;
            downedStarcore = false;
            downedHeart = false;
            downedHeartcore = false;
            downedSoul = false;
            AbyssBiome = false;
            Altar = false;
            Altar1 = false;
            Altar2 = false;
            Altar3 = false;
            Altar4 = false;
            AbyssBiome = false; 
        }

        #region saving/loading
        public override void SaveWorldData(TagCompound tag)
        {
            var downed = new List<string>();
            if (downedObserver) downed.Add("O1");
            if (downedObserverV) downed.Add("02");
            if (downedVoid) downed.Add("VOID");

            if (downedStarcore) downed.Add("Star");
            if (downedHeart) downed.Add("Molten");
            if (downedHeartcore) downed.Add("Heart");
            if (downedSoul) downed.Add("Soul");

            if (MeteorMessage) downed.Add("Comet");
            if (KillDoors) downed.Add("door");
            if (AbyssBiome) downed.Add("AB");

            return;
        }
        public override void LoadWorldData(TagCompound tag)
        {
            var downed = tag.GetList<string>("downed");
            downedObserver = downed.Contains("O1");
            downedObserverV = downed.Contains("O2");
            downedVoid = downed.Contains("VOID");

            downedStarcore = downed.Contains("Star");
            downedHeart = downed.Contains("Molten");
            downedHeartcore = downed.Contains("Heart");
            downedSoul = downed.Contains("Soul");

            MeteorMessage = downed.Contains("Comet");
            KillDoors = downed.Contains("door");
            AbyssBiome = downed.Contains("AB");
        }

        public override void NetSend(BinaryWriter writer)
        {
            BitsByte flags = new BitsByte();
            flags[0] = downedObserver;
            flags[1] = downedObserverV;
            flags[2] = MeteorMessage;
            flags[3] = downedVoid;
            flags[4] = KillDoors;
            flags[5] = downedStarcore;
            flags[6] = downedHeartcore;
            flags[7] = downedSoul;
            writer.Write(flags);

            BitsByte flags0 = new BitsByte();
            flags0[0] = downedHeart;
            flags0[1] = AbyssBiome;
            writer.Write(flags0);
        }
        public override void NetReceive(BinaryReader reader)
        {
            BitsByte flags = reader.ReadByte();
            downedObserver = flags[0];
            downedObserverV = flags[1];
            MeteorMessage = flags[2];
            downedVoid = flags[3];
            KillDoors = flags[4];
            downedStarcore = flags[5];
            downedHeartcore = flags[6];
            downedSoul = flags[7];


            BitsByte flags0 = reader.ReadByte();
            downedHeart = flags0[0];
            AbyssBiome = flags0[1];
        }
        #endregion

        public override void TileCountsAvailable(ReadOnlySpan<int> tileCounts)
        {
            CometTiles = tileCounts[ModContent.TileType<CometOreTile>()];

            AbyssTiles = tileCounts[ModContent.TileType<AbyssBricks>()] +
                tileCounts[ModContent.TileType<AbyssSand>()] +
                tileCounts[ModContent.TileType<Abice>()] +
                tileCounts[ModContent.TileType<AbyssSandstone>()] +
                tileCounts[ModContent.TileType<HardenedAbyssSand>()] +
                tileCounts[ModContent.TileType<AbyssStone>()] +
                tileCounts[ModContent.TileType<AbyssGrass>()];
        }

        public override void ModifyWorldGenTasks(List<GenPass> tasks, ref double totalWeight)
        {
            int shiniesIndex2 = tasks.FindIndex(genpass => genpass.Name.Equals("Final Cleanup"));

        }

        private void Structures(GenerationProgress progress)
        {

        }

        public static float NovacoreAI = 0;
        public static float NovacoreCounter = 0;

        public override void PostUpdateWorld()
        {
            if (!Main.dayTime && NPC.downedBoss2)
            {
                float num143 = Main.maxTilesX / 4200;
                if (Main.rand.Next(8000) < 5f * num143)
                {
                    int num144 = 12;
                    int num145 = Main.rand.Next(Main.maxTilesX - 50) + 100;
                    num145 *= 16;
                    int num146 = Main.rand.Next((int)(Main.maxTilesY * 0.05));
                    num146 *= 16;
                    Vector2 vector = new Vector2(num145, num146);
                    float num147 = Main.rand.Next(-100, 101);
                    float num148 = Main.rand.Next(200) + 100;
                    float num149 = (float)Math.Sqrt(num147 * num147 + num148 * num148);
                    num149 = num144 / num149;
                    num147 *= num149;
                    num148 *= num149;
                    Projectile.NewProjectile(Main.LocalPlayer.GetSource_FromThis(), vector.X, vector.Y, num147, num148, ModContent.ProjectileType<FallenShard>(), 1000, 10f, Main.myPlayer, 0f, 0f);
                }
            }

            if (Main.dayTime && Main.time == 5)
            {
                Altar = false;
                Altar1 = false;
                Altar2 = false;
                Altar3 = false;
                Altar4 = false;
            }

            //Point AbyssPos = Point.Zero;

            if (NPC.downedMoonlord && !AbyssBiome)
            {
                string downedMoonlord = this.GetLocalization("Chat.downedMoonlord").Value; 
                Main.NewText(downedMoonlord, new Color(61, 41, 81));
                AbyssBiome = true;
            }

            if (downedObserverV && !KillDoors)
            {
                for (int j = 0; j < Main.maxTilesX; j++)
                {
                    for (int k = 0; k < Main.maxTilesY; k++)
                    {
                        if (Main.tile[j, k].HasTile && Main.tile[j, k].TileType == (ushort)ModContent.TileType<AbyssDoor>())
                        {
                            WorldGen.KillTile(j, k, false, false, true);
                        }
                    }
                }
                string downedObserverV = this.GetLocalization("Chat.downedObserverV").Value;
                Main.NewText(downedObserverV, new Color(61, 41, 81));
                KillDoors = true;
            }
        }

        public static void DropMeteor()
        {
            bool flag = true;
            if (Main.netMode == NetmodeID.MultiplayerClient)
            {
                return;
            }
            for (int i = 0; i < 255; i++)
            {
                if (Main.player[i].active)
                {
                    flag = false;
                    break;
                }
            }
            int num = 0;
            float num2 = Main.maxTilesX / 4200;
            int num3 = (int)(400f * num2);
            for (int j = 5; j < Main.maxTilesX - 5; j++)
            {
                int num4 = 5;
                while (num4 < Main.worldSurface)
                {
                    if (Main.tile[j, num4].HasTile && Main.tile[j, num4].TileType == (ushort)ModContent.TileType<CometOreTile>())
                    {
                        num++;
                        if (num > num3)
                        {
                            return;
                        }
                    }
                    num4++;
                }
            }
            float num5 = 600f;
            while (!flag)
            {
                float num6 = Main.maxTilesX * 0.08f;
                int num7 = Main.rand.Next(500, Main.maxTilesX - 500);
                while (num7 > Main.spawnTileX - num6 && num7 < Main.spawnTileX + num6)
                {
                    num7 = Main.rand.Next(500, Main.maxTilesX - 500);
                }
                int k = (int)(Main.worldSurface * 0.3);
                while (k < Main.maxTilesY)
                {
                    if (Main.tile[num7, k].HasTile && Main.tileSolid[Main.tile[num7, k].TileType])
                    {
                        int num8 = 0;
                        int num9 = 15;
                        for (int l = num7 - num9; l < num7 + num9; l++)
                        {
                            for (int m = k - num9; m < k + num9; m++)
                            {
                                if (WorldGen.SolidTile(l, m))
                                {
                                    num8++;
                                    if (Main.tile[l, m].TileType == 189 || Main.tile[l, m].TileType == 202)
                                    {
                                        num8 -= 100;
                                    }
                                }
                                else if (Main.tile[l, m].LiquidAmount > 0)
                                {
                                    num8--;
                                }
                            }
                        }
                        if (num8 < num5)
                        {
                            num5 -= 0.5f;
                            break;
                        }
                        flag = Meteor(num7, k);
                        if (flag)
                        {
                            break;
                        }
                        break;
                    }
                    else
                    {
                        k++;
                    }
                }
                if (num5 < 100f)
                {
                    return;
                }
            }
        }

        public static bool Meteor(int i, int j)
        {
            if (i < 50 || i > Main.maxTilesX - 50)
            {
                return false;
            }
            if (j < 50 || j > Main.maxTilesY - 50)
            {
                return false;
            }
            int num = 35;
            Rectangle rectangle = new Rectangle((i - num) * 16, (j - num) * 16, num * 2 * 16, num * 2 * 16);
            for (int k = 0; k < 255; k++)
            {
                if (Main.player[k].active)
                {
                    Rectangle value = new Rectangle((int)(Main.player[k].position.X + Main.player[k].width / 2 - NPC.sWidth / 2 - NPC.safeRangeX), (int)(Main.player[k].position.Y + Main.player[k].height / 2 - NPC.sHeight / 2 - NPC.safeRangeY), NPC.sWidth + NPC.safeRangeX * 2, NPC.sHeight + NPC.safeRangeY * 2);
                    if (rectangle.Intersects(value))
                    {
                        return false;
                    }
                }
            }
            for (int l = 0; l < 200; l++)
            {
                if (Main.npc[l].active)
                {
                    Rectangle value2 = new Rectangle((int)Main.npc[l].position.X, (int)Main.npc[l].position.Y, Main.npc[l].width, Main.npc[l].height);
                    if (rectangle.Intersects(value2))
                    {
                        return false;
                    }
                }
            }
            for (int m = i - num; m < i + num; m++)
            {
                for (int n = j - num; n < j + num; n++)
                {
                    if (Main.tile[m, n].HasTile && Main.tile[m, n].TileType == 21)
                    {
                        return false;
                    }
                }
            }
            num = WorldGen.genRand.Next(17, 23);
            for (int num2 = i - num; num2 < i + num; num2++)
            {
                for (int num3 = j - num; num3 < j + num; num3++)
                {
                    if (num3 > j + Main.rand.Next(-2, 3) - 5)
                    {
                        float num4 = Math.Abs(i - num2);
                        float num5 = Math.Abs(j - num3);
                        float num6 = (float)Math.Sqrt(num4 * num4 + num5 * num5);
                        if (num6 < num * 0.9 + Main.rand.Next(-4, 5))
                        {
                            if (!Main.tileSolid[Main.tile[num2, num3].TileType])
                            {
                                //Main.tile[num2, num3].HasTile = false;
                            }
                            Main.tile[num2, num3].TileType = (ushort)ModContent.TileType<CometOreTile>();
                        }
                    }
                }
            }
            num = WorldGen.genRand.Next(8, 14);
            for (int num7 = i - num; num7 < i + num; num7++)
            {
                for (int num8 = j - num; num8 < j + num; num8++)
                {
                    if (num8 > j + Main.rand.Next(-2, 3) - 4)
                    {
                        float num9 = Math.Abs(i - num7);
                        float num10 = Math.Abs(j - num8);
                        float num11 = (float)Math.Sqrt(num9 * num9 + num10 * num10);
                        if (num11 < num * 0.8 + Main.rand.Next(-3, 4))
                        {
                            //Main.tile[num7, num8].HasTile = false;
                        }
                    }
                }
            }
            num = WorldGen.genRand.Next(25, 35);
            for (int num12 = i - num; num12 < i + num; num12++)
            {
                for (int num13 = j - num; num13 < j + num; num13++)
                {
                    float num14 = Math.Abs(i - num12);
                    float num15 = Math.Abs(j - num13);
                    float num16 = (float)Math.Sqrt(num14 * num14 + num15 * num15);
                    if (num16 < num * 0.7)
                    {
                        if (Main.tile[num12, num13].TileType == 5 || Main.tile[num12, num13].TileType == 32 || Main.tile[num12, num13].TileType == 352)
                        {
                            WorldGen.KillTile(num12, num13, false, false, false);
                        }
                        Main.tile[num12, num13].LiquidAmount = 0;
                    }
                    if (Main.tile[num12, num13].TileType == (ushort)ModContent.TileType<CometOreTile>())
                    {
                        if (!WorldGen.SolidTile(num12 - 1, num13) && !WorldGen.SolidTile(num12 + 1, num13) && !WorldGen.SolidTile(num12, num13 - 1) && !WorldGen.SolidTile(num12, num13 + 1))
                        {
                            //Main.tile[num12, num13].HasTile = false;
                        }
                        else if ((Main.tile[num12, num13].IsHalfBlock || Main.tile[num12 - 1, num13].TopSlope) && !WorldGen.SolidTile(num12, num13 + 1))
                        {
                           // Main.tile[num12, num13].HasTile = false;
                        }
                    }
                    WorldGen.SquareTileFrame(num12, num13, true);
                    WorldGen.SquareWallFrame(num12, num13, true);
                }
            }
            num = WorldGen.genRand.Next(23, 32);
            for (int num17 = i - num; num17 < i + num; num17++)
            {
                for (int num18 = j - num; num18 < j + num; num18++)
                {
                    if (num18 > j + WorldGen.genRand.Next(-3, 4) - 3 && Main.tile[num17, num18].HasTile && Main.rand.Next(10) == 0)
                    {
                        float num19 = Math.Abs(i - num17);
                        float num20 = Math.Abs(j - num18);
                        float num21 = (float)Math.Sqrt(num19 * num19 + num20 * num20);
                        if (num21 < num * 0.8)
                        {
                            if (Main.tile[num17, num18].TileType == 5 || Main.tile[num17, num18].TileType == 32 || Main.tile[num17, num18].TileType == 352)
                            {
                                WorldGen.KillTile(num17, num18, false, false, false);
                            }
                            Main.tile[num17, num18].TileType = (ushort)ModContent.TileType<CometOreTile>();
                            WorldGen.SquareTileFrame(num17, num18, true);
                        }
                    }
                }
            }
            num = WorldGen.genRand.Next(30, 38);
            for (int num22 = i - num; num22 < i + num; num22++)
            {
                for (int num23 = j - num; num23 < j + num; num23++)
                {
                    if (num23 > j + WorldGen.genRand.Next(-2, 3) && Main.tile[num22, num23].HasTile && Main.rand.Next(20) == 0)
                    {
                        float num24 = Math.Abs(i - num22);
                        float num25 = Math.Abs(j - num23);
                        float num26 = (float)Math.Sqrt(num24 * num24 + num25 * num25);
                        if (num26 < num * 0.85)
                        {
                            if (Main.tile[num22, num23].TileType == 5 || Main.tile[num22, num23].TileType == 32 || Main.tile[num22, num23].TileType == 352)
                            {
                                WorldGen.KillTile(num22, num23, false, false, false);
                            }
                            Main.tile[num22, num23].TileType = (ushort)ModContent.TileType<CometOreTile>();
                            WorldGen.SquareTileFrame(num22, num23, true);
                        }
                    }
                }
            }
            //NetworkText Death = NetworkText.FromLiteral(this.GetLocalization("Chat.Death").Value);
            string cometText = CWorld.Sky;
            if (Main.netMode != NetmodeID.MultiplayerClient)
            {
                BaseUtility.Chat(cometText, new Color(136, 151, 255), true);
            }
            if (Main.netMode != NetmodeID.MultiplayerClient)
            {
                NetMessage.SendTileSquare(-1, i, j, 40, TileChangeType.None);
            }
            return true;
        }
    }
}