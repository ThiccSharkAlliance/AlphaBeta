using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        private int step = 8;

        public void SpawnTree(int index, Vector3 position)
        {
            GameObject treeObject = Instantiate(treePrefabs[index].treeObjects[Random.Range(0, treePrefabs[index].treeObjects.Length)]);
            treeObject.transform.position = position;
            treeObject.transform.rotation = Quaternion.Euler(0, Random.Range(0,360), 0);
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
                                SpawnTree(0, hit.point);
                            }
                            else if (voxelType == VoxelTerrain.Voxel.VoxelType.Forest)
                            {
                                SpawnTree(1, hit.point);
                            }
                            else if (voxelType == VoxelTerrain.Voxel.VoxelType.SwampForest)
                            {
                                SpawnTree(2, hit.point);
                            }
                            else if (voxelType == VoxelTerrain.Voxel.VoxelType.JungleForest)
                            {
                                SpawnTree(3, hit.point);
                            }
                            else if (voxelType == VoxelTerrain.Voxel.VoxelType.SavannahForest)
                            {
                                SpawnTree(4, hit.point);
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