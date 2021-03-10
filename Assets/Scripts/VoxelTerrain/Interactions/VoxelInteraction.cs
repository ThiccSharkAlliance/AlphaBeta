using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VoxelTerrain.Grid;
using VoxelTerrain.SaveLoad;
using VoxelTerrain.Voxel;
using VoxelTerrain.Voxel.Dependencies;

namespace VoxelTerrain.Mouse
{
    public class VoxelInteraction : MonoBehaviour
    {
        [SerializeField] private VoxelEngine _engine;
        [SerializeField] private VoxelType _setVoxelType;
        [SerializeField] private InteractionSettings _interactionSettings;
        [SerializeField] private FlattenShape _shape = FlattenShape.Single;
        [SerializeField] private ChunkLoader _chunkLoader;
        
        private float _offset = 0;
        
        public FlattenShape Shape
        {
            get => _shape;
            private set => _shape = value;
        }

        private Camera CamMain => Camera.main;
        private float Size => _engine.ChunkInfo.VoxelSize;

        public void SetShape(FlattenShape shape) => Shape = shape;

        public void DestroyVoxel()
        {
            var ray = CamMain.ViewportPointToRay(CamMain.ScreenToViewportPoint(Input.mousePosition));
        
            if (!Physics.Raycast(ray, out var hit)) return;

            var hitPos = GridSnapper.SnapToGrid(hit.point, Size, _offset);

            hitPos.y -= Size;
        
            StartCoroutine(UpdateChunks(hitPos, VoxelType.Default));
        }
    
        public void CreateVoxel()
        {
            var ray = CamMain.ViewportPointToRay(CamMain.ScreenToViewportPoint(Input.mousePosition));
        
            if (!Physics.Raycast(ray, out RaycastHit hit)) return;

            var hitPos = GridSnapper.SnapToGrid(hit.point, Size, _offset);
        
            StartCoroutine(UpdateChunks(hitPos, _setVoxelType));
        }

        private IEnumerator UpdateChunks(Vector3 hitPos, VoxelType voxelType)
        {
            Vector3 chunkPos;
            Chunk chunk;
            Vector3 voxPos;
            Vector3 newChunkPos;
            Vector3 newHitPos;
            List<Chunk> chunkList;
            List<Vector3> posList;
            switch (_shape)
            {
                case FlattenShape.Single:
                    chunkList = new List<Chunk>();
                    posList = new List<Vector3>();
                    for (var i = hitPos.x - _interactionSettings.MouseSize; i <= hitPos.x + _interactionSettings.MouseSize; i += Size)
                    {
                        for (var j = hitPos.z - _interactionSettings.MouseSize; j <= hitPos.z + _interactionSettings.MouseSize; j += Size)
                        {
                            newChunkPos = new Vector3(i, hitPos.y, j);
                            chunkPos = _engine.NearestChunk(newChunkPos);
                            chunk = _engine.WorldData.GetChunkAt(chunkPos);

                            if (chunk == null) continue;
                            
                            if (!chunkList.Contains(chunk))
                            {
                                chunkList.Add(chunk);
                                posList.Add(chunkPos);
                            }
                            
                            if (!InRange(newChunkPos, hitPos, _interactionSettings.MouseSize)) continue;

                            voxPos = (newChunkPos - chunkPos) / Size;
                            newHitPos = hitPos;
                            newHitPos.x = i;
                            newHitPos.z = j;
                            Sphere(hitPos, voxPos, newHitPos, _interactionSettings.MouseSize, voxelType, chunk);
                            yield return null;
                        }
                    }
                    for (int i = 0; i < chunkList.Count; i++)
                    {
                        chunkList[i].SetMesh(posList[i]);
                        if (_chunkLoader) _chunkLoader.SaveChunk(chunkList[i], new ChunkId(posList[i].x, posList[i].y, posList[i].z));
                        yield return null;
                    }
                    break;
                case FlattenShape.Square:
                    chunkList = new List<Chunk>();
                    posList = new List<Vector3>();
                    for (float i = hitPos.x - _interactionSettings.CubeXDistance; i <= hitPos.x + _interactionSettings.CubeXDistance; i += Size)
                    {
                        for (float j = hitPos.z - _interactionSettings.CubeZDistance; j <= hitPos.z + _interactionSettings.CubeZDistance; j += Size)
                        {
                            newChunkPos = new Vector3(i, hitPos.y, j);
                            chunkPos = _engine.NearestChunk(newChunkPos);
                            chunk = _engine.WorldData.GetChunkAt(chunkPos);

                            if (chunk == null) continue;
                            
                            if (!chunkList.Contains(chunk))
                            {
                                chunkList.Add(chunk);
                                posList.Add(chunkPos);
                            }

                            voxPos = (newChunkPos - chunkPos) / Size;
                            
                            Flatten(voxPos, voxelType, _interactionSettings.Height, _interactionSettings.Dig, chunk);
                            yield return null;
                        }
                    }
                    
                    for (int i = 0; i < chunkList.Count; i++)
                    {
                        chunkList[i].SetMesh(posList[i]);
                        if (_chunkLoader) _chunkLoader.SaveChunk(chunkList[i], new ChunkId(posList[i].x, posList[i].y, posList[i].z));
                        yield return null;
                    }

                    break;
                case FlattenShape.Circular:
                    chunkList = new List<Chunk>();
                    posList = new List<Vector3>();
                    for (float i = hitPos.x - _interactionSettings.CircleRadius; i <= hitPos.x + _interactionSettings.CircleRadius; i += Size)
                    {
                        for (float j = hitPos.z - _interactionSettings.CircleRadius; j <= hitPos.z + _interactionSettings.CircleRadius; j += Size)
                        {
                            newChunkPos = new Vector3(i, hitPos.y, j);
                            chunkPos = _engine.NearestChunk(newChunkPos);
                            chunk = _engine.WorldData.GetChunkAt(chunkPos);

                            if (chunk == null) continue;
                            
                            if (!chunkList.Contains(chunk))
                            {
                                chunkList.Add(chunk);
                                posList.Add(chunkPos);
                            }

                            if (!InRange(newChunkPos, hitPos, _interactionSettings.CircleRadius)) continue;
                        
                            voxPos = (newChunkPos - chunkPos) / Size;

                            Flatten(voxPos, voxelType, _interactionSettings.Height, _interactionSettings.Dig, chunk);
                            yield return null;
                        }
                    }

                    for (int i = 0; i < chunkList.Count; i++)
                    {
                        chunkList[i].SetMesh(posList[i]);
                        if (_chunkLoader) _chunkLoader.SaveChunk(chunkList[i], new ChunkId(posList[i].x, posList[i].y, posList[i].z));
                        yield return null;
                    }
                    break;
                case FlattenShape.Sphere:
                    chunkList = new List<Chunk>();
                    posList = new List<Vector3>();
                    for (float i = hitPos.x - _interactionSettings.SphereRadius; i <= hitPos.x + _interactionSettings.SphereRadius; i += Size)
                    {
                        for (float j = hitPos.z - _interactionSettings.SphereRadius; j <= hitPos.z + _interactionSettings.SphereRadius; j += Size)
                        {
                            newChunkPos = new Vector3(i, hitPos.y, j);
                            chunkPos = _engine.NearestChunk(newChunkPos);
                            chunk = _engine.WorldData.GetChunkAt(chunkPos);

                            if (chunk == null) continue;
                            
                            if (!chunkList.Contains(chunk))
                            {
                                chunkList.Add(chunk);
                                posList.Add(chunkPos);
                            }
                            
                            if (!InRange(newChunkPos, hitPos, _interactionSettings.SphereRadius)) continue;

                            voxPos = (newChunkPos - chunkPos) / Size;
                            newHitPos = hitPos;
                            newHitPos.x = i;
                            newHitPos.z = j;
                            Sphere(hitPos, voxPos, newHitPos, _interactionSettings.SphereRadius, voxelType, chunk);
                            yield return null;
                        }
                    }

                    for (int i = 0; i < chunkList.Count; i++)
                    {
                        chunkList[i].SetMesh(posList[i]);
                        if (_chunkLoader) _chunkLoader.SaveChunk(chunkList[i], new ChunkId(posList[i].x, posList[i].y, posList[i].z));
                        yield return null;
                    }
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private bool InRange(Vector3 pos, Vector3 origin, float distance) => Vector3.Distance(origin, pos) <= distance;

        private void Flatten(Vector3 pos, VoxelType voxelType, float flattenHeight, float digDepth, Chunk chunk)
        {
            Vector3 voxPos = pos;

            do
            {
                if (chunk[voxPos.x, voxPos.y, voxPos.z] == (byte)voxelType) break;
                chunk.SetVoxel(voxPos, voxelType);
                voxPos.y++;
            } while (Vector3.Distance(pos, voxPos) <= flattenHeight);

            voxPos = pos;

            do
            {
                if (chunk[voxPos.x, voxPos.y, voxPos.z] == (byte)voxelType) break; 
                chunk.SetVoxel(voxPos, voxelType);
                voxPos.y--;
            } while (Vector3.Distance(pos, voxPos) <= digDepth);
        }

        private void Sphere(Vector3 origin, Vector3 pos, Vector3 newPos, float sphereRadius, VoxelType voxelType, Chunk chunk)
        {
            Vector3 voxPos = pos;
            Vector3 newAdjust = newPos;

            do
            {
                chunk.SetVoxel(voxPos, voxelType);
                voxPos.y++;
                newAdjust.y++;
            } while (Vector3.Distance(origin, newAdjust) <= sphereRadius);

            voxPos = pos;
            newAdjust = newPos;

            do
            {
                chunk.SetVoxel(voxPos, voxelType);
                voxPos.y--;
                newAdjust.y--;
            } while (Vector3.Distance(origin, newAdjust) <= sphereRadius);
        }
    }
}
