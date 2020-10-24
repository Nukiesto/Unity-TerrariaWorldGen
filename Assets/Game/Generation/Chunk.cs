using System;
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

        public static Chunk GenerateChunk(int chunkX, int chunkY)
        {
            if (chunkY == 0)
                return GenerateTerrain(chunkX);

            return GenerateUnderground();
        }

        private static Chunk GenerateTerrain(int chunkX)
        {
            Chunk chunk = new Chunk();
            for (int x = 0; x < chunk.width; x++)
            {
                // Generate the max terrain height for this column
                int worldX = (chunkX * WorldGenerator.ChunkWidth) - (WorldGenerator.ChunkWidth / 2) + x;
                float surfaceHeight = WorldGenerator.GetNoise(worldX, 0);
                surfaceHeight = Mathf.FloorToInt(surfaceHeight);

                // TODO: Create smoother heights, maybe use Perlin Noise as well?
                int dirtHeight = (int) surfaceHeight - WorldGenerator.GetDirtHeight();
                
                for (int y = 0; y < chunk.height; y++)
                {
                    if (y == (int) surfaceHeight)
                        chunk._tileMap[x, y] = TileManager.GetTileIdByName("Grass");
                    else if (y >= dirtHeight && y < (int) surfaceHeight)
                        chunk._tileMap[x, y] = TileManager.GetTileIdByName("Dirt");
                    else if (y < dirtHeight)
                        chunk._tileMap[x, y] = TileManager.GetTileIdByName("Stone");
                }
            }
            
            return chunk;
        }

        private static Chunk GenerateUnderground()
        {
            Chunk chunk = new Chunk();
            // Fill chunk with Stone & Ores
            for (int x = 0; x < chunk.width; x++)
            {
                for (int y = 0; y < chunk.height; y++)
                {
                    // TODO: There has to be a better way of generating ores
                    int chunkTileID = chunk._tileMap[x, y];
                    Tile coal = TileManager.GetTileByName("Coal Ore");
                    Tile iron = TileManager.GetTileByName("Iron Ore");
                    Tile gold = TileManager.GetTileByName("Gold Ore");

                    if (chunkTileID == coal.id || chunkTileID == iron.id || chunkTileID == gold.id)
                    {
                        continue;
                    }

                    int random = WorldGenerator.GetRandom(0, 1000);
                    if (random < gold.rarity)
                    {
                        chunk._tileMap[x, y] = gold.id;
                        GenerateOreVein(x, y, gold, chunk);
                        continue;
                    }
                    if (random < iron.rarity)
                    {
                        chunk._tileMap[x, y] = iron.id;
                        GenerateOreVein(x, y, iron, chunk);
                        continue;
                    }

                    if (random < coal.rarity)
                    {
                        chunk._tileMap[x, y] = coal.id;
                        GenerateOreVein(x, y, coal, chunk);
                        continue;
                    }
                    
                    chunk._tileMap[x, y] = TileManager.GetTileIdByName("Stone");
                }
            }

            return chunk;
        }

        private static void GenerateOreVein(int x, int y, Tile ore, Chunk chunk)
        {
            int veinSize = ore.maxVeinSize / 2;
            for (int newX = x - veinSize; newX <= (x + veinSize); newX++)
            {
                for (int newY = y - veinSize; newY <= (y + veinSize); newY++)
                {
                    if (newX < 0 || newX >= WorldGenerator.ChunkWidth || y < 0 ||
                        y >= WorldGenerator.ChunkHeight) continue;
                    
                    try
                    {
                        if (chunk._tileMap[newX, newY] != TileManager.GetTileIdByName("Stone")) continue;
                        chunk._tileMap[newX, newY] = ore.id;
                    }
                    catch (IndexOutOfRangeException)
                    { }
                }
            }
        }

        public int GetTile(int x, int y)
        {
            return _tileMap[x, y];
        }
    }
}