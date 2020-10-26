using System.Collections.Generic;
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
            for (int x = 0; x < WorldGen.WorldWidth; x++)
            {
                int surfaceHeight = WorldGen.MapToRange(WorldGen.GetNoise(x, WorldGen.MaxSurfaceHeight),
                    -1, 1, WorldGen.MinSurfaceHeight, WorldGen.MaxSurfaceHeight);
                int dirtHeight = WorldGen.MinSurfaceHeight - WorldGen.MapToRange(WorldGen.GetNoise(x, WorldGen.MinDirtHeight),
                    -1, 1, WorldGen.MinDirtHeight, WorldGen.MinSurfaceHeight);

                for (int y = 0; y < WorldGen.WorldHeight; y++)
                {
                    if (y == surfaceHeight)
                        WorldGen.SetTile(x, y, TileManager.GetTile("Grass").Id);
                    else if (y >= WorldGen.MinSurfaceHeight - dirtHeight && y < surfaceHeight)
                        WorldGen.SetTile(x, y, TileManager.GetTile("Dirt").Id);
                    else if (y < surfaceHeight - dirtHeight)
                        WorldGen.SetTile(x, y, TileManager.GetTile("Stone").Id);
                }
            }
            
            return true;
        }
    }
}