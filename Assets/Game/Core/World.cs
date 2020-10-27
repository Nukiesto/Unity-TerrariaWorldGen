using System.IO;
using Game.Generation;
using Game.Serialization;
using Game.Tiles;
using UnityEngine;

namespace Game.Core
{
    public class World : MonoBehaviour
    {
        public static WorldData Data = null;
        public static WorldGenSettings GenSettings; // TODO: Move to a menu script, the menu will decide these options and pass them down

        [SerializeField] private WorldGenSettings genSettings = null;

        private void Awake()
        {
            GenSettings = genSettings;

            if (File.Exists(Application.persistentDataPath + "/world.bin"))
                LoadWorld();
            else
                GenerateNewWorld();
        }

        private static void LoadWorld()
        {
            Data = SaveLoadManager.Load<WorldData>("world.bin");
            TileManager.CreateTileSet();
        }
        
        private static void GenerateNewWorld()
        {
            TileManager.CreateTileSet();
            
            WorldGen generator = new WorldGen();
            var tileMap = generator.GenerateWorld();

            Data = new WorldData
            {
                Width = GenSettings.worldWidth,
                Height = GenSettings.worldHeight,
                TileMap = tileMap
            };
            
            if (!SaveLoadManager.Save(Data, "world.bin")) Debug.LogError("Saving Error: Unable to save");
        }
    }
}