using System.Collections.Generic;
using Game.Tiles;
using UnityEngine;

namespace Game.Generation.GenTasks
{
    public class BaseTask : GenTask
    {
        public override void ModifyWorldGenTasks(List<GenPass> tasks)
        {
            tasks.Insert(0, new GenPass("Surface Creation", SurfaceGeneration));
        }

        private static bool SurfaceGeneration()
        {
            for (int x = 0; x < WorldGen.WorldWidth; x++)
            {
                for (int y = 0; y < WorldGen.WorldHeight; y++)
                {
                    if (y == WorldGen.MaxSurfaceHeight)
                    {
                        WorldGen.SetTile(x, WorldGen.MaxSurfaceHeight, TileManager.GetTile("Grass").Id);
                        continue;
                    }

                    if (y < WorldGen.MaxSurfaceHeight && y >= WorldGen.MaxSurfaceHeight - WorldGen.DirtHeight)
                    {
                        WorldGen.SetTile(x, y, TileManager.GetTile("Dirt").Id);
                        continue;
                    }

                    if (y < WorldGen.MaxSurfaceHeight - WorldGen.DirtHeight)
                    {
                        WorldGen.SetTile(x, y, TileManager.GetTile("Stone").Id);
                        continue;
                    }
                }
            }
            
            Debug.Log("Created Surface");
            return false;
        }
    }
}