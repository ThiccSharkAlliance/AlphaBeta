using UnityEngine;
using VoxelTerrain.SaveLoad;

namespace VoxelTerrain.Voxel.Dependencies
{
    [RequireComponent(typeof(ChunkGenerator), typeof(ChunkLoader))]
    public class WorldGenerationFunctions : MonoBehaviour
    {
        [SerializeField] private VoxelEngine _engine;
        [SerializeField] private ChunkGenerator _chunkGenerator;
        [SerializeField] private ChunkLoader _chunkLoader;

        public void GenerateWorld(Vector3 origin, float distance, float size)
        {
            _chunkGenerator.Engine = _engine;
            for (float x = origin.x - distance; x <= origin.x + distance; x += Chunk.ChunkSize)
            {
                for (float z = origin.z - distance; z <= origin.z + distance; z += Chunk.ChunkSize)
                {
                    GenerateChunkData(new Vector3(x, 0, z));
                }
            }
        }

        public Chunk GenerateChunkData(Vector3 pos)
        {
            Chunk c;

            c = _chunkLoader.LoadChunkAt(pos);

            if (c != null) return c;
            
            c = _chunkGenerator.CreateChunkJob(pos);

            return c;
        }
    }
}
