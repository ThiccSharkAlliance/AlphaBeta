using System;
using TerrainData;
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

        public FastNoiseLite FastAltitude => new FastNoiseLite(Seed);
        public FastNoiseLite FastMoisture => new FastNoiseLite(Seed + 1000);

        // public SimplexNoise.OpenSimplex2SJobs SimplexAltitude => new SimplexNoise.OpenSimplex2SJobs((uint) Seed);
        // public SimplexNoise.OpenSimplex2SJobs SimplexMoisture => new SimplexNoise.OpenSimplex2SJobs((uint) Seed + 1000);

        // private void OnApplicationQuit()
        // {
        //     Dispose();
        // }
        //
        // public void Dispose()
        // {
        //     SimplexAltitude.Dispose();
        //     SimplexMoisture.Dispose();
        // }
    }
}
