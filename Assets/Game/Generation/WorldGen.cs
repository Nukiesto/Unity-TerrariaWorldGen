using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Game.Generation.GenTasks;
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
        public static int DirtHeight { get; private set; }
        #endregion

        [SerializeField] private int worldWidth = 32;
        [SerializeField] private int worldHeight = 64;

        [SerializeField] private int maxSurfaceHeight = 48;
        [SerializeField] private int dirtHeight = 4;

        private readonly List<GenPass> _genTasks = new List<GenPass>();
        private static int[,] _tileMap;

        private void Awake()
        {
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
            DirtHeight = dirtHeight;
            
            _tileMap = new int[WorldWidth, WorldHeight];
        }

        private void Start()
        {
            // Executes all world gen tasks
            foreach (GenPass task in _genTasks)
            {
                Debug.Log(task.Name + ": " + task.Func.Method.Name);
                task.Func();
            }
        }

        public static bool SetTile(int x, int y, int tileId)
        {
            _tileMap[x, y] = tileId;
            return true;
        }

        public static Tile GetTile(int x, int y)
        {
            return TileManager.GetTile(_tileMap[x, y]);
        }
    }
}