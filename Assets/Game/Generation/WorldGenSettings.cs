using UnityEngine;

namespace Game.Generation
{
    [CreateAssetMenu(fileName = "World Gen Settings", menuName = "World Gen Settings")]
    public class WorldGenSettings : ScriptableObject
    {
        // TODO: Create private setters to prevent other scripts from changing the settings
        [Header("World Size Settings")]
        [SerializeField] public int worldWidth = 32;
        [SerializeField] public int worldHeight = 64;
        [SerializeField] public int maxSurfaceHeight = 48;
        [SerializeField] public int minSurfaceHeight = 16;
        [SerializeField] public int minDirtHeight = 8;
        
        [Header("Noise Settings")]
        [SerializeField, Min(0.001f)] public float frequency = 0.2f;
        [SerializeField, Range(1, 8)] public int octaves = 2;
        [SerializeField, Range(0.5f, 8f)] public float lacunarity = 2f;
        [SerializeField, Range(0.005f, 4f)] public float gain = 0.2f;

        [Header("Cave Settings")]
        [SerializeField, Range(0, 100)] public int cavePercentage = 50;
        [SerializeField, Min(1)] public int caveSmoothness = 3;
    }
}