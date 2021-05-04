using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using UnityEngine;
using Debug = UnityEngine.Debug;
using Random = UnityEngine.Random;

namespace TerrainData
{
    [System.Serializable]
    public class TreeWrapper
    {
        public GameObject[] treeObjects;
    }

    public class TreeSpawner : MonoBehaviour
    {
        private VoxelTerrain.Voxel.VoxelEngine _voxelEngine => FindObjectOfType<VoxelTerrain.Voxel.VoxelEngine>();
        private VoxelTerrain.Voxel.InfoData.WorldInfo _worldInfo => FindObjectOfType<VoxelTerrain.Voxel.InfoData.WorldInfo>();
        [SerializeField] private TreeWrapper[] treePrefabs;
        [SerializeField] private float _treeSpawnDistance;
        private int step = 8;

        private List<GameObject> _trees = new List<GameObject>();

        private List<Vector4> _toSpawn = new List<Vector4>();
        
        private bool _spawn;

        public void SpawnTree(int index, Vector3 position)
        {
            GameObject treeObject = Instantiate(treePrefabs[index].treeObjects[Random.Range(0, treePrefabs[index].treeObjects.Length)]);
            treeObject.transform.position = position;
            treeObject.transform.rotation = Quaternion.Euler(0, Random.Range(0,360), 0);
            _trees.Add(treeObject);
        }

        private void Despawn()
        {
            foreach (var tree in _trees)
            {
                if (Vector3.Distance(tree.transform.position, _voxelEngine.Position) > _treeSpawnDistance) tree.SetActive(false);
            }
        }

        private void Spawn()
        {
            foreach (var tree in _trees)
            {
                if (Vector3.Distance(tree.transform.position, _voxelEngine.Position) <= _treeSpawnDistance) tree.SetActive(true);
            }
        }

        private void Generate()
        {
            if (_toSpawn.Count <= 0) return;
            var pos = _toSpawn.First();
            _toSpawn.Remove(pos);

            var tree = Instantiate(
                treePrefabs[(int) pos.w].treeObjects[Random.Range(0, treePrefabs[(int) pos.w].treeObjects.Length)],
                pos, Quaternion.Euler(0, Random.Range(0, 360), 0));

            tree.transform.parent = transform;

            _trees.Add(tree);

            if (_toSpawn.Count <= 0) _spawn = true;
        }

        private void LateUpdate()
        {
            switch (Vector3.Distance(transform.position, _voxelEngine.Position) > _treeSpawnDistance)
            {
                case true:
                    Despawn();
                    break;
                case false:
                    if (_spawn) Spawn();
                    else Generate();
                    break;
            }
        }

        public void StartSpawn()
        {
            for (var x = Random.Range(0,10); x <= _voxelEngine.ChunkSize; x += step + Random.Range(-4, 4))
            {
                for (var z = Random.Range(0, 5); z <= _voxelEngine.ChunkSize; z += step + Random.Range(-4, 4))
                {
                    var position = new Vector3(transform.position.x + x, 32, transform.position.z + z);

                    RaycastHit hit;
                    if (Physics.Raycast(position, -Vector3.up, out hit))
                    {
                        Collider[] hitColliders = Physics.OverlapSphere(hit.point, 2, -10);
                        if (hitColliders.Length == 0)
                        {
                            VoxelTerrain.Voxel.VoxelType voxelType = _voxelEngine.GetVoxelFromWorld(new Vector3(hit.point.x, hit.point.y - 1, hit.point.z));
                            Debug.Log(voxelType);
                            if (voxelType == VoxelTerrain.Voxel.VoxelType.PineForest)
                            {
                                _toSpawn.Add(new Vector4(hit.point.x, hit.point.y, hit.point.z, 0));
                                //SpawnTree(0, hit.point);
                            }
                            else if (voxelType == VoxelTerrain.Voxel.VoxelType.Forest)
                            {
                                _toSpawn.Add(new Vector4(hit.point.x, hit.point.y, hit.point.z, 1));
                                //SpawnTree(1, hit.point);
                            }
                            else if (voxelType == VoxelTerrain.Voxel.VoxelType.SwampForest)
                            {
                                _toSpawn.Add(new Vector4(hit.point.x, hit.point.y, hit.point.z, 2));
                                //SpawnTree(2, hit.point);
                            }
                            else if (voxelType == VoxelTerrain.Voxel.VoxelType.JungleForest)
                            {
                                _toSpawn.Add(new Vector4(hit.point.x, hit.point.y, hit.point.z, 3));
                                //SpawnTree(3, hit.point);
                            }
                            else if (voxelType == VoxelTerrain.Voxel.VoxelType.SavannahForest)
                            {
                                _toSpawn.Add(new Vector4(hit.point.x, hit.point.y, hit.point.z, 4));
                                //SpawnTree(4, hit.point);
                            }
                        }
                    }
                }
            }
        }

        // Start is called before the first frame update
        void Start()
        {
            
        }

        // Update is called once per frame
        void Update()
        {
            
        }
    }
}