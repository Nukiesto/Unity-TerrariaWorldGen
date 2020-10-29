using System.Collections.Generic;
using Game.Core;
using Game.Tiles;
using UnityEngine;

namespace Game.Generation.GenTasks
{
    public class TerrainGenTask : GenTask
    {
        public override void ModifyWorldGenTasks(List<GenPass> tasks)
        {
            tasks.Insert(0, new GenPass("Terrain Generation", TerrainGeneration));
        }

        private bool TerrainGeneration()
        {
            for (int x = 0; x < WorldGen.GenSettings.worldWidth; x++)
            {
                int surfaceHeight = WorldGen.MapToRange(WorldGen.GetNoise(x, WorldGen.GenSettings.maxSurfaceHeight),
                    -1, 1, WorldGen.GenSettings.minSurfaceHeight, WorldGen.GenSettings.maxSurfaceHeight);
                int dirtHeight = WorldGen.GenSettings.minSurfaceHeight -
                                 WorldGen.MapToRange(WorldGen.GetNoise(x, WorldGen.GenSettings.minDirtHeight), 
                                     -1, 1,
                                     WorldGen.GenSettings.minDirtHeight, WorldGen.GenSettings.minSurfaceHeight);

                for (int y = 0; y < WorldGen.GenSettings.worldHeight; y++)
                {
                    if (y == surfaceHeight)
                        WorldGen.SetTile(x, y, TileManager.GetTile("Grass").Id);
                    else if (y >= WorldGen.GenSettings.minSurfaceHeight - dirtHeight && y < surfaceHeight)
                        WorldGen.SetTile(x, y, TileManager.GetTile("Dirt").Id);
                    else if (y < surfaceHeight - dirtHeight)
                        WorldGen.SetTile(x, y, TileManager.GetTile("Stone").Id);
                }
            }
            
            return true;
        }
    }
}