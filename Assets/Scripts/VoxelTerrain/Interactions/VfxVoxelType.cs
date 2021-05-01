using UnityEngine;
using VoxelTerrain.Voxel;

namespace VoxelTerrain.Interactions
{
    public class VfxVoxelType : MonoBehaviour
    {
        [SerializeField] private VoxelType _voxelType = VoxelType.Default;

        public VoxelType VoxelType => _voxelType;
    }
}
