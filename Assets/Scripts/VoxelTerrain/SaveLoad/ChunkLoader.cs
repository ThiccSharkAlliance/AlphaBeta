using System;
using System.IO;
using Newtonsoft.Json;
using UnityEngine;
using VoxelTerrain.Voxel;
using VoxelTerrain.Voxel.Dependencies;

namespace VoxelTerrain.SaveLoad
{
    public class ChunkLoader : MonoBehaviour
    {
        [SerializeField] private string _chunkDirectoryName = "chunks";
        [SerializeField] private bool _enableSaving;
        private string _chunkDirectory;

        private void Awake()
        {
            _chunkDirectory = Application.persistentDataPath + "/" + _chunkDirectoryName + "/";

            if (!Directory.Exists(_chunkDirectory)) Directory.CreateDirectory(_chunkDirectory);
            
#if UNITY_EDITOR

            _chunkDirectory = Application.dataPath + "/" + _chunkDirectoryName + "/";
#endif
        }

        public Chunk LoadChunkAt(Vector3 worldOrigin)
        {
            var fileName = worldOrigin.ToString();

            var fullPath = _chunkDirectory + fileName + ".json";
            if (!File.Exists(fullPath)) return null;

            var fileContents = File.ReadAllText(fullPath);

            var chunk = JsonConvert.DeserializeObject<Chunk>(fileContents);

            if (chunk.Voxels.Length == 0)
            {
                File.Delete(fullPath);
                return null;
            }
            
            Debug.Log("Loaded From: " + fullPath);

            return chunk;
        }

        public void SaveChunk(Chunk chunk, ChunkId chunkId)
        {
            if (!_enableSaving) return;

            if (chunk.Voxels.Length == 0) return;

            var worldOrigin = new Vector3(chunkId.X, chunkId.Y, chunkId.Z);

            var fileName = worldOrigin.ToString();

            var fullPath = _chunkDirectory + fileName + ".json";

            var json = JsonConvert.SerializeObject(chunk);
            
            File.WriteAllText(fullPath, json);
        }
    }
}
