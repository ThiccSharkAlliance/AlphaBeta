using VoxelTerrain.Voxel;

namespace VoxelTerrain.Interactions
{
    public static class InteractionBan
    {
        public static readonly VoxelType[] BanList = {
            VoxelType.Water,         
            VoxelType.OakBranch,
            VoxelType.OakLeaf,
            VoxelType.JungleBranch,
            VoxelType.JungleLeaf,
            VoxelType.SavannahBranch,
            VoxelType.SavannahLeaf,
            VoxelType.PineBranch,
            VoxelType.PineLeaf,
            VoxelType.SwampBranch,
            VoxelType.SwampLeaf};
    }
}
