using System.Collections.Generic;
using Game.Tiles;

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
                int surfaceHeight = WorldGen.GetNoise(x, WorldGen.MaxSurfaceHeight);
                int dirtHeight = WorldGen.GetRandom(WorldGen.MinDirtHeight, WorldGen.MinSurfaceHeight);

                for (int y = 0; y < WorldGen.WorldHeight; y++)
                {
                    if (y == surfaceHeight)
                        WorldGen.SetTile(x, y, TileManager.GetTile("Grass").Id);
                    else if (y >= surfaceHeight - dirtHeight && y < surfaceHeight) // TODO: Randomise dirt height
                        WorldGen.SetTile(x, y, TileManager.GetTile("Dirt").Id);
                    else if (y < surfaceHeight - dirtHeight)
                        WorldGen.SetTile(x, y, TileManager.GetTile("Stone").Id);
                }
            }
            
            return true;
        }
    }
}