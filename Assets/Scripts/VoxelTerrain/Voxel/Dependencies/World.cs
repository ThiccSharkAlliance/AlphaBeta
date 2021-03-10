using System.Collections.Generic;
using TerrainData;
using UnityEngine;

namespace VoxelTerrain.Voxel.Dependencies
{
    //If you are here, leave, you're not welcome
    public class World
    {
        public Dictionary<ChunkId, Chunk> Chunks = new Dictionary<ChunkId, Chunk>();
        public Dictionary<ChunkId, GameObject> ChunkObjects = new Dictionary<ChunkId, GameObject>();
        public VoxelEngine Engine { get; set; }

        public byte this[int x, int y, int z]
        {
            get
            {
                var chunk = Chunks[ChunkId.FromWorldPos(x, y, z)];
                return chunk[x & 0xf, y & 0xf, z & 0xf];
            }

            set
            {
                var chunk = Chunks[ChunkId.FromWorldPos(x, y, z)];
                chunk[x & 0xf, y & 0xf, z & 0xf] = value;
            }
        }

        public Chunk GetChunkAt(Vector3 pos) => Chunks.ContainsKey(ChunkId.FromWorldPos(pos.x, pos.y, pos.z)) ? Chunks[ChunkId.FromWorldPos(pos.x, pos.y, pos.z)] : null;
        
        public Chunk GetNonNullChunkAt(Vector3 pos) => Chunks.ContainsKey(ChunkId.FromWorldPos(pos.x, pos.y, pos.z)) ? Chunks[ChunkId.FromWorldPos(pos.x, pos.y, pos.z)] : Engine.LoadChunkAt(new ChunkId(pos.x, pos.y, pos.z));
        
        public float GetVoxelAt(float x, float y, float z, float scale, Chunk currentChunk = null, Chunk rightChunk = null, Chunk forwardChunk = null, Chunk rightForwardChunk = null)
        {
            var chunkPos = NearestChunk(new Vector3(x, y, z), scale);
            Chunk chunk = null;
            
            if (currentChunk != null && chunkPos == currentChunk.Position) chunk = currentChunk;
            else if (rightChunk != null && chunkPos == rightChunk.Position) chunk = rightChunk;
            else if (forwardChunk != null && chunkPos == forwardChunk.Position) chunk = forwardChunk;
            else if (rightForwardChunk != null && chunkPos == rightForwardChunk.Position) chunk = rightForwardChunk;

            if (chunk == null) return SetVoxelType(x * scale, y * scale, z * scale); 

            var voxPos = (new Vector3(x, y, z) - chunkPos) / scale;
            return chunk[voxPos.x, voxPos.y, voxPos.z];
        }
        
        private Vector3 NearestChunk(Vector3 pos, float scale)
        {
            var curChunkPosX = Mathf.FloorToInt(pos.x / (Chunk.ChunkSize * scale)) * (Chunk.ChunkSize * scale);
            var curChunkPosZ = Mathf.FloorToInt(pos.z / (Chunk.ChunkSize * scale)) * (Chunk.ChunkSize * scale);

            return new Vector3(curChunkPosX, -(Chunk.ChunkHeight * scale) / 2, curChunkPosZ);
        }
        
        private int SetVoxelType(float x, float y, float z)
        {
            var blockType = VoxelType.Default;

            // noise for heightmap
            var simplex1 = Noise.Generate2DNoiseValue( x, z, Engine.NoiseScale, Engine.Seed, Engine.WorldInfo.GroundLevel);

            //under the surface, dirt block
            if (y <= simplex1)
            {
                //blockType = VoxelType.Dirt;
                blockType = VoxelType.Dirt;
            
                //just on the surface, use a grass type
                if (y > simplex1 - 1)
                {
                    blockType = VoxelType.Grass;
                }
            }

            return (int) blockType;
        }
    }
}
