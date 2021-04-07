using Unity.Mathematics;
using UnityEngine;

namespace VoxelTerrain.Voxel.InfoData
{
    public class WorldInfo : MonoBehaviour
    {
        [SerializeField] private float _distance;
        [SerializeField] private Transform _origin;
        [SerializeField] private float _groundLevel;
        [SerializeField] private int _seed;
        
        public float Distance => _distance;
        public Transform Origin => _origin;
        public float GroundLevel => _groundLevel;
        public int Seed => _seed;
        public SimplexNoise.OpenSimplex2S SimplexAltitude => new SimplexNoise.OpenSimplex2S((uint) Seed);
        public SimplexNoise.OpenSimplex2S SimplexMoisture => new SimplexNoise.OpenSimplex2S((uint) Seed + 1000);
    }
}
