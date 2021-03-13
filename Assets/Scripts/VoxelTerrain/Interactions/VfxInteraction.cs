using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;
using VoxelTerrain.Mouse;

[Serializable]
public struct VfxInteraction
{
    [SerializeField] private VisualEffect _vfx;

    [SerializeField] private string _spawnRateStringId;
    [SerializeField] private int _spawnRate;

    [SerializeField] private string _spawnPointStringId;

    [SerializeField] private string _particleLifeStringId;
    [SerializeField] private float _particleLife;

    [SerializeField] private string _sphereRadiusStringId;
    [SerializeField] private float _sphereRadius;

    [SerializeField] private string _boxRadiusStringId;
    [SerializeField] private float _boxRadius;

    [SerializeField] private string _planeRadiusStringId;
    [SerializeField] private float _planeRadius;

    public void VfxPlaya(Vector3 spawnPoint, FlattenShape shape)
    {
        _vfx.SetInt(_spawnRateStringId, _spawnRate);
        switch (shape)
        {
            case FlattenShape.Single:
                break;
            case FlattenShape.Square:
                break;
            case FlattenShape.Circular:
                break;
            case FlattenShape.Sphere:
                break;
            default:
                break;
        }
    }

    public void VfxStopa()
    {
        _vfx.SetInt(_spawnRateStringId, 0);
    }
}
