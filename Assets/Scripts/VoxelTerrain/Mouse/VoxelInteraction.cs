﻿using System;
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
        [SerializeField] private float _xRadius = 5;
        [SerializeField] private float _zRadius = 5;
        [SerializeField] private float _circleRadius = 5;
        [SerializeField] private float _flattenHeight = 2;
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
        
            UpdateChunks(hitPos, VoxelType.Default);
        }
    
        public void CreateVoxel()
        {
            var ray = CamMain.ViewportPointToRay(CamMain.ScreenToViewportPoint(Input.mousePosition));
        
            if (!Physics.Raycast(ray, out RaycastHit hit)) return;

            var hitPos = GridSnapper.SnapToGrid(hit.point, Size, _offset);
        
            UpdateChunks(hitPos, _setVoxelType);
        }

        private void UpdateChunks(Vector3 hitPos, VoxelType voxelType)
        {
            Vector3 chunkPos;
            Chunk chunk;
            Vector3 voxPos;
            Vector3 newPos;
            List<Chunk> chunkList;
            List<Vector3> posList;
            switch (_shape)
            {
                case FlattenShape.Single:
                    chunkPos = _engine.NearestChunk(hitPos);
                    chunk = _engine.WorldData.GetChunkAt(chunkPos);

                    if (chunk == null) return;

                    voxPos = (hitPos - chunkPos) / Size;
                    chunk.SetVoxel(voxPos, voxelType);
                    chunk.SetMesh(chunkPos);
                    if (_chunkLoader) _chunkLoader.SaveChunk(chunk, new ChunkId(chunkPos.x, chunkPos.y, chunkPos.z));
                    break;
                case FlattenShape.Square:
                    chunkList = new List<Chunk>();
                    posList = new List<Vector3>();
                    for (float i = hitPos.x - _xRadius; i < hitPos.x + _xRadius; i += Size)
                    {
                        for (float j = hitPos.z - _zRadius; j < hitPos.z + _zRadius; j += Size)
                        {
                            newPos = new Vector3(i, hitPos.y, j);
                            chunkPos = _engine.NearestChunk(newPos);
                            chunk = _engine.WorldData.GetChunkAt(chunkPos);

                            if (chunk == null) continue;
                            
                            if (!chunkList.Contains(chunk))
                            {
                                chunkList.Add(chunk);
                                posList.Add(chunkPos);
                            }

                            voxPos = (newPos - chunkPos) / Size;

                            if (voxelType == VoxelType.Default) chunk.SetVoxel(voxPos, voxelType);
                            else Flatten(voxPos, voxelType, chunk);
                            chunk.SetMesh(chunkPos);
                        }
                    }
                    
                    for (int i = 0; i < chunkList.Count; i++)
                    {
                        chunkList[i].SetMesh(posList[i]);
                        if (_chunkLoader) _chunkLoader.SaveChunk(chunkList[i], new ChunkId(posList[i].x, posList[i].y, posList[i].z));
                    }

                    break;
                case FlattenShape.Circular:
                    chunkList = new List<Chunk>();
                    posList = new List<Vector3>();
                    for (float i = hitPos.x - _xRadius; i < hitPos.x + _xRadius; i += Size)
                    {
                        for (float j = hitPos.z - _zRadius; j < hitPos.z + _zRadius; j += Size)
                        {
                            newPos = new Vector3(i, hitPos.y, j);
                            chunkPos = _engine.NearestChunk(newPos);
                            chunk = _engine.WorldData.GetChunkAt(chunkPos);

                            if (chunk == null) continue;
                            
                            if (!chunkList.Contains(chunk))
                            {
                                chunkList.Add(chunk);
                                posList.Add(chunkPos);
                            }

                            if (!InRange(newPos, hitPos)) continue;
                        
                            voxPos = (newPos - chunkPos) / Size;
                            if (voxelType == VoxelType.Default) chunk.SetVoxel(voxPos, voxelType);
                            else Flatten(voxPos, voxelType, chunk);
                        }
                    }

                    for (int i = 0; i < chunkList.Count; i++)
                    {
                        chunkList[i].SetMesh(posList[i]);
                        if (_chunkLoader) _chunkLoader.SaveChunk(chunkList[i], new ChunkId(posList[i].x, posList[i].y, posList[i].z));
                    }
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private bool InRange(Vector3 pos, Vector3 origin) => Vector3.Distance(origin, pos) < _circleRadius;

        private void Flatten(Vector3 pos, VoxelType voxelType, Chunk chunk)
        {
            Vector3 newPos = pos;

            do
            {
                chunk.SetVoxel(newPos, VoxelType.Default);
                newPos.y++;
            } while (Vector3.Distance(pos, newPos) <= _flattenHeight);

            newPos = pos;

            do
            {
                if (chunk[newPos.x, newPos.y, newPos.z] == (float)VoxelType.Default) chunk.SetVoxel(newPos, voxelType);
                newPos.y--;
            } while (Vector3.Distance(pos, newPos) <= _flattenHeight);
        }
    }
}
