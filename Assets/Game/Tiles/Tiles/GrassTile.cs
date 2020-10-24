using UnityEngine;

namespace Game.Tiles.Tiles
{
    public class GrassTile : Tile
    {
        public override void SetDefaults()
        {
            Name = "Grass";
            Sprite = Resources.Load<Sprite>("Tiles/Grass.png");
            
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