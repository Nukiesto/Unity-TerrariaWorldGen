using UnityEngine;

namespace Game.Tiles.Tiles
{
    public class IronOreTile : Tile
    {
        public override void SetDefaults()
        {
            Name = "Iron Ore";
            Sprite = Resources.Load<Sprite>("Tiles/Iron Ore");
            VeinSize = 1;
            Rarity = 0.05f;
            
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