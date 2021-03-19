using UnityEngine;

namespace VoxelTerrain.Voxel.InfoData
{
    public class WorldInfo : MonoBehaviour
    {
        [SerializeField] private float _distance;
        [SerializeField] private Transform _origin;
        [SerializeField] private float _groundLevel;
        
        public float Distance => _distance;
        public Transform Origin
        {
            get => _origin;
            set => _origin = value;
        }

        public float GroundLevel => _groundLevel;
    }
}
