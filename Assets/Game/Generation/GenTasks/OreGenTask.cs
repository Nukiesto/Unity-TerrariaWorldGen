using System.Collections.Generic;
using Game.Core;
using Game.Tiles;

namespace Game.Generation.GenTasks
{
    public class OreGenTask : GenTask
    {
        public override void ModifyWorldGenTasks(List<GenPass> tasks)
        {
            // Ensures that the ores are generated once the caves have generated
            int caveGenIndex = tasks.FindIndex(task => task.Name.Equals("Cave Generation"));
            tasks.Insert(caveGenIndex + 1, new GenPass("Ore Generation", OreGeneration));
        }

        private bool OreGeneration()
        {
            Tile coal = TileManager.GetTile("Coal Ore");
            Tile iron = TileManager.GetTile("Iron Ore");
            Tile gold = TileManager.GetTile("Gold Ore");
            
            for (int x = 0; x < WorldGen.GenSettings.worldWidth; x += WorldGen.GetRandom(1, 5))
            {
                for (int y = 0; y < WorldGen.GenSettings.minDirtHeight; y += WorldGen.GetRandom(1, 5))
                {
                    if (WorldGen.GetTile(x, y).Name == "Dirt") continue;
                    
                    int chance = WorldGen.GetRandom(0, 1000);
                    if (chance < gold.Rarity * 1000) SpawnVein(gold, x, y);
                    else if (chance < iron.Rarity * 1000) SpawnVein(iron, x, y);
                    else if (chance < coal.Rarity * 1000) SpawnVein(coal, x, y);
                }
            }
            
            return true;
        }

        private void SpawnVein(Tile tile, int x, int y)
        {
            for (int oreX = x - tile.VeinSize; oreX <= x + tile.VeinSize; oreX++)
            {
                for (int oreY = y - tile.VeinSize; oreY <= y + tile.VeinSize; oreY++)
                {
                    if (WorldGen.GetTile(x, y).Name != "Stone") continue;

                    int shouldSpawn = WorldGen.GetRandom(0, 100);
                    if (shouldSpawn > 50) continue;
                    WorldGen.SetTile(oreX, oreY, tile.Id);
                }
            }
        }
    }
}