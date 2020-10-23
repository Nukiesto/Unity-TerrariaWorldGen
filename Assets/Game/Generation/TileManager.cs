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
    }
}