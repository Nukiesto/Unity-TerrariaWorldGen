using UnityEngine;

namespace Game.Generation
{
    public class TileManager : MonoBehaviour
    {
        [SerializeField]
        private Tile[] tiles;

        private void Awake()
        {
            var loadedTiles = TileLoader.LoadTiles();
            tiles = loadedTiles;
        }
    }
}