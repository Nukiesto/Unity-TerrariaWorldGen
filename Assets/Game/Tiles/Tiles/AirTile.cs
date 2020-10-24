namespace Game.Tiles.Tiles
{
    public class AirTile : Tile
    {
        public override void SetDefaults()
        {
            Name = "Air";
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