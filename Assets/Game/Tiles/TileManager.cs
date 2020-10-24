using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;
using Assembly = System.Reflection.Assembly;

namespace Game.Tiles
{
    public class TileManager : MonoBehaviour
    {
        private static readonly List<Tile> Tiles = new List<Tile>();
        private static readonly Dictionary<string, int> TileIds = new Dictionary<string, int>();

        private void Awake()
        {
            Type tileType = typeof(Tile);
            Assembly assembly = Assembly.GetExecutingAssembly();
            var tileClasses = assembly.GetTypes().Where(t => t.IsSubclassOf(tileType));

            foreach (Type tile in tileClasses)
            {
                Tile tileObject = Activator.CreateInstance(tile) as Tile;
                tile.InvokeMember(
                    "SetDefaults",
                    BindingFlags.InvokeMethod,
                    null,
                    tileObject,
                    null
                );
            }
        }

        public static int AddTile(Tile tile)
        {
            Tiles.Add(tile);
            int tileId = Tiles.Count - 1;
            TileIds.Add(tile.Name, tileId);
                
            Debug.Log("Added tile: " + tile.Name);

            return tileId;
        }

        public static Tile GetTile(string name)
        {
            return GetTile(TileIds[name]);
        }
        
        public static Tile GetTile(int id)
        {
            return Tiles[id];
        }
    }
}