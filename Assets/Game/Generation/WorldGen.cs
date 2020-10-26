using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Game.Tiles;
using UnityEngine;

namespace Game.Generation
{
    public class WorldGen : MonoBehaviour
    {
        #region Static World Settings
        public static int WorldWidth { get; private set; }
        public static int WorldHeight { get; private set; }
        public static int MaxSurfaceHeight { get; private set; }
        public static int MinSurfaceHeight { get; private set; }
        public static int MinDirtHeight { get; private set; }
        #endregion

        [Header("World Size Settings")]
        [SerializeField] private int worldWidth = 32;
        [SerializeField] private int worldHeight = 64;
        [SerializeField] private int maxSurfaceHeight = 48;
        [SerializeField] private int minSurfaceHeight = 16;
        [SerializeField] private int minDirtHeight = 8;
        
        [Header("Noise Settings")]
        [SerializeField, Min(0.001f)] private float frequency = 0.2f;
        [SerializeField, Range(1, 8)] private int octaves = 2;
        [SerializeField, Range(0.5f, 8f)] private float lacunarity = 2f;
        [SerializeField, Range(0.005f, 4f)] private float gain = 0.2f;

        private readonly List<GenPass> _genTasks = new List<GenPass>();
        private static int[,] _tileMap;
        
        private static FastNoise _noise;
        private static System.Random _psuedoNoise;

        private void Awake()
        {
            // Initialize noise
            int seed = DateTime.Now.Millisecond.GetHashCode();
            
            _noise  = new FastNoise(seed);
            _noise.SetNoiseType(FastNoise.NoiseType.Perlin);
            _noise.SetFrequency(frequency);
            _noise.SetFractalOctaves(octaves);
            _noise.SetFractalLacunarity(lacunarity);
            _noise.SetFractalGain(gain);
            
            _psuedoNoise = new System.Random(seed);
            
            // Gets all world gen tasks, and adds them to the list
            Type taskType = typeof(GenTask);
            Assembly assembly = Assembly.GetExecutingAssembly();
            var tasks = assembly.GetTypes().Where(t => t.IsSubclassOf(taskType));

            foreach (Type task in tasks)
            {
                GenTask genTaskObject = Activator.CreateInstance(task) as GenTask;
                task.InvokeMember(
                    "ModifyWorldGenTasks",
                    BindingFlags.InvokeMethod,
                    null,
                    genTaskObject,
                    new object[] { _genTasks }
                );
            }
            
            // Assign all world settings to their static counterparts
            WorldWidth = worldWidth;
            WorldHeight = worldHeight;
            MaxSurfaceHeight = maxSurfaceHeight;
            MinSurfaceHeight = minSurfaceHeight;
            MinDirtHeight = minDirtHeight;
            
            _tileMap = new int[WorldWidth, WorldHeight];
        }

        private void Start()
        {
            // Executes all world gen tasks
            foreach (GenPass task in _genTasks)
            {
                Debug.Log(task.Name);
                task.Func();
            }
        }

        public static bool SetTile(int x, int y, int tileId)
        {
            if (x < 0 || x >= WorldWidth || y < 0 || y >= WorldHeight) return false;
            
            _tileMap[x, y] = tileId;
            return true;
        }

        public static Tile GetTile(int x, int y)
        {
            return TileManager.GetTile(_tileMap[x, y]);
        }

        public static int[,] GetMap()
        {
            return _tileMap;
        }
        
        public static int GetNoise(int x, int y)
        {
            float noise = _noise.GetNoise(x, y);
            
            // Remaps the range of noise from (-1, 1) to (MinSurfaceHeight, MaxSurfaceHeight)
            float normal = (noise + 1) / 2;
            float toMaxAbs = MaxSurfaceHeight - MinSurfaceHeight;
            float toAbs = toMaxAbs * normal;
            
            return Mathf.FloorToInt(toAbs + MinSurfaceHeight);
        }

        public static int GetRandom(int min, int max)
        {
            return _psuedoNoise.Next(min, max);
        }
    }
}