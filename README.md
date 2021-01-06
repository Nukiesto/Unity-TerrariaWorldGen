# Unity Terraria-like World Gen [Abandoned]
Easily extendable Unity Terraria-like world generation

## Features
- Tile and block management
- Easily modifiable generation
- World serialization
  - Save and load your worlds
- World rendering only around the player

## Extending generation
To add your own generation steps to the overall world generation create a new class that inherits GenPass
```csharp
using Game.Core;
using Game.Tiles;
using Game.Generation.GenTasks

public class TerrainGenTask : GenTask
{
    // Required method to add your generation to the list of tasks
    public override void ModifyWorldGenTasks(List<GenPass> tasks)
    {
        // You will need to add your main function to the list of tasks as a GenPass object
        tasks.Insert(0, new GenPass("Custom Generation", CustomGeneration));
    }
    
    // Your main function must return a bool
    private bool CustomGeneration() {
        // Add your generation here
    }
}
```

## Creating tiles
You can easily add your own tiles to use throughout your world generation
```csharp
using Game.Tiles.Tile;

public class CustomTile : Tile
    {
        public override void SetDefaults()
        {
            Name = "CustomTile";
            Sprite = Resources.Load<Sprite>("Tiles/CustomSprite"); // Load the sprite from the resources folder
            
            // Sets the tile's Id and adds it to the TileManager
            base.SetDefaults();
        }

        public override void OnHit()
        {
            // Custom behaviour for when the block is hit
        }

        public override void OnDestroy()
        {
            // Custom hevaiour for when the block is destroyed
        }
    }
```

### Using the tile in a generation pass
With a tile created, you can now use it in your world.
```csharp
public class TerrainGenTask : GenTask
    {
        public override void ModifyWorldGenTasks(List<GenPass> tasks)
        {
            // ...
        }

        private bool TerrainGeneration()
        {
            // ...
            
            WorldGen.SetTile(x, y, TileManager.GetTile("CustomTile").Id);
            
            // ...
        }
    }
```
