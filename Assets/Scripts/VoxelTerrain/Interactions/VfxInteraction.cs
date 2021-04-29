using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;
using VoxelTerrain.Voxel;

namespace VoxelTerrain.Interactions
{
    [Serializable]
    public class VfxInteraction
    {
        private VisualEffect _vfx;

        [SerializeField] private string _spawnRateStringId = "Enter ID";
        [SerializeField] private int _spawnRate;

        [SerializeField] private string _spawnRateBoxStringId = "Enter ID";
        [SerializeField] private int _spawnRateBox;

        [SerializeField] private string _spawnPointStringId = "Enter ID";

        [SerializeField] private string _particleLifeStringId = "Enter ID";
        [SerializeField] private float _particleLife;

        [SerializeField] private string _sphereRadiusStringId = "Enter ID";
        [SerializeField] private float _sphereRadius;

        [SerializeField] private string _boxRadiusXStringId = "Enter ID";
        [SerializeField] private float _boxRadiusX;
        [SerializeField] private string _boxRadiusZStringId = "Enter ID";
        [SerializeField] private float _boxRadiusZ;

        [SerializeField] private string _planeRadiusStringId = "Enter ID";
        [SerializeField] private float _planeRadius;

        [SerializeField] private string _particleRingCountStringId = "Enter Id";
        [SerializeField] private float _particleRingCount;

        [SerializeField] private string _sparkSpawnRateStringId = "Enter ID";
        [SerializeField] private int _sparkSpawnRate;

        [SerializeField] private string _secondWaveColourStringId = "Enter ID";
        [SerializeField] private Color _secondWaveColour; 
        
        [SerializeField] private string _smokeSpawnStringId = "Enter ID";
        [SerializeField] private int _smokeSpawn;
        
        [SerializeField] private string _smokeColourStringId = "Enter ID";
        [SerializeField] private Color _smokeColour;  
        
        [SerializeField] private string _dustColourStringId = "Enter ID";
        [SerializeField] private Color _dustColour; 
        
        [SerializeField] private string _centreGradientStringId = "Enter ID";
        [SerializeField] private float _centreGradient;

        public VisualEffect[] Vfx = new VisualEffect[18];
        public Color[] SecondWaveColour = new Color[18];
        public Color[] SmokeColour = new Color[18];
        public Color[] DustColour = new Color[18];

        public bool ScanForVfx;

        public FlattenShape Shape { get; set; }

        #region Essentials
        
        public string SpawnRateStringId
        {
            get => _spawnRateStringId;
            set => _spawnRateStringId = value;
        }
        
        public int SpawnRate
        {
            get => _spawnRate;
            set => _spawnRate = value;
        }

        public string ParticleLifeStringId
        {
            get => _particleLifeStringId;
            set => _particleLifeStringId = value;
        }

        public float ParticleLife
        {
            get => _particleLife;
            set => _particleLife = value;
        }

        public string SpawnPointStringId
        {
            get => _spawnPointStringId;
            set => _spawnPointStringId = value;
        }  
        
        public string SparkSpawnRateStringId
        {
            get => _sparkSpawnRateStringId;
            set => _sparkSpawnRateStringId = value;
        }
        
        public int SparkSpawnRate
        {
            get => _sparkSpawnRate;
            set => _sparkSpawnRate = value;
        }

        #endregion

        #region Sphere

        public string SphereRadiusStringId
        {
            get => _sphereRadiusStringId;
            set => _sphereRadiusStringId = value;
        }

        public float SphereRadius
        {
            get => _sphereRadius;
            set => _sphereRadius = value;
        } 
        
        public string RingSpawnCountStringId
        {
            get => _particleRingCountStringId;
            set => _particleRingCountStringId = value;
        }
        
        public float RingSpawnCount
        {
            get => _particleRingCount;
            set => _particleRingCount = value;
        }
        public string SecondWaveColourStringId
        {
            get => _secondWaveColourStringId;
            set => _secondWaveColourStringId = value;
        } 
        public string SmokeSpawnStringId
        {
            get => _smokeSpawnStringId;
            set => _smokeSpawnStringId = value;
        } 
        public int SmokeSpawn
        {
            get => _smokeSpawn;
            set => _smokeSpawn = value;
        } 
        
        public string SmokeColourStringId
        {
            get => _smokeColourStringId;
            set => _smokeColourStringId = value;
        } 
  
        public string DustColourStringId
        {
            get => _dustColourStringId;
            set => _dustColourStringId = value;
        }    
        
        public string CentreGradientStringId
        {
            get => _centreGradientStringId;
            set => _centreGradientStringId = value;
        }   
        public float CentreGradient
        {
            get => _centreGradient;
            set => _centreGradient = value;
        } 
       

        #endregion

        #region Square

        public string BoxRadiusXStringId
        {
            get => _boxRadiusXStringId;
            set => _boxRadiusXStringId = value;
        }


        public float BoxRadiusX
        {
            get => _boxRadiusX;
            set => _boxRadiusX = value;
        }

        #endregion
        
        public void VfxPlaya(Vector3 spawnPoint, byte voxelType, InteractionSettings interactionSettings, FlattenShape shape = FlattenShape.Single)
        {
           

            Vfx[voxelType].SetInt(_spawnRateStringId, _spawnRate);
            switch (shape)
            {
                case FlattenShape.Single:
                    Vfx[voxelType].SetInt(_spawnRateStringId, _spawnRate);


                    break;
                case FlattenShape.Square:
                    Vfx[voxelType].SetInt(_spawnRateBoxStringId, _spawnRateBox);
                    Vfx[voxelType].SetFloat(_particleLifeStringId, _particleLife);
                    Vfx[voxelType].SetFloat(_boxRadiusXStringId, _boxRadiusX = interactionSettings.CubeXDistance);
                    Vfx[voxelType].SetFloat(_boxRadiusZStringId, _boxRadiusZ = interactionSettings.CubeZDistance);
                    Vfx[voxelType].SetVector3(_spawnPointStringId, spawnPoint);
                    Vfx[voxelType].SetInt(_sparkSpawnRateStringId, _sparkSpawnRate);
                    Vfx[voxelType].SetInt(_spawnRateStringId, 0);
                    Vfx[voxelType].SetInt(_smokeSpawnStringId, 0);
                    break;
                case FlattenShape.Circular:
                    Vfx[voxelType].SetInt(_spawnRateStringId, _spawnRate);


                    break;
                case FlattenShape.Sphere:
                    Vfx[voxelType].SetInt(_spawnRateStringId, _spawnRate);
                    Vfx[voxelType].SetFloat(_particleLifeStringId, _particleLife);
                    Vfx[voxelType].SetFloat(_sphereRadiusStringId,
                        _sphereRadius = interactionSettings.SphereRadius / 100f + 0.04f);
                    Vfx[voxelType].SetVector3(_spawnPointStringId, spawnPoint);
                    Vfx[voxelType].SetFloat(_particleRingCountStringId,
                        _particleRingCount);
                    Vfx[voxelType].SetInt(_sparkSpawnRateStringId, _sparkSpawnRate);
                    Vfx[voxelType].SetVector4(_secondWaveColourStringId, SecondWaveColour[voxelType]);
                    Vfx[voxelType].SetInt(_smokeSpawnStringId, _smokeSpawn);
                    Vfx[voxelType].SetVector4(_smokeColourStringId, SmokeColour[voxelType]);
                    Vfx[voxelType].SetVector4(_dustColourStringId, DustColour[voxelType]);
                    Vfx[voxelType].SetFloat(_centreGradientStringId, _centreGradient);
                    break;
                default:
                    break;
            }
        }

        public void VfxStopa(FlattenShape shape, byte voxelType) 
        {
            Vfx[voxelType].SetInt(_spawnRateStringId, 0);
            Vfx[voxelType].SetInt(_sparkSpawnRateStringId, 0);
            Vfx[voxelType].SetInt(_smokeSpawnStringId, 0);


            switch (shape)
            {
                case FlattenShape.Single:
                   
                    break;
                case FlattenShape.Square:
                    Vfx[voxelType].SetInt(_spawnRateBoxStringId, 0);
                    break;
                case FlattenShape.Circular:

                    break;
                case FlattenShape.Sphere:
                  
                    break;
                default:
                    break;
            }
        }
    }
}
