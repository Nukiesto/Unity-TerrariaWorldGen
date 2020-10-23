using System;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Generation
{
    public class WorldGenerator : MonoBehaviour
    {
        public static int ChunkWidth, ChunkHeight;
        [SerializeField] private int chunkWidth = 16, chunkHeight = 16;
        [SerializeField, Min(1)] private int worldWidth = 1, worldHeight = 1;

        private Dictionary<Vector2Int, Chunk> _chunks;

        private void Awake()
        {
            ChunkWidth = chunkWidth;
            ChunkHeight = chunkHeight;
            
            GenerateChunks();
            RenderChunks();
        }

        private void GenerateChunks()
        {
            _chunks = new Dictionary<Vector2Int, Chunk>();
            
            for (int chunkY = 0; chunkY < worldHeight; chunkY++)
            {
                for (int chunkX = 0; chunkX < worldWidth; chunkX++)
                {
                    _chunks.Add(new Vector2Int(chunkX, chunkY), Chunk.GenerateChunk());
                }
            }
        }

        private void RenderChunks()
        {
            foreach (var chunk in _chunks)
            {
                GameObject chunkObject = new GameObject("Chunk: " + chunk.Key.x + ", " + chunk.Key.y);
                chunkObject.transform.position = new Vector3(chunk.Key.x * ChunkWidth, chunk.Key.y * ChunkHeight, 0);
                
                for (int y = 0; y < chunk.Value.height; y++)
                {
                    for (int x = 0; x < chunk.Value.width; x++)
                    {
                        GameObject tileObject = TileManager.CreateTile(chunk.Value.GetTile(x, y));
                        tileObject.transform.SetParent(chunkObject.transform);
                        tileObject.transform.position = new Vector3(
                            chunkObject.transform.position.x + x - (ChunkWidth / 2),
                            chunkObject.transform.position.y + y - (ChunkHeight / 2),
                            0
                        );
                    }
                }
            }
        }
    }
}