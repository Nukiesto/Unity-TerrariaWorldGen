using UnityEngine;

namespace Game.Tiles
{
    public abstract class Tile
    {
        public int Id = -1;
        public string Name;
        protected Sprite Sprite;

        public virtual void SetDefaults()
        {
            Id = TileManager.AddTile(this);
        }
        public abstract void OnHit();
        public abstract void OnDestroy();
    }
}