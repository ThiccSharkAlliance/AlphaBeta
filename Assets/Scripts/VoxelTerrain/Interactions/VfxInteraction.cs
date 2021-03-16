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

    [SerializeField] private string _spawnRateBoxStringId;
    [SerializeField] private int _spawnRateBox;

    [SerializeField] private string _spawnPointStringId;

    [SerializeField] private string _particleLifeStringId;
    [SerializeField] private float _particleLife;

    [SerializeField] private string _sphereRadiusStringId;
    [SerializeField] private float _sphereRadius;

    [SerializeField] private string _boxRadiusXStringId;
    [SerializeField] private float _boxRadiusX;
    [SerializeField] private string _boxRadiusZStringId;
    [SerializeField] private float _boxRadiusZ;

    [SerializeField] private string _planeRadiusStringId;
    [SerializeField] private float _planeRadius;

    [SerializeField] private string _particleRingCountStringId;
    [SerializeField] private float _particleRingCount;
    
    [SerializeField] private string _sparkSpawnRateStringId;
    [SerializeField] private int _sparkSpawnRate;

    
    

    public void VfxPlaya(Vector3 spawnPoint, FlattenShape shape, InteractionSettings _interactionSettings)
    {
       
        switch (shape)
        {
            case FlattenShape.Single:
              


                break;
            case FlattenShape.Square:
                _vfx.SetInt(_spawnRateBoxStringId, _spawnRateBox);
                _vfx.SetFloat(_particleLifeStringId, _particleLife);
                _vfx.SetFloat(_boxRadiusXStringId, _boxRadiusX = _interactionSettings.CubeXDistance);
                _vfx.SetFloat(_boxRadiusZStringId, _boxRadiusZ = _interactionSettings.CubeZDistance);
                _vfx.SetVector3(_spawnPointStringId, spawnPoint); 
                _vfx.SetInt(_sparkSpawnRateStringId, _sparkSpawnRate);
                _vfx.SetInt(_spawnRateStringId, 0);


                break;
            case FlattenShape.Circular:
           


                break;
            case FlattenShape.Sphere:
                _vfx.SetInt(_spawnRateStringId, _spawnRate);
                _vfx.SetFloat(_particleLifeStringId, _particleLife);
                _vfx.SetFloat(_sphereRadiusStringId, _sphereRadius = _interactionSettings.SphereRadius / 100f + 0.04f);
                _vfx.SetVector3(_spawnPointStringId, spawnPoint); 
                _vfx.SetFloat(_particleRingCountStringId, _particleRingCount); // this is not an accidient, ring count needs to be the same as the spawn rate or higher
                _vfx.SetInt(_sparkSpawnRateStringId, _sparkSpawnRate);
                _vfx.SetInt(_spawnRateBoxStringId, 0);

                break;
            default:
                break;
        }
    }

    public void VfxStopa() // may need to add a small delay before this method is run on the voxelinteraction script
    {
        _vfx.SetInt(_spawnRateStringId, 0);
        _vfx.SetInt(_sparkSpawnRateStringId, 0);
        _vfx.SetInt(_spawnRateBoxStringId, 0);
    }
}
