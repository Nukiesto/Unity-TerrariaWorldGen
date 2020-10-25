using UnityEngine;

namespace Game.Tiles
{
    public abstract class Tile
    {
        public int Id = -1;
        public string Name;
        public Sprite Sprite;

        public int VeinSize;
        public float Rarity;

        public virtual void SetDefaults()
        {
            Id = TileManager.AddTile(this);
        }
        public abstract void OnHit();
        public abstract void OnDestroy();
    }
}