using System.Linq;
using Game.Generation;
using Game.Tiles;
using UnityEngine;

namespace Game.Rendering
{
    public class ComputeTest : MonoBehaviour
    {
        [SerializeField] private ComputeShader shader = default;

        private void ComputeLighting()
        {
            // TODO: Only execute if the WordRender marks the RenderedTiles as dirty
            // TODO: Pass in only the RenderedTiles into tileMapBuffer
            // TODO: Filter to only pass in tiles that emit light?
            
            // Initialize data
            var tileMap = WorldGen.GetMap().Cast<int>().ToArray();
            ComputeBuffer tileMapBuffer = new ComputeBuffer(tileMap.Length, 4);
            tileMapBuffer.SetData(tileMap);

            CustomRenderTexture tex = new CustomRenderTexture(WorldRenderer.RenderBox.x, WorldRenderer.RenderBox.y)
            {
                enableRandomWrite = true, antiAliasing = 1, filterMode = FilterMode.Point
            };
            tex.Create();
            
            // Execute shader
            int kernelHandle = shader.FindKernel("CSMain");
            
            shader.SetInt("dirt", TileManager.GetTile("Dirt").Id);
            shader.SetInt("width", WorldGen.WorldWidth);
            shader.SetBuffer(kernelHandle, "tile_map", tileMapBuffer);
            shader.SetTexture(kernelHandle, "result", tex);
            // TODO: Fix, value returned is 0.804 for all tiles
            shader.Dispatch(kernelHandle,
                WorldGen.WorldWidth / 8,
                WorldGen.WorldHeight / 8, 1);
            
            tileMapBuffer.Dispose();

            RenderTexture.active = tex;
            Texture2D output = new Texture2D(WorldRenderer.RenderBox.x, WorldRenderer.RenderBox.y);
            for (int x = 0; x < WorldRenderer.RenderBox.x; x++)
            {
                for (int y = 0; y < WorldRenderer.RenderBox.y; y++)
                {
                    TileObject tileObject = WorldRenderer.RenderedTiles[new Vector2Int(x, y)];
                    if (tileObject == null) continue;

                    Color targetColor = output.GetPixel(x, y);
                    tileObject.spriteRenderer.color = new Color(
                        targetColor.r, targetColor.g, targetColor.b, 1);
                }
            }

            RenderTexture.active = null;
            tex.Release();
        }
    }
}