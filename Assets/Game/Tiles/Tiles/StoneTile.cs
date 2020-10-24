using UnityEngine;

namespace Game.Tiles.Tiles
{
    public class StoneTile : Tile
    {
        public override void SetDefaults()
        {
            Name = "Stone";
            Sprite = Resources.Load<Sprite>("Tiles/Stone");
            
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