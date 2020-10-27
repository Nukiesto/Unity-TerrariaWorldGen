using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Assembly = System.Reflection.Assembly;

namespace Game.Tiles
{
    public static class TileManager
    {
        private static List<Tile> _tiles = new List<Tile>();
        private static Dictionary<string, int> _tileIds = new Dictionary<string, int>();

        public static void CreateTileSet()
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
            _tiles.Add(tile);
            int tileId = _tiles.Count - 1;
            _tileIds.Add(tile.Name, tileId);

            return tileId;
        }

        public static Tile GetTile(string name)
        {
            return GetTile(_tileIds[name]);
        }
        
        public static Tile GetTile(int id)
        {
            return _tiles[id];
        }
    }
}