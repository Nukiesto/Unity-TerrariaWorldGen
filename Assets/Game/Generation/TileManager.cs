using System;
using System.Linq;
using UnityEngine;

namespace Game.Generation
{
    public class TileManager : MonoBehaviour
    {
        // TODO: Make it readonly, and only assignable once
        public static Tile[] Tiles;

        private void Awake()
        {
            var loadedTiles = TileLoader.LoadTiles();
            Tiles = loadedTiles;
        }

        public static GameObject CreateTile(int id)
        {
            int tileIndex = Array.FindIndex(Tiles, check => check.id == id);
            if (tileIndex == -1)
            {
                Debug.LogError("[ERROR] Cannot spawn tile");
                return null;
            }

            Tile tile = Tiles[tileIndex];
            GameObject tileObject = new GameObject(tile.name);
            SpriteRenderer sr = tileObject.AddComponent<SpriteRenderer>();
            sr.sprite = tile.sprite;

            return tileObject;
        }

        public static int GetTileIdByName(string name)
        {
            int tileIndex = Array.FindIndex(Tiles, check => check.name == name);
            return tileIndex == -1 ? -1 : Tiles[tileIndex].id;
        }

        public static Tile GetTileByName(string name)
        {
            int tileIndex = Array.FindIndex(Tiles, check => check.name == name);
            return tileIndex == -1 ? null : Tiles[tileIndex];
        }
    }
}