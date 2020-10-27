using System.Collections.Generic;
using Game.Core;
using Game.Generation;
using Game.Tiles;
using UnityEngine;

namespace Game.Rendering
{
    public class WorldRenderer : MonoBehaviour
    {
        [SerializeField] private Transform target = null;
        [SerializeField, Min(8)] private int renderDistance = 16;
        
        public static Dictionary<Vector2Int, TileObject> RenderedTiles { get; private set; }
        private readonly Queue<TileObject> _inactiveTiles = new Queue<TileObject>();

        private void Awake()
        {
            if (target == null)
                target = Camera.main.transform;
            
            RenderedTiles = new Dictionary<Vector2Int, TileObject>();
            
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

        private void LateUpdate()
        {
            // Render all tiles around the target
            Vector2Int targetPos = new Vector2Int((int) target.position.x, (int) target.position.y);
            Dictionary<Vector2Int, TileObject> newRenderedTiles = new Dictionary<Vector2Int, TileObject>(RenderedTiles);
            foreach (Vector2Int tilePos in RenderedTiles.Keys)
            {
                if (tilePos.x > targetPos.x + renderDistance || tilePos.x < targetPos.x - renderDistance ||
                    tilePos.y > targetPos.y + renderDistance || tilePos.y < targetPos.y - renderDistance)
                {
                    TileObject tile = RenderedTiles[tilePos];
                    newRenderedTiles.Remove(tilePos);
                    
                    // "De-activate" the tile/gameObject
                    _inactiveTiles.Enqueue(tile);
                    tile.gameObject.SetActive(false);
                }
            }

            RenderedTiles = newRenderedTiles;
            
            for (int x = targetPos.x - renderDistance; x < targetPos.x + renderDistance; x++)
            {
                for (int y = targetPos.y - renderDistance; y < targetPos.y + renderDistance; y++)
                {
                    Vector2Int position = new Vector2Int(x, y);
                    if (RenderedTiles.ContainsKey(position)) continue;
                    if (x < 0 || x >= World.GenSettings.worldWidth || y < 0  || y >= World.GenSettings.worldHeight) continue;

                    Tile tile = World.Data.GetTile(x, y);
                    TileObject tileObject = _inactiveTiles.Dequeue();
                    tileObject.UpdateTile(tile);
                    tileObject.transform.position = new Vector3(x, y, 0);
                    tileObject.gameObject.SetActive(true);

                    RenderedTiles.Add(position, tileObject);
                }
            }
        }
    }
}