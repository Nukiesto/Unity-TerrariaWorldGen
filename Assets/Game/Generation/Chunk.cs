namespace Game.Generation
{
    public class Chunk
    {
        public readonly int width;
        public readonly int height;

        private int[,] _tileMap;

        private Chunk()
        {
            width = WorldGenerator.ChunkWidth;
            height = WorldGenerator.ChunkHeight;
            
            _tileMap = new int[width, height];
        }

        public static Chunk GenerateChunk()
        {
            Chunk chunk = new Chunk();
            
            for (int y = 0; y < chunk.height; y++)
            {
                for (int x = 0; x < chunk.width; x++)
                {
                    // TODO: Implement proper generation
                    chunk._tileMap[x, y] = 1;
                }
            }
            
            return chunk;
        }

        public int GetTile(int x, int y)
        {
            return _tileMap[x, y];
        }
    }
}