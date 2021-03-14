﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.VFX;
using VoxelTerrain.Grid;
using VoxelTerrain.Mouse;
using VoxelTerrain.SaveLoad;
using VoxelTerrain.Voxel;
using VoxelTerrain.Voxel.Dependencies;

namespace VoxelTerrain.Interactions
{
    public class VoxelInteraction : MonoBehaviour
    {
        [SerializeField] private VoxelEngine _engine;
        [SerializeField] private VoxelType _setVoxelType;
        [Tooltip("Overrides voxel type, will destroy all voxels above position. Voxel type still sets at and below position")]
        [SerializeField] private bool _destroyAboveGround;
        [SerializeField] private InteractionSettings _interactionSettings;
        [SerializeField] private FlattenShape _shape = FlattenShape.Single;
        [SerializeField] private ChunkLoader _chunkLoader;
        [SerializeField] private VfxInteraction[] _interactionEvents;
        
        private float _offset = 0;
        
        public FlattenShape Shape
        {
            get => _shape;
            private set => _shape = value;
        }
        private VoxelType Voxel
        {
            get => _setVoxelType;
            set => _setVoxelType = value;
        }
        private Camera CamMain => Camera.main;
        private float Size => _engine.ChunkInfo.VoxelSize;

        public void SetShape(FlattenShape shape) => Shape = shape;
        public void VfxSwitch(VisualEffect thing) => thing.enabled = !thing.enabled;

        public void SetVoxelType(VoxelType type) => Voxel = type;

        public void EditVoxels()
        {
            var ray = CamMain.ViewportPointToRay(CamMain.ScreenToViewportPoint(Input.mousePosition));
        
            if (!Physics.Raycast(ray, out RaycastHit hit)) return;

            //If we have hit something, snap the hit position to a voxel position
            var hitPos = GridSnapper.SnapToGrid(hit.point, Size, _offset);
            
            //Run vfx if we have it
            if ((byte)Voxel < _interactionEvents.Length) _interactionEvents[(byte)Voxel].Invoke();

            StartCoroutine(UpdateChunks(hitPos));
        }

        //For updating chunk voxel data. Includes updating chunks that don't exist in the scene.
        public IEnumerator UpdateChunks(Vector3 hitPos)
        {
            Vector3 chunkPos;
            Chunk chunk;
            Vector3 voxPos;
            Vector3 newChunkPos;
            Vector3 newHitPos;
            List<Chunk> chunkList;
            List<Vector3> posList;
            
            //Pick the shape type
            switch (_shape)
            {
                //Singular works similar to sphere, but on a continuous input
                //So it is restricted to a smaller space.
                case FlattenShape.Single:
                    chunkList = new List<Chunk>();
                    posList = new List<Vector3>();
                    for (var i = hitPos.x - _interactionSettings.MouseSize; i <= hitPos.x + _interactionSettings.MouseSize; i += Size)
                    {
                        for (var j = hitPos.z - _interactionSettings.MouseSize; j <= hitPos.z + _interactionSettings.MouseSize; j += Size)
                        {
                            //Search for a chunk
                            newChunkPos = new Vector3(i, hitPos.y, j);
                            chunkPos = _engine.NearestChunk(newChunkPos);
                            chunk = _engine.WorldData.GetNonNullChunkAt(chunkPos);

                            //if there isn't a chunk, continuously request one
                            while (chunk == null)
                            {
                                chunk = _engine.WorldData.GetNonNullChunkAt(chunkPos);
                                yield return null;
                            }
                            
                            //Add chunk if it isn't one that has been spawned.
                            if (!_engine.WorldData.Chunks.ContainsValue(chunk)) _engine.WorldData.Chunks.Add(new ChunkId(chunkPos.x, chunkPos.y, chunkPos.z), chunk);
                            
                            //Add to list if we haven't already
                            if (!chunkList.Contains(chunk))
                            {
                                chunkList.Add(chunk);
                                posList.Add(chunkPos);
                            }
                            
                            //Check position is in range, creates circle space
                            if (!InRange(newChunkPos, hitPos, _interactionSettings.MouseSize)) continue;

                            //Get voxel position, as well as current world position adjusted from the original hit.
                            voxPos = (newChunkPos - chunkPos) / Size;
                            newHitPos = hitPos;
                            newHitPos.x = i;
                            newHitPos.z = j;
                            
                            //Sets current voxel data and moves on y position to set height and depth.
                            Sphere(hitPos, voxPos, newHitPos, _interactionSettings.MouseSize, Voxel, chunk);
                            yield return null;
                        }
                    }
                    //If we have a chunk loader, save the edited chunks
                    if (_chunkLoader)
                    {
                        for (int i = 0; i < chunkList.Count; i++)
                        {
                            chunkList[i].Position = posList[i];
                            _chunkLoader.SaveChunk(chunkList[i], new ChunkId(posList[i].x, posList[i].y, posList[i].z));
                            yield return null;
                        }
                    }
                    
                    //Set mesh for remaining chunks. If chunk has no entity then it isn't a scene object
                    //So remove any chunks that were accessed just for their data beyond the render distance
                    for (int i = 0; i < chunkList.Count; i++)
                    {
                        chunkList[i].SetMesh(posList[i]);
                        if (!chunkList[i].GetEntity())
                            _engine.WorldData.Chunks.Remove(new ChunkId(posList[i].x, posList[i].y, posList[i].z));
                        yield return null;
                    }                  
                    //Stop vfx from running
                    if ((byte)Voxel < _interactionEvents.Length) _interactionEvents[(byte)Voxel].Invoke();
                    break;
                //Square can work in cubic space. Height and dig values affect its height range
                //Whereas at default it just effects a square area on x and z
                case FlattenShape.Square:
                    chunkList = new List<Chunk>();
                    posList = new List<Vector3>();
                    for (float i = hitPos.x - _interactionSettings.CubeXDistance; i <= hitPos.x + _interactionSettings.CubeXDistance; i += Size)
                    {
                        for (float j = hitPos.z - _interactionSettings.CubeZDistance; j <= hitPos.z + _interactionSettings.CubeZDistance; j += Size)
                        {
                            //Convert to nearest chunk position and find chunk
                            newChunkPos = new Vector3(i, hitPos.y, j);
                            chunkPos = _engine.NearestChunk(newChunkPos);
                            chunk = _engine.WorldData.GetNonNullChunkAt(chunkPos);

                            //If no chunk is found, it is out of scene bounds.
                            //Continuously check for chunk data at this position
                            while (chunk == null)
                            {
                                chunk = _engine.WorldData.GetNonNullChunkAt(chunkPos);
                                yield return null;
                            }
                            
                            //If world doesn't contain data, add it in for later use.
                            if (!_engine.WorldData.Chunks.ContainsValue(chunk)) _engine.WorldData.Chunks.Add(new ChunkId(chunkPos.x, chunkPos.y, chunkPos.z), chunk);
                            
                            //Add to list if it isn't already there.
                            if (!chunkList.Contains(chunk))
                            {
                                chunkList.Add(chunk);
                                posList.Add(chunkPos);
                            }

                            voxPos = (newChunkPos - chunkPos) / Size;
                            
                            //Sets current voxel value, as well as moving on y position to set height and depth
                            Flatten(voxPos, Voxel, _interactionSettings.Height, _interactionSettings.Dig, chunk);
                            yield return null;
                        }
                    }

                    //If we have a chunk loader, save the chunk data
                    if (_chunkLoader)
                    {
                        for (int i = 0; i < chunkList.Count; i++)
                        {
                            chunkList[i].Position = posList[i];
                            _chunkLoader.SaveChunk(chunkList[i], new ChunkId(posList[i].x, posList[i].y, posList[i].z));
                            yield return null;
                        }
                    }
                    
                    //Set the mesh of all chunks and remove any from the world without an entity object, as they don't exist in the scene
                    for (int i = 0; i < chunkList.Count; i++)
                    {
                        chunkList[i].SetMesh(posList[i]);
                        if (!chunkList[i].GetEntity())
                            _engine.WorldData.Chunks.Remove(new ChunkId(posList[i].x, posList[i].y, posList[i].z));
                        yield return null;
                    }                    
                    //stop any vfx
                    if ((byte)Voxel < _interactionEvents.Length) _interactionEvents[(byte)Voxel].Invoke();
                    break;
                
                //Circular works on a round space, if height and depth values are set then it will act cylindrical
                //Logic behaviour similar to previous methods. See Square comments for code.
                case FlattenShape.Circular:
                    chunkList = new List<Chunk>();
                    posList = new List<Vector3>();
                    for (float i = hitPos.x - _interactionSettings.CircleRadius; i <= hitPos.x + _interactionSettings.CircleRadius; i += Size)
                    {
                        for (float j = hitPos.z - _interactionSettings.CircleRadius; j <= hitPos.z + _interactionSettings.CircleRadius; j += Size)
                        {
                            newChunkPos = new Vector3(i, hitPos.y, j);
                            chunkPos = _engine.NearestChunk(newChunkPos);
                            chunk = _engine.WorldData.GetNonNullChunkAt(chunkPos);
                            
                            while (chunk == null)
                            {
                                chunk = _engine.WorldData.GetNonNullChunkAt(chunkPos);
                                yield return null;
                            }
                            if (!_engine.WorldData.Chunks.ContainsValue(chunk)) _engine.WorldData.Chunks.Add(new ChunkId(chunkPos.x, chunkPos.y, chunkPos.z), chunk);
                            
                            if (!chunkList.Contains(chunk))
                            {
                                chunkList.Add(chunk);
                                posList.Add(chunkPos);
                            }

                            if (!InRange(newChunkPos, hitPos, _interactionSettings.CircleRadius)) continue;
                        
                            voxPos = (newChunkPos - chunkPos) / Size;

                            Flatten(voxPos, Voxel, _interactionSettings.Height, _interactionSettings.Dig, chunk);
                            yield return null;
                        }
                    }

                    if (_chunkLoader)
                    {
                        for (int i = 0; i < chunkList.Count; i++)
                        {
                            chunkList[i].Position = posList[i];
                            _chunkLoader.SaveChunk(chunkList[i], new ChunkId(posList[i].x, posList[i].y, posList[i].z));
                            yield return null;
                        }
                    }

                    for (int i = 0; i < chunkList.Count; i++)
                    {
                        chunkList[i].SetMesh(posList[i]);
                        if (!chunkList[i].GetEntity())
                            _engine.WorldData.Chunks.Remove(new ChunkId(posList[i].x, posList[i].y, posList[i].z));
                        yield return null;
                    }
                    if ((byte)Voxel < _interactionEvents.Length) _interactionEvents[(byte)Voxel].Invoke();
                    break;
                //Spherical effects a 3D sphere space, but unlike mouse is only run on one click.
                //This allows spherical types to run on a larger area, because it takes ages.
                case FlattenShape.Sphere:
                    chunkList = new List<Chunk>();
                    posList = new List<Vector3>();
                    for (float i = hitPos.x - _interactionSettings.SphereRadius; i <= hitPos.x + _interactionSettings.SphereRadius; i += Size)
                    {
                        for (float j = hitPos.z - _interactionSettings.SphereRadius; j <= hitPos.z + _interactionSettings.SphereRadius; j += Size)
                        {
                            newChunkPos = new Vector3(i, hitPos.y, j);
                            chunkPos = _engine.NearestChunk(newChunkPos);
                            chunk = _engine.WorldData.GetNonNullChunkAt(chunkPos);

                            while (chunk == null)
                            {
                                chunk = _engine.WorldData.GetNonNullChunkAt(chunkPos);
                                yield return null;
                            }
                            
                            if (!_engine.WorldData.Chunks.ContainsValue(chunk)) _engine.WorldData.Chunks.Add(new ChunkId(chunkPos.x, chunkPos.y, chunkPos.z), chunk);
                            
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
                            Sphere(hitPos, voxPos, newHitPos, _interactionSettings.SphereRadius, Voxel, chunk);
                            yield return null;
                        }
                    }

                    if (_chunkLoader)
                    {
                        for (int i = 0; i < chunkList.Count; i++)
                        {
                            chunkList[i].Position = posList[i];
                            _chunkLoader.SaveChunk(chunkList[i], new ChunkId(posList[i].x, posList[i].y, posList[i].z));
                            yield return null;
                        }
                    }

                    for (int i = 0; i < chunkList.Count; i++)
                    {
                        chunkList[i].SetMesh(posList[i]);
                        if (!chunkList[i].GetEntity())
                            _engine.WorldData.Chunks.Remove(new ChunkId(posList[i].x, posList[i].y, posList[i].z));
                        yield return null;
                    }
                    if ((byte)Voxel < _interactionEvents.Length) _interactionEvents[(byte)Voxel].Invoke();
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
        
        private bool InRange(Vector3 pos, Vector3 origin, float distance) => Vector3.Distance(origin, pos) <= distance;

        private void Flatten(Vector3 pos, VoxelType voxelType, float flattenHeight, float digDepth, Chunk chunk)
        {
            Vector3 voxPos = pos;
            var voxType = voxelType;

            //For all voxels above the y position, update them
            //Destroy them if Destroy Above Ground is true
            //This bool allows both destruction and creation of voxels above ground
            do
            {
                chunk.SetVoxel(voxPos, voxType);
                if (_destroyAboveGround) voxType = VoxelType.Default;
                voxPos.y++;
            } while (Vector3.Distance(pos, voxPos) <= flattenHeight);

            voxPos = pos;

            //Sets all voxels below ground
            do
            {
                voxPos.y--;
                chunk.SetVoxel(voxPos, voxelType);
            } while (Vector3.Distance(voxPos, pos) <= digDepth);
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
