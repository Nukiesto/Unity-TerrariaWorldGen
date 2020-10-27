using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Game.Core;
using Game.Tiles;
using UnityEngine;

namespace Game.Generation
{
    public class WorldGen
    {
        private readonly List<GenPass> _genTasks = new List<GenPass>();
        private static int[,] _tileMap;
        
        private static FastNoise _noise;
        private static System.Random _pseudoNoise;

        public int[,] GenerateWorld()
        {
            _tileMap = new int[World.GenSettings.worldWidth, World.GenSettings.worldHeight];
            // Initialize noise
            int seed = DateTime.Now.Millisecond.GetHashCode();
            
            _noise  = new FastNoise(seed);
            _noise.SetNoiseType(FastNoise.NoiseType.Perlin);
            _noise.SetFrequency(World.GenSettings.frequency);
            _noise.SetFractalOctaves(World.GenSettings.octaves);
            _noise.SetFractalLacunarity(World.GenSettings.lacunarity);
            _noise.SetFractalGain(World.GenSettings.gain);
            
            _pseudoNoise = new System.Random(seed);
            
            // Gets all world gen tasks, and adds them to the list to do
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
            
            // Executes all world gen tasks
            foreach (GenPass task in _genTasks)
            {
                Debug.Log(task.Name);
                task.Func();
            }

            return _tileMap;
        }

        public static bool SetTile(int x, int y, int tileId)
        {
            if (x < 0 || x >= World.GenSettings.worldWidth || y < 0 || y >= World.GenSettings.worldHeight) return false;
            
            _tileMap[x, y] = tileId;
            return true;
        }

        public static Tile GetTile(int x, int y)
        {
            return TileManager.GetTile(_tileMap[x, y]);
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