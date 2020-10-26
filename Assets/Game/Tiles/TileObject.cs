using System;
using UnityEngine;

namespace Game.Tiles
{
    public class TileObject : MonoBehaviour
    {
        private Tile _tile;
        public SpriteRenderer spriteRenderer;

        private void Awake()
        {
            spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        }

        public void UpdateTile(Tile newTile)
        {
            _tile = newTile;
            gameObject.name = _tile.Name;
            spriteRenderer.sprite = _tile.Sprite;
        }
    }
}