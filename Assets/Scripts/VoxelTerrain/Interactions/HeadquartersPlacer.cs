using System;
using System.Collections;
using UnityEngine;
using VoxelTerrain.Mouse;
using VoxelTerrain.Voxel;
using VoxelTerrain.Voxel.Dependencies;

namespace VoxelTerrain.Interactions
{
    public class HeadquartersPlacer : MonoBehaviour
    {
        [SerializeField] private VoxelEngine _engine;
        [SerializeField] private VoxelInteraction _interaction;

        private void Awake()
        {
            StartCoroutine(SpawnHeadquarters());
        }

        private IEnumerator SpawnHeadquarters()
        {
            var chunkPoint = Vector3.zero;

            chunkPoint = _engine.NearestChunk(chunkPoint);

            Chunk chunk;
            var chunkId = new ChunkId(chunkPoint.x, chunkPoint.y, chunkPoint.z);

            do
            {
                chunk = _engine.ChunkAt(chunkId, false);
                yield return null;
            } while (chunk == null);

            var xz = Chunk.ChunkSize / 2;
            
            int yCounter = 0;

            byte voxel = chunk[xz, yCounter, xz];

            while (voxel != 0)
            {
                yCounter++;
                voxel = chunk[xz, yCounter, xz];
                yield return null;
            }

            transform.position = new Vector3(xz,chunkPoint.y + yCounter, xz);
            var pos = transform.position;
            pos.y -= 1;

            StartCoroutine(_interaction.UpdateChunks(pos));
        }
    }
}
