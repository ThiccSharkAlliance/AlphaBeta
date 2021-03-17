using System;
using UnityEngine;
using UnityEngine.VFX;

namespace VoxelTerrain.Interactions
{
    [CreateAssetMenu(fileName = "Vfx Interaction", menuName = "VFX/Interaction", order = 1), Serializable]
    public class VfxInteraction : ScriptableObject
    {
        [SerializeField] private VisualEffect _vfx;

        [SerializeField] private string _spawnRateStringId = "Enter ID";
        [SerializeField] private int _spawnRate;

        [SerializeField] private string _spawnPointStringId = "Enter ID";

        [SerializeField] private string _particleLifeStringId = "Enter ID";
        [SerializeField] private float _particleLife;

        [SerializeField] private string _sphereRadiusStringId = "Enter ID";
        [SerializeField] private float _sphereRadius;

        [SerializeField] private string _boxRadiusStringId = "Enter ID";
        [SerializeField] private float _boxRadius;

        [SerializeField] private string _planeRadiusStringId = "Enter ID";
        [SerializeField] private float _planeRadius;
    
        public FlattenShape Shape { get; set; }

        #region Essentials

        public VisualEffect VFX
        {
            get => _vfx;
            set => _vfx = value;
        }

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

        public string SpawnPointStringId
        {
            get => _spawnPointStringId;
            set => _spawnPointStringId = value;
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
        #endregion

        #region Square

        public string BoxRadiusStringId
        {
            get => _boxRadiusStringId;
            set => _boxRadiusStringId = value;
        }

        public float BoxRadius
        {
            get => _boxRadius;
            set => _boxRadius = value;
        }

        #endregion

        public void VfxPlaya(Vector3 spawnPoint, FlattenShape shape)
        {
            _vfx.SetInt(_spawnRateStringId, _spawnRate);
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
                    // _vfx.SetFloat(_particleLifeStringId, _particleLife);
                    // _vfx.SetFloat(_sphereRadiusStringId, _sphereRadius);
                    _vfx.SetVector3(_spawnPointStringId, spawnPoint); 


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
}
