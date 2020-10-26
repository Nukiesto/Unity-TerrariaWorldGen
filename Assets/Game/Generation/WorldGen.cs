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
        public static int CavePercentage { get; private set; }
        public static int CaveSmoothness { get; private set; }
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

        [Header("Cave Settings")]
        [SerializeField, Range(0, 100)] private int cavePercentage = 50;
        [SerializeField, Min(1)] private int caveSmoothness = 3;

        private readonly List<GenPass> _genTasks = new List<GenPass>();
        private static int[,] _tileMap;
        
        private static FastNoise _noise;
        private static System.Random _pseudoNoise;

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
            
            _pseudoNoise = new System.Random(seed);
            
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
            CavePercentage = cavePercentage;
            CaveSmoothness = caveSmoothness;
            
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
        
        public static float GetNoise(int x, int y)
        {
            return _noise.GetNoise(x, y);
        }

        public static int GetRandom(int min, int max)
        {
            return _pseudoNoise.Next(min, max);
        }

        public static int MapToRange(float from, int fromMin, int fromMax, int toMin, int toMax)
        {
            float fromAbs  =  from - fromMin;
            float fromMaxAbs = fromMax - fromMin;      
       
            float normal = fromAbs / fromMaxAbs;
 
            float toMaxAbs = toMax - toMin;
            float toAbs = toMaxAbs * normal;

            return Mathf.FloorToInt(toAbs + toMin);
        }
    }
}