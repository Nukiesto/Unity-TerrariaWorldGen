using System;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Generation
{
    public class WorldGenerator : MonoBehaviour
    {
        public static int ChunkWidth, ChunkHeight;
        public static int WorldWidth, WorldHeight;
        
        private static FastNoise _noise;
        private static System.Random _pseudoNoise;

        private static int _minDirtHeight;
        private static int _maxDirtHeight;
        
        [Header("World Size Options")]
        [SerializeField] private int chunkWidth = 16;
        [SerializeField] private int chunkHeight = 16;
        [SerializeField, Min(1)] private int worldWidth = 1;
        [SerializeField, Min(1)] private int worldHeight = 1;

        [Header("World Gen Options")]
        [SerializeField] private int minDirtHeight = 3;
        [SerializeField] private int maxDirtHeight = 8;

        [Header("Noise Options")]
        [SerializeField, Min(0.001f)] private float frequency = 0.2f;
        [SerializeField, Range(1, 8)] private int octaves = 2;
        [SerializeField, Range(0.5f, 8f)] private float lacunarity = 2f;
        [SerializeField, Range(0.005f, 4f)] private float gain = 0.2f;

        private static Dictionary<Vector2Int, Chunk> _chunks;
        private List<GameObject> _createdChunks = new List<GameObject>();

        private void Awake()
        {
            ChunkWidth = chunkWidth;
            ChunkHeight = chunkHeight;
            WorldWidth = worldWidth;
            WorldHeight = worldHeight;

            _minDirtHeight = minDirtHeight;
            _maxDirtHeight = maxDirtHeight;
        }

        // Temporary to allow for quick generation of terrain in play mode
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                foreach (GameObject chunk in _createdChunks)
                {
                    Destroy(chunk);
                }
                _createdChunks = new List<GameObject>();

                int seed = DateTime.Now.Millisecond.GetHashCode();
                _noise = new FastNoise(seed);
                _noise.SetNoiseType(FastNoise.NoiseType.Perlin);
                _noise.SetFrequency(frequency);
                _noise.SetFractalOctaves(octaves);
                _noise.SetFractalLacunarity(lacunarity);
                _noise.SetFractalGain(gain);
                
                _pseudoNoise = new System.Random(seed);
                
                GenerateChunks();
                RenderChunks();
            }
        }

        private void GenerateChunks()
        {
            _chunks = new Dictionary<Vector2Int, Chunk>();
            
            for (int chunkY = 0; chunkY > -worldHeight; chunkY--)
            {
                for (int chunkX = 0; chunkX < worldWidth; chunkX++)
                {
                    _chunks.Add(new Vector2Int(chunkX, chunkY), Chunk.GenerateChunk(chunkX, chunkY));
                }
            }
        }

        private void RenderChunks()
        {
            foreach (var chunk in _chunks)
            {
                GameObject chunkObject = new GameObject("Chunk: " + chunk.Key.x + ", " + chunk.Key.y);
                chunkObject.transform.position = new Vector3(chunk.Key.x * ChunkWidth, chunk.Key.y * ChunkHeight, 0);
                _createdChunks.Add(chunkObject);
                
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

        public static Chunk GetChunk(int x, int y)
        {
            return GetChunk(new Vector2Int(x, y));
        }
        
        public static Chunk GetChunk(Vector2Int pos)
        {
            return _chunks[pos];
        }

        public static float GetNoise(int x, int y)
        {
            float noise = _noise.GetNoise(x, y);
            
            // Remaps the range of noise from (-1, 1) to (0, chunkHeight)
            float normal = (noise + 1) / 2;
            return ChunkHeight * normal;
        }

        public static int GetDirtHeight()
        {
            return _pseudoNoise.Next(_minDirtHeight, _maxDirtHeight);
        }

        public static int GetRandom(int min, int max)
        {
            return _pseudoNoise.Next(min, max);
        }
    }
}