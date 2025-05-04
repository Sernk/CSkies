using System.Collections.Generic;
using Terraria;
using Terraria.DataStructures;
using Microsoft.Xna.Framework;
using Terraria.GameContent.Generation;
using Terraria.ModLoader;
using Terraria.WorldBuilding;
using StructureHelper.API;
using static StructureHelper.API.Generator;

namespace CSkies.Utilities
{
    public class CSCH : ModSystem
    {
        private List<Point16> placedRuins = new List<Point16>();

        public override void ModifyWorldGenTasks(List<GenPass> tasks, ref double totalWeight)
        {
            int ShiniesIndex = tasks.FindIndex(genpass => genpass.Name.Equals("Shinies"));
            if (ShiniesIndex == -1)
            {
                return;
            }
            tasks.Insert(ShiniesIndex + 1, new PassLegacy("Generating dungeon", (progress, configuration) =>
            {
                progress.Message = "Generating ruins...";

                CreateRuin();
            }));
        }

        private void CreateRuin()
        {
            int worldWidth = Main.maxTilesX;
            int worldHeight = Main.maxTilesY;

            int centerX = worldWidth / 2;
            int placeY = worldHeight - 600;

            if (worldWidth <= 4200)
            {
                Point16 ruin3Pos = new Point16(centerX, placeY);
                Point16 ruin2Pos = new Point16(centerX - 500, placeY);
                Point16 ruin1Pos = new Point16(centerX + 500, placeY);

                GenerateStructure("Structures/Ruin3", ruin3Pos, CSkies.Instance);
                GenerateStructure("Structures/Ruin2", ruin2Pos, CSkies.Instance);
                GenerateStructure("Structures/Ruin1", ruin1Pos, CSkies.Instance);

                placedRuins.Add(ruin3Pos);
                placedRuins.Add(ruin2Pos);
                placedRuins.Add(ruin1Pos);
            }
            else
            {
                for (int i = 0; i < 4; i++)
                {
                    PlaceRuin("Structures/Ruin3", worldWidth, worldHeight, 750);
                }

                for (int i = 0; i < 3; i++)
                {
                    PlaceRuin("Structures/Ruin2", worldWidth, worldHeight, 500);
                }

                for (int i = 0; i < 2; i++)
                {
                    PlaceRuin("Structures/Ruin1", worldWidth, worldHeight, 250);
                }
            }
        }


        private void PlaceRuin(string structurePath, int worldWidth, int worldHeight, int minDistance)
        {
            int attempts = 0;
            while (attempts < 100)
            {
                int ruinX = WorldGen.genRand.Next(100, worldWidth - 100);
                int ruinY = WorldGen.genRand.Next(worldHeight - 1000, worldHeight - 500);
                Point16 newRuin = new Point16(ruinX, ruinY);

                bool tooClose = false;
                foreach (var ruin in placedRuins)
                {
                    if (Vector2.Distance(new Vector2(ruinX, ruinY), new Vector2(ruin.X, ruin.Y)) < minDistance)
                    {
                        tooClose = true;
                        break;
                    }
                }

                if (!tooClose)
                {
                    GenerateStructure(structurePath, newRuin, CSkies.Instance);
                    placedRuins.Add(newRuin);
                    break;
                }
                attempts++;
            }
        }
    }
}