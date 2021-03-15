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

    [SerializeField] private string _particleRingCountStringId;
    [SerializeField] private float _particleRingCount;
    
    [SerializeField] private string _sparkSpawnRateStringId;
    [SerializeField] private int _sparkSpawnRate;

    public void VfxPlaya(Vector3 spawnPoint, FlattenShape shape, InteractionSettings _interactionSettings)
    {
       
        switch (shape)
        {
            case FlattenShape.Single:
                _vfx.SetInt(_spawnRateStringId, _spawnRate);


                break;
            case FlattenShape.Square:
                _vfx.SetInt(_spawnRateStringId, _spawnRate);


                break;
            case FlattenShape.Circular:
                _vfx.SetInt(_spawnRateStringId, _spawnRate);


                break;
            case FlattenShape.Sphere:
                _vfx.SetInt(_spawnRateStringId, _spawnRate);
                _vfx.SetFloat(_particleLifeStringId, _particleLife);
                _vfx.SetFloat(_sphereRadiusStringId, _sphereRadius = _interactionSettings.SphereRadius / 100f + 0.04f);
                _vfx.SetVector3(_spawnPointStringId, spawnPoint); 
                _vfx.SetFloat(_particleRingCountStringId, _spawnRate); // this is not an accidient, ring count needs to be the same as the spawn rate or higher
                _vfx.SetInt(_sparkSpawnRateStringId, _sparkSpawnRate); 

                break;
            default:
                break;
        }
    }

    public void VfxStopa()
    {
        _vfx.SetInt(_spawnRateStringId, 0);
        _vfx.SetInt(_sparkSpawnRateStringId, 0);
    }
}
