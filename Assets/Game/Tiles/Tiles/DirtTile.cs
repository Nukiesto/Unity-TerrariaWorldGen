using UnityEngine;

namespace Game.Tiles.Tiles
{
    public class DirtTile : Tile
    {
        public override void SetDefaults()
        {
            Name = "Dirt";
            Sprite = Resources.Load<Sprite>("Tiles/Dirt");
            
            base.SetDefaults();
        }

        public override void OnHit()
        {
            throw new System.NotImplementedException();
        }

        public override void OnDestroy()
        {
            throw new System.NotImplementedException();
        }
    }
}