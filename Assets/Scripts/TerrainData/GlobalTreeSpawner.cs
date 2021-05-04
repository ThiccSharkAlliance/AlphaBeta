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
    public class GlobalTreeSpawner : MonoBehaviour
    {
        private VoxelTerrain.Voxel.VoxelEngine _voxelEngine => FindObjectOfType<VoxelTerrain.Voxel.VoxelEngine>();
        private VoxelTerrain.Voxel.InfoData.WorldInfo _worldInfo => FindObjectOfType<VoxelTerrain.Voxel.InfoData.WorldInfo>();
        [SerializeField] private TreeWrapper[] treePrefabs;
        [SerializeField] private float _treeSpawnDistance;

        private List<GameObject> _trees = new List<GameObject>();

        private List<Vector4> _toSpawn = new List<Vector4>();

        public void SpawnTree(int index, Vector3 position)
        {
            GameObject treeObject = Instantiate(treePrefabs[index].treeObjects[Random.Range(0, treePrefabs[index].treeObjects.Length)]);
            treeObject.transform.position = position;
            treeObject.transform.rotation = Quaternion.Euler(0, Random.Range(0, 360), 0);
            _trees.Add(treeObject);
        }

        private void Despawn()
        {
            if (_trees.Count > 0)
            {
                for (int i = _trees.Count - 1; i >= 0; i--)
                {
                    if (Vector3.Distance(_trees[i].transform.position, new Vector3(_voxelEngine.Position.x, _trees[i].transform.position.y, _voxelEngine.Position.z)) > _treeSpawnDistance)
                    {
                        GameObject treeToDestroy = _trees[i];
                        _trees.Remove(_trees[i]);
                        Destroy(treeToDestroy);
                    }

                    if (_trees.Count == 0)
                    {
                        return;
                    }
                }
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

            if (_trees.Count >= 50)
            {
                _toSpawn.Clear();
                return;
            }

            var tree = Instantiate(
                treePrefabs[(int)pos.w].treeObjects[Random.Range(0, treePrefabs[(int)pos.w].treeObjects.Length)],
                pos, Quaternion.Euler(0, Random.Range(0, 360), 0));

            tree.transform.parent = transform;

            _trees.Add(tree);
        }

        private void LateUpdate()
        {
            Despawn();
            Generate();
        }

        public void StartSpawn()
        {
            var rndPos = Random.insideUnitCircle * _treeSpawnDistance;
            Vector3 snappedPos = VoxelTerrain.Grid.GridSnapper.SnapToGrid(new Vector3(_voxelEngine.Position.x + rndPos.x, 32, _voxelEngine.Position.z + rndPos.y));
            var position = new Vector3(snappedPos.x, 32, snappedPos.z);

            RaycastHit hit;
            if (Physics.Raycast(position, -Vector3.up, out hit))
            {
                Collider[] hitColliders = Physics.OverlapSphere(hit.point, Random.Range(5, 8), -10);
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

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            StartSpawn();
        }
    }
}