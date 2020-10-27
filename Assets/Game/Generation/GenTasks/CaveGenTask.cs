using System.Collections.Generic;
using Game.Core;
using Game.Tiles;

namespace Game.Generation.GenTasks
{
    public class CaveGenTask : GenTask
    {
        public override void ModifyWorldGenTasks(List<GenPass> tasks)
        {
            tasks.Insert(0, new GenPass("Cave Generation", GenerateCaveSystems));
        }

        private int[,] _map;
        private int _width;
        private int _height;
        private bool GenerateCaveSystems()
        {
            _width = World.GenSettings.worldWidth;
            _height = World.GenSettings.minDirtHeight;
            _map = new int[_width, _height];
            
            // Randomly fills map
            for (int x = 0; x < _width; x++)
            {
                for (int y = 0; y < _height; y++)
                {
                    _map[x, y] = (WorldGen.GetRandom(0, 100) < World.GenSettings.cavePercentage) ? 0 : 1;
                }
            }
            
            // Smooth map
            for (int i = 0; i < World.GenSettings.caveSmoothness; i++)
            {
                SmoothMap();
            }
            
            // Populate actual world map
            int air = TileManager.GetTile("Air").Id;
            int stone = TileManager.GetTile("Stone").Id;
            for (int x = 0; x < _width; x++)
            {
                for (int y = 0; y < _height; y++)
                {
                    WorldGen.SetTile(x, y, _map[x, y] == 0 ? air : stone);
                }
            }
            
            return true;
        }

        private void SmoothMap()
        {
            for (int x = 0; x < _width; x++)
            {
                for (int y = 0; y < _height; y++)
                {
                    int neighbours = GetNeighbourCount(x, y);
                    if (neighbours > 4)
                        _map[x, y] = 1;
                    else if (neighbours < 4)
                        _map[x, y] = 0;
                }
            }
        }

        private int GetNeighbourCount(int x, int y)
        {
            int count = 0;
            
            for (int neighbourX = x - 1; neighbourX <= x + 1; neighbourX ++) {
                for (int neighbourY = y - 1; neighbourY <= y + 1; neighbourY ++) {
                    if (neighbourX >= 0 && neighbourX < _width && neighbourY >= 0 && neighbourY < _height) {
                        if (neighbourX != x || neighbourY != y) {
                            count += _map[neighbourX,neighbourY];
                        }
                    } else {
                        count ++;
                    }
                }
            }

            return count;
        }
    }
}