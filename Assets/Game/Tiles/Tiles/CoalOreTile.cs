using UnityEngine;

namespace Game.Tiles.Tiles
{
    public class CoalOreTile : Tile
    {
        public override void SetDefaults()
        {
            Name = "Coal Ore";
            Sprite = Resources.Load<Sprite>("Tiles/Coal Ore");
            VeinSize = 2;
            Rarity = 0.08f;
            
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