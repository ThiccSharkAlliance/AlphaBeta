﻿using System;
using System.Collections.Generic;
using UnityEngine;
using VoxelTerrain.MMesh;
using VoxelTerrain.Voxel.Jobs;

namespace VoxelTerrain.Voxel
{
    public class Chunk
    {
        public const int ChunkSize = 16; //Leave at this size
        public const int ChunkHeight = 64; //This should be 16 too, but I wanted taller chunks
        public byte[] Voxels;
        private VoxelEngine Engine;
        private GameObject Entity;

        //Used to find voxel at position
        public byte this[float x, float y, float z]
        {
            get => Voxels[PosToIndex((int)x, (int)y, (int)z)];
            set => Voxels[PosToIndex((int)x, (int)y, (int)z)] = value;
        }

        public void AddEntity(GameObject entity) => Entity = entity;
        public void AddEngine(VoxelEngine engine) => Engine = engine;

        public void SetVoxel(Vector3 pos, VoxelType vox) => this[(int) pos.x, (int) pos.y, (int) pos.z] = (byte) vox;

        public void SetMesh(Vector3 origin)
        {
            var meshCreator = new MeshCreator(origin, Engine.ChunkInfo.VoxelSize, Engine.WorldData);

            meshCreator.SetMesh(Voxels, origin.x, origin.y, origin.z,
                Engine.ChunkInfo.VoxelSize);

            var monoGo = Entity.GetComponent<MonoChunk>();
            
            var mesh = new Mesh();
            //Update mesh
            mesh.vertices = meshCreator.Vertices.ToArray();
            mesh.triangles = meshCreator.Triangles.ToArray();
            
            mesh.SetUVs(0, new List<Vector2>(mesh.vertices.Length));
            mesh.SetUVs(1, meshCreator.uv0);
            mesh.SetUVs(2, meshCreator.uv1);
            
            mesh.RecalculateBounds();
            mesh.RecalculateNormals();
            
            mesh.name = "Chunk: " + origin;

            monoGo.MeshFilter.sharedMesh = mesh;
            monoGo.MeshCollider.sharedMesh = mesh;
        }

        public static int PosToIndex(int x, int y, int z) => z * (ChunkSize) * (ChunkHeight) + y * (ChunkSize) + x;

        public Chunk(VoxelEngine engine)
        {
            Engine = engine;
            Voxels = new byte[ChunkSize * ChunkHeight * ChunkSize];
        }
    }
}
