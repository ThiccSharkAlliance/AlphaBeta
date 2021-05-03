using VoxelTerrain.Voxel;

namespace VoxelTerrain.Interactions
{
    public static class InteractionBan
    {
        public static readonly VoxelType[] InteractionBanList = {
            VoxelType.Water,
        };

        public static readonly VoxelType[] TreeBanList =
        {
            VoxelType.OakBranch,
            VoxelType.OakLeaf,
            VoxelType.JungleBranch,
            VoxelType.JungleLeaf,
            VoxelType.SavannahBranch,
            VoxelType.SavannahLeaf,
            VoxelType.PineBranch,
            VoxelType.PineLeaf,
            VoxelType.SwampBranch,
            VoxelType.SwampLeaf
        };
    }
}
