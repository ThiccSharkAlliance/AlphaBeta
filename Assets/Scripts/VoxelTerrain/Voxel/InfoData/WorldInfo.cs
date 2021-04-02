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
        public Transform Origin
        {
            get => _origin;
            set => _origin = value;
        }
        public float GroundLevel => _groundLevel;
        public int Seed => _seed;
        public Unity.Mathematics.Random NumGenAltitude => new Unity.Mathematics.Random((uint) Seed);
        public Unity.Mathematics.Random NumGenMoisture => new Unity.Mathematics.Random((uint) Seed + 1000);
    }
}
