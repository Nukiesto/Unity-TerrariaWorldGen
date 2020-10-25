using System;
using System.Collections.Generic;
using Game.Generation;
using Game.Tiles;
using UnityEngine;

namespace Game.Core
{
    public class WorldRenderer : MonoBehaviour
    {
        [SerializeField] private Transform target = null;
        [SerializeField, Min(8)] private int renderDistance = 16;

        private Dictionary<Vector2Int, TileObject> _renderedTiles = new Dictionary<Vector2Int, TileObject>();
        private readonly Queue<TileObject> _inactiveTiles = new Queue<TileObject>();

        private void Awake()
        {
            if (target == null)
                target = Camera.main.transform;
            
            // Create all the tiles that will be needed for the rendering - used for object pooling
            GameObject tileHolder = new GameObject("Tiles");
            int requiredTiles = (int) Mathf.Pow((renderDistance + 1) * 2, 2);
            for (int i = 0; i < requiredTiles; i++)
            {
                GameObject tileGameObject = new GameObject("Tile");
                tileGameObject.transform.SetParent(tileHolder.transform);
                tileGameObject.transform.position = new Vector3(0, 0, 0);

                tileGameObject.AddComponent<SpriteRenderer>();
                TileObject tileObject = tileGameObject.AddComponent<TileObject>();
                _inactiveTiles.Enqueue(tileObject);
            }
        }

        private void Update()
        {
            // TODO: Temporary, used to quickly generate world without re-entering play mode
            if (Input.GetKeyDown(KeyCode.Space))
            {
                foreach (Vector2Int tilePos in _renderedTiles.Keys)
                {
                    TileObject tile = _renderedTiles[tilePos];
                    // "De-activate" the tile/gameObject
                    _inactiveTiles.Enqueue(tile);
                    tile.gameObject.SetActive(false);
                }

                _renderedTiles.Clear();
            }
        }

        private void LateUpdate()
        {
            Vector2Int targetPos = new Vector2Int((int) target.position.x, (int) target.position.y);
            Dictionary<Vector2Int, TileObject> newRenderedTiles = new Dictionary<Vector2Int, TileObject>(_renderedTiles);
            foreach (Vector2Int tilePos in _renderedTiles.Keys)
            {
                if (tilePos.x > targetPos.x + renderDistance || tilePos.x < targetPos.x - renderDistance ||
                    tilePos.y > targetPos.y + renderDistance || tilePos.y < targetPos.y - renderDistance)
                {
                    TileObject tile = _renderedTiles[tilePos];
                    newRenderedTiles.Remove(tilePos);
                    
                    // "De-activate" the tile/gameObject
                    _inactiveTiles.Enqueue(tile);
                    tile.gameObject.SetActive(false);
                }
            }

            _renderedTiles = newRenderedTiles;
            
            for (int x = targetPos.x - renderDistance; x < targetPos.x + renderDistance; x++)
            {
                for (int y = targetPos.y - renderDistance; y < targetPos.y + renderDistance; y++)
                {
                    Vector2Int position = new Vector2Int(x, y);
                    if (_renderedTiles.ContainsKey(position)) continue;
                    if (x < 0 || x >= WorldGen.WorldWidth || y < 0  || y >= WorldGen.WorldHeight) continue;

                    Tile tile = WorldGen.GetTile(x, y);
                    TileObject tileObject = _inactiveTiles.Dequeue();
                    tileObject.UpdateTile(tile);
                    tileObject.transform.position = new Vector3(x, y, 0);
                    tileObject.gameObject.SetActive(true);

                    _renderedTiles.Add(position, tileObject);
                }
            }
        }
    }
}