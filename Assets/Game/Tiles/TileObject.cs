using System;
using UnityEngine;

namespace Game.Tiles
{
    public class TileObject : MonoBehaviour
    {
        private Tile _tile;
        private SpriteRenderer _spriteRenderer;

        private void Awake()
        {
            _spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        }

        public void UpdateTile(Tile newTile)
        {
            _tile = newTile;
            gameObject.name = _tile.Name;
            _spriteRenderer.sprite = _tile.Sprite;
        }
    }
}