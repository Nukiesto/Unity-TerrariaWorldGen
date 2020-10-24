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

        private void Awake()
        {
            if (target == null)
                target = Camera.main.transform;
        }

        // TODO: Make use of object pooling instead of instantiating and destroying each update
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
                    
                    Destroy(tile.gameObject);
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
                    GameObject tileGameObject = new GameObject(tile.Name);
                    tileGameObject.transform.position = new Vector3(x, y, 0);
            
                    SpriteRenderer spriteRenderer = tileGameObject.AddComponent<SpriteRenderer>();
                    spriteRenderer.sprite = tile.Sprite;
            
                    TileObject tileObject = tileGameObject.AddComponent<TileObject>();
                    tileObject.tile = tile;

                    _renderedTiles.Add(position, tileObject);
                }
            }
        }
    }
}