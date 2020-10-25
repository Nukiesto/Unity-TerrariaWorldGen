using UnityEngine;

namespace Game.Tiles.Tiles
{
    public class GoldOreTile : Tile
    {
        public override void SetDefaults()
        {
            Name = "Gold Ore";
            Sprite = Resources.Load<Sprite>("Tiles/Gold Ore");
            VeinSize = 1;
            Rarity = 0.02f;
            
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