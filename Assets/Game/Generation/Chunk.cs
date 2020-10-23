using UnityEngine;

namespace Game.Generation
{
    public class Chunk
    {
        public readonly int width;
        public readonly int height;

        private int[,] _tileMap;

        private Chunk()
        {
            width = WorldGenerator.ChunkWidth;
            height = WorldGenerator.ChunkHeight;
            
            _tileMap = new int[width, height];
        }

        public static Chunk GenerateChunk(int chunkX)
        {
            Chunk chunk = new Chunk();
            for (int x = 0; x < chunk.width; x++)
            {
                // Generate the max terrain height for this column
                int worldX = (chunkX * WorldGenerator.ChunkWidth) - (WorldGenerator.ChunkWidth / 2) + x;
                float terrainHeight = WorldGenerator.GetNoise(worldX, 0);
                terrainHeight = Mathf.FloorToInt(terrainHeight);
                
                for (int y = 0; y < chunk.height; y++)
                {
                    if (y == (int) terrainHeight) chunk._tileMap[x, y] = TileManager.GetTileIdByName("Grass");
                    else if (y <= (int) terrainHeight) chunk._tileMap[x, y] = TileManager.GetTileIdByName("Dirt");
                }
            }
            
            return chunk;
        }

        public int GetTile(int x, int y)
        {
            return _tileMap[x, y];
        }
    }
}