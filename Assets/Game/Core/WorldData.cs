using System.Collections.Generic;
using Game.Tiles;
using MessagePack;

namespace Game.Core
{
    [MessagePackObject]
    public class WorldData
    {
        [Key(0)]
        public int Width { get; set; }
        
        [Key(1)]
        public int Height { get; set; }
        
        [Key(2)]
        public int[,] TileMap { get; set; }

        public Tile GetTile(int x, int y)
        {
            return TileManager.GetTile(TileMap[x, y]);
        }
    }
}